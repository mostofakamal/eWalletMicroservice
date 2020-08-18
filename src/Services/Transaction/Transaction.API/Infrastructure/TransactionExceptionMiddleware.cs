using System;
using System.Net;
using System.Threading.Tasks;
using Core.Lib.Middlewares.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Transaction.Domain.Exceptions;

namespace Transaction.API.Infrastructure
{
    public class TransactionExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public TransactionExceptionMiddleware(RequestDelegate next, ILogger<TransactionExceptionMiddleware> logger)
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
            
            catch (InValidInputException inValidInputException)
            {
                _logger.LogError($"An user input related exception occured!. Error Details: {inValidInputException}");
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await HandleExceptionAsync(httpContext, inValidInputException);
            }
            catch (TransactionDomainException transactionDomainException)
            {
                _logger.LogError($"A transaction domain exception occured!. Error Details: {transactionDomainException}");
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await HandleExceptionAsync(httpContext, transactionDomainException);
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
