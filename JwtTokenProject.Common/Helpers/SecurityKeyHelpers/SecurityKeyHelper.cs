using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace JwtTokenProject.Common.Helpers.SecurityKeyHelpers
{
    public static class SecurityKeyHelper
    {
        public static SecurityKey GetSecurityKey(string key)
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        }
    }
}
