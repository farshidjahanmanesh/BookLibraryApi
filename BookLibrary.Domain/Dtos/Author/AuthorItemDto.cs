using BookLibrary.Domain.Dtos.Book;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLibrary.Domain.Dtos.Author
{
    public class AuthorItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string OpenLibraryId { get; set; }
        public string Description { get; set; }
        public string WikipediaUrl { get; set; }
        public DateTime Birthday { get; set; }

        #region navigartion property
        public IList<BookItemDto> Books { get; set; }
        #endregion
    }
}
