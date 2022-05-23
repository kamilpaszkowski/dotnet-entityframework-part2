using System;
using Euvic.StaffTraining.WebAPI.Entities.Enums;

namespace Euvic.StaffTraining.WebAPI.Entities
{
    public class TrainingAttendee
    {
        public Guid TrainingId { get; set; }
        public long AttendeeId { get; set; }
        public int StatusId { get; set; }

        public TrainingAttendeeStatus Status { get; set; }
        public Training Training { get; set; }
        public Attendee Attendee { get; set; }

        public bool IsConfirmed() => StatusId == (int)TrainingAttendeeStatuses.Confirmed;
    }
}
