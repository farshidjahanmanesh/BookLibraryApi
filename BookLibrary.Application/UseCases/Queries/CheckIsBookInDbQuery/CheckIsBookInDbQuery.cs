using BookLibrary.Domain.Interfaces.ReadRepositories.Book;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BookLibrary.Application.UseCases.Queries.CheckIsBookInDbQuery
{
    public record CheckIsBookInDbQuery(string BookName):IRequest<bool>;

    public class CheckIsBookInDbQueryHandler : IRequestHandler<CheckIsBookInDbQuery, bool>
    {
        private readonly IReadBookRepository readBookRepository;

        public CheckIsBookInDbQueryHandler(IReadBookRepository readBookRepository)
        {
            this.readBookRepository = readBookRepository;
        }
        public async Task<bool> Handle(CheckIsBookInDbQuery request, CancellationToken cancellationToken)
        {
            return await readBookRepository.CheckIsBookInDb(request.BookName);
        }
    }
}
