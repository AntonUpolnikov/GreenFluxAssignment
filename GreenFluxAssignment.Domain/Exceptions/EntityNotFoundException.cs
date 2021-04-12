using System;

namespace GreenFluxAssignment.Domain.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public string Type { get; }
        public string Id { get; }

        public EntityNotFoundException(string type, string id)
            : base($"Entity {type} with Id {id} is not found.")
        {
            Type = type;
            Id = id;
        }
    }
}
