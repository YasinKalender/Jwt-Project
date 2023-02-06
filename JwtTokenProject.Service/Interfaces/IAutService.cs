using JwtTokenProject.Common.DTOs;
using JwtTokenProject.Common.Responses;

namespace JwtTokenProject.Service.Interfaces
{
    public interface IAutService
    {
        Task<Response<TokenDto>> CreateTokenAsync(LoginDto loginDto);
        Task<Response<TokenDto>> CreateTokenByRefreshToken(string refreshToken);
        Task<bool> DeleteRefreshToken(string refreshToken);
        Task<Response<ClientTokenDto>> CreateClientTokenAsync(ClientLoginDto clientLoginDto);
    }
}
