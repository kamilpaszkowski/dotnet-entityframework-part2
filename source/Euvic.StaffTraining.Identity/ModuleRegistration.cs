using Euvic.StaffTraining.Identity.Abstractions;
using Euvic.StaffTraining.Identity.Infrastructure.EntityFramework;
using Euvic.StaffTraining.Identity.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;

namespace Euvic.StaffTraining.Identity
{
    public static class ModuleRegistration
    {
        public static void AddIdentity(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<IdentityContext>(
                options => options.UseSqlServer(connectionString,
                config => config.MigrationsHistoryTable(
                    HistoryRepository.DefaultTableName, IdentityContext.Schema)));

            services.AddScoped<IIdentityProvider, IdentityProvider>();
        }
    }
}
