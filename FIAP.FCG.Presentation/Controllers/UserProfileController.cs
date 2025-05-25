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
    public class UserProfileController : ApiController
    {
        private readonly IUserProfileApplicationService _userProfileApplicationService;

        public UserProfileController(IUserProfileApplicationService userProfileApplicationService)
        {
            _userProfileApplicationService = userProfileApplicationService;
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<ValidationResultDTO<UserProfile>> Register([FromForm] UserProfileDTO userProfileDTO)
        {
            ValidationResultDTO<UserProfile> user = await _userProfileApplicationService.Register(userProfileDTO);

            return user;
        }

        [AllowAnonymous]
        [HttpPost("RegisterInKeyCloak")]
        public async Task<bool> RegisterInKeyCloak([FromForm] UserProfileDTO userProfileDTO)
        {
            bool user = await _userProfileApplicationService.RegisterInKeyCloak(userProfileDTO);

            return user;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetById")]
        public async Task<UserProfileDTO> GetById(Guid id)
        {
            return await _userProfileApplicationService.GetById(id);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAll")]
        public async Task<IEnumerable<UserProfileDTO>> GetAll()
        {
            return await _userProfileApplicationService.GetAll();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser(UserProfileDTO user)
        {
            return CustomResponse(await _userProfileApplicationService.UpdateUser(user));
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(string email, string password)
        {
            return CustomResponse(await _userProfileApplicationService.Login(email, password));
        }

        [AllowAnonymous]
        [HttpPost("GenerateToken")]
        public async Task<string> GenerateToken(string email, string password)
        {
            return await _userProfileApplicationService.GenerateToken(email, password);
        }
    }
}
