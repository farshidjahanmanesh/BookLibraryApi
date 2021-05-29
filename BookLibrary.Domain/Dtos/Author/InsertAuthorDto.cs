using System;

namespace BookLibrary.Domain.Dtos.Author
{
    public class InsertAuthorDto
    {
        public string Name { get; set; }
        public string OpenLibraryId { get; set; }
        public string Description { get; set; }
        public string WikipediaUrl { get; set; }
        public string Birthday { get; set; }

    }
}
