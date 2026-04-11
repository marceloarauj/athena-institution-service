using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Institution.Infrastructure.Database
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var apiProjectPath = Path.Combine(Directory.GetCurrentDirectory(), "../Institution");

            var configuration = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(apiProjectPath, "appsettings.json"), optional: false, reloadOnChange: true)
                .AddJsonFile(Path.Combine(apiProjectPath, "appsettings.Development.json"), optional: true, reloadOnChange: true)
                .Build();

            var connectionString = configuration.GetConnectionString("postgresql");

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
