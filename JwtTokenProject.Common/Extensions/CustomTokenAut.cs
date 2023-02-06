using JwtTokenProject.Common.Helpers.SecurityKeyHelpers;
using JwtTokenProject.Common.JWT;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;

namespace JwtTokenProject.Common.Extensions
{
    public static class CustomTokenAut
    {
        public static void AddCustomToken(this IServiceCollection services, CustomTokenOptions tokenOptions)
        {
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

                //Yukarıda ki değeri aşağıda ki değeri tanımladık
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt =>
            {

                opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidIssuer = tokenOptions.Issuer,
                    ValidAudience = tokenOptions.Audience[0],
                    IssuerSigningKey = SecurityKeyHelper.GetSecurityKey(tokenOptions.SecurityKey),

                    //doğrulama işlemş
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

            });

        }
    }
}
