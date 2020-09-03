using System;
using Automatonymous;

namespace wallet.sagas.core.StateMachines
{
    public class RewardTransactionStateData: SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }

        public string CurrentState { get; set; }
        public DateTime RequestStartedOn { get; set; }

        public Guid RewardSenderWalletUserId { get; set; }

        public Guid RewardReceiverUserId { get; set; }

        public decimal Amount { get; set; }

        public DateTime? RequestCancelledOn { get; set; }

        public DateTime? RequestCompletedOn { get; set; }


    }
}
