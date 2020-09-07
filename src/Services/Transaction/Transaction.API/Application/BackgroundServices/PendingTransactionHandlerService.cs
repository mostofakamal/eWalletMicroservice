using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Transaction.API.Application.IntegrationEvents;
using Transaction.Domain.AggregateModel;
using Transaction.Domain.Services;

namespace Transaction.API.Application.BackgroundServices
{
    public class HandlePendingTransactionCommand : IRequest
    {

    }

    public class PendingTransactionHandler : IRequestHandler<HandlePendingTransactionCommand>
    {
        private readonly IServiceProvider _iServiceProvider;
        private readonly ILogger<PendingTransactionHandler> _logger;

        public PendingTransactionHandler(IServiceProvider iServiceProvider, ILogger<PendingTransactionHandler> logger)
        {
            _iServiceProvider = iServiceProvider;
            _logger = logger;
        }

        public async Task<Unit> Handle(HandlePendingTransactionCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("starting handling of pending transactions..");
            using (var scope = _iServiceProvider.GetService<IServiceScopeFactory>().CreateScope())
            {
                var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
                var userTransactionService = scope.ServiceProvider.GetRequiredService<IUserTransactionService>();
                var transactionIntegrationLogService = scope.ServiceProvider.GetRequiredService<ITransactionIntegrationDataService>();
                var pendingTransactions = await userRepository.GetAllPendingTransactions();
                _logger.LogInformation($"Found {pendingTransactions.Count} pending transaction to handle ");
                foreach (var pendingTransaction in pendingTransactions)
                {
                    try
                    {
                        _logger.LogInformation(
                            $"handling pending transaction with correlationid: {pendingTransaction.CorrelationId}  amount: {pendingTransaction.Amount} type : {pendingTransaction.TransactionType.Name}  sender : {pendingTransaction.SenderUser.UserIdentityGuid} receiver : {pendingTransaction.ReceiverUser.UserIdentityGuid} ");
                        await userTransactionService.DoTransaction(pendingTransaction.Amount,
                            pendingTransaction.SenderUser.UserIdentityGuid,
                            pendingTransaction.ReceiverUser.UserIdentityGuid,
                            pendingTransaction.TransactionType, pendingTransaction.CorrelationId ?? default);
                        pendingTransaction.MarkAsHandled();
                        await userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning($"An error handling pending transaction. Message: {ex.Message}");
                    }
                }

                await transactionIntegrationLogService.PublishAllPending();
            }

            return Unit.Value;
        }
    }
    public class PendingTransactionHandlerService : BackgroundService
    {
        private readonly ILogger<PendingTransactionHandlerService> _logger;
        private readonly IServiceProvider _iServiceProvider;


        public PendingTransactionHandlerService(ILogger<PendingTransactionHandlerService> logger, IServiceProvider iServiceProvider)
        {
            _logger = logger;
            _iServiceProvider = iServiceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(60000, stoppingToken);
                await HandlePendingTransactions();
            }
        }


        private async Task HandlePendingTransactions()
        {
            try
            {
                using var scope = _iServiceProvider.GetService<IServiceScopeFactory>().CreateScope();
                var mediator= scope.ServiceProvider.GetRequiredService<IMediator>();
                await mediator.Send(new HandlePendingTransactionCommand());
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"An exception occured in PendingTransactionHandler service. Details: {ex}");
            }
        }

    }
}