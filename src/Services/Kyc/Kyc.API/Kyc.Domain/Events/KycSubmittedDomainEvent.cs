using Kyc.Domain.AggregateModel;
using MediatR;
using System;

namespace Kyc.Domain.Events
{
    public class KycSubmittedDomainEvent : INotification
    {
        public Guid UserId { get; set; }
        public Guid KycId { get; set; }
        public string NID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public User User { get; }

        public KycSubmittedDomainEvent(User user, Guid kycId, string NID, string firstName, string lastName)
        {
            this.User = user;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.KycId = kycId;
            this.NID = NID;
        }
    }
}
