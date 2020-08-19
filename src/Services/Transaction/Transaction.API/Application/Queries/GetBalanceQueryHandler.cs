using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Lib.Services;
using MediatR;
using Transaction.Domain.AggregateModel;

namespace Transaction.API.Application.Queries
{

    public class GetBalanceQueryHandler : IRequestHandler<GetBalanceQuery, BalanceResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IIdentityService _identityService;

        public GetBalanceQueryHandler(IUserRepository userRepository, IIdentityService identityService)
        {
            _userRepository = userRepository;
            _identityService = identityService;
        }

        public async Task<BalanceResponse> Handle(GetBalanceQuery request, CancellationToken cancellationToken)
        {
            var currentUserId = Guid.Parse(_identityService.GetUserIdentity());
            var user= await _userRepository.GetAsync(currentUserId);
            return new BalanceResponse
            {
                Balance = user.GetBalance()
            };
        }
    }
}