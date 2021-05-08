using AutoMapper;
using BookLibrary.Api.Dtos;
using BookLibrary.Application.UseCases.Commands.CreateBookCommand;
using BookLibrary.Application.UseCases.Queries.GetBookByIdQuery;
using BookLibrary.Application.UseCases.Queries.GetListOfBooksQuery;
using BookLibrary.Domain.Dtos.Author;
using BookLibrary.Domain.Dtos.Book;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookLibrary.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public BookController(IMediator mediator, IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult> GetListOfBook(BookSearchInputsDto inputsDto)
        {
            var listOfBookQuery = mapper.Map<GetListOfBooksQuery>(inputsDto);
            var bookListDto = await mediator.Send(listOfBookQuery);
            return Ok(bookListDto);
        }

        [HttpGet("{bookId}")]
        public async Task<ActionResult> GetBookById(int bookId)
        {
            if (bookId <= 0)
                return BadRequest();
            var bookItemDto = await mediator.Send(new GetBookByIdQuery(bookId));
            return Ok(bookItemDto);
        }

        [HttpPost]
        public async Task<ActionResult> InsertBook(InsertBookDto createBook)
        {
            await mediator.Send(new CreateBookCommand(createBook));
            return Ok();
        }
    }
}
