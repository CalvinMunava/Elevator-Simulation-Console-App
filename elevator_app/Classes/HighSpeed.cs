using elevator_app.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace elevator_app.Classes
{
    public class HighSpeed : Elevator
    {
        public int MinSpeed { get; private set; }
        public int MaxSpeed { get; private set; }

        public HighSpeed() : base()
        {
        }

        public HighSpeed(int elevatorNumber, int initialFloor, bool isMoving, Direction direction, int currentCapacity, int maxCapacity, bool logMovement, int destinationFloor, int minSpeed, int maxSpeed)
            : base(elevatorNumber, initialFloor, isMoving, direction, currentCapacity, maxCapacity, logMovement, destinationFloor)
        {
            MinSpeed = minSpeed;
            MaxSpeed = maxSpeed;
        }


        public override void AddTo(int numPassengers, int destinationFloor)
        {
            if (CurrentCapacity + numPassengers <= MaxCapacity)
            {
                CurrentCapacity += numPassengers;
                if (LogMovement)
                    Console.WriteLine($"{numPassengers} passengers entered elevator {ElevatorNumber}.");

                // Set destination After adding
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
