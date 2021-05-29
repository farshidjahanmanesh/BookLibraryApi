using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Http;

namespace BookLibrary.Infra.WebFramework.Configurations
{
    public class ClientQueryStringResolveContributor : IClientResolveContributor
    {
        private IHttpContextAccessor httpContextAccessor;

        public ClientQueryStringResolveContributor(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public string ResolveClient()
        {
            return httpContextAccessor.HttpContext.User.Identity.Name;
        }
    }
}
