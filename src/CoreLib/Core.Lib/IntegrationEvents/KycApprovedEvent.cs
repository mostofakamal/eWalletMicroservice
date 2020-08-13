using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Lib.IntegrationEvents
{
    public class KycApprovedEvent : IKycApprovedEvent
    {
        public Guid UserId { get ; set ; }
    }
}
