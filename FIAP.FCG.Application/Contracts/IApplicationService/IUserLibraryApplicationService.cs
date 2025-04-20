using FIAP.FCG.Application.DTOs;
using FIAP.FCG.Application.Implementations;
using FIAP.FCG.Domain.Entities;

namespace FIAP.FCG.Application.Contracts.IApplicationService
{
    public interface IUserLibraryApplicationService
    {
        Task<ValidationResultDTO<UserLibrary>> AddToLibrary(UserLibraryDTO userLibraryDTO);
        Task<IEnumerable<UserLibraryDTO>> GetByUserProfileId(Guid userProfileId);
    }
}
