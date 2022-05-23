using Euvic.StaffTraining.WebAPI.Entities;
using Euvic.StaffTraining.WebAPI.Infrastructure.EntityFramework.Configurations.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Euvic.StaffTraining.WebAPI.Infrastructure.EntityFramework.Configurations
{
    public class TrainingAttendeeStatusesConfiguration : IEntityTypeConfiguration<TrainingAttendeeStatus>
    {
        public void Configure(EntityTypeBuilder<TrainingAttendeeStatus> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasData(TrainingAttendeeStatusesSeed.GetSeed());
        }
    }
}
