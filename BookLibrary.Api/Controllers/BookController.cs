using BookLibrary.Application.UseCases.Commands.CreateBookCommand;
using BookLibrary.Domain.Dtos.Author;
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

        public BookController(IMediator mediator)
        {
            this.mediator = mediator;
        }
       
        [HttpPost]
        public async Task<ActionResult> InsertBook(CreateBookCommand createBook)
        {
            await mediator.Send(createBook);
            return Ok();
        }
    }
}
