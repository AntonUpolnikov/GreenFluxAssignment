using System;
using GreenFluxAssignment.Domain.Entities;

namespace GreenFluxAssignment.Domain.Exceptions
{
    public class ExcessiveCurrentException : Exception
    {
        public Group Group { get; }
        public decimal ExcessiveCurrent { get; }

        public ExcessiveCurrentException(Group group, decimal excessiveCurrent)
            : base("Excessive current capacity.")
        {
            Group = group;
            ExcessiveCurrent = excessiveCurrent;
        }
    }
}
