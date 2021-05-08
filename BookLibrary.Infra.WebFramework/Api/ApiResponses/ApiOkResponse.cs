using Newtonsoft.Json;

namespace BookLibrary.Infra.WebFramework.Api.ApiResponses
{

    public class ApiOkResponse : ApiResponse
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object Result { get; }
        public ApiOkResponse(object result,string url) :base(200,url)
        {
            Result = result;
        }
    }
}
