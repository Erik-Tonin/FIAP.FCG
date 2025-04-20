using FIAP.FCG.Application.Contracts.IApplicationService;
using FIAP.FCG.Application.DTOs;
using FIAP.FCG.Domain.Contracts.IRepositories;
using FIAP.FCG.Domain.Entities;
using FluentValidation.Results;

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

        public async Task<GameDTO> GetById(Guid id)
        {
            Game game = await _gameRepository.GetById(id);

            return new GameDTO()
            {
                Id = id,
                Name = game.Name,
                Category = game.Category,
                Censorship = game.Censorship,
                Price = game.Price,
                DateRelease = game.DateRelease
            };
        }

        public async Task<IEnumerable<GameDTO>> GetAll()
        {
            var games = _gameRepository.GetAll();
            return await Task.FromResult(games.Select(x => new GameDTO()
            {
                Id = x.Id,
                Name = x.Name,
                Category = x.Category,
                Censorship = x.Censorship,
                Price = x.Price,
                DateRelease = x.DateRelease
            }));
        }

        public async Task<ValidationResult> UpdateGame(GameDTO gameDTO)
        {
            Game game = await _gameRepository.GetById(gameDTO.Id);

            if (game.IsValid())
            {
                game.UpdateGame(
                    gameDTO.Name!,
                    gameDTO.Category,
                    gameDTO.Censorship,
                    gameDTO.Price,
                    gameDTO.DateRelease,
                    gameDTO.ImageURL);

                _gameRepository.Update(game);
            }
            else
            {
                return game.ValidationResult;
            }

            return game.ValidationResult;
        }

        public async Task<Game> GetByName(string name)
        {
            return await _gameRepository.GetByName(name);
        }
    }
}
