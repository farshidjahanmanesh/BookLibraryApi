using System.Collections.Generic;
using BookLibrary.Domain.Dtos.Author;

namespace BookLibrary.Domain.Dtos.Book
{
    public class InsertBookDto
    {
        public InsertBookDto()
        {
            Authors = new List<InsertAuthorDto>();
        }

        public string Title { get; set; }
        public string OpenLibraryId { get; set; }
        public int FirstPublishYear { get; set; }
        public string Description { get; set; }
        #region navigartion property
        public IList<InsertAuthorDto> Authors { get; set; }
        #endregion
    }
}
