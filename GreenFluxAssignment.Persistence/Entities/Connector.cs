using System;

namespace GreenFluxAssignment.Persistence.Entities
{
    public class Connector
    {
        public Guid ChargeStationId { get; set; }
        public int Id { get; set; }
        public decimal MaxCurrent { get; set; }
    }
}
