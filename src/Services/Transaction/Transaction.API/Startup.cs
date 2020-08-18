using System.IdentityModel.Tokens.Jwt;
using Core.Lib.RabbitMq.Configs;
using Core.Lib.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Transaction.API.Infrastructure;

namespace Transaction.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddMvc(options => options.EnableEndpointRouting = false);
            var appSettingsSection = Configuration.GetSection("QueueSettings");
            var appSettings = appSettingsSection.Get<QueueSettings>();
            services.Configure<QueueSettings>(appSettingsSection);
            services.AddSingleton(appSettings);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IIdentityService, IdentityService>();

            services.ConfigQueue();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Transaction Service", Version = "v1" });
            });

            services.RegisterDbAccess(Configuration);

            services.ConfigureAppServices();
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = Configuration["IdentityServerUrl"];
                    options.Audience = "transaction";
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                    {
                        ValidateIssuer = false,
                        ValidAudiences = new[] { "transaction" }
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Transaction Service V1");
                c.RoutePrefix = "";
            });

            app.UseRouting()
                .UseAuthentication();
            app.UseAuthorization();
            app.ConfigureExceptionMiddleware();
            app.InitializeDatabase()
                .UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            }); ;
        }
    }
}
