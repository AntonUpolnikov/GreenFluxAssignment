using System;
using System.Collections.Generic;

namespace GreenFluxAssignment.Persistence.Entities
{
    public class Group
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Capacity { get; set; }
        public ICollection<ChargeStation> ChargeStations { get; set; } = new List<ChargeStation>();
        public byte[] Timestamp { get; set; }
    }
}
