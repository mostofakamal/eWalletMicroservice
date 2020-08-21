using Core.Lib.Middlewares.Exceptions;
using Kyc.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Kyc.API.Infrastructure
{
    public class KycServiceExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public KycServiceExceptionMiddleware(RequestDelegate next, ILogger<KycServiceExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
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
            catch (KycDomainException transactionDomainException)
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
                ErrorCode = exception.GetType().Name,
                ErrorMessage = exception.Message
            }.ToString());
        }

    }
}