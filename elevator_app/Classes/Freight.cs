using elevator_app.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace elevator_app.Classes
{
    public class Freight : Elevator  // Elevator Child Class : Passanger
    {
        // Properties
        public bool MaximumLoad { get; private set; }
        public DoorType DoorType { get; private set; }


        // Default Constructors
        public Freight() : base()
        {
        }

        public Freight(int elevatorNumber, int initialFloor, bool isMoving, Direction direction, int currentCapacity, int maxCapacity, bool logMovement, int destinationFloor, bool maximumLoad, DoorType doorType)
            : base(elevatorNumber, initialFloor, isMoving, direction, currentCapacity, maxCapacity, logMovement, destinationFloor)
        {
            MaximumLoad = maximumLoad;
            DoorType = doorType;
        }


        // Methods
        public override void AddTo(int numItems, int destinationFloor)
        {
            if (CurrentCapacity + numItems <= MaxCapacity)
            {
                CurrentCapacity += numItems;
                if (LogMovement)
                    Console.WriteLine($"{numItems} Items added to elevator {ElevatorNumber}.");

                // Set destination After adding
                this.DestinationFloor = destinationFloor;

                // Add the new destination floor to the elevator
                AddDestinationFloor(destinationFloor);

                if (CurrentFloor != DestinationFloor)
                {

                    MoveTo(DestinationFloor);  // Move elevator to destination floor
                }
                else
                {
                    SetRandomDestinationFloor();  // Set New Random Floor
                }
            }
            else
            {
                Console.WriteLine($"Adding {numItems} Items would exceed elevator {ElevatorNumber}'s capacity of {MaxCapacity}.");
            }
        }

        public override void RemoveFrom(int numItems)
        {
            CurrentCapacity -= numItems;
            if (CurrentCapacity < 0)
            {
                CurrentCapacity = 0;
                if (LogMovement)
                    Console.WriteLine($"{numItems} were removed from elevator {ElevatorNumber}.");
            }



            if (CurrentCapacity == 0)
            {
                SetRandomDestinationFloor();
            }
        }

        public void SetRandomDestinationFloor()
        {
            Random random = new Random();
            int randomFloor = random.Next(1, 16);  // Only 15 floors in building
            while (randomFloor == CurrentFloor)   // Ensure destination floor is different from current Floor
            {
                randomFloor = random.Next(1, 16);
            }
            Direction = Direction.stationary;
            IsMoving = false;
            DestinationFloor = randomFloor;    // Assign the random floor as the destination floor
            if (LogMovement)
                Console.WriteLine($"Elevator {ElevatorNumber} selected destination floor {randomFloor}.");

        }




    }
}
