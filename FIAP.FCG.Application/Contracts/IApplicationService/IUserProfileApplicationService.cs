using FIAP.FCG.Application.DTOs;
using FIAP.FCG.Application.Implementations;
using FIAP.FCG.Domain.Entities;
using FluentValidation.Results;

namespace FIAP.FCG.Application.Contracts.IApplicationService
{
    public interface IUserProfileApplicationService
    {
        Task<ValidationResultDTO<UserProfile>> Register(UserProfileDTO userProfileDTO);
        Task<bool> RegisterInKeyCloak(UserProfileDTO userProfileDTO);
        Task<UserProfileDTO> GetById(Guid id);
        Task<IEnumerable<UserProfileDTO>> GetAll();
        Task<ValidationResult> UpdateUser(UserProfileDTO user);
        Task<ValidationResult> Login(string email, string password);
        Task<string> GenerateToken(string email, string password);
    }
}
