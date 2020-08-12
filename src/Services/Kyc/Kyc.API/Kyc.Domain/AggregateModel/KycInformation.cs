using Kyc.Domain.Events;
using Kyc.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace Kyc.Domain.AggregateModel
{
    public class KycInformation : Entity, IAggregateRoot
    {

        public string NID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
       
        public short KycStatusId { get; set; }
        public KycStatus KycStatus { get; set; }
        
        public Guid UserId { get; set; }
        public User User { get; set; }

        public KycInformation()
        {

        }

        public KycInformation(Guid userId, string NID, string firstName, string lastName, KycStatuses kycStatus)
        {
            this.Id = Guid.NewGuid();
            this.NID = NID;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.UserId = userId;
            this.KycStatusId = (short)kycStatus;
            var kycStartedDomainEvent = new KycSubmittedDomainEvent(this, NID, firstName,lastName);
            this.AddDomainEvent(kycStartedDomainEvent);
        }
    }
}
