using JwtTokenProject.Common.DTOs;
using JwtTokenProject.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JwtTokenProject.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AutController : ControllerBase
    {
        private readonly IAutService _autService;

        public AutController(IAutService autService)
        {
            _autService = autService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateToken(LoginDto loginDto)
        {
            var result = await _autService.CreateTokenAsync(loginDto);

            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTokenClient(ClientLoginDto loginDto)
        {
            var result = await _autService.CreateClientTokenAsync(loginDto);

            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRefreshToken(string refreshToken)
        {
            var result = await _autService.CreateTokenByRefreshToken(refreshToken);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveRefreshToken(string refreshToken)
        {
            var result = await _autService.DeleteRefreshToken(refreshToken);

            return Ok();
        }

    }
}
