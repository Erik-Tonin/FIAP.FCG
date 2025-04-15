using FIAP.FCG.Application.Contracts.IApplicationService;
using FIAP.FCG.Application.Contracts.IRepositories;
using FIAP.FCG.Application.DTOs;
using FIAP.FCG.Domain.Entities;

namespace FIAP.FCG.Application.Implementations
{
    public class UserProfileApplicationService : IUserProfileApplicationService
    {
        private readonly IUserProfileRepository _userProfileRepository;
        public UserProfileApplicationService(IUserProfileRepository userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
        }
        public async Task<UserProfileDTO> GetById(Guid id)
        {
            UserProfile user = await _userProfileRepository.GetById(id);

            return new UserProfileDTO()
            {
                Id = id,
                Name = user.Name,
                Email = user.Email,
                CPF = user.Cpf,
                Birthday = user.Birthday
            };
        }
    }
}
