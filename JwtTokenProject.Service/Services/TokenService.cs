using JwtTokenProject.Common.DTOs;
using JwtTokenProject.Common.DTOs.Model;
using JwtTokenProject.Common.Helpers.SecurityKeyHelpers;
using JwtTokenProject.Common.JWT;
using JwtTokenProject.Entities;
using JwtTokenProject.Service.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace JwtTokenProject.Service.Services
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<UserApp> _userManager;
        private readonly CustomTokenOptions _tokenOptions;

        public TokenService(UserManager<UserApp> userManager, IOptions<CustomTokenOptions> options)
        {
            _userManager = userManager;
            _tokenOptions = options.Value;
        }
        public ClientTokenDto CreateClientToken(Client client)
        {
            var accessTokenExpration = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration);

            var securtiyKey = SecurityKeyHelper.GetSecurityKey(_tokenOptions.SecurityKey);

            SigningCredentials signingCredentials = new SigningCredentials(securtiyKey, SecurityAlgorithms.HmacSha256Signature);

            JwtSecurityToken jwtSecurityToken = new(
                issuer: _tokenOptions.Issuer,
                expires: accessTokenExpration,
                notBefore: DateTime.Now,
                claims: GetClaimsByClient(client),
                signingCredentials: signingCredentials
                );

            var handler = new JwtSecurityTokenHandler();

            var token = handler.WriteToken(jwtSecurityToken);

            var tokenDto = new ClientTokenDto()
            {
                AccessToken = token,
                ExprationTime = accessTokenExpration,

            };

            return tokenDto;
        }

        public async Task<TokenDto> CreateToken(UserApp userApp)
        {
            var accessTokenExpration = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration);
            var refreshTokenExpration = DateTime.Now.AddMinutes(_tokenOptions.RefreshTokenExpration);

            var securtiyKey = SecurityKeyHelper.GetSecurityKey(_tokenOptions.SecurityKey);

            SigningCredentials signingCredentials = new SigningCredentials(securtiyKey, SecurityAlgorithms.HmacSha256Signature);

            JwtSecurityToken jwtSecurityToken = new(
                issuer: _tokenOptions.Issuer,
                expires: accessTokenExpration,
                notBefore: DateTime.Now,
                claims: await GetClaims(userApp, _tokenOptions.Audience),
                signingCredentials: signingCredentials
                );

            var handler = new JwtSecurityTokenHandler();

            var token = handler.WriteToken(jwtSecurityToken);

            var tokenDto = new TokenDto()
            {
                AccessToken = token,
                RefreshToken = CreateRefreshToken(),
                ExprationTime = accessTokenExpration,
                RefreshTokenExprationTime = refreshTokenExpration

            };

            return tokenDto;
        }

        private string CreateRefreshToken()
        {
            var numberByte = new Byte[32];

            using var rnd = RandomNumberGenerator.Create();

            rnd.GetBytes(numberByte);

            return Convert.ToBase64String(numberByte);

            //return Guid.NewGuid().ToString();
        }

        private async Task<IEnumerable<Claim>> GetClaims(UserApp userApp, List<string> audiences)
        {
            var roles = await _userManager.GetRolesAsync(userApp);

            var userList = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,userApp.Id),
                new Claim(ClaimTypes.Name,userApp.FirstName),
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Email,userApp.Email),
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim("LastName",userApp.LastName),
                new Claim("BirthDate",userApp.BirthDate.ToString())
            };

            userList.AddRange(audiences.Select(i => new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Aud, i)));
            userList.AddRange(roles.Select(i => new Claim(ClaimTypes.Role, i)));

            return userList;
        }

        private IEnumerable<Claim> GetClaimsByClient(Client client)
        {
            var claims = new List<Claim>();

            new Claim(JwtRegisteredClaimNames.Sub, client.ClientId);
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString());

            claims.AddRange(client.Audiences.Select(i => new Claim(JwtRegisteredClaimNames.Aud, i)));

            return claims;

        }

    }
}
