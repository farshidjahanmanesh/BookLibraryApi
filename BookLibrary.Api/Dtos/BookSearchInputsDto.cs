using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookLibrary.Api.Dtos
{
    public record BookSearchInputsDto(string Name = "",
        [Range(1,int.MaxValue)] int Count = 1);

}
