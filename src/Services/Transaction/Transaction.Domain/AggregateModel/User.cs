using System;
using System.Collections.Generic;
using System.Linq;
using Transaction.Domain.Events;
using Transaction.Domain.Exceptions;
using Transaction.Domain.SeedWork;

namespace Transaction.Domain.AggregateModel
{
    public class User : Entity, IAggregateRoot
    {
        public Guid UserIdentityGuid { get; private set; }

        public bool IsTransactionEligible { get; private set; }

        public int CountryId { get; private set; }

        public string PhoneNumber { get; private set; }

        private readonly List<Transaction> _transactions;
        public IEnumerable<Transaction> Transactions => _transactions.AsReadOnly();

        protected User()
        {
            _transactions = new List<Transaction>();
        }

        public User(Guid userId, int countryId,string phoneNumber)
        {
            UserIdentityGuid = userId;
            CountryId = countryId;
            PhoneNumber = phoneNumber;
        }

        public User MarkUserAsTransactionEligible()
        {
            IsTransactionEligible = true;
            //TODO: Add domain events here if any 
            return this;
        }

        public void CreateDebitTransaction(decimal amount, Guid counterPartyUserGuid, TransactionType type)
        {
            if (!CanDebit(amount))
            {
                throw new InSufficientBalanceDomainException("User does not have sufficient balance to make the transaction!");
            }
            var transaction = new Transaction(-amount, counterPartyUserGuid, type);
            _transactions.Add(transaction);
            AddDomainEvent(new TransactionCreatedDomainEvent(amount, UserIdentityGuid, counterPartyUserGuid,
                type, transaction.TransactionGuid));
        }

        public void CreateCreditTransaction(decimal amount, Guid counterPartyUserGuid, TransactionType type)
        {
            var transaction = new Transaction(amount, counterPartyUserGuid, type);
            _transactions.Add(transaction);
        }

        public decimal GetBalance()
        {
            return Transactions?.Sum(x => x.Amount) ?? 0;
        }

        public bool CanDebit(decimal amount)
        {
            return GetBalance() >= amount;
        }

        public void UpdateUserAsTransactionEligible()
        {
            IsTransactionEligible = true;
        }
    }
}
