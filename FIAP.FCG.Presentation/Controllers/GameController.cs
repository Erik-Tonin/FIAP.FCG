using FIAP.FCG.Application.Contracts.IApplicationService;
using FIAP.FCG.Application.DTOs;
using FIAP.FCG.Application.Implementations;
using FIAP.FCG.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FIAP.FCG.Presentation.Controllers
{
    [Route("api/[controller]")]
    public class GameController : ApiController
    {
        private readonly IGameApplicationService _gameApplicationService;

        public GameController(IGameApplicationService gameApplicationService)
        {
            _gameApplicationService = gameApplicationService;
        }

        [AllowAnonymous]
        [HttpPost("RegisterGame")]
        public async Task<ValidationResultDTO<Game>> RegisterGame([FromForm] GameDTO gameDTO)
        {
            ValidationResultDTO<Game> game = await _gameApplicationService.RegisterGame(gameDTO);

            return game;
        }
    }
}
