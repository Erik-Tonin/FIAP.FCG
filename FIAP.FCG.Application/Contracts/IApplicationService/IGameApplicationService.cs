using FIAP.FCG.Application.DTOs;
using FIAP.FCG.Application.Implementations;
using FIAP.FCG.Domain.Entities;

namespace FIAP.FCG.Application.Contracts.IApplicationService
{
    public interface IGameApplicationService
    {
        Task<ValidationResultDTO<Game>> RegisterGame(GameDTO gameDTO);
    }
}
