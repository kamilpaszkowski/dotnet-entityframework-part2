using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Euvic.StaffTraining.Common
{
    public class Middleware
    {
        private readonly RequestDelegate _next;
        public Middleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            // Execute before next middleware 

            await _next(context);

            // Execute before after other middlewares 
        }
    }

    public static class RequestTimeMiddlewareExtensions
    {
        public static void UseCustomMiddleware(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseMiddleware<Middleware>();
        }
    }
}
