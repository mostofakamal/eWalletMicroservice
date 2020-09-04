using System;
using Automatonymous;
using Core.Lib.IntegrationEvents;

namespace wallet.sagas.core.StateMachines
{
    public class RewardTransactionStateMachine : MassTransitStateMachine<RewardTransactionStateData>
    {
        public RewardTransactionStateMachine()
        {
            Event(() => RewardProcessStartedEvent, x => x.CorrelateById(m => m.Message.CorrelationId));
            Event(() => TransactionFailedEvent, x => x.CorrelateById(m => m.Message.CorrelationId));
            Event(() => DebitTransactionCreatedEvent, x => x.CorrelateById(m => m.Message.CorrelationId));
            Event(() => RewardProcessCancelledEvent, x => x.CorrelateById(m => m.Message.CorrelationId));
            Event(() => RewardProcessCompletedEvent, x => x.CorrelateById(m => m.Message.CorrelationId));


            InstanceState(x => x.CurrentState);

            Initially(
                When(RewardProcessStartedEvent)
                    .Then(context =>
                    {
                        Console.WriteLine($"RewardProcess Started with the request Guid: {context.Data.CorrelationId}");
                        context.Instance.RequestStartedOn = DateTime.Now;
                        context.Instance.RewardSenderWalletUserId = context.Data.SenderUserGuid;
                        context.Instance.RewardReceiverUserId = context.Data.ReceiverUserGuid;
                        context.Instance.Amount = context.Data.Amount;
                        context.Instance.CorrelationId = context.Data.CorrelationId;
                    })

                    .TransitionTo(ProcessStarted)
                    .Send(context => new TransactionIntegrationMessage(context.Data.Amount, context.Data.SenderUserGuid,
                                    context.Data.ReceiverUserGuid, 3, context.Data.CorrelationId))
                    .Catch<Exception>(e=>e.Then(x =>
                    {
                        Console.WriteLine("An exception occured. Error details: "+ x.Exception.Message);
                    }))
                   );



            During(ProcessStarted,
                When(TransactionFailedEvent)
                    .Then(context =>
                    {
                        context.Instance.RequestCancelledOn =
                            DateTime.Now;
                        context.Instance.TransactionFailedReason = context.Data.FailedReason;
                    })
                    .TransitionTo(ProcessCancelled)
                    .Publish(context => new RewardProcessCancelledEvent(context.Data.Amount, context.Data.SenderUserGuid, context.Data.ReceiverUserGuid, context.Data.CorrelationId,context.Data.FailedReason))

            );

            During(ProcessStarted,
                When(DebitTransactionCreatedEvent)
                    .Then(context =>
                    {
                        Console.WriteLine($"Debit transaction created with correlationId: {context.Data.CorrelationId} and Amount: {context.Data.Amount}");
                        context.Instance.RequestCompletedOn =
                            DateTime.Now;
                    })
                    .TransitionTo(ProcessCompleted)
                    .Publish(context => new RewardProcessCompletedEvent(context.Data.CorrelationId,context.Data.ReceiverUserGuid))

            );

        }

        public State ProcessStarted { get; private set; }
        public State ProcessCancelled { get; private set; }

        public State ProcessCompleted { get; private set; }

        public Event<RewardProcessStartedEvent> RewardProcessStartedEvent { get; private set; }

        public Event<TransactionFailedIntegrationEvent> TransactionFailedEvent { get; private set; }

        public Event<DebitTransactionCreatedIntegrationEvent> DebitTransactionCreatedEvent { get; private set; }
        public Event<RewardProcessCancelledEvent> RewardProcessCancelledEvent { get; private set; }

        public Event<RewardProcessCompletedEvent> RewardProcessCompletedEvent { get; private set; }
    }
}