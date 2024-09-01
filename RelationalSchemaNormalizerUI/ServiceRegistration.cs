using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AppContext = RelationalSchemaNormalizerLibrary.Models.AppContext;

namespace RelationalSchemaNormalizerUI
{
    public static class ServiceRegistration
    {

        /// <summary>
        /// Adds required services from the library to the given IServiceCollection.
        /// Configures the AppContext with SQL Server using the provided configuration.
        /// </summary>
        /// <param name="services">The IServiceCollection to add services to.</param>
        /// <param name="configuration">The configuration to use for getting connection strings, etc.</param>
        public static void AddLibraryServices(this IServiceCollection services, IConfiguration configuration)
        {
            // It is a good practice to ensure the connection string key exists in the configuration
            var connectionString = configuration.GetConnectionString("AppDatabase");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("No connection string named 'AppDatabase' found.");
            }

            services.AddDbContext<AppContext>(options =>
                options.UseSqlServer(connectionString, ProviderOptions => ProviderOptions.EnableRetryOnFailure()));

        }
    }
}
