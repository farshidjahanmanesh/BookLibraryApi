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
        public Book(string title,string openLibraryId,int firstPublishYear):base()
        {
            this.Title = title;
            this.OpenLibraryId = openLibraryId;
            this.FirstPublishYear = firstPublishYear;
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string OpenLibraryId { get; set; }
        public int FirstPublishYear { get; set; }

        #region navigartion property
        public IList<Author> Authors { get; set; }
        #endregion
    }
}
