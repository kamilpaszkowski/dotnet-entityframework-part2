using Euvic.StaffTraining.WebAPI.Entities.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Euvic.StaffTraining.WebAPI.Infrastructure.EntityFramework.Configurations
{
    public class AttendeesSummaryConfigurations : IEntityTypeConfiguration<AttendeeSummary>
    {
        public void Configure(EntityTypeBuilder<AttendeeSummary> builder)
        {
            builder.HasNoKey();
            builder.ToView("dbo.AttendeeSummary");
        }
    }
}
