using System.Text;

namespace EmployeesApi.Middlewares
{
    public class HttpMessagesLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<HttpMessagesLoggingMiddleware> _logger;

        public HttpMessagesLoggingMiddleware(RequestDelegate next, ILogger<HttpMessagesLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Request.EnableBuffering();
            await LogRequest(context.Request.Body);

            Stream originalBody = context.Response.Body;
            try
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    context.Response.Body = memoryStream;

                    await _next(context);

                    await LogResponse(memoryStream);
                    await memoryStream.CopyToAsync(originalBody);
                }
            }
            finally
            {
                context.Response.Body = originalBody;
            }
        }

        private async Task LogRequest(Stream requestBodyStream)
        {
            try
            {
                string requestBody = await new StreamReader(requestBodyStream, Encoding.UTF8).ReadToEndAsync();

                requestBodyStream.Position = 0;

                if (string.IsNullOrWhiteSpace(requestBody) == false)
                    _logger.LogInformation($"Request: \n{requestBody}");
            }
            catch (Exception exception)
            {
                // неуспешният лог не трябва да е пречина за спиране на заявката
                _logger.LogError(exception, exception.Message);
            }
        }

        private async Task LogResponse(Stream responseBodyStream) 
        {
            try
            {
                responseBodyStream.Position = 0;

                string responseBody = await new StreamReader(responseBodyStream).ReadToEndAsync();

                responseBodyStream.Position = 0;

                if (string.IsNullOrWhiteSpace(responseBody) == false)
                    _logger.LogInformation($"Respone: \n{responseBody}");
            }
            catch (Exception exception)
            {
                // неуспешният лог не трябва да е пречина за спиране на заявката
                _logger.LogError(exception, exception.Message);
            }
        }
    }
}
