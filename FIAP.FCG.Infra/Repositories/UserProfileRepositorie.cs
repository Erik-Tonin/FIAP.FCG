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

        public Task<UserProfile> GetByEmail(string email)
        {
            return _context.UserProfile.FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}
