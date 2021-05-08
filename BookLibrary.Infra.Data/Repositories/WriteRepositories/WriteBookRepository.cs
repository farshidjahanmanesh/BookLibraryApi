using AutoMapper;
using BookLibrary.Domain.Domains.Books;
using BookLibrary.Domain.Dtos.Book;
using BookLibrary.Domain.Interfaces.WriteRepositories.Book;
using BookLibrary.Infra.Data.Data;
using BookLibrary.Infra.WebFramework.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BookLibrary.Infra.Data.Repositories.WriteRepositories
{
    public class WriteBookRepository : IWriteBookRepository
    {
        private readonly BookLibraryDbContext _ctx;
        private readonly IMapper mapper;
        private readonly DbSet<Book> _books;
        private readonly IQueryable<Book> _booksAsNoTracking;
        public WriteBookRepository(BookLibraryDbContext ctx, IMapper mapper)
        {
            this._ctx = ctx;
            this.mapper = mapper;
            _books = _ctx.Set<Book>();
            _booksAsNoTracking = _ctx.Set<Book>().AsNoTracking();
        }
        public async Task InsertBook(InsertBookDto model)
        {
            if (model == null)
                throw new ApiException(statusCode: System.Net.HttpStatusCode.BadRequest);
            var bookModel = mapper.Map<Book>(model);
            await _books.AddAsync(bookModel);
        }
        public async Task<int> SaveChanges(CancellationToken token = default)
        {
            return await _ctx.SaveChangesAsync(token);
        }
    }
}
