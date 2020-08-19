using Core.Lib.Extensions;
using Kyc.Insfrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kyc.API.Application.Behaviors
{
    public class TransactionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly UserContext _dbContext;
        private readonly ILogger<TransactionBehaviour<TRequest, TResponse>> logger;

        public TransactionBehaviour(UserContext dbContext, ILogger<TransactionBehaviour<TRequest, TResponse>> logger)
        {
            this._dbContext = dbContext;
            this.logger = logger;
        }
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var response = default(TResponse);
            var typeName = request.GetGenericTypeName();

            try
            {
                var strategy = _dbContext.Database.CreateExecutionStrategy();

                await strategy.ExecuteAsync(async () =>
                {
                    Guid transactionId;
                    logger.LogInformation("before handling command");
                    response = await next();
                    logger.LogInformation("after handling command and domain events");
                });

                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
