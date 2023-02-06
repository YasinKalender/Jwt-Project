using JwtTokenProject.Common.DTOs;
using System.Text.Json.Serialization;

namespace JwtTokenProject.Common.Responses
{
    public class Response<T> where T : class
    {
        public T Data { get; private set; }
        public int StatusCode { get; private set; }
        public ErrorDto Errors { get; private set; }

        [JsonIgnore]
        public bool IsSuccessful { get; private set; }

        public static Response<T> Success(T data, int statusCode)
        {
            return new Response<T> { Data = data, StatusCode = statusCode,IsSuccessful=true };
        }
        public static Response<T> Success(int statusCode)
        {
            return new Response<T> { StatusCode = statusCode, IsSuccessful = true };
        }
        public static Response<T> Error(int statusCode, ErrorDto errorDto)
        {
            return new Response<T> { StatusCode = statusCode, Errors = errorDto, IsSuccessful = false };
        }
        public static Response<T> Error(string errorMesaage,int statusCodes,bool isShow)
        {
            var errorDto = new ErrorDto(errorMesaage,isShow);

            return new Response<T> { Errors = errorDto, StatusCode = statusCodes, IsSuccessful = false };
        }
    }
}
