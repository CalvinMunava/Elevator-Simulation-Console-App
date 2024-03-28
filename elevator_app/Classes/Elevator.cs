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

        public List<int> destinationFloors = new List<int>();
        private Timer doorTimer;
        private readonly object moveLock = new object();
        private readonly object floorLock = new object();
      
        
        // Properties  

        public int ElevatorNumber { get; private set; }
        public int CurrentFloor { get;  set; } = 1;
        public Direction Direction { get;  set; } = Direction.stationary;
        public bool IsMoving { get;  set; } = false;
        public int CurrentCapacity { get; set; } = 0;
        public int MaxCapacity { get; set; }
        public bool LogMovement { get; set; } = false;
        public int DestinationFloor { get; set; } = 0;
        public bool IsDoorOpen { get; private set; } = false;
    

        // Default Constructors 
        public Elevator()
        {

        }
           
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
        public void OpenDoor()
        {
            if (!IsDoorOpen)
            {
                IsDoorOpen = true;
                if (LogMovement)
                    Console.WriteLine($"Elevator {ElevatorNumber} doors opened.");

                // Start a timer to close the doors after a delay : 5 seconds
                doorTimer = new Timer(CloseDoor, null, 5000, Timeout.Infinite);
            }
            else
            {
                if (LogMovement)
                    Console.WriteLine($"Elevator {ElevatorNumber} doors are already open.");
            }
        }
    
        public void CloseDoor(object state)
        {
            if (IsDoorOpen)
            {
                IsDoorOpen = false;
                if (LogMovement)
                    Console.WriteLine($"Elevator {ElevatorNumber} doors closed.");
            }
        }


        public void MoveTo(int targetFloor)
        {
            lock (moveLock) // Lock the elevator object to prevent concurrent modifications
            {
                lock (floorLock)
                {
                    if (targetFloor < 0 || targetFloor > 15)
                    {
                        if (LogMovement)
                            Console.WriteLine($"Invalid floor requested: {targetFloor}");
                        return;
                    }


                    if (targetFloor == CurrentFloor)
                    {
                        if (LogMovement)
                            Console.WriteLine($"Elevator {ElevatorNumber} is already on floor {CurrentFloor}.");
                        return;
                    }

                    // Determine direction of movement
                    Direction = targetFloor > CurrentFloor ? Direction.up : Direction.down;

                    lock (moveLock)
                    {
                        IsMoving = true;
                    };
                }
                // Simulate elevator movement
                while (IsMoving)
                {
                    // Simulate movement delay
                    Thread.Sleep(1000);
                    lock (floorLock)
                    {

                        // Move the elevator
                        if (Direction == Direction.up)
                        {
                            if (CurrentFloor < targetFloor)
                            {
                                CurrentFloor++;
                            }
                            else if (CurrentFloor > targetFloor) // If somehow the elevator passed the target floor
                            {
                                CurrentFloor--;
                            }
                            else // Reached the target floor
                            {
                                IsMoving = false;
                                Direction = Direction.stationary;
                            }
                        }
                        else if (Direction == Direction.down)
                        {
                            if (CurrentFloor > targetFloor)
                            {
                                CurrentFloor--;
                            }
                            else if (CurrentFloor < targetFloor) // If somehow the elevator passed the target floor
                            {
                                CurrentFloor++;
                            }
                            else // Reached the target floor
                            {
                                IsMoving = false;
                                Direction = Direction.stationary;
                            }
                        }
                    }
                    // Log movement
                    if (LogMovement)
                    {
                        Console.WriteLine($"Elevator {ElevatorNumber} is on floor {CurrentFloor}...");
                    }
                }

                if (LogMovement)
                {
                    Console.WriteLine($"Elevator {ElevatorNumber} has arrived at floor {CurrentFloor}.");
                }

                OpenDoor();

                
            }

        }

        // Add passengers to the elevator
        public abstract void AddTo(int numPassangers, int destinationFloor);

        // Remove passengers from the elevator
        public abstract void RemoveFrom(int numPassangers);
        
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
