namespace BookLibrary.Infra.WebFramework.Api.ApiResponses
{
    public class ApiUnSupportResponse : ApiResponse
    {
        public ApiUnSupportResponse(string url):base(415,url,"unSupport Inputs")
        {

        }
    }
}
