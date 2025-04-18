using FIAP.FCG.Domain.Contracts.IRepositories;
using FIAP.FCG.Domain.Entities;
using FIAP.FCG.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace FIAP.FCG.Infra.Repositories
{
    public class GameRepositorie : RepositoryBase<Game>, IGameRepository
    {
        public GameRepositorie(MicroServiceContext context) : base(context)
        {
        }

        public async Task<Game> GetByName(string name) => await _context.Game.FirstOrDefaultAsync(x => x.Name   == name);
    }
}
