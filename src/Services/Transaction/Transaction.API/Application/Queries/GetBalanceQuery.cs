using MediatR;

namespace Transaction.API.Application.Queries
{
    public class GetBalanceQuery : IRequest<BalanceResponse>
    {
    }
}