using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookLibrary.Infra.WebFramework.Api.ApiResponses
{
    public class ApiBadRequestResponse : ApiResponse
    {

        public List<string> Errors { get; }

        public ApiBadRequestResponse(string url, IEnumerable<string> errors)
            : base(400, url)
        {
            Errors = new();
            Errors.AddRange(errors);
        }
    }
}
