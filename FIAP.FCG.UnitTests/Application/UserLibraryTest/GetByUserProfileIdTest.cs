using FIAP.FCG.Application.Implementations;
using FIAP.FCG.Domain.Contracts.IRepositories;
using FIAP.FCG.Domain.Entities;
using Moq;
using Xunit;

namespace FIAP.FCG.UnitTests.Application.UserLibraryTest
{
    public class GetByUserProfileIdTest
    {
        private readonly Mock<IUserLibraryRepository> _userLibraryRepositoryMock;
        private readonly UserLibraryApplicationService _service;

        public GetByUserProfileIdTest()
        {
            _userLibraryRepositoryMock = new Mock<IUserLibraryRepository>();
            _service = new UserLibraryApplicationService(_userLibraryRepositoryMock.Object);
        }

        [Fact]
        public async Task GetByUserProfileId_WhenLibraryExists_ReturnsMappedDTOs()
        {
            // Arrange
            var userProfileId = Guid.NewGuid();
            var gameId = Guid.NewGuid();
            var libraryItems = new List<UserLibrary>
            {
                new UserLibrary(userProfileId, gameId)
            };

            _userLibraryRepositoryMock
                .Setup(repo => repo.GetByUserProfileId(userProfileId))
                .ReturnsAsync(libraryItems);

            // Act
            var result = await _service.GetByUserProfileId(userProfileId);

            // Assert
            Assert.NotNull(result);
            var resultList = result.ToList();
            Assert.Single(resultList);
            Assert.Equal(userProfileId, resultList[0].UserProfileId);
            Assert.Equal(gameId, resultList[0].GameId);
        }

        [Fact]
        public async Task GetByUserProfileId_WhenLibraryIsEmpty_ReturnsEmptyList()
        {
            // Arrange
            var userProfileId = Guid.NewGuid();

            _userLibraryRepositoryMock
                .Setup(repo => repo.GetByUserProfileId(userProfileId))
                .ReturnsAsync(new List<UserLibrary>());

            // Act
            var result = await _service.GetByUserProfileId(userProfileId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
