using Kyc.API.Application.DomainEventHandlers;
using Kyc.API.Application.IntegrationEvents;
using Kyc.Domain.AggregateModel;
using Kyc.Domain.Events;
using Kyc.Insfrastructure;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Kyc.API.Infrastructure
{
    public static class AppServiceRegistration
    {
        public static IServiceCollection ConfigureAppServices(this IServiceCollection services)
        {
            services.AddTransient<INotificationHandler<KycSubmittedDomainEvent>, KycSubmittedDomainEventHandler>();
            services.AddScoped<UserCreatedIntegratedEventConsumer>();
            services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly);
            services.AddScoped<IUserRepository, UserRepository>();
            return services;
        }
    }
}
