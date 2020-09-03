using System;
using System.Threading.Tasks;
using Transaction.Domain.AggregateModel;
using Transaction.Domain.Exceptions;

namespace Transaction.Domain.Services
{
    public class UserTransactionService : IUserTransactionService
    {
        private readonly IUserRepository _userRepository;

        public UserTransactionService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Guid> TransferMoney(decimal amount, Guid senderUserGuid, string receiverPhoneNumber)
        {
            var senderUser = await _userRepository.GetAsync(senderUserGuid);
            var receiverUser = await _userRepository.GetAsync(receiverPhoneNumber);
            return await PerformTransactionCore(amount, TransactionType.Transfer, senderUser, receiverUser,Guid.NewGuid());
        }

        public async Task<Guid> DoTransaction(decimal amount, Guid senderUserGuid, Guid receiverUserGuid,
            TransactionType transactionType,Guid correlationId)
        {
            var senderUser = await _userRepository.GetAsync(senderUserGuid);
            var receiverUser = await _userRepository.GetAsync(receiverUserGuid);
            return await PerformTransactionCore(amount, transactionType, senderUser, receiverUser,correlationId);
        }

        private async Task<Guid> PerformTransactionCore(decimal amount,
            TransactionType transactionType, User senderUser, User receiverUser,Guid correlationId)
        {
            CheckPreconditions(senderUser, receiverUser);
            // Debit transaction from sender
            var debitTransactionId = senderUser.CreateDebitTransaction(amount, receiverUser, transactionType,correlationId);
            // Credit transaction to receiver
            receiverUser.CreateCreditTransaction(amount, senderUser, transactionType);
            await _userRepository.UnitOfWork.SaveEntitiesAsync();
            return debitTransactionId;
        }

        private void CheckPreconditions(User senderUser, User receiverUser)
        {
            CheckSenderAndReceiverAreSame(senderUser.UserIdentityGuid, receiverUser.UserIdentityGuid);
            CheckSenderReceiverDifferentCountry(senderUser, receiverUser);
            CheckTransactionEligibilityOfSenderAndReceiver(senderUser, receiverUser);
        }

        private void CheckSenderReceiverDifferentCountry(User senderUser, User receiverUser)
        {
            if (senderUser.CountryId != receiverUser.CountryId)
            {
                throw new SenderReceiverCountryMismatchTransactionDomainException("Sender and receiver country must be same!");
            }
        }

        private void CheckSenderAndReceiverAreSame(Guid senderUserGuid, Guid receiverUserGuid)
        {
            if (senderUserGuid == receiverUserGuid)
            {
                throw new SameSenderReceiverTransactionDomainException("Can not make transaction to and from same user");
            }
        }

        private void CheckTransactionEligibilityOfSenderAndReceiver(User senderUser, User receiverUser)
        {
            if (!senderUser.IsTransactionEligible)
            {
                throw new TransactionInEligibleDomainException("Sender is not Transaction eligible!");
            }

            if (!receiverUser.IsTransactionEligible)
            {
                throw new TransactionInEligibleDomainException("Receiver is not Transaction eligible!");
            }
        }
    }
}
