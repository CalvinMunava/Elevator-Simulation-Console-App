using elevator_app.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace elevator_app.Classes
{
    public abstract class Elevator
    {

        // Properties  

        public int ElevatorNumber { get; private set; }
        public int CurrentFloor { get;  set; } = 1;
        public Direction Direction { get;  set; } = Direction.stationary;
        public bool IsMoving { get;  set; } = false;
        public int CurrentCapacity { get; set; } = 0;
        public int MaxCapacity { get; set; }
        public bool LogMovement { get; set; } = false;
        public int DestinationFloor { get; set; } = -1; // No intial Destination

        protected List<int> destinationFloors = new List<int>();

        // Default Constructors 

        // Parameterless Constructor
        public Elevator()
        {

        }
            
        // Parametered Constructor
        public Elevator(int elevatorNumber, int initialFloor, bool isMoving, Direction direction, int currentCapacity, int maxCapacity, bool logMovement,int destinationFloor)
        {
                ElevatorNumber = elevatorNumber;
                CurrentFloor = initialFloor;
                Direction = direction;
                IsMoving = isMoving;
                CurrentCapacity = currentCapacity;
                MaxCapacity = maxCapacity;
                LogMovement = logMovement;
                DestinationFloor = destinationFloor;
        }


        // Methods 

        public void MoveTo(int targetFloor)
        {
            if (targetFloor == CurrentFloor)
            {
                if (LogMovement)
                    Console.WriteLine($"Elevator {ElevatorNumber} is already on floor {CurrentFloor}.");
                return;
            }

            if (targetFloor > CurrentFloor)
            {
                Direction = Direction.up;
                if (LogMovement)
                    Console.WriteLine($"Elevator {ElevatorNumber} is moving up from floor {CurrentFloor} to floor {targetFloor}.");
            }
            else
            {
                Direction = Direction.down;
                if (LogMovement)
                    Console.WriteLine($"Elevator {ElevatorNumber} is moving down from floor {CurrentFloor} to floor {targetFloor}.");
            }

            IsMoving = true;

            // Simulate elevator movement
            while (CurrentFloor != targetFloor)
            {
                // Simulate movement delay
                Thread.Sleep(1000);

                // Move the elevator
                if (Direction == Direction.up)
                    CurrentFloor++;
                else
                    CurrentFloor--;

                // Log movement if needed
                if (LogMovement)
                    Console.WriteLine($"Elevator {ElevatorNumber} is on floor {CurrentFloor}...");
            }

            IsMoving = false;
            Direction = Direction.stationary;
            if (LogMovement)
                Console.WriteLine($"Elevator {ElevatorNumber} has arrived at floor {CurrentFloor}.");
        }


        // Add passengers to the elevator
        public abstract void AddTo(int count, int destinationFloor);

        // Remove passengers from the elevator
        public abstract void RemoveFrom(int count);
        
        // Pre load Destination Floors
        public virtual void AddDestinationFloor(int floor)
        {
            destinationFloors.Add(floor);
            SortDestinationFloors();
        }

        // Method to sort the destination floors
        protected virtual void SortDestinationFloors()
        {
            QuickSort(destinationFloors, 0, destinationFloors.Count - 1);
        }

        // Quicksort algorithm
        private void QuickSort(List<int> floors, int low, int high)
        {
            if (low < high)
            {
                int pi = Partition(floors, low, high);

                QuickSort(floors, low, pi - 1);
                QuickSort(floors, pi + 1, high);
            }
        }
        private int Partition(List<int> floors, int low, int high)
        {
            int pivot = floors[high];
            int i = (low - 1);

            for (int j = low; j < high; j++)
            {
                if (floors[j] < pivot)
                {
                    i++;
                    int temp = floors[i];
                    floors[i] = floors[j];
                    floors[j] = temp;
                }
            }

            int temp1 = floors[i + 1];
            floors[i + 1] = floors[high];
            floors[high] = temp1;

            return i + 1;
        }



    }
}
