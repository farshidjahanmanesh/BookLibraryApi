using AutoMapper;
using BookLibrary.Common.Exceptions;
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
    public class ReadBookRepository : IReadBookRepository
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

        public async Task<BookItemDto> GetBookByAsync(int id)
        {
            if (id <= 0)
                throw new ApiException(statusCode: System.Net.HttpStatusCode.BadRequest, "id is not valid");
            var bookEntity = await _booksAsNoTracking.Include(c => c.Authors).FirstOrDefaultAsync(c => c.Id == id);
            if (bookEntity == null)
            {
                throw new ApiException(statusCode: System.Net.HttpStatusCode.NotFound, "book with this id not found");
            }
            var bookDto = mapper.Map<BookItemDto>(bookEntity);
            return bookDto;
        }

        public async Task<BookListDto> GetBooksAsync(string name = "", int count = 1)
        {
            if (count <= 0)
                throw new ApiException(statusCode: System.Net.HttpStatusCode.BadRequest, "id is not valid");
            BookListDto booksDto = new();
            if (string.IsNullOrEmpty(name))
            {
                var countNumberOfBooks = await _booksAsNoTracking.Take(count).Include(c => c.Authors).ToListAsync();
                booksDto.Books = mapper.Map<List<BookItemDto>>(countNumberOfBooks);
            }
            else
            {
                var countNumberOfBooks = await _booksAsNoTracking.Where(c => c.Title.Contains(name))
                    .Take(count).Include(c => c.Authors).ToListAsync();
                booksDto.Books = mapper.Map<List<BookItemDto>>(countNumberOfBooks);
            }
            return booksDto;
        }

        public async Task<bool> CheckIsBookInDb(string bookName)
        {
            return await _booksAsNoTracking.AnyAsync(c => c.Title.ToLower() == bookName.ToLower());
        }

        public async Task<BookItemDto> GetBookByNameAsync(string name)
        {
            var bookObject = await _booksAsNoTracking
                .Include(c=>c.Authors)
                .FirstOrDefaultAsync(c => c.Title.ToLower().Contains(name.ToLower()));
            return mapper.Map<BookItemDto>(bookObject);
        }
    }

}
