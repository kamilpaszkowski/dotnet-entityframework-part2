using Euvic.StaffTraining.WebAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Euvic.StaffTraining.WebAPI.Infrastructure.EntityFramework.Configurations
{
    public class LecturerConfiguration : IEntityTypeConfiguration<Lecturer>
    {
        public void Configure(EntityTypeBuilder<Lecturer> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(x => x.Firstname)
                 .HasMaxLength(50)
                 .IsRequired();

            builder.Property(x => x.Lastname)
                 .HasMaxLength(50)
                 .IsRequired();

            builder.Ignore(x => x.TrainingsCount);
            //  builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
