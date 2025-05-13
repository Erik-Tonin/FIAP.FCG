using FIAP.FCG.Application.DTOs;
using FIAP.FCG.Application.Implementations;
using FIAP.FCG.Domain.Contracts.IRepositories;
using FIAP.FCG.Domain.Entities;
using Moq;
using Xunit;

namespace FIAP.FCG.UnitTests.Application.UserLibraryTest
{
    public class AddToLibraryTest
    {
        private readonly Mock<IUserLibraryRepository> _userLibraryRepositoryMock;
        private readonly UserLibraryApplicationService _service;

        public AddToLibraryTest()
        {
            _userLibraryRepositoryMock = new Mock<IUserLibraryRepository>();
            _service = new UserLibraryApplicationService(_userLibraryRepositoryMock.Object);
        }

        [Fact]
        public async Task AddToLibrary_WhenGameAlreadyExists_ThrowsException()
        {
            // Arrange
            var userProfileId = Guid.NewGuid();
            var gameId = Guid.NewGuid();

            var existingEntry = new UserLibrary(userProfileId, gameId);

            _userLibraryRepositoryMock
                .Setup(repo => repo.FindLibraryEntryForUser(userProfileId, gameId))
                .ReturnsAsync(existingEntry);

            var dto = new UserLibraryDTO
            {
                UserProfileId = userProfileId,
                GameId = gameId
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.AddToLibrary(dto));
            Assert.Equal("Já existe um jogo cadastrado em sua biblioteca", exception.Message);

            _userLibraryRepositoryMock.Verify(repo => repo.Add(It.IsAny<UserLibrary>()), Times.Never);
        }

        [Fact]
        public async Task AddToLibrary_WhenGameDoesNotExist_AddsSuccessfully()
        {
            // Arrange
            var userProfileId = Guid.NewGuid();
            var gameId = Guid.NewGuid();

            _userLibraryRepositoryMock
                .Setup(repo => repo.FindLibraryEntryForUser(userProfileId, gameId))
                .ReturnsAsync((UserLibrary)null);

            var dto = new UserLibraryDTO
            {
                UserProfileId = userProfileId,
                GameId = gameId
            };

            // Act
            var result = await _service.AddToLibrary(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.ValidationProblemDetails);
            Assert.NotNull(result.Response);
            Assert.Equal(userProfileId, result.Response.UserProfileId);
            Assert.Equal(gameId, result.Response.GameId);

            _userLibraryRepositoryMock.Verify(repo => repo.Add(It.IsAny<UserLibrary>()), Times.Once);
        }
    }
}
