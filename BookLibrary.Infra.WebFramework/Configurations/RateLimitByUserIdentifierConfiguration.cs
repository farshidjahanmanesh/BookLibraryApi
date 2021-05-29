using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace BookLibrary.Infra.WebFramework.Configurations
{
    public class RateLimitByUserIdentifierConfiguration : RateLimitConfiguration
    {

        public RateLimitByUserIdentifierConfiguration(
       IHttpContextAccessor httpContextAccessor,
       IOptions<IpRateLimitOptions> ipOptions,
       IOptions<ClientRateLimitOptions> clientOptions)
           : base(httpContextAccessor, ipOptions, clientOptions)
        {
        }

        protected override void RegisterResolvers()
        {
            ClientResolvers.Add(new ClientQueryStringResolveContributor(HttpContextAccessor));
        }
    }
}
