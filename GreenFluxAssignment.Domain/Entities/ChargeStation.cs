using System;
using System.Collections.Generic;
using System.Linq;
using GreenFluxAssignment.Domain.Exceptions;

namespace GreenFluxAssignment.Domain.Entities
{
    public class ChargeStation
    {
        private const int MaxConnectors = 5;
        private const int MinId = 1;
        private readonly HashSet<int> _availableIds;

        internal decimal TotalCurrent { get; private set; } = 0m;

        public Guid Id { get; }
        public string Name { get; private set; }
        public Dictionary<int, Connector> Connectors { get; } = new Dictionary<int, Connector>();

        public ChargeStation(Guid id, string name, decimal connectorCurrent)
            : this(id, name)
        {
            AddConnector(connectorCurrent);
        }

        public ChargeStation(Guid id, string name, IEnumerable<Connector> connectors)
            : this(id, name)
        {
            foreach (var connector in connectors)
            {
                AddConnector(connector.Id, connector.MaxCurrent);
            }
        }

        private ChargeStation(Guid id, string name)
        {
            _availableIds = new HashSet<int>(Enumerable.Range(MinId, MaxConnectors));

            Id = id;
            ChangeName(name);
        }

        internal void ChangeName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new InvalidArgumentException(nameof(name), "Name cannot be null or empty.");
            }

            Name = name;
        }

        internal Connector AddConnector(decimal connectorCurrent)
        {
            int connectorId = GetNextId();
            return AddConnector(connectorId, connectorCurrent);
        }

        internal Connector AddConnector(int connectorId, decimal connectorCurrent)
        {
            if (!_availableIds.Contains(connectorId))
            {
                throw new InvalidArgumentException(
                    nameof(connectorId),
                    $"Cannot add connector with Id: {connectorId}.");
            }

            Connector connector = new Connector(connectorId, connectorCurrent);

            Connectors.Add(connectorId, connector);
            TotalCurrent += connectorCurrent;
            _availableIds.Remove(connectorId);

            return connector;
        }

        internal Connector RemoveConnector(int connectorId)
        {
            var connector = GetConnector(connectorId);

            Connectors.Remove(connectorId);
            TotalCurrent -= connector.MaxCurrent;

            _availableIds.Add(connectorId);

            return connector;
        }

        internal void ChangeConnectorCurrent(int connectorId, decimal connectorCurrent)
        {
            var connector = GetConnector(connectorId);

            TotalCurrent += (connectorCurrent - connector.MaxCurrent);
            connector.ChangeCurrent(connectorCurrent);
        }

        internal decimal EstimateCurrentDelta(int connectorId, decimal connectorCurrent)
        {
            var connector = GetConnector(connectorId);

            return connectorCurrent - connector.MaxCurrent;
        }

        private int GetNextId()
        {
            if (_availableIds.Count == 0)
            {
                throw new ConnectorsLimitException(MaxConnectors);
            }

            return _availableIds.First();
        }

        private Connector GetConnector(int connectorId)
        {
            if (!Connectors.TryGetValue(connectorId, out var connector))
            {
                throw new EntityNotFoundException(nameof(Connector), connectorId.ToString());
            }

            return connector;
        }
    }
}
