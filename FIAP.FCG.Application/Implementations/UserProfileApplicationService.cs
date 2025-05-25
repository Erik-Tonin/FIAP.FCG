using FIAP.FCG.Application.Contracts.IApplicationService;
using FIAP.FCG.Application.DTOs;
using FIAP.FCG.Domain.Contracts.IRepositories;
using FIAP.FCG.Domain.Entities;
using FluentValidation.Results;
using Keycloak.Net;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace FIAP.FCG.Application.Implementations
{
    public class UserProfileApplicationService : ApplicationServiceBase, IUserProfileApplicationService
    {
        private readonly KeycloakClient _keycloakClient;
        private readonly KeycloakOptions _options;
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly HttpClient _httpClient;

        public UserProfileApplicationService(KeycloakClient keycloakClient, IUserProfileRepository userProfileRepository, IOptions<KeycloakOptions> options)
        {
            _keycloakClient = keycloakClient;
            _userProfileRepository = userProfileRepository;
            _httpClient = new HttpClient();
            _options = options.Value;
        }

        public async Task<ValidationResultDTO<UserProfile>> Register(UserProfileDTO userProfileDTO)
        {
            if(!await EmailValido(userProfileDTO.Email!))
                throw new HttpStatusCodeException(400, "E-mail com má formatação.");

            UserProfile user = await GetByEmail(userProfileDTO.Email!);

            if (user != null)
                throw new HttpStatusCodeException(409, "Já existe um usuário cadastrado com o mesmo e-mail.");

            if (userProfileDTO.Password != userProfileDTO.ConfirmPassword)
                throw new HttpStatusCodeException(400, "As senhas não correspondem.");

            if (!await PassawordIsCorret(userProfileDTO.Password))
                throw new HttpStatusCodeException(400, "A senha não está no formato correto.");

            string hashedPassword = PasswordHasher.HashPassword(userProfileDTO.Password);
            string hashedConfirmPassword = PasswordHasher.HashPassword(userProfileDTO.ConfirmPassword);

            user = new UserProfile(
                userProfileDTO.Name!,
                userProfileDTO.Email!,
                userProfileDTO.CPF!,
                userProfileDTO.Birthday,
                hashedPassword,
                hashedConfirmPassword);

            await _userProfileRepository.Add(user);

            return CustomValidationDataResponse<UserProfile>(user);
        }

        public async Task<UserProfileDTO> GetById(Guid id)
        {
            UserProfile user = await _userProfileRepository.GetById(id);

            if (user == null)
                throw new HttpStatusCodeException(404, "Usuário não encontrado.");

            return new UserProfileDTO()
            {
                Id = id,
                Name = user.Name,
                Email = user.Email,
                CPF = user.Cpf,
                Birthday = user.Birthday!
            };
        }

        public async Task<IEnumerable<UserProfileDTO>> GetAll()
        {
            var users = _userProfileRepository.GetAll();

            if (users == null)
                throw new HttpStatusCodeException(404, "Usuário não encontrado.");

            return await Task.FromResult(users.Select(x => new UserProfileDTO()
            {
                Id = x.Id,
                Name = x.Name,
                Email = x.Email,
                CPF = x.Cpf,
                Birthday = x.Birthday
            }));
        }

        public async Task<ValidationResult> UpdateUser(UserProfileDTO userProfileDTO)
        {
            UserProfile user = await GetByEmail(userProfileDTO.Email!);

            if(user == null)
                throw new HttpStatusCodeException(404, "Usuário não encontrado.");

            if (user.IsValid())
            {
                user.UpdateUser(
                    userProfileDTO.Name!,
                    userProfileDTO.Email!,
                    userProfileDTO.CPF!,
                    userProfileDTO.Birthday);

                _userProfileRepository.Update(user);
            }
            else
            {
                return user.ValidationResult;
            }

            return user.ValidationResult;
        }

        public async Task<ValidationResult> Login(string email, string password)
        {
            UserProfile user = await GetByEmail(email);

            string hashedPassword = PasswordHasher.HashPassword(password);

            bool login = user.Login(email, hashedPassword);

            if (!login)
                throw new Exception("Não foi possivel fazer o login");

            return user.ValidationResult;
        }

        public async Task<UserProfile> GetByEmail(string email)
        {
            return await _userProfileRepository.GetByEmail(email);
        }

        public async Task<string> ObterTokenAsync()
        {
            var parametros = new Dictionary<string, string>
            {
                { "client_id", _options.ClientId! },
                { "client_secret", _options.ClientSecret! },
                { "grant_type", "client_credentials" }
            };

            var conteudo = new FormUrlEncodedContent(parametros);

            var resposta = await _httpClient.PostAsync(_options.TokenUrl, conteudo);

            if (!resposta.IsSuccessStatusCode)
            {
                var erro = await resposta.Content.ReadAsStringAsync();
                throw new Exception($"Erro ao obter token do Keycloak: {erro}");
            }

            var json = await resposta.Content.ReadAsStringAsync();
            var dados = JsonSerializer.Deserialize<JsonElement>(json);
            return dados.GetProperty("access_token").GetString();
        }

        public async Task<bool> PassawordIsCorret(string passaword)
        {
            var regex = new Regex(@"^(?=.*[a-zA-Z])(?=.*\d)(?=.*[\W_]).{8,}$");
            if (regex.IsMatch(passaword))
                return true;

            return false;
        }

        public async Task<bool> EmailValido(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            string padraoEmail = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, padraoEmail, RegexOptions.IgnoreCase);
        }

        public async Task<string> GenerateToken(string email, string password)
        {
            var client = new HttpClient();

            var parameters = new Dictionary<string, string>
            {
                { "client_id", "fcg-user-registration" },
                { "grant_type", "password" },
                { "username", email },
                { "password", password },
                { "client_secret", _options.ClientSecret! }
            };

            var content = new FormUrlEncodedContent(parameters);

            var response = await client.PostAsync("http://localhost:8080/realms/fcg-realm/protocol/openid-connect/token", content);

            if (!response.IsSuccessStatusCode)
                throw new HttpStatusCodeException(401, "E-mail ou senha inválidos.");

            var result = await response.Content.ReadAsStringAsync();

            var json = System.Text.Json.JsonDocument.Parse(result);
            var token = "Bearer " + json.RootElement.GetProperty("access_token").GetString();

            return token!;
        }

        public async Task<bool> RegisterInKeyCloak(UserProfileDTO userProfileDTO)
        {
            var token = await ObterTokenAsync();

            var client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var novoUsuario = new
            {
                username = userProfileDTO.Email!,
                email = userProfileDTO.Email!,
                firstName = userProfileDTO.Name!,
                lastName = userProfileDTO.Name!,
                enabled = true,
                emailVerified = true,
                credentials = new[]
                {
                new {
                        type = "password",
                        value = userProfileDTO.Password,
                        temporary = false
                    }
                },
                realmRoles = new[] { "User" }
            };

            var json = JsonSerializer.Serialize(novoUsuario);
            var conteudo = new StringContent(json, Encoding.UTF8, "application/json");

            var resposta = await client.PostAsync("http://localhost:8080/admin/realms/fcg-realm/users", conteudo);

            if (!resposta.IsSuccessStatusCode)
            {
                var erro = await resposta.Content.ReadAsStringAsync();
                throw new Exception($"Erro ao criar usuário: {erro}");
            }

            return true;
        }
    }
}
