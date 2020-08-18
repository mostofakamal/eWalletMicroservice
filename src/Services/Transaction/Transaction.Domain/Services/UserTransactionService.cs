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

        public async Task DoTransaction(decimal amount, Guid senderUserGuid, Guid receiverUserGuid,
            TransactionType transactionType)
        {
            var senderUser = await _userRepository.GetAsync(senderUserGuid);
            var receiverUser = await _userRepository.GetAsync(receiverUserGuid);
            CheckSenderAndReceiverAreSame(senderUserGuid, receiverUserGuid);
            CheckSenderReceiverDifferentCountry(senderUser, receiverUser);
            CheckTransactionEligibilityOfSenderAndReceiver(senderUser, receiverUser);
            // Debit transaction from sender
            senderUser.CreateDebitTransaction(amount, receiverUserGuid, transactionType);
            // Credit transaction to receiver
            receiverUser.CreateCreditTransaction(amount, senderUserGuid, transactionType);
            await _userRepository.UnitOfWork.SaveEntitiesAsync();
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
