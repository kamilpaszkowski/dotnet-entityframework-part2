using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Euvic.StaffTraining.Common
{
    public static class ModuleExtensions
    {
        public static void AddConfiguration<TOptions>(this IServiceCollection services, IConfiguration configuration, string sectionName)
            where TOptions : class
        {
            services.Configure<TOptions>(
                sectionName == null ? configuration : configuration.GetSection(sectionName)
            );

            services.AddSingleton(x => x.GetRequiredService<IOptions<TOptions>>().Value);
        }

        public static void AddConfiguration<TOptions>(this IServiceCollection services, IConfiguration configuration)
            where TOptions : class
        {
            services.Configure<TOptions>(configuration.GetSection(typeof(TOptions).Name));
            services.AddSingleton(x => x.GetRequiredService<IOptions<TOptions>>().Value);
        }

        public static async void Migrate<TDbContext>(this IApplicationBuilder applicationBuilder)
           where TDbContext : DbContext
        {
            // AUTOMIGRATION ARE ALLOWED ONLY FOR LOCAL DEVELOPMENT

            var scopeFactory = applicationBuilder
                .ApplicationServices
                .GetRequiredService<IServiceScopeFactory>();

            using var scope = scopeFactory.CreateScope();
            using var dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();

            dbContext.Database.Migrate();
        }
    }
}
