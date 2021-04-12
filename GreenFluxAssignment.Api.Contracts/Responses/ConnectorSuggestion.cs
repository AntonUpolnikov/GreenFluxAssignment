using System;

namespace GreenFluxAssignment.Api.Contracts.Responses
{
    public class ConnectorSuggestion
    {
        public Guid StationId { get; set; }
        public int ConnectorId { get; set; }
    }
}
