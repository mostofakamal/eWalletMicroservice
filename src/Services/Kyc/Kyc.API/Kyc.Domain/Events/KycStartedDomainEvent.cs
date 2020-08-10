using Kyc.Domain.AggregateModel;
using MediatR;

namespace Kyc.Domain.Events
{
    public class KycStartedDomainEvent : INotification
    {
        public string UserId { get; set; }
        public string NID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public KycInformation KycAggregateModel { get; }

        public KycStartedDomainEvent(KycInformation kyc, string userId, string NID, string firstName, string lastName)
        {
            this.KycAggregateModel = kyc;
            this.UserId = userId;
            this.FirstName = firstName;
            this.LastName = lastName;
        }
    }
}
