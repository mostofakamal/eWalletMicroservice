using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Lib.IntegrationEvents
{
    public interface IKycApprovedEvent
    {
        Guid UserId { get; set; }
    }
}
