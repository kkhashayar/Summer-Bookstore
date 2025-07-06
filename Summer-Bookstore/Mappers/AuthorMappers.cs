using AutoMapper;
using Summer_Bookstore.DTOs;
using Summer_Bookstore_Domain.Entities;

namespace Summer_Bookstore.Mappers;

public class AuthorMappers: Profile
{
    public AuthorMappers()
    {
        CreateMap<AuthorCreateDto, Author>().ReverseMap();
    }
}
