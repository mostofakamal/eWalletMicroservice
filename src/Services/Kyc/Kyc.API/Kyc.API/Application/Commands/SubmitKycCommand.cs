using MediatR;

namespace Kyc.API.Application.Commands
{

    public class SubmitKycCommand : IRequest<SumitKycResponse>
    {
        public string NID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
