using FIAP.FCG.Application.DTOs;
using FIAP.FCG.Application.Implementations;
using FIAP.FCG.Domain.Contracts.IRepositories;
using FIAP.FCG.Domain.Entities;
using FluentAssertions;
using Keycloak.Net;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace FIAP.FCG.UnitTests.Application.UserProfileTest
{
    [Collection(nameof(RegisterUserProfileTest))]
    public class RegisterUserProfileTest
    {
        private readonly Mock<IUserProfileRepository> _userProfileRepositoryMock;
        private readonly Mock<KeycloakClient> _keycloakClientMock;
        private readonly Mock<IOptions<KeycloakOptions>> _keycloakOptionsMock;
        private readonly UserProfileApplicationService _userProfileService;

        public RegisterUserProfileTest()
        {
            _keycloakOptionsMock = new Mock<IOptions<KeycloakOptions>>();
            _keycloakOptionsMock.Setup(o => o.Value).Returns(new KeycloakOptions
            {
                ClientId = "meu-client-id",
                ClientSecret = "meu-client-secret",
                Realm = "meu-reino",
                ServerUrl = "https://keycloak.meuservidor.com"
            });

            _userProfileRepositoryMock = new Mock<IUserProfileRepository>();

            var keycloakClientMock = new Mock<KeycloakClient>("meu-client-id", "meu-client-secret");

            _userProfileService = new UserProfileApplicationService(
                keycloakClientMock.Object,
                _userProfileRepositoryMock.Object,
                _keycloakOptionsMock.Object
            );
        }

[Fact(DisplayName = nameof(Register_UserWithInvalidEmail_ShouldThrowBadRequestException))]
[Trait("Application", "Register - Use Cases")]
public async Task Register_UserWithInvalidEmail_ShouldThrowBadRequestException()
{
    // Arrange
    var userProfileDTO = new UserProfileDTO
    {
        Email = "invalid-email",
        Password = "Password123!",
        ConfirmPassword = "Password123!",
        Name = "Test User",
        CPF = "12345678900",
        Birthday = new DateTime(1990, 01, 01)
    };

    // Act
    Func<Task> act = async () => await _userProfileService.Register(userProfileDTO);

    // Assert
    await act.Should()
        .ThrowAsync<HttpStatusCodeException>()
        .Where(ex => ex.StatusCode == 400 && ex.Message == "E-mail com má formatação.");
}


        [Fact(DisplayName = nameof(Register_UserAlreadyExists_ShouldThrowConflictException))]
        [Trait("Application", "Register - Use Cases")]
        public async Task Register_UserAlreadyExists_ShouldThrowConflictException()
        {
            // Arrange
            var userProfileDTO = new UserProfileDTO
            {
                Email = "existing-email@example.com",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                Name = "Test User",
                CPF = "12345678900",
                Birthday = new DateTime(1990, 01, 01)
            };

            var existingUser = new UserProfile(
                "Existing User", "existing-email@example.com", "12345678900", new DateTime(1990, 01, 01), "hashedPassword", "hashedPassword"
            );

            _userProfileRepositoryMock.Setup(r => r.GetByEmail(userProfileDTO.Email))
                                      .ReturnsAsync(existingUser);

            // Act
            Func<Task> act = async () => await _userProfileService.Register(userProfileDTO);

            // Assert
            await act.Should()
                .ThrowAsync<HttpStatusCodeException>()
                .Where(ex => ex.StatusCode == 409 && ex.Message == "Já existe um usuário cadastrado com o mesmo e-mail.");
        }


        [Fact(DisplayName = nameof(Register_PasswordsDoNotMatch_ShouldThrowException))]
        [Trait("Application", "Register - Use Cases")]
        public async Task Register_PasswordsDoNotMatch_ShouldThrowException()
        {
            // Arrange
            var userProfileDTO = new UserProfileDTO
            {
                Email = "new-email@example.com",
                Password = "Password123!",
                ConfirmPassword = "DifferentPassword123!", // senhas diferentes
                Name = "Test User",
                CPF = "12345678900",
                Birthday = new DateTime(1990, 01, 01)
            };

            // Act
            var result = async () => await _userProfileService.Register(userProfileDTO);

            // Assert
            await result.Should().ThrowAsync<Exception>()
                .WithMessage("As senhas não correspondem.");
        }

        [Fact(DisplayName = nameof(Register_InvalidPasswordFormat_ShouldThrowException))]
        [Trait("Application", "Register - Use Cases")]
        public async Task Register_InvalidPasswordFormat_ShouldThrowException()
        {
            // Arrange
            var userProfileDTO = new UserProfileDTO
            {
                Email = "new-email@example.com",
                Password = "123",
                ConfirmPassword = "123",
                Name = "Test User",
                CPF = "12345678900",
                Birthday = new DateTime(1990, 01, 01)
            };

            // Act
            var result = async () => await _userProfileService.Register(userProfileDTO);

            // Assert
            await result.Should()
                .ThrowAsync<HttpStatusCodeException>()
                .WithMessage("A senha não está no formato correto.");
        }

        [Fact(DisplayName = nameof(Register_SuccessfulRegistration_ShouldReturnValidResult))]
        [Trait("Application", "Register - Use Cases")]
        public async Task Register_SuccessfulRegistration_ShouldReturnValidResult()
        {
            // Arrange
            var userProfileDTO = new UserProfileDTO
            {
                Email = "new-email@example.com",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                Name = "Test User",
                CPF = "12345678900",
                Birthday = new DateTime(1990, 01, 01)
            };

            _userProfileRepositoryMock.Setup(r => r.GetByEmail(userProfileDTO.Email))
                                      .ReturnsAsync((UserProfile)null);

            _userProfileRepositoryMock.Setup(r => r.Add(It.IsAny<UserProfile>()))
                                      .Returns(Task.CompletedTask);

            // Act
            var result = await _userProfileService.Register(userProfileDTO);

            // Assert
            result.Should().NotBeNull();
            result.ValidationProblemDetails.Should().BeNull("não deve haver erros de validação em um cadastro bem-sucedido");
            result.Response.Should().NotBeNull("o usuário deve ser retornado com sucesso");
            result.Response!.Email.Should().Be(userProfileDTO.Email);
            result.Response.Name.Should().Be(userProfileDTO.Name);

            _userProfileRepositoryMock.Verify(r => r.Add(It.IsAny<UserProfile>()), Times.Once);
            _userProfileRepositoryMock.Verify(r => r.GetByEmail(userProfileDTO.Email), Times.Once);
        }
    }
}
