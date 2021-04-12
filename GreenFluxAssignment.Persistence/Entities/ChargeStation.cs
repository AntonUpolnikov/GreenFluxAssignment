using System;
using System.Collections.Generic;

namespace GreenFluxAssignment.Persistence.Entities
{
    public class ChargeStation
    {
        public Guid Id { get; set; }
        public Guid GroupId { get; set; }
        public string Name { get; set; }
        public ICollection<Connector> Connectors { get; set; } = new List<Connector>();
    }
}
