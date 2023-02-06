using JwtTokenProject.Common.DTOs;
using JwtTokenProject.Common.DTOs.Model;
using JwtTokenProject.Common.Responses;
using JwtTokenProject.DAL.Repositories;
using JwtTokenProject.DAL.Uow;
using JwtTokenProject.Entities;
using JwtTokenProject.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace JwtTokenProject.Service.Services
{
    public class AutService : IAutService
    {
        private readonly List<Client> _clients;
        private readonly ITokenService _tokenService;
        private readonly UserManager<UserApp> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<UserRefreshToken> _userRefreshTokenService;
        private readonly IOptions<List<Client>> _options;

        public AutService(ITokenService tokenService, UserManager<UserApp> userManager, IUnitOfWork unitOfWork, IRepository<UserRefreshToken> userRefreshTokenService, IOptions<List<Client>> options)
        {
            _clients = options.Value;
            _tokenService = tokenService;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _userRefreshTokenService = userRefreshTokenService;
        }


        public async Task<Response<ClientTokenDto>> CreateClientTokenAsync(ClientLoginDto clientLoginDto)
        {
            var client = _clients.SingleOrDefault(i => i.ClientId == clientLoginDto.ClientId && i.Secret == clientLoginDto.Secret);
            if (client == null)
                return Response<ClientTokenDto>.Error("", StatusCodes.Status400BadRequest, false);

            var token = _tokenService.CreateClientToken(client);

            return Response<ClientTokenDto>.Success(token, StatusCodes.Status200OK);
        }

        public async Task<Response<TokenDto>> CreateTokenAsync(LoginDto loginDto)
        {
            if (loginDto == null)
                return Response<TokenDto>.Error("", StatusCodes.Status400BadRequest, false);

            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null)
                return Response<TokenDto>.Error("", StatusCodes.Status400BadRequest, false);

            if (!await _userManager.CheckPasswordAsync(user, loginDto.Password))
                return Response<TokenDto>.Error("", StatusCodes.Status400BadRequest, false);

            var token =await _tokenService.CreateToken(user);

            var userRefReshToken = _userRefreshTokenService.Where(i => i.UserId == user.Id).FirstOrDefault();

            if (userRefReshToken == null)
                await _userRefreshTokenService.AddAsync(new UserRefreshToken() { UserId = user.Id, RefreshToken = token.RefreshToken, Expration = token.ExprationTime });
            else
            {
                userRefReshToken.RefreshToken = token.RefreshToken;
                userRefReshToken.Expration = token.ExprationTime;
            }

            _unitOfWork.SaveChanges();

            return Response<TokenDto>.Success(token, StatusCodes.Status200OK);
        }

        public async Task<Response<TokenDto>> CreateTokenByRefreshToken(string refreshToken)
        {
            var refreshTokenExist = _userRefreshTokenService.Where(i => i.RefreshToken == refreshToken).FirstOrDefault();

            var user = await _userManager.FindByIdAsync(refreshTokenExist.UserId);

            var tokenDto =await _tokenService.CreateToken(user);

            refreshTokenExist.RefreshToken = tokenDto.RefreshToken;
            refreshTokenExist.Expration = tokenDto.ExprationTime;

            _unitOfWork.SaveChanges();

            return Response<TokenDto>.Success(tokenDto, StatusCodes.Status200OK);

        }

        public async Task<bool> DeleteRefreshToken(string refreshToken)
        {
            var refreshTokenExist = _userRefreshTokenService.Where(i => i.RefreshToken == refreshToken).FirstOrDefault();

            _userRefreshTokenService.Remove(refreshTokenExist);
            _unitOfWork.SaveChanges();

            return true;



        }
    }
}
