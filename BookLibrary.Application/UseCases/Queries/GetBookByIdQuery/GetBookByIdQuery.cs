using BookLibrary.Domain.Dtos.Book;
using BookLibrary.Domain.Interfaces.ReadRepositories.Book;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BookLibrary.Application.UseCases.Queries.GetBookByIdQuery
{
    public record GetBookByIdQuery(int bookId) : IRequest<BookItemDto>;

    public class GetBookByIdQueryHandler : IRequestHandler<GetBookByIdQuery, BookItemDto>
    {
        private readonly IReadBookRepository readRepository;

        public GetBookByIdQueryHandler(IReadBookRepository readRepository)
        {
            this.readRepository = readRepository;
        }
        public async Task<BookItemDto> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
        {
            var bookItemDto = await readRepository.GetBookByAsync(request.bookId);
            return bookItemDto;
        }
    }
}
