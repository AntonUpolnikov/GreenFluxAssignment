using System;

namespace GreenFluxAssignment.Domain.ValueObjects
{
    public struct ConnectorRemovalSuggestion
    {
        public Guid StationId;
        public int ConnectorId;
        public decimal ConnectorCurrent;

        public ConnectorRemovalSuggestion(Guid stationId, int connectorId, decimal connectorCurrent)
        {
            StationId = stationId;
            ConnectorId = connectorId;
            ConnectorCurrent = connectorCurrent;
        }
    }
}
