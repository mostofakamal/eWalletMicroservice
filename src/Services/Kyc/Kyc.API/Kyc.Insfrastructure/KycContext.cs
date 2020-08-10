using Kyc.Domain.AggregateModel;
using Kyc.Domain.SeedWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kyc.Insfrastructure
{
    public class KycContext : DbContext, IUnitOfWork
    {
        private readonly IMediator _mediator;

        public DbSet<KycInformation> Kycs { get; set; }

        public KycContext(IMediator mediator)
        {
            this._mediator = mediator;
        }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            await _mediator.DispatchDomainEventsAsync(this);

            // After executing this line all the changes (from the Command Handler and Domain Event Handlers) 
            // performed through the DbContext will be committed
            var result = await base.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
