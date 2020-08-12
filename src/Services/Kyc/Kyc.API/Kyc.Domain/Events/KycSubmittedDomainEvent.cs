using Kyc.Domain.AggregateModel;
using MediatR;
using System;

namespace Kyc.Domain.Events
{
    public class KycSubmittedDomainEvent : INotification
    {
        public Guid KycId { get; set; }
        public string NID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public KycInformation KycAggregateModel { get; }

        public KycSubmittedDomainEvent(KycInformation kyc, string NID, string firstName, string lastName)
        {
            this.KycAggregateModel = kyc;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.KycId = kyc.Id;
        }
    }
}
