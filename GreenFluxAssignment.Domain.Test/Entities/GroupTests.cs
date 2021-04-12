using System;
using System.Linq;
using GreenFluxAssignment.Domain.Entities;
using GreenFluxAssignment.Domain.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GreenFluxAssignment.Domain.Tests.Entities
{
    [TestClass]
    public class GroupTests
    {
        [TestMethod]
        public void Group_ConstructedProperly()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            string name = "Group 1";
            decimal capacity = 200;

            // Act
            Group group = new Group(id, name, capacity);

            // Assert
            Assert.AreEqual(id, group.Id);
            Assert.AreEqual(name, group.Name);
            Assert.AreEqual(capacity, group.Capacity);
        }

        [TestMethod]
        public void ChangeName_UpdatesGroupName()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            string name = "Group 1";
            decimal capacity = 200;
            string newName = "Group 11";

            Group group = new Group(id, name, capacity);

            // Act
            group.ChangeName(newName);

            // Assert
            Assert.AreEqual(newName, group.Name);
        }

        [TestMethod]
        public void ChangeName_ThrowsException_OnNull()
        {
            // Arrange        
            Group group = CreateDeaultGroup(200);
            string newName = null;

            // Act
            // Assert
            Assert.ThrowsException<InvalidArgumentException>(() => group.ChangeName(newName));
        }

        [TestMethod]
        public void ChangeName_ThrowsException_OnEmptyValue()
        {
            // Arrange        
            Group group = CreateDeaultGroup(200);
            string newName = string.Empty;

            // Act
            // Assert
            Assert.ThrowsException<InvalidArgumentException>(() => group.ChangeName(newName));
        }

        [TestMethod]
        public void ChangeCapacity_UpdatesCapacity()
        {
            // Arrange
            decimal capacity = 200;
            decimal stationsCapacity = 100;
            decimal newCapacity = 150;

            Group group = CreateDeaultGroup(capacity);
            group.AddStation(CreateStation(stationsCapacity));

            // Act
            group.ChangeCapacity(newCapacity);

            // Assert
            Assert.AreEqual(newCapacity, group.Capacity);
        }

        [TestMethod]
        public void ChangeCapacity_ThrowsException_OnInsufficientCapacity()
        {
            // Arrange
            decimal capacity = 200;
            decimal stationsCapacity = 100;
            decimal newCapacity = 50;

            Group group = CreateDeaultGroup(capacity);
            group.AddStation(CreateStation(stationsCapacity));

            // Act
            // Assert
            Assert.ThrowsException<ExcessiveCurrentException>(() => group.ChangeCapacity(newCapacity));
        }

        [TestMethod]
        public void AddStation_AddsStationToGroup()
        {
            // Arrange
            decimal capacity = 200;
            decimal stationsCapacity = 100;
            Group group = CreateDeaultGroup(capacity);
            ChargeStation station = CreateStation(stationsCapacity);

            int expectedStationsCount = 1;
            decimal expectedCurrent = 100;

            // Act
            group.AddStation(station);

            // Assert
            Assert.AreEqual(expectedStationsCount, group.Stations.Count);
            Assert.AreEqual(expectedCurrent, group.TotalCurrent);
        }

        [TestMethod]
        public void AddStation_ThrowsException_OnInsufficientCapacity()
        {
            // Arrange
            decimal capacity = 200;
            decimal stationsCapacity = 100;
            decimal newStationsCapacity = 150;

            Group group = CreateDeaultGroup(capacity);
            ChargeStation station = CreateStation(stationsCapacity);
            group.AddStation(station);

            ChargeStation newStation = CreateStation(newStationsCapacity);

            // Act
            // Assert
            Assert.ThrowsException<ExcessiveCurrentException>(() => group.AddStation(newStation));
        }

        [TestMethod]
        public void RemoveSatations_ThrowsException_OnAbsentStation()
        {
            // Arrange
            decimal capacity = 200;
            decimal stationsCapacity = 100;
            Guid absentId = Guid.NewGuid();

            Group group = CreateDeaultGroup(capacity);
            ChargeStation station = CreateStation(stationsCapacity);
            group.AddStation(station);

            // Act
            // Assert
            Assert.ThrowsException<EntityNotFoundException>(() => group.RemoveStation(absentId));
        }

        [TestMethod]
        public void RemoveSatations_RemovesStationFromGroup()
        {
            // Arrange
            decimal capacity = 200;
            decimal stationsCapacity = 100;
            Guid removedId = Guid.NewGuid();

            Group group = CreateDeaultGroup(capacity);
            ChargeStation station = CreateStation(stationsCapacity);
            ChargeStation stationToRemove = CreateStation(stationsCapacity, removedId);
            group.AddStation(station);
            group.AddStation(stationToRemove);

            int expectedStationsCount = 1;
            decimal expectedCurrent = 100;

            // Act
            ChargeStation removed = group.RemoveStation(removedId);

            // Assert
            Assert.AreEqual(expectedStationsCount, group.Stations.Count);
            Assert.AreSame(stationToRemove, removed);
            Assert.AreEqual(expectedCurrent, group.TotalCurrent);
        }

        [TestMethod]
        public void ChangeStationName_ThrowsException_OnAbsentStation()
        {
            // Arrange
            decimal capacity = 200;
            decimal stationsCapacity = 100;
            Guid absentId = Guid.NewGuid();
            string newName = "Station 123";

            Group group = CreateDeaultGroup(capacity);
            ChargeStation station = CreateStation(stationsCapacity);
            group.AddStation(station);

            // Act
            // Assert
            Assert.ThrowsException<EntityNotFoundException>(() => group.ChangeStationName(absentId, newName));
        }

        [TestMethod]
        public void ChangeStationName_UpdatesStationName()
        {
            // Arrange
            decimal capacity = 200;
            decimal stationsCapacity = 100;
            Guid stationId = Guid.NewGuid();
            string newName = "Station 123";

            Group group = CreateDeaultGroup(capacity);
            ChargeStation station = CreateStation(stationsCapacity, stationId);
            group.AddStation(station);

            // Act
            group.ChangeStationName(stationId, newName);

            // Assert
            Assert.AreEqual(newName, station.Name);
        }

        [TestMethod]
        public void ChangeConnectorCurrent_ThrowsException_OnAbsentStation()
        {
            // Arrange
            decimal capacity = 200;
            decimal connectorCurrent = 100;
            Guid absentId = Guid.NewGuid();
            decimal newCurrent = 150;
            int connectorId = 1;

            Group group = CreateDeaultGroup(capacity);
            ChargeStation station = CreateStation(connectorCurrent);
            group.AddStation(station);

            // Act
            // Assert
            Assert.ThrowsException<EntityNotFoundException>(() => group.ChangeConnectorCurrent(absentId, connectorId, newCurrent));
        }

        [TestMethod]
        public void ChangeConnectorCurrent_ThrowsException_OnAbsentConnector()
        {
            // Arrange
            decimal capacity = 200;
            decimal connectorCurrent = 100;
            Guid stationId = Guid.NewGuid();
            decimal newCurrent = 150;
            int connectorId = 5;

            Group group = CreateDeaultGroup(capacity);
            ChargeStation station = CreateStation(connectorCurrent, stationId);
            group.AddStation(station);

            // Act
            // Assert
            Assert.ThrowsException<EntityNotFoundException>(() => group.ChangeConnectorCurrent(stationId, connectorId, newCurrent));
        }

        [TestMethod]
        public void ChangeConnectorCurrent_ThrowsException_OnInsufficientCapacity()
        {
            // Arrange
            decimal capacity = 200;
            decimal connectorCurrent = 100;
            Guid stationId = Guid.NewGuid();
            decimal newCurrent = 250;
            int connectorId = 1;

            Group group = CreateDeaultGroup(capacity);
            ChargeStation station = CreateStation(connectorCurrent, stationId);
            group.AddStation(station);

            // Act
            // Assert
            Assert.ThrowsException<ExcessiveCurrentException>(() => group.ChangeConnectorCurrent(stationId, connectorId, newCurrent));
        }

        [TestMethod]
        public void ChangeConnectorCurrent_UpdatesConnectorCapacity()
        {
            // Arrange
            decimal capacity = 200;
            decimal connectorCurrent = 100;
            Guid stationId = Guid.NewGuid();
            decimal newCurrent = 150;
            int connectorId = 1;
            decimal expectedCurrent = 150;

            Group group = CreateDeaultGroup(capacity);
            ChargeStation station = CreateStation(connectorCurrent, stationId);
            group.AddStation(station);

            // Act
            group.ChangeConnectorCurrent(stationId, connectorId, newCurrent);

            // Assert
            Assert.AreEqual(newCurrent, group.Stations.First().Value.Connectors.First().Value.MaxCurrent);
            Assert.AreEqual(expectedCurrent, group.TotalCurrent);
        }

        [TestMethod]
        public void AddConnector_ThrowsException_OnAbsentStation()
        {
            // Arrange
            decimal capacity = 200;
            decimal connectorCurrent = 100;
            Guid absentId = Guid.NewGuid();
            decimal newConnectorCurrent = 50;

            Group group = CreateDeaultGroup(capacity);
            ChargeStation station = CreateStation(connectorCurrent);
            group.AddStation(station);

            // Act
            // Assert
            Assert.ThrowsException<EntityNotFoundException>(() => group.AddConnector(absentId, newConnectorCurrent));
        }

        [TestMethod]
        public void AddConnector_ThrowsException_OnInsufficientCapacity()
        {
            // Arrange
            decimal capacity = 200;
            decimal connectorCurrent = 100;
            Guid stationId = Guid.NewGuid();
            decimal newConnectorCurrent = 150;

            Group group = CreateDeaultGroup(capacity);
            ChargeStation station = CreateStation(connectorCurrent, stationId);
            group.AddStation(station);

            // Act
            // Assert
            Assert.ThrowsException<ExcessiveCurrentException>(() => group.AddConnector(stationId, newConnectorCurrent));
        }

        [TestMethod]
        public void AddConnector_AddsConnectorToStation()
        {
            // Arrange
            decimal capacity = 200;
            decimal connectorCurrent = 100;
            Guid stationId = Guid.NewGuid();
            decimal newConnectorCurrent = 50;

            Group group = CreateDeaultGroup(capacity);
            ChargeStation station = CreateStation(connectorCurrent, stationId);
            group.AddStation(station);

            decimal expectedCurrent = 150;

            // Act
            Connector connector = group.AddConnector(stationId, newConnectorCurrent);

            // Assert
            Assert.AreEqual(newConnectorCurrent, connector.MaxCurrent);
            Assert.AreEqual(expectedCurrent, group.TotalCurrent);
        }

        [TestMethod]
        public void RemoveConnector_ThrowsException_OnAbsentStation()
        {
            // Arrange
            decimal capacity = 200;
            decimal connectorCurrent = 100;
            int connectorId = 1;
            Guid absentId = Guid.NewGuid();

            Group group = CreateDeaultGroup(capacity);
            ChargeStation station = CreateStation(connectorCurrent);
            group.AddStation(station);

            // Act
            // Assert
            Assert.ThrowsException<EntityNotFoundException>(() => group.RemoveConnector(absentId, connectorId));
        }

        [TestMethod]
        public void RemoveConnector_RemovesConnectorFromStation()
        {
            // Arrange
            decimal capacity = 200;
            decimal connectorCurrent = 100;
            int connectorId = 1;
            Guid stationId = Guid.NewGuid();

            Group group = CreateDeaultGroup(capacity);
            ChargeStation station = CreateStation(connectorCurrent, stationId);
            group.AddStation(station);

            decimal expectedCurrent = 0;
            decimal expectedStationsCount = 0;

            // Act
            Connector connector = group.RemoveConnector(stationId, connectorId);

            // Assert
            Assert.AreEqual(connectorCurrent, connector.MaxCurrent);
            Assert.AreEqual(expectedCurrent, group.TotalCurrent);
            Assert.AreEqual(expectedStationsCount, group.Stations.Count);
        }

        private Group CreateDeaultGroup(decimal capacity)
        {
            Guid id = Guid.NewGuid();
            string name = "Group 1";

            return new Group(id, name, capacity);
        }

        private ChargeStation CreateStation(decimal totalCurrent, Guid? id = null)
        {
            id ??= Guid.NewGuid();
            string name = $"Station {id}";

            return new ChargeStation(id.Value, name, totalCurrent);
        }
    }
}
