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
    public class UpdateUserUserProfileTest
    {
        private readonly Mock<IUserProfileRepository> _userProfileRepositoryMock;
        private readonly Mock<KeycloakClient> _keycloakClientMock;
        private readonly Mock<IOptions<KeycloakOptions>> _keycloakOptionsMock;
        private readonly UserProfileApplicationService _userProfileService;

        public UpdateUserUserProfileTest()
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

        [Fact(DisplayName = nameof(UpdateUser_ShouldUpdateUserSuccessfully))]
        [Trait("Application", "UpdateUser - Use Cases")]
        public async Task UpdateUser_ShouldUpdateUserSuccessfully()
        {
            // Arrange
            var userProfileDTO = new UserProfileDTO
            {
                Email = "existing-email@example.com",
                Name = "Updated Name",
                CPF = "12345678900",
                Birthday = new DateTime(1990, 01, 01)
            };

            var existingUser = new UserProfile("Existing User", "existing-email@example.com", "12345678900", new DateTime(1990, 01, 01), "hashedPassword", "hashedPassword");

            _userProfileRepositoryMock.Setup(repo => repo.GetByEmail(userProfileDTO.Email))
                                      .ReturnsAsync(existingUser);

            // Act
            var result = await _userProfileService.UpdateUser(userProfileDTO);

            // Assert
            result.Should().NotBeNull();
            existingUser.Name.Should().Be("Updated Name");
            existingUser.Email.Should().Be("existing-email@example.com");
            existingUser.Cpf.Should().Be("12345678900");
        }

        [Fact(DisplayName = nameof(UpdateUser_UserNotFound_ShouldThrowException))]
        [Trait("Application", "UpdateUser - Use Cases")]
        public async Task UpdateUser_UserNotFound_ShouldThrowException()
        {
            // Arrange
            var userProfileDTO = new UserProfileDTO
            {
                Email = "non-existing-email@example.com",
                Name = "Updated Name",
                CPF = "12345678900",
                Birthday = new DateTime(1990, 01, 01)
            };

            _userProfileRepositoryMock.Setup(repo => repo.GetByEmail(userProfileDTO.Email))
                                      .ReturnsAsync((UserProfile)null);

            // Act
            Func<Task> act = async () => await _userProfileService.UpdateUser(userProfileDTO);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Usuário não encontrado.");
        }

        [Fact(DisplayName = nameof(UpdateUser_InvalidUser_ShouldReturnValidationResult))]
        [Trait("Application", "UpdateUser - Use Cases")]
        public async Task UpdateUser_InvalidUser_ShouldReturnValidationResult()
        {
            // Arrange
            var userProfileDTO = new UserProfileDTO
            {
                Email = "existing-email@example.com",
                Name = "Invalid User",
                CPF = "12345678900",
                Birthday = new DateTime(1990, 01, 01)
            };

            var invalidUser = new UserProfile("Existing User", "existing-email@example.com", "12345678900", new DateTime(1990, 01, 01), "hashedPassword", "hashedPassword");
            invalidUser.AddValidationError("Name", "Invalid name format.");

            _userProfileRepositoryMock.Setup(repo => repo.GetByEmail(userProfileDTO.Email))
                                      .ReturnsAsync(invalidUser);

            // Act
            var result = await _userProfileService.UpdateUser(userProfileDTO);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact(DisplayName = nameof(UpdateUser_UserWithInvalidName_ShouldReturnValidationError))]
        [Trait("Application", "UpdateUser - Use Cases")]
        public async Task UpdateUser_UserWithInvalidName_ShouldReturnValidationError()
        {
            // Arrange
            var userProfileDTO = new UserProfileDTO
            {
                Email = "existing-email@example.com",
                Name = "",
                CPF = "12345678900",
                Birthday = new DateTime(1990, 01, 01)
            };

            var existingUser = new UserProfile("Existing User", "existing-email@example.com", "12345678900", new DateTime(1990, 01, 01), "hashedPassword", "hashedPassword");

            _userProfileRepositoryMock.Setup(repo => repo.GetByEmail(userProfileDTO.Email))
                                      .ReturnsAsync(existingUser);

            // Act
            var result = await _userProfileService.UpdateUser(userProfileDTO);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
