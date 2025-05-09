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
    public class GetByIdUserProfileTest
    {
        private readonly Mock<IUserProfileRepository> _userProfileRepositoryMock;
        private readonly Mock<KeycloakClient> _keycloakClientMock;
        private readonly Mock<IOptions<KeycloakOptions>> _keycloakOptionsMock;
        private readonly UserProfileApplicationService _userProfileService;

        public GetByIdUserProfileTest()
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

        [Fact(DisplayName = nameof(GetById_ShouldReturnCorrectUserProfileDTO))]
        [Trait("Application", "GetById - Use Cases")]
        public async Task GetById_ShouldReturnCorrectUserProfileDTO()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new UserProfile(
                name: "Test User",
                email: "test@example.com",
                cpf: "12345678900",
                birthday: new DateTime(1990, 01, 01),
                password: "hashedPassword",
                confirmPassword: "hashedPassword"
            );

            _userProfileRepositoryMock.Setup(repo => repo.GetById(userId))
                                      .ReturnsAsync(user);

            // Act
            var result = await _userProfileService.GetById(userId);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(userId);
            result.Name.Should().Be(user.Name);
            result.Email.Should().Be(user.Email);
            result.CPF.Should().Be(user.Cpf);
            result.Birthday.Should().Be(user.Birthday);
        }

        [Fact(DisplayName = nameof(GetById_UserNotFound_ShouldThrowException))]
        [Trait("Application", "GetById - Use Cases")]
        public async Task GetById_UserNotFound_ShouldThrowException()
        {
            // Arrange
            var userId = Guid.NewGuid();

            _userProfileRepositoryMock.Setup(repo => repo.GetById(userId))
                                      .ReturnsAsync((UserProfile)null);

            // Act
            Func<Task> act = async () => await _userProfileService.GetById(userId);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Usuário não encontrado.");
        }
    }
}
