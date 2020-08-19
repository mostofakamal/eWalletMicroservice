using MediatR;

namespace Transaction.API.Application.Queries
{
    public class GetTransactionHistoryQuery : IRequest<TransactionHistoryResponse>
    {

    }
}