using AutoMapper;
using BookLibrary.Api.Dtos;
using BookLibrary.Application.Services;
using BookLibrary.Application.UseCases.Commands.CreateBookCommand;
using BookLibrary.Application.UseCases.Queries.GetBookByIdQuery;
using BookLibrary.Application.UseCases.Queries.GetBookByNameQuery;
using BookLibrary.Application.UseCases.Queries.GetListOfBooksQuery;
using BookLibrary.Domain.Dtos.Book;
using BookLibrary.Domain.Interfaces.ReadRepositories.Book;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BookLibrary.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private readonly ILogger<BookController> logger;
        private readonly IReadDataFromApi s;

        public BookController(IMediator mediator, IMapper mapper, ILogger<BookController> logger, IReadDataFromApi s,
            Lazy<IReadBookRepository> read)
        {
            this.mediator = mediator;
            this.mapper = mapper;
            this.logger = logger;
            this.s = s;
        }

        //[Authorize]
        [HttpGet]
        public async Task<ActionResult> GetListOfBook(BookSearchInputsDto inputsDto)
        {
            var listOfBookQuery = mapper.Map<GetListOfBooksQuery>(inputsDto);
            var bookListDto = await mediator.Send(listOfBookQuery);
            return Ok(bookListDto);
        }

        //[Authorize]
        [HttpGet("{bookId:int}")]
        public async Task<ActionResult> GetBookById(int bookId)
        {
            if (bookId <= 0)
                return BadRequest();
            var bookItemDto = await mediator.Send(new GetBookByIdQuery(bookId));
            return Ok(bookItemDto);
        }
        [HttpGet("{bookName}")]
        public async Task<ActionResult> GetBookByName(string bookName)
        {
            var bookItem = await mediator.Send(new GetBookByNameQuery(bookName));
            if (bookItem == null)
                return NotFound("This Book Not Found");
            return Ok(bookItem);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> InsertBook(InsertBookDto createBook)
        {
            await mediator.Send(new CreateBookCommand(createBook));
            return Ok();
        }

    }
}
