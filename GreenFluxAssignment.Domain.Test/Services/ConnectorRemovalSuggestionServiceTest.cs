using System;
using System.Collections.Generic;
using GreenFluxAssignment.Domain.Entities;
using GreenFluxAssignment.Domain.Exceptions;
using GreenFluxAssignment.Domain.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GreenFluxAssignment.Domain.Tests.Services
{
    [TestClass]
    public class ConnectorRemovalSuggestionServiceTest
    {
        [TestMethod]
        public void SuggestConnectors_ThrowsError_OnNullGroup()
        {
            // Arrange
            Group group = null;
            decimal excessiveCurrent = 10;

            // Act
            // Assert
            Assert.ThrowsException<InvalidArgumentException>(() =>
                ConnectorRemovalSuggestionService.SuggestConnectors(group, excessiveCurrent));
        }

        [TestMethod]
        public void SuggestConnectors_ThrowsError_OnZeroCurrent()
        {
            // Arrange
            Group group = null;
            decimal excessiveCurrent = 0;

            // Act
            // Assert
            Assert.ThrowsException<InvalidArgumentException>(() =>
                ConnectorRemovalSuggestionService.SuggestConnectors(group, excessiveCurrent));
        }

        [TestMethod]
        public void SuggestConnectors_ThrowsError_OnNegativeCurrent()
        {
            // Arrange
            Group group = null;
            decimal excessiveCurrent = -10;

            // Act
            // Assert
            Assert.ThrowsException<InvalidArgumentException>(() =>
                ConnectorRemovalSuggestionService.SuggestConnectors(group, excessiveCurrent));
        }

        [TestMethod]
        public void SuggestConnectors_ReturnsEmptySuggestions_OnEmptyGroup()
        {
            // Arrange
            decimal excessiveCurrent = 10;
            int expectedSuggestions = 0;

            Group group = CreateDeaultGroup();

            // Act
            var suggestions = ConnectorRemovalSuggestionService.SuggestConnectors(group, excessiveCurrent);

            // Assert
            Assert.AreEqual(expectedSuggestions, suggestions.Count);
        }

        [TestMethod]
        public void SuggestConnectors_ReturnsEmptySuggestions_OnNoMatch()
        {
            // Arrange
            decimal excessiveCurrent = 10;
            int expectedSuggestions = 0;

            Group group = CreateDeaultGroup();
            ChargeStation station = CreateStation(new[] { new Connector(1, 20) });
            ChargeStation anotherStation = CreateStation(new[] { new Connector(1, 30) });
            group.AddStation(station);
            group.AddStation(anotherStation);

            // Act
            var suggestions = ConnectorRemovalSuggestionService.SuggestConnectors(group, excessiveCurrent);

            // Assert
            Assert.AreEqual(expectedSuggestions, suggestions.Count);
        }

        [TestMethod]
        public void SuggestConnectors_ReturnsMultipleEqualSuggestion()
        {
            // Arrange
            decimal excessiveCurrent = 10;
            int expectedSuggestions = 2;
            int expectedConnectors = 1;

            Group group = CreateDeaultGroup();
            ChargeStation station = CreateStation(new[] { new Connector(1, 20), new Connector(2, 30) });
            ChargeStation anotherStation = CreateStation(new[] { new Connector(1, 10), new Connector(2, 10) });
            group.AddStation(station);
            group.AddStation(anotherStation);

            // Act
            var suggestions = ConnectorRemovalSuggestionService.SuggestConnectors(group, excessiveCurrent);

            // Assert
            Assert.AreEqual(expectedSuggestions, suggestions.Count);
            Assert.AreEqual(expectedConnectors, suggestions[0].Count);
        }

        [TestMethod]
        public void SuggestConnectors_ReturnsSingleSuggestion_WithMultipleConnectors()
        {
            // Arrange
            decimal excessiveCurrent = 10;
            int expectedSuggestions = 1;
            int expectedConnectors = 2;

            Group group = CreateDeaultGroup();
            ChargeStation station = CreateStation(new[] { new Connector(1, 20), new Connector(2, 5) });
            ChargeStation anotherStation = CreateStation(new[] { new Connector(1, 5), new Connector(2, 11) });
            group.AddStation(station);
            group.AddStation(anotherStation);

            // Act
            var suggestions = ConnectorRemovalSuggestionService.SuggestConnectors(group, excessiveCurrent);

            // Assert
            Assert.AreEqual(expectedSuggestions, suggestions.Count);
            Assert.AreEqual(expectedConnectors, suggestions[0].Count);
        }

        [TestMethod]
        public void SuggestConnectors_ReturnsMultipleSuggestion_WithMultipleConnectors()
        {
            // Arrange
            decimal excessiveCurrent = 10;
            int expectedSuggestions = 2;
            int expectedConnectors = 2;

            Group group = CreateDeaultGroup();
            ChargeStation station = CreateStation(new[] {
                new Connector(1, 20),
                new Connector(2, 5),
                new Connector(3, 3) });
            ChargeStation anotherStation = CreateStation(new[] {
                new Connector(1, 5),
                new Connector(2, 11),
                new Connector(3, 7) });
            group.AddStation(station);
            group.AddStation(anotherStation);

            // Act
            var suggestions = ConnectorRemovalSuggestionService.SuggestConnectors(group, excessiveCurrent);

            // Assert
            Assert.AreEqual(expectedSuggestions, suggestions.Count);
            Assert.AreEqual(expectedConnectors, suggestions[0].Count);
        }

        [TestMethod]
        public void SuggestConnectors_ReturnsSingleSuggestions_WithLowestConnectorsCount()
        {
            // Arrange
            decimal excessiveCurrent = 10;
            int expectedSuggestions = 1;
            int expectedConnectors = 1;

            Group group = CreateDeaultGroup();
            ChargeStation station = CreateStation(new[] {
                new Connector(1, 20),
                new Connector(3, 3) });
            ChargeStation anotherStation = CreateStation(new[] {
                new Connector(1, 10),
                new Connector(3, 7) });
            group.AddStation(station);
            group.AddStation(anotherStation);

            // Act
            var suggestions = ConnectorRemovalSuggestionService.SuggestConnectors(group, excessiveCurrent);

            // Assert
            Assert.AreEqual(expectedSuggestions, suggestions.Count);
            Assert.AreEqual(expectedConnectors, suggestions[0].Count);
        }

        private Group CreateDeaultGroup()
        {
            Guid id = Guid.NewGuid();
            string name = "Group 1";
            decimal capacity = 200;

            return new Group(id, name, capacity);
        }

        private ChargeStation CreateStation(IEnumerable<Connector> connectors)
        {
            Guid id = Guid.NewGuid();
            string name = $"Station {id}";

            return new ChargeStation(id, name, connectors);
        }
    }
}
