using FIAP.FCG.Domain.Entities;

namespace FIAP.FCG.Domain.Contracts.IRepositories
{
    public interface IGameRepository : IRepositoryBase<Game>
    {
        Task<Game> GetByName(string name);
    }
}
