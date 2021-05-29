using System.Collections.Generic;
using BookLibrary.Domain.Domains.Authors;
using BookLibrary.Domain.Dtos.Author;

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
        public string? Description { get; set; }

        #region navigartion property
        public IList<AuthorItemDto> Authors { get; set; }
        #endregion
    }
}
