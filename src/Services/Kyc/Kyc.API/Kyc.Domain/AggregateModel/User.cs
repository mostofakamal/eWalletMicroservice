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

        private readonly List<KycInformation> _kycInformations;
        public IReadOnlyCollection<KycInformation> KycInformations => _kycInformations;

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
            _kycInformations.Add(kycInformation);
            var kycStartedDomainEvent = new KycSubmittedDomainEvent(this, kycInformation.Id, kycInformation.NID, kycInformation.FirstName, kycInformation.LastName);
            this.AddDomainEvent(kycStartedDomainEvent);
        }
        public void UpdateKycStatus(short kycStatus)
        {
            var kycToUpdate = this.KycInformations.OrderByDescending(d => d.CreatedTime).FirstOrDefault();
            kycToUpdate.SetStatus(kycStatus);
        }

        public void SetVerifiedStatus(short kycVerificationResult)
        {
            if(kycVerificationResult == (short) KycStatuses.Approved)
            {
                IsKycVerified = true;

                //here intergration event should be added
            }
        }
    }
}
