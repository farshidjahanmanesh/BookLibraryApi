
using BookLibrary.Domain.Domains.Authors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLibrary.Domain.Domains.Books
{
    public class Book
    {
        public Book()
        {
            Authors = new List<Author>();
        }
        public Book(string title,string openLibraryId,int firstPublishYear,string description) :base()
        {
            this.Title = title;
            this.OpenLibraryId = openLibraryId;
            this.FirstPublishYear = firstPublishYear;
            this.Description = description;
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string OpenLibraryId { get; set; }
        public int FirstPublishYear { get; set; }
        public string Description { get; set; }
        #region navigartion property
        public IList<Author> Authors { get; set; }
        #endregion
    }
}
