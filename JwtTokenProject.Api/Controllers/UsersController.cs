using JwtTokenProject.Common.DTOs;
using JwtTokenProject.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtTokenProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Users(RegisterUserDto registerUserDto)
        {
            var createRole = await _userService.RegisterUser(registerUserDto);
            return Ok(true);
        }

        [HttpGet]
        [Authorize(Policy = "AgePolicy")]
        [Authorize(Roles = "Admin", Policy = "KalenderPolicy")]
        public async Task<IActionResult> GetUser()
        {
            var data = await _userService.GetUser(HttpContext.User.Identity.Name);
            return Ok(data);
        }

        [HttpPost("CrateRole")]
        public async Task<IActionResult> CrateRole(string name)
        {
            var data = await _userService.CreateRoles(name);
            return Ok(data);
        }

    }
}
