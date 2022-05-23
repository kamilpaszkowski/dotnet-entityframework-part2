using System.Collections.Generic;
using Euvic.StaffTraining.WebAPI.Entities;
using Euvic.StaffTraining.WebAPI.Entities.Enums;

namespace Euvic.StaffTraining.WebAPI.Infrastructure.EntityFramework.Configurations.Seed
{
    public static class TrainingAttendeeStatusesSeed
    {
        public static IEnumerable<TrainingAttendeeStatus> GetSeed() =>
            new List<TrainingAttendeeStatus>()
            {
                new TrainingAttendeeStatus()
                {
                    Id = (int)TrainingAttendeeStatuses.Interested,
                    Name = TrainingAttendeeStatuses.Interested.ToString()
                },
                new TrainingAttendeeStatus()
                {
                    Id = (int)TrainingAttendeeStatuses.Confirmed,
                    Name = TrainingAttendeeStatuses.Confirmed.ToString()
                },
                new TrainingAttendeeStatus()
                {
                    Id = (int)TrainingAttendeeStatuses.Declined,
                    Name = TrainingAttendeeStatuses.Declined.ToString()
                }
            };
    }
}
