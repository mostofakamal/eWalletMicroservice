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

        public Guid CreateDebitTransaction(decimal amount, User counterPartyUser, TransactionType type, Guid correlationId)
        {
            if (!CanDebit(amount))
            {
                throw new InSufficientBalanceDomainException("User does not have sufficient balance to make the transaction!");
            }

            var description = $"Debit for {type.Name} to {counterPartyUser.PhoneNumber}";
            var transaction = new Transaction(-amount, counterPartyUser.UserIdentityGuid, type, description);
            _transactions.Add(transaction);
            AddDomainEvent(new DebitTransactionCreatedDomainEvent(amount, UserIdentityGuid, counterPartyUser.UserIdentityGuid,
                type, transaction.TransactionGuid,correlationId));
            return transaction.TransactionGuid;
        }

        public void CreateCreditTransaction(decimal amount, User counterPartyUser, TransactionType type)
        {
            var description = $"Credit for {type.Name} from {counterPartyUser.PhoneNumber}";
            var transaction = new Transaction(amount, counterPartyUser.UserIdentityGuid, type,description);
            _transactions.Add(transaction);
            AddDomainEvent(new CreditTransactionCreatedDomainEvent(amount, UserIdentityGuid, counterPartyUser.UserIdentityGuid,
                type, transaction.TransactionGuid));
        }

        public decimal GetBalance()
        {
            return Transactions?
                       .Where(x=>x.TransactionStatus.Id == TransactionStatus.Ok.Id)
                       .Sum(x => x.Amount) ?? 0;
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
