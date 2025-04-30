using FIAP.FCG.Application.Implementations;
using FIAP.FCG.Domain.Contracts.IRepositories;
using FIAP.FCG.Domain.Entities;
using FluentAssertions;
using Moq;
using Xunit;

namespace FIAP.FCG.UnitTests.Application.GameTest
{
    [Collection(nameof(GetGameByIdTest))]
    public class GetGameByIdTest
    {
        private readonly Mock<IGameRepository> _gameRepositoryMock;
        private readonly GameApplicationService _gameAppService;

        public GetGameByIdTest()
        {
            _gameRepositoryMock = new Mock<IGameRepository>();
            _gameAppService = new GameApplicationService(_gameRepositoryMock.Object);
        }

        [Fact(DisplayName = nameof(ShouldReturnGameByIdWhenGameExists))]
        [Trait("Application", "GetGameById - Use Cases")]
        public async Task ShouldReturnGameByIdWhenGameExists()
        {
            // Arrange
            Game game = new Game("Hollow Knight", Guid.NewGuid(), 10, 30.0f, DateTime.Parse("24/02/2017"), null);

            _gameRepositoryMock.Setup(x => x.GetById(game.Id))
                               .ReturnsAsync(game);

            // Act
            var result = await _gameAppService.GetById(game.Id);

            // Assert
            result.Should().NotBeNull();
            result.Response.Should().NotBeNull();
            result.Response.Name.Should().Be(game.Name);
            result.Response.Price.Should().Be(game.Price);
            result.Response.Censorship.Should().Be(game.Censorship);
            result.Response.DateRelease.Should().Be(game.DateRelease);
        }

        [Fact(DisplayName = nameof(GetById_ShouldReturnNull_WhenGameDoesNotExist))]
        [Trait("Application", "GetGameById  - Use Cases")]
        public async Task GetById_ShouldReturnNull_WhenGameDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();
            _gameRepositoryMock.Setup(r => r.GetById(id)).ReturnsAsync((Game)null);

            // Act
            var result = await _gameAppService.GetById(id);

            // Assert
            result.Should().NotBeNull();
            result.Response.Should().BeNull();
            result.ValidationProblemDetails.Should().NotBeNull();
            result.ValidationProblemDetails.Errors.Should().ContainKey("Game");
            result.ValidationProblemDetails.Errors["Game"].Should().Contain("Não foi encontrado o jogo!.");
        }
    }
}
