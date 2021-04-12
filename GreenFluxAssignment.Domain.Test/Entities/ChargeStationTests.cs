using System;
using System.Collections.Generic;
using System.Linq;
using GreenFluxAssignment.Domain.Entities;
using GreenFluxAssignment.Domain.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GreenFluxAssignment.Domain.Tests.Entities
{
    [TestClass]
    public class ChargeStationTests
    {
        private readonly Random currentGenerator = new Random();

        [TestMethod]
        public void ChargeStation_Constructed_WithConnectorCurrent()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            string name = "Station 1";
            decimal connectorCurrent = 10;

            int expectedConntectorsCount = 1;
            int expectedConnectorId = 1;

            // Act
            ChargeStation station = new ChargeStation(id, name, connectorCurrent);

            // Assert
            Assert.AreEqual(id, station.Id);
            Assert.AreEqual(name, station.Name);
            Assert.AreEqual(expectedConntectorsCount, station.Connectors.Count);
            Assert.AreEqual(expectedConnectorId, station.Connectors.First().Value.Id);
        }

        [TestMethod]
        public void ChargeStation_Constructed_WithMultipleConnectors()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            string name = "Station 1";
            int connectorsCount = 3;

            IEnumerable<Connector> connectors = CreateConnectors(connectorsCount);

            // Act
            ChargeStation station = new ChargeStation(id, name, connectors);

            // Assert
            Assert.AreEqual(id, station.Id);
            Assert.AreEqual(name, station.Name);
            Assert.AreEqual(connectorsCount, station.Connectors.Count);
        }

        [TestMethod]
        public void ChangeName_UpdatesStationName()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            string name = "Station 1";
            decimal connectorCurrent = 10;
            string newName = "Station 11";

            ChargeStation station = new ChargeStation(id, name, connectorCurrent);

            // Act
            station.ChangeName(newName);

            // Assert
            Assert.AreEqual(newName, station.Name);
        }

        [TestMethod]
        public void ChangeName_ThrowsException_OnNull()
        {
            // Arrange
            string newName = null;
            ChargeStation station = CreateDefaultStationWithConnectors(1);

            // Act
            // Assert
            Assert.ThrowsException<InvalidArgumentException>(() => station.ChangeName(newName));
        }

        [TestMethod]
        public void ChangeName_ThrowsException_OnEmptyValue()
        {
            // Arrange
            string newName = string.Empty;
            ChargeStation station = CreateDefaultStationWithConnectors(1);

            // Act
            // Assert
            Assert.ThrowsException<InvalidArgumentException>(() => station.ChangeName(newName));
        }

        [TestMethod]
        public void AddConnector_CreatesNewConnector()
        {
            // Arrange
            int connectors = 1;
            decimal newConnectorCurrent = 2;
            int expectedConnectorsCount = 2;

            ChargeStation station = CreateDefaultStationWithConnectors(connectors);

            // Act
            station.AddConnector(newConnectorCurrent);

            // Assert
            Assert.AreEqual(expectedConnectorsCount, station.Connectors.Count);
        }

        [TestMethod]
        public void AddConnector_ThrowsException_OnConnectorsLimitReached()
        {
            // Arrange
            int connectors = 5;
            decimal newConnectorCurrent = 2;

            ChargeStation station = CreateDefaultStationWithConnectors(connectors);
            // Act
            // Assert
            Assert.ThrowsException<ConnectorsLimitException>(() => station.AddConnector(newConnectorCurrent));
        }

        [TestMethod]
        public void AddConnectorWithId_CreatesNewConnector()
        {
            // Arrange
            int connectors = 1;
            decimal newConnectorCurrent = 2;
            int newConnectorsId = 2;
            int expectedConnectorsCount = 2;

            ChargeStation station = CreateDefaultStationWithConnectors(connectors);

            // Act
            station.AddConnector(newConnectorsId, newConnectorCurrent);

            // Assert
            Assert.AreEqual(expectedConnectorsCount, station.Connectors.Count);
        }

        [TestMethod]
        public void AddConnectorWithId_ThrowsException_OnAlreadyUsedId()
        {
            // Arrange
            int connectors = 2;
            int newConnectorsId = 2;
            decimal newConnectorCurrent = 2;

            ChargeStation station = CreateDefaultStationWithConnectors(connectors);

            // Act
            // Assert
            Assert.ThrowsException<InvalidArgumentException>(() => station.AddConnector(newConnectorsId, newConnectorCurrent));
        }

        [TestMethod]
        public void RemoveConnector_ThrowsException_ForAbsentConnector()
        {
            // Arrange
            int connectors = 2;
            int connectorId = 3;

            ChargeStation station = CreateDefaultStationWithConnectors(connectors);

            // Act
            // Assert
            Assert.ThrowsException<EntityNotFoundException>(() => station.RemoveConnector(connectorId));
        }

        [TestMethod]
        public void RemoveConnector_RemovesConnector()
        {
            // Arrange
            int connectors = 2;
            int connectorId = 2;
            int expectedConnectorsCount = 1;

            ChargeStation station = CreateDefaultStationWithConnectors(connectors);

            // Act
            Connector connector = station.RemoveConnector(connectorId);

            // Assert
            Assert.AreEqual(expectedConnectorsCount, station.Connectors.Count);
            Assert.AreEqual(connectorId, connector.Id);
        }


        [TestMethod]
        public void EstimateCurrentDelta_ThrowsException_ForAbsentConnector()
        {
            // Arrange
            int connectors = 2;
            decimal newCurrent = 10;
            int connectorId = 3;

            ChargeStation station = CreateDefaultStationWithConnectors(connectors);
            // Act
            // Assert
            Assert.ThrowsException<EntityNotFoundException>(() => station.EstimateCurrentDelta(connectorId, newCurrent));
        }

        [TestMethod]
        public void EstimateCurrentDelta_OnIncreasedCurrent()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            string name = "Station 1";
            decimal connectorCurrent = 10;
            int connectorId = 1;
            decimal newCurrent = 20;
            decimal expectedDelta = 10;

            ChargeStation station = new ChargeStation(id, name, connectorCurrent);

            // Act
            decimal actual = station.EstimateCurrentDelta(connectorId, newCurrent);

            // Assert
            Assert.AreEqual(expectedDelta, actual);
        }

        [TestMethod]
        public void EstimateCurrentDelta_OnDecreasedCurrent()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            string name = "Station 1";
            decimal connectorCurrent = 10;
            int connectorId = 1;
            decimal newCurrent = 5;
            decimal expectedDelta = -5;

            ChargeStation station = new ChargeStation(id, name, connectorCurrent);

            // Act
            decimal actual = station.EstimateCurrentDelta(connectorId, newCurrent);

            // Assert
            Assert.AreEqual(expectedDelta, actual);
        }

        private ChargeStation CreateDefaultStationWithConnectors(int connectorsCount)
        {
            Guid id = Guid.NewGuid();
            string name = "Station 1";
            IEnumerable<Connector> connectors = CreateConnectors(connectorsCount);

            return new ChargeStation(id, name, connectors);
        }

        private List<Connector> CreateConnectors(int connectorsCount)
        {
            return Enumerable.Range(1, connectorsCount)
                .Select(idx => new Connector(idx, currentGenerator.Next(1, 100)))
                .ToList();
        }
    }
}
