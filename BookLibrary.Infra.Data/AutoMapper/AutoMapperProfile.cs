using AutoMapper;
using BookLibrary.Domain.Domains.Authors;
using BookLibrary.Domain.Domains.Books;
using BookLibrary.Domain.Dtos.Author;
using BookLibrary.Domain.Dtos.Book;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLibrary.Infra.Data.AutoMapper
{
    class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<InsertBookDto, Book>();
            CreateMap<InsertAuthorDto, Author>();
            CreateMap<Book, BookItemDto>();
            CreateMap<Author, AuthorItemDto>();
        }
    }
}
