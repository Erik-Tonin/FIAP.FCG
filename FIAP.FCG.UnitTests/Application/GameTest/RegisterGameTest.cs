using FIAP.FCG.Application.DTOs;
using FIAP.FCG.Application.Implementations;
using FIAP.FCG.Domain.Contracts.IRepositories;
using FIAP.FCG.Domain.Entities;
using FluentAssertions;
using Moq;
using Xunit;

namespace FIAP.FCG.UnitTests.Application.GameTest
{
    [Collection(nameof(RegisterGameTest))]
    public class RegisterGameTest
    {
        private readonly Mock<IGameRepository> _gameRepositoryMock;
        private readonly GameApplicationService _gameAppService;

        public RegisterGameTest()
        {
            _gameRepositoryMock = new Mock<IGameRepository>();
            _gameAppService = new GameApplicationService(_gameRepositoryMock.Object); ;
        }

        [Fact(DisplayName = nameof(ShouldRegisterGameWhenGameIsValidAndDoesNotExist))]
        [Trait("Application", "RegisterGame - Use Cases")]
        public async Task ShouldRegisterGameWhenGameIsValidAndDoesNotExist()
        {
            // Arrange
            var gameDTO = new GameDTO
            {
                Name = "Hades",
                Category = Guid.NewGuid(),
                Censorship = 16,
                Price = 79.90f,
                DateRelease = new DateTime(2020, 09, 17)
            };

            _gameRepositoryMock.Setup(r => r.GetByName(gameDTO.Name!))
                               .ReturnsAsync((Game)null);

            _gameRepositoryMock.Setup(r => r.Add(It.IsAny<Game>()))
                               .Returns(Task.CompletedTask);

            // Act
            var result = await _gameAppService.RegisterGame(gameDTO);

            // Assert
            result.Should().NotBeNull();
            result.IsValid.Should().BeTrue();
        }

        [Fact(DisplayName = nameof(ShouldReturnValidationErrorWhenGameAlreadyExists))]
        [Trait("Application", "RegisterGame - Use Cases")]
        public async Task ShouldReturnValidationErrorWhenGameAlreadyExists()
        {
            // Arrange
            var gameDTO = new GameDTO
            {
                Name = "Hades",
                Category = Guid.NewGuid(),
                Censorship = 16,
                Price = 79.90f,
                DateRelease = new DateTime(2020, 09, 17)
            };

            var existingGame = new Game(
                gameDTO.Name!,
                gameDTO.Category!,
                gameDTO.Censorship!,
                gameDTO.Price!,
                gameDTO.DateRelease,
                gameDTO.ImageURL);

            _gameRepositoryMock.Setup(r => r.GetByName(gameDTO.Name!))
                               .ReturnsAsync(existingGame);

            // Act
            var result = await _gameAppService.RegisterGame(gameDTO);

            // Assert
            result.Should().NotBeNull();
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.ErrorMessage.Contains("Já existe um jogo com este nome"));
        }
    }
}
