using FIAP.FCG.Application.Contracts.IApplicationService;
using FIAP.FCG.Application.DTOs;
using FIAP.FCG.Domain.Contracts.IRepositories;
using FIAP.FCG.Domain.Entities;

namespace FIAP.FCG.Application.Implementations
{
    public class UserLibraryApplicationService : ApplicationServiceBase, IUserLibraryApplicationService
    {
        private readonly IUserLibraryRepository _userLibraryRepository;

        public UserLibraryApplicationService(IUserLibraryRepository userLibraryRepository)
        {
            _userLibraryRepository = userLibraryRepository;
        }

        public async Task<ValidationResultDTO<UserLibrary>> AddToLibrary(UserLibraryDTO userLibraryDTO)
        {
            UserLibrary userLibrary = await FindLibraryEntryForUser(userLibraryDTO.UserProfileId!, userLibraryDTO.GameId);

            if (userLibrary != null)
                AddValidationError("Jogo já cadastrado.", "Já existe um jogo cadastrado em sua biblioteca");

            userLibrary = new UserLibrary(
                userLibraryDTO.UserProfileId!,
                userLibraryDTO.GameId!);

            await _userLibraryRepository.Add(userLibrary);

            return CustomValidationDataResponse<UserLibrary>(userLibrary);
        }

        public async Task<UserLibrary> FindLibraryEntryForUser(Guid userProfileId, Guid gameId)
        {
            return await _userLibraryRepository.FindLibraryEntryForUser(userProfileId, gameId);
        }
    }
}
