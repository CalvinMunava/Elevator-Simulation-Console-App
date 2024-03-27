using elevator_app.Emuns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace elevator_app.Classes
{
    public class Passenger : Elevator
    {
        public bool AC { get; private set; }
        public bool Music { get; private set; }

        public Passenger() : base()
        {
        }

        public Passenger(int elevatorNumber, int initialFloor, bool isMoving, Direction direction, int currentCapacity, int maxCapacity, bool logMovement, bool ac, bool music, int destinationFloor)
            : base(elevatorNumber, initialFloor, isMoving, direction, currentCapacity, maxCapacity, logMovement, destinationFloor)
        {
            AC = ac;
            Music = music;
        }

        public override void AddTo(int count, int destinationFloor)
        {
            if (CurrentCapacity + count <= MaxCapacity)
            {
                CurrentCapacity += count;
                if (LogMovement)
                    Console.WriteLine($"{count} passengers entered elevator {ElevatorNumber}.");

                // Set destination After adding
                this.DestinationFloor = destinationFloor;
                if(CurrentFloor != DestinationFloor) { 
                
                    MoveTo(DestinationFloor); // Move elevator to destination floor
                    RemoveFrom(CurrentCapacity); // Remove Passangers
                    SetRandomDestinationFloor(); // Set New Random Floor
                }
                else
                {
                    RemoveFrom(CurrentCapacity); // Remove from Elevator
                    SetRandomDestinationFloor(); // Set New Random Floor
                }
            }
            else
            {
                Console.WriteLine($"Adding {count} passengers would exceed elevator {ElevatorNumber}'s capacity of {MaxCapacity}.");
            }
        }

        public override void RemoveFrom(int count)
        {
            if (CurrentCapacity - count >= 0)
            {
                CurrentCapacity -= count;
                if (LogMovement)
                    Console.WriteLine($"{count} passengers exited elevator {ElevatorNumber}.");
            }
            else
            {
                Console.WriteLine($"Cannot remove {count} passengers. There are only {CurrentCapacity} passengers in elevator {ElevatorNumber}.");
            }

            if(DestinationFloor == -1)
            {
                SetRandomDestinationFloor();
            }
        }

        public void SetRandomDestinationFloor()
        {
            Random random = new Random();
            int randomFloor = random.Next(1, 16); // only 15 floors in building
            while (randomFloor == CurrentFloor) // Ensure destination floor is different from current Floor
            {
                randomFloor = random.Next(1, 16);
            }
            if (LogMovement)
                Console.WriteLine($"Elevator {ElevatorNumber} selected destination floor {randomFloor}.");

        }





    }
}
