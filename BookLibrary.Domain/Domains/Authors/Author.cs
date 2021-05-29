using BookLibrary.Domain.Domains.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLibrary.Domain.Domains.Authors
{
    public class Author
    {
        public Author()
        {
            Books = new List<Book>();
        }
        public Author(string name,string openLibraryId, string description, string wikipediaUrl, string birthday) : base()
        {
            this.Name = name;
            this.OpenLibraryId = openLibraryId;
            this.Description = description;
            this.WikipediaUrl = wikipediaUrl;
            this.Birthday = birthday;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string OpenLibraryId { get; set; }
        public string Description { get; set; }
        public string WikipediaUrl { get; set; }
        public string Birthday { get; set; }

        #region navigartion property
        public IList<Book> Books { get; set; }
        #endregion
    }
}
