using Euvic.StaffTraining.Identity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Euvic.StaffTraining.Identity.Infrastructure.EntityFramework.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Email)
                .IsUnique();

            builder.Property(x => x.Email).IsRequired();

            builder.HasOne(x => x.Settings)
                .WithOne()
                .HasForeignKey<UserSettings>(x => x.UserId);

            builder.OwnsOne(x => x.Profile);
        }
    }
}
