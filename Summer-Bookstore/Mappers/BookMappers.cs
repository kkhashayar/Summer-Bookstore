using AutoMapper;
using Summer_Bookstore.DTOs;
using Summer_Bookstore_Domain.Entities;

namespace Summer_Bookstore.Mappers;

public class BookMappers : Profile
{
    public BookMappers()
    {
        // Mapping configuration for Book entity and DTOs

        CreateMap<BookCreateDto, Book>()
            .ForMember(dest => dest.PublishedDate, opt => opt.MapFrom(src => src.PublishedDate.ToDateTime(TimeOnly.MinValue)))
            .ForPath(dest => dest.Author.Name, opt => opt.MapFrom(src => src.AuthorName))

            .ReverseMap()
            .ForMember(dest => dest.PublishedDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.PublishedDate)))
            .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author!.Name));


        CreateMap<BookUpdateDto, Book>()
            .ForMember(dest => dest.PublishedDate,
                       opt => opt.MapFrom(src => src.PublishedDate.ToDateTime(TimeOnly.MinValue)))
            .ForPath(dest => dest.Author.Name,
                     opt => opt.MapFrom(src => src.AuthorName))
            .ReverseMap()
            .ForMember(dest => dest.PublishedDate,
                       opt => opt.MapFrom(src => DateOnly.FromDateTime(src.PublishedDate)))
            .ForMember(dest => dest.AuthorName,
                       opt => opt.MapFrom(src => src.Author!.Name));


        CreateMap<Book, BookReadDto>()
            .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.Name))
            .ForMember(dest => dest.PublishedDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.PublishedDate)));
            
    }
}
