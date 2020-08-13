using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace ApiGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config
                        .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                        .AddJsonFile("appsettings.json", true, true)
                        .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
                        .AddJsonFile("ocelot.json")
                        .AddEnvironmentVariables();
                })
                .ConfigureServices((hostingContext, services) =>
                {
                    var authenticationProviderKey = "IdentityApiKey";
                    services.AddAuthentication()
                        .AddJwtBearer(authenticationProviderKey, x =>
                        {
                            x.Authority = hostingContext.Configuration["IdentityServerUrl"];
                            x.RequireHttpsMetadata = false;
                            x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                            {

                                ValidateIssuer = false,
                                ValidAudiences = new[] { "kyc", "transaction", "reward", "notification" }
                            };
                        });
                    services.AddOcelot();
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConsole();
                    logging.AddDebug();
                })
                .UseIISIntegration()
                .Configure(app =>
                {
                    IdentityModelEventSource.ShowPII = true;
                    app.UseOcelot().Wait();
                    app.UseRouting();
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapGet("/", async context =>
                        {
                            await context.Response.WriteAsync("Welcome to ewallet api Gateway");
                        });
                    });
                })
                .Build()
                .Run();
        }

    }
}
