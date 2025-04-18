using FIAP.FCG.Domain.Contracts.IRepositories;
using FIAP.FCG.Domain.Entities;
using FIAP.FCG.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace FIAP.FCG.Infra.Repositories
{
    public class UserProfileRepositorie : RepositoryBase<UserProfile>, IUserProfileRepository
    {
        public UserProfileRepositorie(MicroServiceContext context) : base(context)
        {
        }

        public async Task<UserProfile> GetByEmail(string email) => await _context.UserProfile.FirstOrDefaultAsync(x => x.Email == email);
    }
}
