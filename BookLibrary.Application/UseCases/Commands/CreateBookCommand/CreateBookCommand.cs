using BookLibrary.Domain.Dtos.Book;
using BookLibrary.Domain.Interfaces.WriteRepositories.Book;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BookLibrary.Application.UseCases.Commands.CreateBookCommand
{
    public record CreateBookCommand(InsertBookDto insertBook) :IRequest;

    public class CreateBookCommandHandler : AsyncRequestHandler<CreateBookCommand>
    {
        private readonly IWriteBookRepository writeRepository;

        public CreateBookCommandHandler(IWriteBookRepository writeRepository)
        {
            this.writeRepository = writeRepository;
        }
        protected async override Task Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            await writeRepository.InsertBook(request.insertBook);
            await writeRepository.SaveChanges();
        }
    }
}
