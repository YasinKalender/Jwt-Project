using JwtTokenProject.Common.DTOs;
using JwtTokenProject.Common.Responses;
using JwtTokenProject.Entities;
using JwtTokenProject.Service.Interfaces;
using JwtTokenProject.Service.Mappers;
using Microsoft.AspNetCore.Identity;
using System.Data;

namespace JwtTokenProject.Service.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<UserApp> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(UserManager<UserApp> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<Response<string>> CreateRoles(string name)
        {
            await _roleManager.CreateAsync(new IdentityRole() { Name = name });

            return Response<string>.Success(200);
        }

        public async Task<Response<UserDto>> GetUser(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            return Response<UserDto>.Success(ObjectMapper.Mapper.Map<UserDto>(user), 200);
        }

        public Task<Response<UserDto>> LoginUser(LoginDto loginDto)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<UserDto>> RegisterUser(RegisterUserDto registerUserDto)
        {


            var user = new UserApp()
            {
                Email = registerUserDto.Email,
                FirstName = registerUserDto.FirstName,
                LastName = registerUserDto.LastName,
                UserName = registerUserDto.UserName,

            };
            var result = await _userManager.CreateAsync(user, registerUserDto.Password);
            //await AddRoles(user);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(i => i.Description).ToList();
            }
            return Response<UserDto>.Success(ObjectMapper.Mapper.Map<UserDto>(user), 200);


        }

        private async Task<bool> AddRoles(UserApp user)
        {
            var users = await _userManager.FindByEmailAsync(user.Email);

            var roles = await _roleManager.GetRoleNameAsync(new IdentityRole() { Name = "Admin" });

            await _userManager.AddToRoleAsync(users, roles);

            return true;

        }


    }
}
