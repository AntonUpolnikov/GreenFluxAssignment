using System;

namespace GreenFluxAssignment.Persistence.Exceptions
{
    public class ConcurrencyConflictException : Exception
    {
        public string Id { get; }
        public ConcurrencyConflictException(string id, Exception exception)
            : base(
                  $"Unable to update group with Id: {id}. Entity has already been updated by other transaction.",
                  exception)
        {
            Id = id;
        }
    }
}
