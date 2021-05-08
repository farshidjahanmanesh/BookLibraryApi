namespace BookLibrary.Infra.WebFramework.Api.ApiResponses
{
    public class ApiServerErrorResponse : ApiResponse
    {
        public ApiServerErrorResponse(string url):base(500, url)
        {

        }
    }
}
