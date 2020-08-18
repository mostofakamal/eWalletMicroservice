using Kyc.Domain.Events;
using System;

namespace Kyc.Domain.AggregateModel
{
    public class KycInformation 
    {
        public Guid Id { get; set; }
        public string NID { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }

        public short KycStatusId { get; private set; }
        public KycStatus KycStatus { get; private set; }

        public Guid UserId { get; set; }
        public User User { get; private set; }

        public KycInformation() { }

        public KycInformation(Guid userId, string NID, string firstName, string lastName, KycStatuses kycStatus)
        {
            this.Id = Guid.NewGuid();
            this.NID = NID;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.UserId = userId;
            this.KycStatusId = (short)kycStatus;
        }

        public void SetStatus(short kycStatusId)
        {
            this.KycStatusId = kycStatusId;
        }
    }
}
