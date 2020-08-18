using System;

namespace Kyc.Insfrastructure
{
    internal class KycApprovedEvent
    {
        public KycApprovedEvent()
        {
        }

        public Guid UserId { get; set; }
    }
}