using BookLibrary.Domain.Dtos.Book;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLibrary.Application.Services
{
    public interface IReadDataFromApi
    {
        public string SiteName { get; }
        public string BookUrl { get; }
        public string AuthorUrl { get; }
        public string SearchForBookWithNameUrl { get;}
        public Task StartGetDataAsync();
        public Task<bool> StartToSearchForBookWith(string bookName); 
    }
}
