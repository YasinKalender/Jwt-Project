using JwtTokenProject.Common.DTOs;
using JwtTokenProject.Common.Exceptions;
using JwtTokenProject.Common.Responses;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace JwtTokenProject.Common.Extensions
{
    public static class CustomExceptionHandler
    {
        public static void UseCustomExceptionHandler(this IApplicationBuilder builder)
        {
            builder.UseExceptionHandler(opt =>
            {
                opt.Run(async context =>
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";

                    var errorFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (errorFeature != null)
                    {
                        var ex = errorFeature.Error;

                        ErrorDto errorDto = null;

                        if (ex is CustomException)
                        {
                            errorDto = new(ex.Message, true);
                        }

                        else
                        {
                            errorDto = new ErrorDto(ex.Message, false);
                        }

                        var response = Response<ErrorDto>.Error(500, errorDto);

                        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                    }
                });

            });

        }
    }
}
