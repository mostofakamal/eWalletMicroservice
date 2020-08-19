using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kyc.API.Infrastructure
{
    public static class Extensions
    {
        public static IApplicationBuilder ConfigureExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<KycServiceExceptionMiddleware>();
            return app;
        }
    }
}
