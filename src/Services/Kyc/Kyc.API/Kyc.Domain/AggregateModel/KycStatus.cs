using System;
using System.Collections.Generic;
using System.Text;

namespace Kyc.Domain.AggregateModel
{
    public class KycStatus
    {
        public short Id { get; set; }
        public string Name { get; set; }
    }

    public enum KycStatuses
    {
        Approved = 1,
        Pending = 2,
        Failed = 3
    }
}
