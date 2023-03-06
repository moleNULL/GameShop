using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Infrastructure.Filters
{
    public class HttpGlobalValidationActionFilter : IActionFilter
    {
        private readonly ILogger<HttpGlobalValidationActionFilter> _logger;

        public HttpGlobalValidationActionFilter(ILogger<HttpGlobalValidationActionFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = new Dictionary<string, string[]>();

                foreach (var entry in context.ModelState)
                {
                    if (entry.Value.Errors.Count > 0)
                    {
                        foreach (var error in entry.Value.Errors)
                        {
                            errors.Add(entry.Key, new string[] { error.ErrorMessage });
                        }
                    }
                }

                string errorsString = string.Join(
                    Environment.NewLine,
                    errors.SelectMany(kv => kv.Value, (kv, v) => $"{kv.Key}: {v}"));

                var problemDetails = new ValidationProblemDetails(errors)
                {
                    Instance = context.HttpContext.Request.Path,
                    Status = StatusCodes.Status400BadRequest,
                };

                context.Result = new BadRequestObjectResult(problemDetails);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                _logger.LogError(errorsString);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is ValidationException validationException)
            {
                context.Result = new BadRequestObjectResult(validationException.Message);
                context.ExceptionHandled = true;
            }

            var statusCode = context.HttpContext.Response.StatusCode;
            _logger.LogInformation($"Response status code: {statusCode}");
        }
    }
}
