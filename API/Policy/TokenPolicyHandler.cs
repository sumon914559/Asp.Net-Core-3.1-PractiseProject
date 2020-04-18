using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;

namespace API.Policy
{
    public class TokenPolicyHandler:AuthorizationHandler<TokenPolicy>
    {
        private readonly IDistributedCache _cache;

        public TokenPolicyHandler(IDistributedCache cache)
        {
            _cache = cache;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TokenPolicy requirement)
        {
            
             var userId = context.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).SingleOrDefault();
            if (userId == null)
            {
                throw new UnauthorizedAccessException();
            }
            var accessTokenKey = userId.ToString() + "_accesstoken";
            var cacheToken = _cache.GetString(accessTokenKey);
            if (cacheToken == null)
            {
                 throw new UnauthorizedAccessException();
            }
            else
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
