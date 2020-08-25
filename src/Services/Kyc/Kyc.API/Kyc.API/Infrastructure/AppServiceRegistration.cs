using IntegrationDataLog.Services;
using Kyc.API.Application.IntegrationEvents;
using Kyc.API.Application.Services;
using Kyc.Domain.AggregateModel;
using Kyc.Domain.Events;
using Kyc.Insfrastructure;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Kyc.API.Infrastructure
{
    public static class AppServiceRegistration
    {
        public static IServiceCollection ConfigureAppServices(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddScoped<UserCreatedIntegratedKEventInKycConsumer>();
            services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly);
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IKycVerificationService, KycVerificationService>();
            services.AddHttpClient<IExternalKycVerifier, ExternalKycVerifier>(x =>
            {
                x.BaseAddress = new Uri(Configuration["NIDServerUrl"]);
            });
            services.AddTransient<Func<DbConnection, ILogger<IIntegrationDataLogService>, IIntegrationDataLogService>>(
                sp => (c, l) => new IntegrationDataLogService(c, l));

            services.AddTransient<IKycIntegrationDataService, KycIntegrationDataService>();
            return services;
        }
    }
}
