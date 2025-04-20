using FIAP.FCG.Domain.Entities;
using System.Threading.Tasks;

namespace FIAP.FCG.Domain.Contracts.IRepositories
{
    public interface IUserLibraryRepository : IRepositoryBase<UserLibrary>
    {
        Task<UserLibrary> FindLibraryEntryForUser(Guid userProfileId, Guid gameId);
        Task<IEnumerable<UserLibrary>> GetByUserProfileId(Guid userProfileId);
    }
}
