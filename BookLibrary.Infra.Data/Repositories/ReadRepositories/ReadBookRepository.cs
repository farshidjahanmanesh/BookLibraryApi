using AutoMapper;
using BookLibrary.Domain.Domains.Books;
using BookLibrary.Domain.Dtos.Book;
using BookLibrary.Domain.Interfaces.ReadRepositories.Book;
using BookLibrary.Infra.Data.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLibrary.Infra.Data.Repositories.ReadRepositories
{
    class ReadBookRepository : IReadBookRepository
    {
        private readonly BookLibraryDbContext _ctx;
        private readonly IMapper mapper;
        private readonly DbSet<Book> _books;
        private readonly IQueryable<Book> _booksAsNoTracking;
        public ReadBookRepository(BookLibraryDbContext ctx, IMapper mapper)
        {
            this._ctx = ctx;
            this.mapper = mapper;
            _books = _ctx.Set<Book>();
            _booksAsNoTracking = _ctx.Set<Book>().AsNoTracking();
        }
        public async Task<BookListDto> GetBooksAsync(string name = "", int count = 1)
        {
            if (count <= 0)
                throw new Exception("invalid input");
            BookListDto booksDto = new();
            if (string.IsNullOrEmpty(name))
            {
                var countNumberOfBooks = await _booksAsNoTracking.TakeLast(count).ToListAsync();
                booksDto.Books = mapper.Map<List<BookItemDto>>(countNumberOfBooks);
            }
            else
            {
                var countNumberOfBooks = await _booksAsNoTracking.Where(c=>c.Title.Contains(name))
                    .TakeLast(count).ToListAsync();
                booksDto.Books = mapper.Map<List<BookItemDto>>(countNumberOfBooks);
            }
            return booksDto;
        }
    }

}
