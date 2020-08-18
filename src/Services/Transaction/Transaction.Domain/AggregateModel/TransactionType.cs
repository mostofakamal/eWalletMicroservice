using System;
using System.Collections.Generic;
using System.Linq;
using Transaction.Domain.Exceptions;
using Transaction.Domain.SeedWork;

namespace Transaction.Domain.AggregateModel
{
    public class TransactionType : Enumeration
    {
        public static TransactionType Transfer = new TransactionType(1, nameof(Transfer));
        public static TransactionType BillPayment = new TransactionType(2, nameof(BillPayment));
        public static TransactionType Reward = new TransactionType(3, nameof(Reward));

        public TransactionType(int id, string name)
            : base(id, name)
        {
        }

        public static IEnumerable<TransactionType> List() =>
            new[] { Transfer, BillPayment, Reward };

        public static TransactionType FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => string.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new TransactionDomainException($"Possible values for TransactionType: {string.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

        public static TransactionType From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new TransactionDomainException($"Possible values for TransactionType: {string.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
    }
}