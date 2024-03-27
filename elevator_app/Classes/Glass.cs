using elevator_app.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace elevator_app.Classes
{
    public class Glass :Elevator
    {
        public GlassType GlassType { get; set; }

        public Glass() : base()
        {
        }

        public Glass(int elevatorNumber, int initialFloor, bool isMoving, Direction direction, int currentCapacity, int maxCapacity, bool logMovement, int destinationFloor, bool maximumLoad, GlassType glassType)
            : base(elevatorNumber, initialFloor, isMoving, direction, currentCapacity, maxCapacity, logMovement, destinationFloor)
        {
            GlassType = glassType;
        }

        public override void AddTo(int numPassengers, int destinationFloor)
        {
            if (CurrentCapacity + numPassengers <= MaxCapacity)
            {
                CurrentCapacity += numPassengers;
                if (LogMovement)
                    Console.WriteLine($"{numPassengers} passengers entered elevator {ElevatorNumber}.");

               
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
                Console.WriteLine($"Adding {numPassengers} passengers would exceed elevator {ElevatorNumber}'s capacity of {MaxCapacity}.");
            }
        }

        public override void RemoveFrom(int numPassengers)
        {
            CurrentCapacity -= numPassengers;
            if (CurrentCapacity < 0)
            {
                CurrentCapacity = 0;
                if (LogMovement)
                    Console.WriteLine($"{numPassengers} passengers exited elevator {ElevatorNumber}.");
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
