namespace GreenFluxAssignment.Api.Contracts.Requests
{
    public class CreateChargeStation
    {
        public string Name { get; set; }

        public decimal ConnectorMaxCurrent { get; set; }
    }
}
