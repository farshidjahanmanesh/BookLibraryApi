using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;

namespace BookLibrary.Infra.WebFramework.Api
{
    public class ApiResponse
    {
        public int StatusCode { get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; }
        public string Url { get; set; }
        public ApiResponse(int statusCode,string url, string message = null)
        {
            Url = url;
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }
        private static string GetDefaultMessageForStatusCode(int statusCode)
        {
            switch (statusCode)
            {
                case 200:
                    return "Ok";
                case 400:
                    return "Invalid Input";
                case 401:
                    return "you are unAuthorize";
                case 404:
                    return "Resource not found";
                case 500:
                    return "An unhandled error occurred";
                default:
                    return null;
            }
        }
    }
}
