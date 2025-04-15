using FIAP.FCG.Application.Contracts.IApplicationService;
using FIAP.FCG.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FIAP.FCG.Presentation.Controllers
{
    [Route("api/[controller]")]
    public class UserProfileController
    {
        private readonly IUserProfileApplicationService _userProfileApplicationService;

        public UserProfileController(IUserProfileApplicationService userProfileApplicationService)
        {
            _userProfileApplicationService = userProfileApplicationService;
        }

        [AllowAnonymous]
        [HttpGet("GetById")]
        public async Task<UserProfileDTO> GetById(Guid id)
        {
            return await _userProfileApplicationService.GetById(id);
        }
    }
}
