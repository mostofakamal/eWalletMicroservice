using Kyc.Domain.AggregateModel;
using MediatR;
using System;

namespace Kyc.Domain.Events
{
    public class KycSubmittedDomainEvent : INotification
    {
        public Guid UserId { get; set; }
        public string NID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public KycInformation KycAggregateModel { get; }

        public KycSubmittedDomainEvent(KycInformation kyc, Guid userId, string NID, string firstName, string lastName)
        {
            this.KycAggregateModel = kyc;
            this.UserId = userId;
            this.FirstName = firstName;
            this.LastName = lastName;
        }
    }
}
