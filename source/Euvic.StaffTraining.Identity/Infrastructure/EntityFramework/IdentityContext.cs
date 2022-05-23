using Euvic.StaffTraining.Identity.Entities;
using Euvic.StaffTraining.Identity.Infrastructure.EntityFramework.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Euvic.StaffTraining.Identity.Infrastructure.EntityFramework
{
    public class IdentityContext : DbContext
    {
        public const string Schema = "identity";

        public IdentityContext(DbContextOptions<IdentityContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema(Schema);

            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserSettingsConfiguration());
        }
    }
}
