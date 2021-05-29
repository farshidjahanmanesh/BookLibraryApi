using BookLibrary.Application.Services;
using BookLibrary.Domain.Dtos.Book;
using BookLibrary.Domain.Interfaces.ReadRepositories.Book;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BookLibrary.Application.UseCases.Queries.GetBookByNameQuery
{
    public record GetBookByNameQuery(string bookName) : IRequest<BookItemDto>;

    public class GetBookByNameQueryHandler : IRequestHandler<GetBookByNameQuery, BookItemDto>
    {
        private readonly IReadBookRepository readBookRepository;
        private readonly IEnumerable<IReadDataFromApi> apis;

        public GetBookByNameQueryHandler(IReadBookRepository readBookRepository,
            IEnumerable<IReadDataFromApi> apis)
        {
            this.readBookRepository = readBookRepository;
            this.apis = apis;
        }
        public async Task<BookItemDto> Handle(GetBookByNameQuery request, CancellationToken cancellationToken)
        {
            var bookItem = await readBookRepository.GetBookByNameAsync(request.bookName);
            if (bookItem == null)
            {
                foreach (var item in apis)
                {
                    var isFindBook = await item.StartToSearchForBookWith(request.bookName);
                    if (isFindBook == true)
                        break;
                }
                bookItem = await readBookRepository.GetBookByNameAsync(request.bookName);
            }
            return bookItem;
        }
    }
}
