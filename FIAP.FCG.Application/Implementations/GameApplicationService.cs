using FIAP.FCG.Application.Contracts.IApplicationService;
using FIAP.FCG.Application.DTOs;
using FIAP.FCG.Domain.Contracts.IRepositories;
using FIAP.FCG.Domain.Entities;

namespace FIAP.FCG.Application.Implementations
{
    public class GameApplicationService : ApplicationServiceBase, IGameApplicationService
    {
        private readonly IGameRepository _gameRepository;

        public GameApplicationService(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public async Task<ValidationResultDTO<Game>> RegisterGame(GameDTO gameDTO)
        {
            Game game = await GetByName(gameDTO.Name!);

            if (game != null)
                AddValidationError("Jogo já cadastrado.", "Já existe um jogo com este nome");

            game = new Game(
               gameDTO.Name!,
               gameDTO.Category!,
               gameDTO.Censorship!,
               gameDTO.Price!,
               gameDTO.DateRelease,
               gameDTO.ImageURL);

            await _gameRepository.Add(game);

            return CustomValidationDataResponse<Game>(game);
        }

        public async Task<Game> GetByName(string name)
        {
            return await _gameRepository.GetByName(name);
        }
    }
}
