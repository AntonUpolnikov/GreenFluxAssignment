using System;

namespace GreenFluxAssignment.Domain.Exceptions
{
    public class ConnectorsLimitException : Exception
    {
        public int MaxConnectors { get; }

        public ConnectorsLimitException(int maxConnectors)
            : base($"Connectors limited to {maxConnectors} per charge station.")
        {
            MaxConnectors = maxConnectors;
        }
    }
}
