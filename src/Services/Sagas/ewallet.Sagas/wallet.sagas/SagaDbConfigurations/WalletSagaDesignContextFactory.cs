using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace wallet.sagas.core.SagaDbConfigurations
{
    public class WalletSagaDesignContextFactory:IDesignTimeDbContextFactory<WalletSagaDbContext>
    {
        public WalletSagaDbContext CreateDbContext(string[] args)
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                LoadConnectionString();
            }
            var builder = new DbContextOptionsBuilder<WalletSagaDbContext>();
            builder.UseSqlServer(_connectionString);
            return new WalletSagaDbContext(builder.Options);
        }

        private static string _connectionString;

        private static void LoadConnectionString()
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json");

            var configuration = builder.Build();
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
    }
}
