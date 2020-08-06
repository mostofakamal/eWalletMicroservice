using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Core.Lib.Middlewares.Exceptions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (UnAuthorizedExceptions unAuthorizedException)
            {
                _logger.LogError($"UnAuthorized exception occured!. Error Details: {unAuthorizedException}");
                httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await HandleExceptionAsync(httpContext, unAuthorizedException);
            }
            catch (InValidInputException inValidInputException)
            {
                _logger.LogError($"An user input related exception occured!. Error Details: {inValidInputException}");
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await HandleExceptionAsync(httpContext, inValidInputException);
            }
            catch (InvalidOperationException invalidOperationException)
            {
                _logger.LogError($"An invalid  operation exception requested! . Error Details: {invalidOperationException}");
                httpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                await HandleExceptionAsync(httpContext, invalidOperationException);
            }
            catch (Exception ex)
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                _logger.LogError($"Something went wrong: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }

        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            return context.Response.WriteAsync(new ErrorDetails
            {
                ErrorMessage = exception.Message
            }.ToString());
        }
    }
}