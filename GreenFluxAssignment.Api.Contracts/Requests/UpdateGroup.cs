using System;

namespace GreenFluxAssignment.Api.Contracts.Requests
{
    public class UpdateGroup
    {
        public string Name { get; set; }

        public decimal? Capacity { get; set; }
    }
}
