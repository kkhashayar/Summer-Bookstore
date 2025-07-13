using AutoMapper;
using Summer_Bookstore.DTOs;
using Summer_Bookstore_Domain.Entities;

namespace Summer_Bookstore.Mappers;

public class UserRegisterMappers
{
    public class UserMappers : Profile
    {
        public UserMappers()
        {
            CreateMap<UserRegisterDto, User>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()))
                .ReverseMap();
        }
    }
}
