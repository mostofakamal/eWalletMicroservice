﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Lib.IntegrationEvents
{
    public interface IKycApprovedEvent : IItegration
    {
        Guid UserId { get; set; }
    }
}
