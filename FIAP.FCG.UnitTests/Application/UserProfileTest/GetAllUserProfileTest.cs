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
    public class GetAllUserProfileTest
    {
        private readonly Mock<IUserProfileRepository> _userProfileRepositoryMock;
        private readonly Mock<KeycloakClient> _keycloakClientMock;
        private readonly Mock<IOptions<KeycloakOptions>> _keycloakOptionsMock;
        private readonly UserProfileApplicationService _userProfileService;

        public GetAllUserProfileTest()
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

        [Fact(DisplayName = nameof(GetAll_ShouldReturnAllUserProfileDTOs))]
        [Trait("Application", "GetAll - Use Cases")]
        public async Task GetAll_ShouldReturnAllUserProfileDTOs()
        {
            // Arrange
            var users = new List<UserProfile>
            {
                new UserProfile("User 1", "user1@example.com", "12345678900", new DateTime(1990, 01, 01), "hashedPassword", "hashedPassword"),
                new UserProfile("User 2", "user2@example.com", "12345678901", new DateTime(1992, 02, 02), "hashedPassword", "hashedPassword")
            };

            _userProfileRepositoryMock.Setup(repo => repo.GetAll())
                                      .Returns(users);

            // Act
            var result = await _userProfileService.GetAll();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2); // Ensure that we have 2 users returned
            result.First().Name.Should().Be("User 1");
            result.Last().Name.Should().Be("User 2");
        }

        [Fact(DisplayName = nameof(GetAll_UsersNotFound_ShouldThrowException))]
        [Trait("Application", "GetAll - Use Cases")]
        public async Task GetAll_UsersNotFound_ShouldThrowException()
        {
            // Arrange
            _userProfileRepositoryMock.Setup(repo => repo.GetAll())
                                      .Returns((IEnumerable<UserProfile>)null);

            // Act
            Func<Task> act = async () => await _userProfileService.GetAll();

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Usuários não encontrados.");
        }
    }
}
