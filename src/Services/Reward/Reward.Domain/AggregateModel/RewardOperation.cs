using Reward.Domain.Exceptions;
using Reward.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reward.Domain.AggregateModel
{
    public class RewardOperation : Enumeration
    {

        public static RewardOperation SubmitKyc = new RewardOperation(1, nameof(SubmitKyc));
        public static RewardOperation TransferMoney = new RewardOperation(2, nameof(TransferMoney));
        public static RewardOperation BillPayment = new RewardOperation(3, nameof(BillPayment));

        public RewardOperation(int id, string name) : base(id, name)
        {
        }

        public static IEnumerable<RewardOperation> List() =>
            new[] { SubmitKyc, TransferMoney, BillPayment };

        public static RewardOperation FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => string.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new RewardDomainException($"Possible values for TransactionType: {string.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

        public static RewardOperation From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new RewardDomainException($"Possible values for TransactionType: {string.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
    }
}
