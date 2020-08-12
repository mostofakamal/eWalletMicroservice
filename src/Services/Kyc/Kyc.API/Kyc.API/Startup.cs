using Core.Lib.IntegrationEvents;
using Core.Lib.RabbitMq;
using Core.Lib.RabbitMq.Abstractions;
using Core.Lib.RabbitMq.Configs;
using Kyc.API.Application.IntegrationEvents;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;

namespace Kyc.API
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddControllers();


            var appSettingsSection = Configuration.GetSection("QueueSettings");
            var appSettings = appSettingsSection.Get<QueueSettings>();
            services.Configure<QueueSettings>(appSettingsSection);
            services.AddSingleton<QueueSettings>(appSettings);

            services.AddScoped<UserCreatedIntegratedEventConsumer>();

            //services.RegisterQueueService(Configuration);

            services.AddMassTransit(config =>
            {
                config.AddConsumer<UserCreatedIntegratedEventConsumer>();

                config.AddBus(EventBusRabbitMq.ConfigureBus);
            });

            services.AddSingleton<IPublishEndpoint>(provider => provider.GetRequiredService<IBusControl>());
            services.AddSingleton<ISendEndpointProvider>(provider => provider.GetRequiredService<IBusControl>());
            services.AddSingleton<IBus>(provider => provider.GetRequiredService<IBusControl>());
            services.AddSingleton<IHostedService, EventBusHostedService>();
            services.AddSingleton<IEventPublisher, EventPublisher>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Kyc Service", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthorization();
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Kyc Service V1");
                c.RoutePrefix = "";
            });
        }
    }
}
