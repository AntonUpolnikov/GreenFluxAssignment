using System;
using System.Collections.Generic;

namespace GreenFluxAssignment.Api.Contracts.Responses
{
    public class ChargeStation
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<Connector> Connectors { get; set; }
    }
}
