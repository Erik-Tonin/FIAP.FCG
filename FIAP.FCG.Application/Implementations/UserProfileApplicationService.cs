using FIAP.FCG.Application.Contracts.IApplicationService;
using FIAP.FCG.Application.DTOs;
using FIAP.FCG.Domain.Contracts.IRepositories;
using FIAP.FCG.Domain.Entities;
using FluentValidation.Results;
using System.Text.RegularExpressions;

namespace FIAP.FCG.Application.Implementations
{
    public class UserProfileApplicationService : ApplicationServiceBase, IUserProfileApplicationService
    {
        private readonly IUserProfileRepository _userProfileRepository;
        public UserProfileApplicationService(IUserProfileRepository userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
        }

        public async Task<ValidationResultDTO<UserProfile>> Register(UserProfileDTO userProfileDTO)
        {
            UserProfile user = await GetByEmail(userProfileDTO.Email);

            if (user != null)            
                AddValidationError("Usuário já cadastrado.", "Já existe um usuário cadastrado com o mesmo e-mail.");
            
            if (userProfileDTO.Password != userProfileDTO.ConfirmPassword)            
                throw new Exception("As senhas não correspondem.");

            if(await PassawordIsCorret(userProfileDTO.Password) == false)
                throw new Exception("As senhas não estão no formato correto.");

            string hashedPassword = PasswordHasher.HashPassword(userProfileDTO.Password);
            string hashedConfirmPassword = PasswordHasher.HashPassword(userProfileDTO.ConfirmPassword);

            user = new UserProfile(
                userProfileDTO.Name,
                userProfileDTO.Email,
                userProfileDTO.CPF,
                userProfileDTO.Birthday,
                hashedPassword,
                hashedConfirmPassword);

            await _userProfileRepository.Add(user);

            return CustomValidationDataResponse<UserProfile>(user);
        }

        public async Task<UserProfileDTO> GetById(Guid id)
        {
            UserProfile user = await _userProfileRepository.GetById(id);

            return new UserProfileDTO()
            {
                Id = id,
                Name = user.Name,
                Email = user.Email,
                CPF = user.Cpf,
                Birthday = user.Birthday
            };
        }

        public async Task<IEnumerable<UserProfileDTO>> GetAll()
        {
            var users = _userProfileRepository.GetAll();
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
            UserProfile user = await GetByEmail(userProfileDTO.Email);

            if (user.IsValid())
            {
                user.UpdateUser(
                    userProfileDTO.Name,
                    userProfileDTO.Email,
                    userProfileDTO.CPF,
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

        public async Task<bool> PassawordIsCorret(string passaword)
        {
            var regex = new Regex(@"^(?=.*[a-zA-Z])(?=.*\d)(?=.*[\W_]).{8,}$");
            if (regex.IsMatch(passaword))            
                return true;
            
            return false;
        }
    }
}
