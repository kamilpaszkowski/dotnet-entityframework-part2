using System;

namespace Euvic.StaffTraining.WebAPI.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException(string message) : base(message)
        {
        }
    }
}
