using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MyBankIdentity.Middleware
{
    public static class MyBankIdentityMiddlewareExtensions
    {
        public static IServiceCollection AddMyBankIdentity(this IServiceCollection service, Action<JwtMiddlewareOptions> options = default)
        {
            options = options ?? (opts => { });

            service.Configure(options);
            return service;
        }

        public static IApplicationBuilder UseMyBankIdentity(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JwtMiddleware>();
        }

    }
}
