using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookLibrary.Infra.WebFramework.Api.ApiResponses
{
    public class ApiNotFoundResponse : ApiResponse
    {
        public List<string> Errors { get; }
        public ApiNotFoundResponse( string url, IEnumerable<string> errors) :base(404,url)
        {
            Errors = new();
            Errors.AddRange(errors);
        }
    }
}
