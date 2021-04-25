namespace O3.SeaBattle.Service.Middleware
{
    using System;
    using System.Net;
    using System.Net.Mime;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;

    public class ExceptionToHttpStatusConverter
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionToHttpStatusConverter> _logger;

        public ExceptionToHttpStatusConverter(
          RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = loggerFactory?.CreateLogger<ExceptionToHttpStatusConverter>() ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        public async Task Invoke(HttpContext httpContext)
        {

            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                if (httpContext.Response.HasStarted)
                {
                    _logger.LogWarning("The response has already started, the http status code middleware will not be executed.");
                    throw;
                }
                httpContext.Response.Clear();

                switch (ex)
                {
                    case ArgumentOutOfRangeException argEx:
                        httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;

                    case ArgumentNullException argEx:
                        httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;

                    case ArgumentException argEx:
                        httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;

                    default:
                        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                        httpContext.Response.ContentType = MediaTypeNames.Application.Json;
                        await httpContext.Response.WriteAsJsonAsync(new { Error = "Internal server error" });
                        _logger.LogError(ex, "Unexpected exception type.");
                        return;
                }

                httpContext.Response.ContentType = MediaTypeNames.Application.Json;
                await httpContext.Response.WriteAsJsonAsync(
                    new { 
                        detail = ex.Message, 
                        status = httpContext.Response.StatusCode 
                    });
            }
        }
    }
}
