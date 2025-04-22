using FIAP.FCG.Application.Contracts.IApplicationService;
using FIAP.FCG.Application.DTOs;
using FIAP.FCG.Application.Implementations;
using FIAP.FCG.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FIAP.FCG.Presentation.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class GameController : ApiController
    {
        private readonly IGameApplicationService _gameApplicationService;

        public GameController(IGameApplicationService gameApplicationService)
        {
            _gameApplicationService = gameApplicationService;
        }

        [Authorize(Roles = "default-roles-fiap-fcg")]
        [HttpPost("RegisterGame")]
        public async Task<ValidationResultDTO<Game>> RegisterGame([FromForm] GameDTO gameDTO)
        {
            ValidationResultDTO<Game> game = await _gameApplicationService.RegisterGame(gameDTO);

            return game;
        }

        [AllowAnonymous]
        [HttpGet("GetById")]
        public async Task<GameDTO> GetById(Guid id)
        {
            return await _gameApplicationService.GetById(id);
        }

        [Authorize(Roles = "User")]
        [HttpGet("GetAll")]
        public async Task<IEnumerable<GameDTO>> GetAll()
        {
            return await _gameApplicationService.GetAll();
        }

        [AllowAnonymous]
        [HttpPut("UpdateGame")]
        public async Task<IActionResult> UpdateGame(GameDTO game)
        {
            return CustomResponse(await _gameApplicationService.UpdateGame(game));
        }
    }
}
