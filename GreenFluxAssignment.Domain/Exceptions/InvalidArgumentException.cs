using System;

namespace GreenFluxAssignment.Domain.Exceptions
{
    public class InvalidArgumentException : Exception
    {
        public string ArgumentName { get; }

        public InvalidArgumentException(string argumentName, string errorMessage)
            : base($"{errorMessage} ({argumentName})")
        {
            ArgumentName = argumentName;
        }
    }
}
