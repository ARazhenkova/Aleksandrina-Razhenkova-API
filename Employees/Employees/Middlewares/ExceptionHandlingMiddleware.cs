using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeesApi.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch(BadHttpRequestException exception)
            {
                _logger.LogError(exception, exception.Message);

                context.Response.StatusCode = StatusCodes.Status400BadRequest;

                ProblemDetails problemDetails = new ProblemDetails();
                problemDetails.Status = context.Response.StatusCode;
                problemDetails.Title = exception.Message;

                await context.Response.WriteAsJsonAsync(problemDetails);
            }
            catch (DbUpdateConcurrencyException exception)
            {
                _logger.LogError(exception, exception.Message);

                context.Response.StatusCode = StatusCodes.Status400BadRequest;

                ProblemDetails problemDetails = new ProblemDetails();
                problemDetails.Status = context.Response.StatusCode;
                problemDetails.Title = "Data may have been changed or deleted since the last load. Please refresh.";

                await context.Response.WriteAsJsonAsync(problemDetails);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                ProblemDetails problemDetails = new ProblemDetails();
                problemDetails.Status = context.Response.StatusCode;

                // Скриване на грешката за потребителите.
                // Реалната грешка може да се види в логовете.
                problemDetails.Title = "Internal server error";

                await context.Response.WriteAsJsonAsync(problemDetails);
            }
        }
       
    }

}
