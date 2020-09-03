using System;
using System.Reflection;
using System.Threading.Tasks;
using Core.Lib.IntegrationEvents;
using Core.Lib.RabbitMq;
using Core.Lib.RabbitMq.Configs;
using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using wallet.sagas.core.SagaDbConfigurations;
using wallet.sagas.core.StateMachines;

namespace ewallet.SagaStateMachine
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
           
                var hostBuilder = new HostBuilder()
                    .ConfigureServices((hostContext, services) =>
                    {
                       
                        var appSettingsSection = config.GetSection("QueueSettings");
                        var appSettings = appSettingsSection.Get<QueueSettings>();
                        services.Configure<QueueSettings>(appSettingsSection);
                        services.AddSingleton(appSettings);
                        services.AddSingleton<IConfiguration>(config);
                        var connectionString = config.GetConnectionString("DefaultConnection");
                        services.AddDbContext<WalletSagaDbContext>(options => options.UseSqlServer(
                            connectionString,
                            b =>
                            {
                                b.MigrationsAssembly(typeof(RewardTransactionStateData).Assembly.FullName);
                                //b.EnableRetryOnFailure(5);
                            }));
                        services.AddMassTransit(cfg =>
                        {
                            cfg.AddSagaStateMachine<RewardTransactionStateMachine, RewardTransactionStateData>()

                                .EntityFrameworkRepository(r =>
                                {
                                    r.ConcurrencyMode =
                                        ConcurrencyMode.Pessimistic; // or use Optimistic, which requires RowVersion

                                    r.AddDbContext<DbContext, WalletSagaDbContext>((provider, builder) =>
                                    {
                                        builder.UseSqlServer(connectionString, m =>
                                        {
                                            m.MigrationsAssembly(typeof(WalletSagaDbContext).Assembly.GetName().Name);
                                            m.MigrationsHistoryTable($"__EFMigrationsHistory","dbo");
                                        });
                                    });
                                });

                            cfg.AddBus(provider => EventBusRabbitMq.ConfigureBusForSaga(provider));
                        });
                        EndpointConvention.Map<TransactionIntegrationMessage>(new Uri($"queue:{nameof(TransactionIntegrationMessage)}"));

                        services.AddMassTransitHostedService();
                    }).Build();

                using (var scope = hostBuilder.Services.GetService<IServiceScopeFactory>().CreateScope())
                {
                    scope.ServiceProvider.GetRequiredService<WalletSagaDbContext>().Database.Migrate();
                }
            try
            {
                Console.WriteLine("Starting saga...");
                await hostBuilder.RunAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
