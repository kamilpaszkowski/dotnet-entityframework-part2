using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Euvic.StaffTraining.WebAPI.Entities;
using Euvic.StaffTraining.WebAPI.Infrastructure.EntityFramework.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Euvic.StaffTraining.WebAPI.Infrastructure.EntityFramework
{
    public class StaffTrainingContext : DbContext
    {
        public const string Schema = "dbo";

        public StaffTrainingContext(DbContextOptions<StaffTrainingContext> options)
            : base(options)
        {
        }

        public DbSet<Attendee> Attendees { get; set; }
        public DbSet<Lecturer> Lecturers { get; set; }
        public DbSet<Technology> Technologies { get; set; }
        public DbSet<Training> Trainings { get; set; }
        public DbSet<TrainingAttendeeStatus> TrainingAttendeeStatuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // -- Bulk registration
            modelBuilder.ApplyConfigurationsFromAssembly(
                Assembly.GetAssembly(typeof(StaffTrainingContext)
                ));

            // -- Single registration
            // modelBuilder.ApplyConfiguration(new AttendeeConfiguration());
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ProcessEntitiesWithDates();

            return base.SaveChangesAsync(cancellationToken);
        }

        private void ProcessEntitiesWithDates()
        {
            var modifiedEntries = ChangeTracker
               .Entries()
               .Where(e => (e.Entity is IUpdateDate && e.State == EntityState.Modified));

            var addedEntries = ChangeTracker
               .Entries()
               .Where(e => (e.Entity is ICreateDate && e.State == EntityState.Added));

            foreach (var entityEntry in modifiedEntries)
            {
                if (entityEntry.Entity is IUpdateDate entity)
                    entity.UpdateDate = DateTime.UtcNow;
            }

            foreach (var entityEntry in addedEntries)
            {
                if (entityEntry.Entity is ICreateDate entity && entity.CreateDate == default)
                    entity.CreateDate = DateTime.UtcNow;

                if (entityEntry.Entity is IUpdateDate updateEntity)
                    updateEntity.UpdateDate = DateTime.UtcNow;
            }
        }
    }
}
