using System;
using System.Collections.Generic;
using Euvic.StaffTraining.WebAPI.Infrastructure.EntityFramework.Abstractions;

namespace Euvic.StaffTraining.WebAPI.Entities
{
    public class Attendee : IUpdateDate, ICreateDate
    {
        public long Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int AllowedHours { get; set; }
        public bool IsDeleted { get; set; }
        public long UserProviderId { get; set; }

        public ICollection<TrainingAttendee> Trainings { get; set; }
    }
}
