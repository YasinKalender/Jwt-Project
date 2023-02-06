using JwtTokenProject.Common.DTOs;
using JwtTokenProject.Common.DTOs.Model;
using JwtTokenProject.Entities;

namespace JwtTokenProject.Service.Interfaces
{
    public interface ITokenService
    {
        Task<TokenDto> CreateToken(UserApp userApp);
        ClientTokenDto CreateClientToken(Client client);
    }
}
