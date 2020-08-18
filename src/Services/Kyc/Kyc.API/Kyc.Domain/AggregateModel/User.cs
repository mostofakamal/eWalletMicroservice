using Kyc.Domain.Events;
using Kyc.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kyc.Domain.AggregateModel
{
    public class User : Entity, IAggregateRoot
    {
        public bool IsKycVerified { get; private set; }
        public List<KycInformation> KycInformations { get; private set; }
        public int CountryId { get; private set; }
        public Country Country { get; private set; }
        public User()
        {

        }

        public User(Guid userId, bool isKycVerified, int countryId)
        {
            this.Id = userId;
            this.IsKycVerified = isKycVerified;
            this.CountryId = countryId;
        }

        public void AddKyc(KycInformation kycInformation)
        {
            this.KycInformations.Add(kycInformation);
            var kycStartedDomainEvent = new KycSubmittedDomainEvent(this, kycInformation.Id, kycInformation.NID, kycInformation.FirstName, kycInformation.LastName);
            this.AddDomainEvent(kycStartedDomainEvent);
        }
        public void UpdateKyc(Guid kycId, short kycStatus)
        {
            var kyc = this.KycInformations.Where(k => k.Id == kycId).FirstOrDefault();
            kyc.SetStatus(kycStatus);
        }
    }
}
