using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
            ApplicationConfiguration.Initialize();
            Application.Run(ServiceProvider.GetRequiredService<Dashboard>());
        }
        static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddTransient<Dashboard>();

                    services.AddDbContext<AppContext>(options =>
                options.UseSqlServer(context.Configuration.GetConnectionString("AppDatabase"), b => b.MigrationsAssembly("RelationalSchemaNormalizerUI")));

                });
        }
    }
}