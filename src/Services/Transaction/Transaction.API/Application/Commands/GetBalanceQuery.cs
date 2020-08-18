using MediatR;

namespace Transaction.API.Application.Commands
{
    public class GetBalanceQuery : IRequest<BalanceResponse>
    {
    }

    public class BalanceResponse
    {
        public decimal Balance { get; set; }

        //TODO: Add currency
    }
}