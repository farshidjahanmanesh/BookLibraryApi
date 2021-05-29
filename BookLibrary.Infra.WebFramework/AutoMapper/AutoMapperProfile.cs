using AutoMapper;
using BookLibrary.Application.Services;
using BookLibrary.Domain.Domains.Authors;
using BookLibrary.Domain.Domains.Books;
using BookLibrary.Domain.Dtos.Author;
using BookLibrary.Domain.Dtos.Book;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BookLibrary.Application.Services.OpenLibraryApiSearchTemplate;

namespace BookLibrary.Infra.WebFramework.AutoMapper
{

    public class AutoMapperProfile : Profile
    {
       
        public AutoMapperProfile()
        {
            CreateMap<InsertBookDto, Book>();
            CreateMap<InsertAuthorDto, Author>();
            CreateMap<Book, BookItemDto>();
            CreateMap<Author, AuthorItemDto>();

            CreateMap<OpenLibraryApiBookDetail, InsertBookDto>()
                .ForMember(c => c.Description, o => o.MapFrom(y => y.Description.Value));

            CreateMap<OpenLibraryApiBook, InsertBookDto>()
                .ForMember(c => c.FirstPublishYear, o => o.MapFrom(y => y.first_publish_year))
                .ForMember(c => c.OpenLibraryId, o => o.MapFrom(y => y.Key))
                .ForMember(c => c.Title, o => o.MapFrom(y => y.Title));

            CreateMap<OpenLibraryApiAuthorDetail, InsertAuthorDto>()
                .ForMember(c=>c.Birthday,o=>o.MapFrom(y=>y.birth_date))
                .ForMember(c=>c.Name,o=>o.MapFrom(y=>y.Fuller_name))
                .ForMember(c=>c.OpenLibraryId,o=>o.MapFrom(y=>y.Key))
                .ForMember(c=>c.WikipediaUrl,o=>o.MapFrom(y=>y.WikiPedia))
                .ForMember(c=>c.Description,o=>o.MapFrom(y=>y.Bio.Value));
        }
    }
}
