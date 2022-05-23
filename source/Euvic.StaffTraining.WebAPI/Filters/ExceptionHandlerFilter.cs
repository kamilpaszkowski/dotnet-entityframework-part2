using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Euvic.StaffTraining.WebAPI.Filters
{
    public class ExceptionHandlerFilter : IExceptionFilter
    {
        private readonly ILogger<ExceptionHandlerFilter> _logger;
        private readonly IWebHostEnvironment _environment;

        public ExceptionHandlerFilter(
            ILogger<ExceptionHandlerFilter> logger,
            IWebHostEnvironment environment)
        {
            _logger = logger;
            _environment = environment;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "Unexpected error occured");

            context.Result = new JsonResult(_environment.IsProduction()
                ? string.Empty
                : $"{context.Exception.Message} {context.Exception.StackTrace}")
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };

            context.ExceptionHandled = true;
        }
    }
}
