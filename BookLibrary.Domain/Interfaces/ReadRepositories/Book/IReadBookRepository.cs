using BookLibrary.Domain.Dtos.Book;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLibrary.Domain.Interfaces.ReadRepositories.Book
{
    public interface IReadBookRepository
    {
        Task<BookListDto> GetBooksAsync(string name = "", int count = 1);
        Task<BookItemDto> GetBookByAsync(int id);
    }
}
