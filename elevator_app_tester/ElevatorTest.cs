using elevator_app.Classes;
using NUnit.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using elevator_app.Enums;

namespace elevator_app_tester
{
    [TestClass]
    public class ElevatorTest
    {
        public static Passenger elevator = new Passenger(1, 0, false, Direction.stationary, 0, 12, false, true, true, 10);

        [Test]
        public void Elevator_Moves_To_Target_Floor()
        {
            // Arrange
            Passenger elevator = new Passenger();
            int targetFloor = 5;

            // Act
            elevator.MoveTo(targetFloor);

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(targetFloor, elevator.CurrentFloor);
        }
        





    }
}