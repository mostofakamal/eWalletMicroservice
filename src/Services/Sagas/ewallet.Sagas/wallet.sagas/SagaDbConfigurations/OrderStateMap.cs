using MassTransit.EntityFrameworkCoreIntegration.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using wallet.sagas.core.StateMachines;

namespace wallet.sagas.core.SagaDbConfigurations
{
    public class OrderStateMap :
        SagaClassMap<RewardTransactionStateData>
    {
        protected override void Configure(EntityTypeBuilder<RewardTransactionStateData> entity, ModelBuilder model)
        {
            entity.Property(x => x.CurrentState).HasMaxLength(80);
        }
    }
}