using FIAP.FCG.Application.Contracts.IApplicationService;
using FIAP.FCG.Application.DTOs;
using FIAP.FCG.Application.Implementations;
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

        [Authorize(Roles = "Admin")]
        [HttpPost("RegisterGame")]
        public async Task<IActionResult> RegisterGame([FromForm] GameDTO gameDTO)
        {
            return CustomResponse(await _gameApplicationService.RegisterGame(gameDTO));
        }

        [Authorize(Roles = "User,Admin")]
        [HttpGet("GetById")]
        public async Task<ValidationResultDTO<GameDTO>> GetById(Guid id)
        {
            return await _gameApplicationService.GetById(id);
        }

        [Authorize(Roles = "User,Admin")]
        [HttpGet("GetAll")]
        public async Task<IEnumerable<GameDTO>> GetAll()
        {
            return await _gameApplicationService.GetAll();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateGame")]
        public async Task<IActionResult> UpdateGame(GameDTO game)
        {
            return CustomResponse(await _gameApplicationService.UpdateGame(game));
        }
    }
}
