using AutoMapper;
using Summer_Bookstore.DTOs;
using Summer_Bookstore_Domain.Entities;

namespace Summer_Bookstore.Mappers;

public class BookMappers : Profile
{
    public BookMappers()
    {
        CreateMap<BookCreateDto, Book>().ReverseMap();  
    }
}
