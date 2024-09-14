using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RelationalSchemaNormalizerLibrary.Interfaces;
using RelationalSchemaNormalizerLibrary.Models;
using RelationalSchemaNormalizerLibrary.Services;
using System;
using AppContext = RelationalSchemaNormalizerLibrary.Models.AppContext;

namespace RelationalSchemaNormalizerUI
{
    internal static class Program
    {
        public static IServiceProvider ServiceProvider { get; private set; }
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var host = CreateHostBuilder().Build();
            ServiceProvider = host.Services;
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.

            // Automatically apply migrations and update the database
            using (var scope = host.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppContext>();
                dbContext.Database.Migrate();  // Applies any pending migrations
                EnsureDatabaseDetailExists(dbContext);
            }
            ApplicationConfiguration.Initialize();
            Application.Run(ServiceProvider.GetRequiredService<Dashboard>());
        }
        static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddTransient<Dashboard>();
                    services.AddTransient<HomeControl>();
                    services.AddTransient<TableControl>();
                    services.AddTransient<TablesControl>();
                    services.AddTransient<CreateTableControl>();

                    services.AddTransient<IAppDBService, AppDBService>();
                    services.AddTransient<IDynamicDBService, DynamicDBService>();
                    services.AddTransient<INormalizerService, NormalizerService>();
                    services.AddDbContext<AppContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("AppDatabase"),
                        sqlServerOptions =>
                        {
                            sqlServerOptions.MigrationsAssembly("RelationalSchemaNormalizerUI");
                        }
                    ),
                    ServiceLifetime.Transient // Ensure the DbContext is transient
                );
                });
        }
        private static void EnsureDatabaseDetailExists(AppContext dbContext)
        {
            // Check if a DatabaseDetail with the given ID exists
            var databaseDetailId = "e1d6f8b6-8f14-4f7e-9338-2f7d8b3e6b6e";
            var databaseDetailExists = dbContext.DatabaseDetails.Any(dd => dd.Id == databaseDetailId);

            if (!databaseDetailExists)
            {
                // Create and add the DatabaseDetail
                var databaseDetail = new DatabaseDetail
                {
                    Id = databaseDetailId,
                    DataBaseName = "appContextDB",
                    ConnectionString = $"Server=(localdb)\\MSSQLLocalDB;Database=appContextDB;Trusted_Connection=True;",
                    TablesDetails = new List<TableDetail>(),

                };

                dbContext.DatabaseDetails.Add(databaseDetail);
                dbContext.SaveChanges(); // Save the changes to the database
            }
        }
    }
}