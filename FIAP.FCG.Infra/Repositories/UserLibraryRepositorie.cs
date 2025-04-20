using FIAP.FCG.Domain.Contracts.IRepositories;
using FIAP.FCG.Domain.Entities;
using FIAP.FCG.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace FIAP.FCG.Infra.Repositories
{
    public class UserLibraryRepositorie : RepositoryBase<UserLibrary>, IUserLibraryRepository
    {
        public UserLibraryRepositorie(MicroServiceContext context) : base(context)
        {
        }

        public async Task<UserLibrary> FindLibraryEntryForUser(Guid userProfileId, Guid gameId)
        {
            return await _context.UserLibrary.Where(x => x.UserProfileId == userProfileId && x.GameId == gameId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<UserLibrary>> GetByUserProfileId(Guid userProfileId)
        {
            return await _context.UserLibrary
                .Include(x => x.Game)
                .Where(x => x.UserProfileId == userProfileId)
                .ToListAsync();
        }
    }
}

