using AutoMapper;
using Summer_Bookstore.DTOs;
using Summer_Bookstore_Domain.Entities;

namespace Summer_Bookstore.Mappers;

public class BookMappers : Profile
{
    public BookMappers()
    {
        CreateMap<BookCreateDto, Book>()
            .ForMember(dest => dest.PublishedDate, opt => opt.MapFrom(src => src.PublishedDate.ToDateTime(TimeOnly.MinValue))) // Convert DateOnly to DateTime
            .ReverseMap()
            .ForMember(dest => dest.PublishedDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.PublishedDate))); // Convert DateTime to DateOnly
    }
}
