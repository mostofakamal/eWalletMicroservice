using Core.Lib.IdentityServer;
using Core.Lib.RabbitMq.Configs;
using Kyc.API.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

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
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddControllers();

            var appSettingsSection = Configuration.GetSection("QueueSettings");
            var appSettings = appSettingsSection.Get<QueueSettings>();
            services.Configure<QueueSettings>(appSettingsSection);
            services.AddSingleton<QueueSettings>(appSettings);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IIdentityService, IdentityService>();

            services.ConfigQueue();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Kyc Service", Version = "v1" });
            });

            services.RegisterDbAccess(Configuration);

            services.ConfigureAppServices();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.Authority = "http://localhost:5010";
                        options.Audience = "kyc";
                        options.RequireHttpsMetadata = false;
                    });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();
            app.UseMvc();
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Kyc Service V1");
                c.RoutePrefix = "";
            });

            app.InitializeDatabase();
            //app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
