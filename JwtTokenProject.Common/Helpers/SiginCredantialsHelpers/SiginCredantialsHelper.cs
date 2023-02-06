using Microsoft.IdentityModel.Tokens;

namespace JwtTokenProject.Common.Helpers.SiginCredantialsHelpers
{
    public static class SiginCredantialsHelper
    {
        public static SigningCredentials GetSigningCredentials(SecurityKey securityKey)
        {

            return new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
        }
    }
}
