using GreenFluxAssignment.Domain.Exceptions;

namespace GreenFluxAssignment.Domain.Entities
{
    public class Connector
    {
        public int Id { get; }
        public decimal MaxCurrent { get; private set; }

        public Connector(int id, decimal current)
        {
            Id = id;
            ChangeCurrent(current);
        }

        internal void ChangeCurrent(decimal current)
        {
            if (current <= 0)
            {
                throw new InvalidArgumentException(nameof(current), "Current cannot be equal or below 0.");
            }

            MaxCurrent = current;
        }
    }
}
