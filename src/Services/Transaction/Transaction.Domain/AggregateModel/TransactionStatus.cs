using System;
using System.Collections.Generic;
using System.Linq;
using Transaction.Domain.Exceptions;
using Transaction.Domain.SeedWork;

namespace Transaction.Domain.AggregateModel
{
    public class TransactionStatus : Enumeration
    {
        public static TransactionStatus Pending = new TransactionStatus(1, nameof(Pending));
        public static TransactionStatus Failed = new TransactionStatus(2, nameof(Failed));
        public static TransactionStatus Ok = new TransactionStatus(3, nameof(Ok));

        public TransactionStatus(int id, string name)
            : base(id, name)
        {
        }

        public static IEnumerable<TransactionStatus> List() =>
            new[] { Pending,Failed,Ok };

        public static TransactionStatus FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => string.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new TransactionDomainException($"Possible values for TransactionStatus: {string.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

        public static TransactionStatus From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new TransactionDomainException($"Possible values for TransactionStatus: {string.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
    }
}