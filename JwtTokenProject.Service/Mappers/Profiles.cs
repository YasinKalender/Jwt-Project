using AutoMapper;
using JwtTokenProject.Common.DTOs;
using JwtTokenProject.Entities;

namespace JwtTokenProject.Service.Mappers
{
    public class Profiles : Profile
    {
        public Profiles()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<UserApp, UserDto>().ReverseMap();
            CreateMap<UserApp,RegisterUserDto>().ReverseMap();
        }
    }
}
