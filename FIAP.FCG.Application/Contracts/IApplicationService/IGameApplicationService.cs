using FIAP.FCG.Application.DTOs;
using FIAP.FCG.Application.Implementations;
using FIAP.FCG.Domain.Entities;
using FluentValidation.Results;

namespace FIAP.FCG.Application.Contracts.IApplicationService
{
    public interface IGameApplicationService
    {
        Task<ValidationResultDTO<Game>> RegisterGame(GameDTO gameDTO);
        Task<GameDTO> GetById(Guid id);
        Task<IEnumerable<GameDTO>> GetAll();
        Task<ValidationResult> UpdateGame(GameDTO game);
    }
}
