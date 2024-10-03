using BankingSystem.Domain.Interfaces;
using BankingSystem.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BankingSystem.WebAPI
{
    public static class StartupConfiguration
    {
        public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BankingDbContext>(options =>
            {
                var host = configuration.GetValue<string>("postgres-host");
                var user = configuration.GetValue<string>("postgres-user");
                var pass = configuration.GetValue<string>("postgres-pass");
                var db = configuration.GetValue<string>("postgres-db");

                var connstr = $"Host={host};Database={db};Username={user};Password={pass};SSL Mode=Prefer;";

                options.UseNpgsql(connstr, options => options.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds));
                options.UseLazyLoadingProxies();
                options.ConfigureWarnings(warnings => warnings.Ignore(CoreEventId.DetachedLazyLoadingWarning));
            });

            services.AddScoped<IBankingDbContext, BankingDbContext>();

            return services;
        }
    }
}
