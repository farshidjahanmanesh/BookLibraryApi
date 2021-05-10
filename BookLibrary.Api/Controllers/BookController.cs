using AutoMapper;
using BookLibrary.Api.Dtos;
using BookLibrary.Application.UseCases.Commands.CreateBookCommand;
using BookLibrary.Application.UseCases.Queries.GetBookByIdQuery;
using BookLibrary.Application.UseCases.Queries.GetListOfBooksQuery;
using BookLibrary.Domain.Dtos.Author;
using BookLibrary.Domain.Dtos.Book;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
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
        private readonly ILogger<BookController> logger;

        public BookController(IMediator mediator, IMapper mapper, ILogger<BookController> logger)
        {
            this.mediator = mediator;
            this.mapper = mapper;
            this.logger = logger;
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

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult> InsertBook(InsertBookDto createBook)
        {
            await mediator.Send(new CreateBookCommand(createBook));
            return Ok();
        }

    }
}
