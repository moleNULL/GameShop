using System.ComponentModel.DataAnnotations;
using System.Net;
using Infrastructure.Exceptions;
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

        public void OnException(ExceptionContext exceptionContext)
        {
            if (exceptionContext.Exception is BusinessException ex)
            {
                var problemDetails = new ValidationProblemDetails()
                {
                    Instance = exceptionContext.HttpContext.Request.Path,
                    Status = StatusCodes.Status400BadRequest,
                    Detail = ex.Message
                };

                exceptionContext.Result = new BadRequestObjectResult(problemDetails);
                exceptionContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                exceptionContext.ExceptionHandled = true;

                _logger.LogError(exceptionContext.Exception.Message);
            }
            else
            {
                _logger.LogError(
                    new EventId(exceptionContext.Exception.HResult),
                    exceptionContext.Exception,
                    exceptionContext.Exception.Message);
            }
        }
    }
}
