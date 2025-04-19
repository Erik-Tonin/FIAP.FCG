using FIAP.FCG.Domain.Entities;

namespace FIAP.FCG.Domain.Contracts.IRepositories
{
    public interface IUserLibraryRepository : IRepositoryBase<UserLibrary>
    {
        Task<UserLibrary> FindLibraryEntryForUser(Guid userProfileId, Guid gameId);
    }
}
