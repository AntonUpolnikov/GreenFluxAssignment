namespace GreenFluxAssignment.Api.Controllers
{
    public static class Routes
    {
        public const string GroupPath = "api/Groups";
        public const string ChargeStationsPath = "api/Groups/{groupId}/ChargeStations";
        public const string ConnectorsPath = "api/Groups/{groupId}/ChargeStations/{stationId}/Connectors";

        public const string GroupIdSegment = "{groupId}";
        public const string StationIdSegment = "{stationId}";
        public const string ConnectorIdSegment = "{connectorId}";
    }
}
