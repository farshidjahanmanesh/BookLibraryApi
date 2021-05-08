using BookLibrary.Domain.Dtos.Book;
using BookLibrary.Domain.Interfaces.ReadRepositories.Book;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BookLibrary.Application.UseCases.Queries.GetListOfBooksQuery
{
    public record GetListOfBooksQuery(string Name, int Count) : IRequest<BookListDto>;

    public class GetListOfBooksQueryHandler : IRequestHandler<GetListOfBooksQuery, BookListDto>
    {

        private readonly IReadBookRepository readRepository;

        public GetListOfBooksQueryHandler(IReadBookRepository readRepository)
        {
            this.readRepository = readRepository;
        }
        public async Task<BookListDto> Handle(GetListOfBooksQuery request, CancellationToken cancellationToken)
        {
            var bookList = await readRepository.GetBooksAsync(request.Name, request.Count);
            return bookList;
        }
    }
}
