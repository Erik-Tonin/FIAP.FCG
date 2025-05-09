using FIAP.FCG.Application.DTOs;
using FIAP.FCG.Application.Implementations;
using FIAP.FCG.Domain.Contracts.IRepositories;
using FIAP.FCG.Domain.Entities;
using FluentAssertions;
using FluentValidation.Results;
using Moq;
using Xunit;

namespace FIAP.FCG.UnitTests.Application.GameTest
{
    public class UpdateGameTest
    {
        private readonly Mock<IGameRepository> _gameRepositoryMock;
        private readonly GameApplicationService _gameAppService;

        public UpdateGameTest()
        {
            _gameRepositoryMock = new Mock<IGameRepository>();
            _gameAppService = new GameApplicationService(_gameRepositoryMock.Object); ;
        }

        [Fact(DisplayName = nameof(UpdateGame_GameIsValid_ShouldUpdateGame))]
        [Trait("Application", "UpdateGame - Use Cases")]
        public async Task UpdateGame_GameIsValid_ShouldUpdateGame()
        {
            //ARRANGE
            var gameId = Guid.NewGuid();
            var gameDTO = new GameDTO
            {
                Id = gameId,
                Name = "Hades 12",
                Category = Guid.NewGuid(),
                Censorship = 16,
                Price = 79.90f,
                DateRelease = new DateTime(2020, 09, 17),
            };

            var game = new Game(gameDTO.Name, gameDTO.Category, gameDTO.Censorship, gameDTO.Price, gameDTO.DateRelease, gameDTO.ImageURL);

            typeof(Game)
                .GetProperty("ValidationResult")!
                .SetValue(game, new ValidationResult());

            _gameRepositoryMock.Setup(r => r.GetById(gameDTO.Id))
                               .ReturnsAsync(game);
            
            _gameRepositoryMock
                .Setup(r => r.Update(It.IsAny<Game>()));

            // Act
            var result = await _gameAppService.UpdateGame(gameDTO);

            // Assert
            result.Should().NotBeNull();
            result.IsValid.Should().BeTrue();
        }

        [Fact(DisplayName = nameof(UpdateGame_GameIsNotValid_ShouldNotUpdateGame))]
        [Trait("Application", "UpdateGame - Use Cases")]
        public async Task UpdateGame_GameIsNotValid_ShouldNotUpdateGame()
        {
            // Arrange
            var gameDTO = new GameDTO
            {
                Id = Guid.NewGuid(),
                Name = "Hades",
                Category = Guid.NewGuid(),
                Censorship = 16,
                Price = 79.90f,
                DateRelease = new DateTime(2020, 09, 17)
            };

            var existingGame = new Game(
                "Hades",
                gameDTO.Category,
                gameDTO.Censorship,
                gameDTO.Price,
                gameDTO.DateRelease,
                null
            )
            {
                Id = Guid.NewGuid()
            };

            _gameRepositoryMock.Setup(r => r.GetByName(gameDTO.Name))
                               .ReturnsAsync(existingGame);

            // Act
            var result = await _gameAppService.UpdateGame(gameDTO);

            // Assert
            result.Should().NotBeNull();
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.ErrorMessage.Contains("Já existe um jogo com este nome"));
        }
    }
}
