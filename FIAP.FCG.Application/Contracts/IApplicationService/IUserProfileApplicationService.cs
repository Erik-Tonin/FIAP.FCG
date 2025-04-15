using FIAP.FCG.Application.DTOs;

namespace FIAP.FCG.Application.Contracts.IApplicationService
{
    public interface IUserProfileApplicationService
    {
        Task<UserProfileDTO> GetById(Guid id);
    }
}
