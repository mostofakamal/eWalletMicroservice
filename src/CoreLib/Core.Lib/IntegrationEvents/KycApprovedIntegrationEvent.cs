using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Lib.IntegrationEvents
{
    public class KycApprovedIntegrationEvent : IntegrationEvent
    {
        public Guid UserId { get ; set ; }
    }
}
