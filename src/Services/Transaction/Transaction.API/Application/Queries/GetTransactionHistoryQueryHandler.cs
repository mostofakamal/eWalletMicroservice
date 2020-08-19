using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Lib.Services;
using MediatR;
using Transaction.Domain.AggregateModel;

namespace Transaction.API.Application.Queries
{
    public class GetTransactionHistoryQueryHandler : IRequestHandler<GetTransactionHistoryQuery, TransactionHistoryResponse>
    {
        private readonly IIdentityService _identityService;
        private readonly IUserRepository _userRepository;

        public GetTransactionHistoryQueryHandler(IIdentityService identityService, IUserRepository userRepository)
        {
            _identityService = identityService;
            _userRepository = userRepository;
        }

        public async Task<TransactionHistoryResponse> Handle(GetTransactionHistoryQuery request, CancellationToken cancellationToken)
        {
            var currentUserId = Guid.Parse(_identityService.GetUserIdentity());
            var currentUser = await _userRepository.GetAsync(currentUserId);
            var transactionHistories= currentUser.Transactions.Select(x => new TransactionHistory()
            {
                Amount = x.Amount,
                TransactionType = x.TransactionType.Name,
                Status = x.TransactionStatus.Name,
                TransactionId = x.TransactionGuid,
                CounterPartyUserGuid = x.CounterPartyUserGuid,
                Description = x.Description,
                Date = x.CreateDate
            });
            return new TransactionHistoryResponse
            {
                TransactionHistories = new List<TransactionHistory>(transactionHistories)
            };
        }
    }
}