using System;
using System.Collections.Generic;
using GreenFluxAssignment.Domain.Exceptions;

namespace GreenFluxAssignment.Domain.Entities
{
    public class Group
    {
        internal decimal TotalCurrent { get; set; } = 0m;

        public Guid Id { get; }
        public string Name { get; private set; }
        public decimal Capacity { get; private set; }
        public Dictionary<Guid, ChargeStation> Stations { get; } = new Dictionary<Guid, ChargeStation>();

        public Group(Guid id, string name, decimal capacity)
        {
            Id = id;
            Capacity = capacity;

            ChangeName(name);
        }

        public void ChangeName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new InvalidArgumentException(nameof(name), "Name cannot be null or empty.");
            }

            Name = name;
        }

        public void ChangeCapacity(decimal newCapacity)
        {
            if (newCapacity < TotalCurrent)
            {
                throw new ExcessiveCurrentException(this, newCapacity - TotalCurrent);
            }

            Capacity = newCapacity;
        }

        public void AddStation(ChargeStation station)
        {
            if (TotalCurrent + station.TotalCurrent > Capacity)
            {
                throw new ExcessiveCurrentException(this, TotalCurrent + station.TotalCurrent - Capacity);
            }

            TotalCurrent += station.TotalCurrent;
            Stations.Add(station.Id, station);
        }

        public ChargeStation RemoveStation(Guid stationId)
        {
            var station = GetStation(stationId);

            TotalCurrent -= station.TotalCurrent;
            Stations.Remove(stationId);

            return station;
        }

        public void ChangeStationName(Guid stationId, string name)
        {
            var station = GetStation(stationId);
            station.ChangeName(name);
        }

        public void ChangeConnectorCurrent(Guid stationId, int connectorId, decimal newCurrent)
        {
            var station = GetStation(stationId);

            decimal delta = station.EstimateCurrentDelta(connectorId, newCurrent);
            if (TotalCurrent + delta > Capacity)
            {
                throw new ExcessiveCurrentException(this, TotalCurrent + delta - Capacity);
            }

            TotalCurrent += delta;
            station.ChangeConnectorCurrent(connectorId, newCurrent);
        }

        public Connector AddConnector(Guid stationId, decimal maxCurrent)
        {
            if (TotalCurrent + maxCurrent > Capacity)
            {
                throw new ExcessiveCurrentException(this, TotalCurrent + maxCurrent - Capacity);
            }

            var station = GetStation(stationId);
            var connector = station.AddConnector(maxCurrent);

            TotalCurrent += maxCurrent;
            return connector;
        }

        public Connector RemoveConnector(Guid stationId, int connectorId)
        {
            var station = GetStation(stationId);
            var connector = station.RemoveConnector(connectorId);

            if (station.Connectors.Count == 0)
            {
                RemoveStation(station.Id);
            }

            TotalCurrent -= connector.MaxCurrent;
            return connector;
        }

        private ChargeStation GetStation(Guid stationId)
        {
            if (!Stations.TryGetValue(stationId, out var station))
            {
                throw new EntityNotFoundException(nameof(ChargeStation), stationId.ToString());
            }

            return station;
        }
    }
}
