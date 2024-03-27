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

        [Test]
        public void CallElevator_Nearest_Elevator_Is_Called()
        {
            // Arrange
            BuildingManager buildingManager = new BuildingManager(4);
            buildingManager.elevators[0].CurrentFloor = 3;
            buildingManager.elevators[1].CurrentFloor = 7;
            buildingManager.elevators[2].CurrentFloor = 4;
            buildingManager.elevators[3].CurrentFloor = 9;

            // Act
            buildingManager.CallElevator(5, 2);

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(buildingManager.elevators.Any(e => e.CurrentFloor == 4)); // Elevator with floor 4 should be called
        }






    }
}