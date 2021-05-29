using BookLibrary.Common.Exceptions;
using BookLibrary.Infra.WebFramework.Api;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BookLibrary.Infra.WebFramework.Middlewares
{
    public static class CustomExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomExceptionHandlerMiddleware>();
        }
    }

    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<CustomExceptionHandlerMiddleware> _logger;

        public CustomExceptionHandlerMiddleware(RequestDelegate next,
            IWebHostEnvironment env,ILogger<CustomExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _env = env;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var url = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}";
            string message = null;
            HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;

            try
            {
                await _next(context);
            }

            catch (ApiException exception)
            {
                httpStatusCode = exception.StatusCode;
                message = exception.AdditionalData.ToString();
                _logger.LogError(message, exception);
                await WriteToResponseAsync();
            }
            catch (UnauthorizedAccessException exception)
            {
                SetUnAuthorizeResponse(exception);
                _logger.LogError(message, exception);
                await WriteToResponseAsync();
            }
            catch (SecurityTokenExpiredException exception)
            {
                _logger.LogError(exception, exception.Message);
                SetUnAuthorizeResponse(exception);
                await WriteToResponseAsync();
            }
            catch (Exception exception)
            {
                message = exception.Message;
                //message = "have server error";
                _logger.LogError(message, exception);
                await WriteToResponseAsync();
            }
            async Task WriteToResponseAsync()
            {
                if (context.Response.HasStarted)
                    throw new InvalidOperationException("The response has already started, the http status code middleware will not be executed.");

                var result = new ApiResponse((int)httpStatusCode, url, message);
                var json = JsonConvert.SerializeObject(result);

                context.Response.StatusCode = (int)httpStatusCode;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(json);
            }

            void SetUnAuthorizeResponse(Exception exception)
            {
                httpStatusCode = HttpStatusCode.Unauthorized;
            }
        }
    }
}
