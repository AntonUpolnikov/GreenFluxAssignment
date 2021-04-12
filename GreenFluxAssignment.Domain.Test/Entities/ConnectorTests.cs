using GreenFluxAssignment.Domain.Entities;
using GreenFluxAssignment.Domain.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GreenFluxAssignment.Domain.Tests.Entities
{
    [TestClass]
    public class ConnectorTests
    {
        [TestMethod]
        public void Connector_ConstructedProperly()
        {
            // Arrange
            int id = 1;
            decimal current = 10;

            // Act
            Connector connector = new Connector(id, current);

            // Assert
            Assert.AreEqual(id, connector.Id);
            Assert.AreEqual(current, connector.MaxCurrent);
        }

        [TestMethod]
        public void ChangeCurrent_ThrowsException_OnZeroCurrent()
        {
            // Arrange
            Connector connector = new Connector(1, 20);
            decimal newCurrent = 0;

            // Act
            // Assert
            Assert.ThrowsException<InvalidArgumentException>(() => connector.ChangeCurrent(newCurrent));
        }

        [TestMethod]
        public void ChangeCurrent_ThrowsException_OnNegativeCurrent()
        {
            // Arrange
            Connector connector = new Connector(1, 20);
            decimal newCurrent = -10;

            // Act
            // Assert
            Assert.ThrowsException<InvalidArgumentException>(() => connector.ChangeCurrent(newCurrent));
        }

        [TestMethod]
        public void ChangeCurrent_UpdatesConnectorsCurrent()
        {
            // Arrange
            Connector connector = new Connector(1, 20);
            decimal newCurrent = 5;
            decimal expectedCurrent = 5;

            // Act
            connector.ChangeCurrent(newCurrent);

            // Assert
            Assert.AreEqual(connector.MaxCurrent, expectedCurrent);
        }
    }
}
