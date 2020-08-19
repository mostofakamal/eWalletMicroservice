using Kyc.Domain.AggregateModel;
using Kyc.Domain.SeedWork;
using Kyc.Insfrastructure.EntityConfigurations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kyc.Insfrastructure
{
    public class UserContext : DbContext, IUnitOfWork
    {
        private readonly IMediator _mediator;

        public DbSet<KycInformation> Kycs { get; set; }
        public DbSet<KycStatus> KycStatuses { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Country> Countries { get; set; }

        public UserContext(IMediator mediator, DbContextOptions options) : base(options)
        {
            this._mediator = mediator;
        }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            // After executing this line all the changes (from the Command Handler and Domain Event Handlers) 
            // performed through the DbContext will be committed
            var result = await base.SaveChangesAsync(cancellationToken);
            await _mediator.DispatchDomainEventsAsync(this);
            
            return true;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new KycStatusConfiguration());
            modelBuilder.ApplyConfiguration(new KycInformationConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new CountryConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
