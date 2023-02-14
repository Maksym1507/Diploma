using System.Net;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Infrastructure.Filters
{
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<HttpGlobalExceptionFilter> _logger;

        public HttpGlobalExceptionFilter(ILogger<HttpGlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext.Exception is BusinessException ex)
            {
                var problemDetails = new ValidationProblemDetails()
                {
                    Instance = filterContext.HttpContext.Request.Path,
                    Status = StatusCodes.Status400BadRequest,
                    Detail = ex.Message
                };

                filterContext.Result = new BadRequestObjectResult(problemDetails);
                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                filterContext.ExceptionHandled = true;
            }
            else
            {
                _logger.LogError(
                    new EventId(filterContext.Exception.HResult),
                    filterContext.Exception,
                    filterContext.Exception.Message);
            }
        }
    }
}
