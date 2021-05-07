using BookLibrary.Domain.Dtos.Author;
using System.Collections.Generic;

namespace BookLibrary.Domain.Dtos.Book
{
    public class BookItemDto
    {
        public BookItemDto()
        {
            Authors = new List<AuthorItemDto>();
        }
       
        public int Id { get; set; }
        public string Title { get; set; }
        public string OpenLibraryId { get; set; }
        public int FirstPublishYear { get; set; }

        #region navigartion property
        public IList<AuthorItemDto> Authors { get; set; }
        #endregion
    }
}
