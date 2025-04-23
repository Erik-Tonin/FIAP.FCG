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
    public class UserLibraryController : ApiController
    {
        private readonly IUserLibraryApplicationService _userLibraryApplicationService;

        public UserLibraryController(IUserLibraryApplicationService userLibraryApplicationService)
        {
            _userLibraryApplicationService = userLibraryApplicationService;
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPost("AddToLibrary")]
        public async Task<ValidationResultDTO<UserLibrary>> AddToLibrary([FromForm] UserLibraryDTO userLibraryDTO)
        {
            ValidationResultDTO<UserLibrary> user = await _userLibraryApplicationService.AddToLibrary(userLibraryDTO);

            return user;
        }

        [Authorize(Roles = "User,Admin")]
        [HttpGet("GetByUserProfileId")]
        public async Task<IEnumerable<UserLibraryDTO>> GetByUserProfileId(Guid userProfileId)
        {
            return await _userLibraryApplicationService.GetByUserProfileId(userProfileId);
        }
    }
}
