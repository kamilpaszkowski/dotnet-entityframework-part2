using System;

namespace Euvic.StaffTraining.WebAPI.Exceptions
{
    public class DomainException : Exception
    {
        public DomainException(string message) : base(message)
        {
        }
    }
}
