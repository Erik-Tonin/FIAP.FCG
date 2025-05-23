﻿using FIAP.FCG.Application.Implementations;
using FIAP.FCG.Domain.Contracts.IRepositories;
using FIAP.FCG.Domain.Entities;
using FluentAssertions;
using Moq;
using Xunit;

namespace FIAP.FCG.UnitTests.Application.GameTest
{
    [Collection(nameof(GetAllGamesTest))]
    public class GetAllGamesTest
    {
        private readonly Mock<IGameRepository> _gameRepositoryMock;
        private readonly GameApplicationService _gameAppService;

        public GetAllGamesTest()
        {
            _gameRepositoryMock = new Mock<IGameRepository>();
            _gameAppService = new GameApplicationService(_gameRepositoryMock.Object);
        }

        [Fact(DisplayName = nameof(ShouldReturnAllGamesWhenGamesExist))]
        [Trait("Application", "GetAllGames - Use Cases")]
        public async Task ShouldReturnAllGamesWhenGamesExist()
        {
            // Arrange
            List<Game> games = new List<Game>
            {
                new Game("God of War", Guid.NewGuid(), 18, 150.0f, DateTime.Parse("10/09/1995"), null),
                new Game("The Last of Us", Guid.NewGuid(), 16, 60.0f, DateTime.Parse("14/06/2013"), null)
            };

            _gameRepositoryMock.Setup(x => x.GetAll())
                               .Returns(games);

            // Act
            var result = await _gameAppService.GetAll();

            // Assert
            Assert.NotEmpty(result);
            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
            result.Should().OnlyContain(game => !string.IsNullOrEmpty(game.Name));
            result.Should().OnlyContain(game => game.Id != Guid.Empty);
            result.Should().OnlyContain(game => game.Censorship >= 0);
            result.Should().OnlyContain(game => game.Price > 0);
            result.Should().OnlyContain(game => game.DateRelease <= DateTime.Now);
        }

        [Fact(DisplayName = nameof(ShouldReturnEmptyListWhenNoGamesExist))]
        [Trait("Application", "GetAllGames - Use Cases")]
        public async Task ShouldReturnEmptyListWhenNoGamesExist()
        {
            // Arrange
            var games = new List<Game>();

            _gameRepositoryMock.Setup(x => x.GetAll())
                               .Returns(games);

            // Act
            var result = await _gameAppService.GetAll();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }
    }
}
