using FIAP.FCG.Domain.Entities;

namespace FIAP.FCG.Domain.Contracts.IRepositories
{
    public interface IUserProfileRepository : IRepositoryBase<UserProfile>
    {
        Task<UserProfile> GetByEmail(string email);
    }
}
