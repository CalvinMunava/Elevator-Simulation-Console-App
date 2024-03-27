using elevator_app.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace elevator_app.Classes
{
    public class Freight : Elevator
    {

        public bool MaximumLoad { get; private set; }
        public DoorType DoorType { get; private set; }

        public Freight() : base()
        {
        }

        public Freight(int elevatorNumber, int initialFloor, bool isMoving, Direction direction, int currentCapacity, int maxCapacity, bool logMovement, int destinationFloor, bool maximumLoad, DoorType doorType)
            : base(elevatorNumber, initialFloor, isMoving, direction, currentCapacity, maxCapacity, logMovement, destinationFloor)
        {
            MaximumLoad = maximumLoad;
            DoorType = doorType;
        }

        public override void AddTo(int numItems, int destinationFloor)
        {
            if (CurrentCapacity + numItems <= MaxCapacity)
            {
                CurrentCapacity += numItems;
                if (LogMovement)
                    Console.WriteLine($"{numItems} Items added to elevator {ElevatorNumber}.");


                this.DestinationFloor = destinationFloor;

                if (CurrentFloor != DestinationFloor)
                {

                    MoveTo(DestinationFloor);
                }
                else
                {
                    SetRandomDestinationFloor();
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
            int randomFloor = random.Next(1, 16);
            while (randomFloor == CurrentFloor)
            {
                randomFloor = random.Next(1, 16);
            }
            Direction = Direction.stationary;
            IsMoving = false;
            DestinationFloor = randomFloor;
            if (LogMovement)
                Console.WriteLine($"Elevator {ElevatorNumber} selected destination floor {randomFloor}.");

        }




    }
}
