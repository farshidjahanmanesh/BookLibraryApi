using AutoMapper;
using BookLibrary.Api.Dtos;
using BookLibrary.Application.UseCases.Commands.CreateBookCommand;
using BookLibrary.Application.UseCases.Queries.GetListOfBooksQuery;
using BookLibrary.Domain.Dtos.Book;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookLibrary.Api.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<BookSearchInputsDto, GetListOfBooksQuery>();
            
        }
    }
}
