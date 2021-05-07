using BookLibrary.Domain.Dtos.Book;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BookLibrary.Domain.Interfaces.WriteRepositories.Book
{
    public interface IWriteBookRepository
    {
        Task InsertBook(InsertBookDto model);
        public Task<int> SaveChanges(CancellationToken token = default);
    }
}
