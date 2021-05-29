using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BookLibrary.Common.Exceptions
{
    public class ApiException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public object AdditionalData { get; set; }
        public ApiException(HttpStatusCode statusCode)
        {
            this.StatusCode = statusCode;
        }
        public ApiException(HttpStatusCode statusCode, object additionalData) : this(statusCode)
        {
            this.AdditionalData = additionalData;
        }
    }
}
