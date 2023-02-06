using JwtTokenProject.Common.DTOs;
using JwtTokenProject.Common.Responses;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace JwtTokenProject.Common.Extensions
{
    public static class CustomValidationResponse 
    {
        public static void AddCustomValidationResponse(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(opt =>
            {
                opt.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState.Values.Where(i=>i.Errors.Count() > 0).SelectMany(x=>x.Errors).Select(k=>k.ErrorMessage);

                    ErrorDto errorDto = new(errors.ToList(),true);

                    var response = Response<ErrorDto>.Error(404, errorDto);

                    return new BadRequestObjectResult(response);

                };

            });

        }

    }
}
