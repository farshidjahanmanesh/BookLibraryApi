using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLibrary.Domain.Dtos.Book
{

    public class BookListDto
    {
        public List<BookItemDto> Books { get; set; }
    }
}
