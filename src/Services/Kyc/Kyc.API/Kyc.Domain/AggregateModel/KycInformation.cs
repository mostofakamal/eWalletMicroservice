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

        public KycInformation(string userId, string NID, string firstName, string lastName)
        {
            this.NID = NID;
            this.FirstName = firstName;
            this.LastName = lastName;

            var kycStartedDomainEvent = new KycStartedDomainEvent(this,userId, NID, firstName,lastName);
            this.AddDomainEvent(kycStartedDomainEvent);
        }
    }
}
