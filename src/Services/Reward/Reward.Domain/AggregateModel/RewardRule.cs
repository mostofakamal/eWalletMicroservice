using System;
using System.Collections.Generic;
using System.Text;

namespace Reward.Domain.AggregateModel
{
    public class RewardRule
    {
        public int Id { get; set; }
        public int OperationId { get; set; }
        public RewardOperation Operation { get; set; }
        public decimal Amount { get; set; }
        public bool IsEnabled { get; set; }
        public int RequiredMinOccurance { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }

    }
}
