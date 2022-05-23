using Euvic.StaffTraining.WebAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Euvic.StaffTraining.WebAPI.Infrastructure.EntityFramework.Configurations
{
    public class AttendeeConfiguration : IEntityTypeConfiguration<Attendee>
    {
        public void Configure(EntityTypeBuilder<Attendee> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(x => x.Firstname)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.Lastname)
                .HasMaxLength(50)
                .IsRequired();

            // builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
