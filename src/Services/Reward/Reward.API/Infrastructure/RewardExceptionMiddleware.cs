using System;
using System.Net;
using System.Threading.Tasks;
using Core.Lib.Middlewares.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Reward.Domain.Exceptions;

namespace Reward.API.Infrastructure
{
    public class RewardExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public RewardExceptionMiddleware(RequestDelegate next, ILogger<RewardExceptionMiddleware> logger)
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
            catch (RewardDomainException rewardDomainException)
            {
                _logger.LogError($"A reward domain exception occured!. Error Details: {rewardDomainException}");
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await HandleExceptionAsync(httpContext, rewardDomainException);
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
