using Euvic.StaffTraining.WebAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Euvic.StaffTraining.WebAPI.Infrastructure.EntityFramework.Configurations
{
    public class TrainingConfiguration : IEntityTypeConfiguration<Training>
    {
        public void Configure(EntityTypeBuilder<Training> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .ValueGeneratedNever();

            builder.Property(x => x.Title)
                 .HasMaxLength(200)
                 .IsRequired();

            builder.Property(x => x.Description)
              .HasMaxLength(500);

            builder.HasOne(x => x.Lecturer)
                .WithMany(x => x.Trainings)
                .HasForeignKey(x => x.LecturerId);

            builder.HasOne(x => x.Technology)
                .WithMany()
                .HasForeignKey(x => x.TechnologyId);
        }
    }
}
