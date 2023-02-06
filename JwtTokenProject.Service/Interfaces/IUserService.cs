using JwtTokenProject.Common.DTOs;
using JwtTokenProject.Common.Responses;

namespace JwtTokenProject.Service.Interfaces
{
    public interface IUserService
    {
        public Task<Response<UserDto>> RegisterUser(RegisterUserDto registerUserDto);
        public Task<Response<UserDto>> GetUser(string username);
        public Task<Response<UserDto>> LoginUser(LoginDto loginDto);

        public Task<Response<string>> CreateRoles(string name);
    }
}
