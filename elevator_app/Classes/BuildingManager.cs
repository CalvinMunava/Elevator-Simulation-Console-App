using elevator_app.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace elevator_app.Classes
{
    public class BuildingManager
    {
        public List<Elevator> elevators = new List<Elevator>();
        public Queue<Tuple<int, int, int>> callQueue;

        public BuildingManager(int totalElevators)
        {
            elevators = new List<Elevator>();
            callQueue = new Queue<Tuple<int, int, int>>();
            Random random = new Random();
            lock (elevators) // Lock access to the elevators list during initialization
            {
                for (int i = 0; i < totalElevators; i++)
                {
                    bool ac = random.Next(2) == 0 ? true : false;      //  Set Air Conditioning
                    bool music = random.Next(2) == 0 ? true : false;   //  Set Music Playing
                    Passenger elevator = new Passenger(i + 1, 1, false, Direction.stationary, 0, 12, false, ac, music, -1);
                    elevators.Add(elevator);
                }
            }
        }

        public void CallElevator(int floor, int numPassengers, int DestinationFloor)
        {

                // Find Nearest Available Elevator with capacity that is suffcient
                Elevator nearestElevator = null;
                int minDistance = int.MaxValue;
                bool elevatorFound = false;
                foreach (var elevator in elevators) //Lcoate Nearest Elevator
                {
                    if ((elevator.CurrentCapacity + numPassengers) <= elevator.MaxCapacity)
                    {
                        int distance = Math.Abs(elevator.CurrentFloor - floor);
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            nearestElevator = elevator;
                        }

                        if (!elevatorFound)
                            elevatorFound = true;
                    }
                }

                if (elevatorFound)
                {
                    if (nearestElevator != null)
                    {
                        if (!nearestElevator.IsDoorOpen)
                        {
                            Console.WriteLine($"Elevator {nearestElevator.ElevatorNumber} called to floor {floor}");
                            nearestElevator.LogMovement = true;
                            nearestElevator.MoveTo(floor);
                            nearestElevator.AddTo(numPassengers, DestinationFloor);
                        }
                        else
                        {
                            if (nearestElevator.IsDoorOpen)
                            {
                                Console.WriteLine("Cannot call elevator: elevator's doors are open.");
                            }
                        }
                    }
                }
                else
                {
                    // If no Elevator Available , enque the elevator
                    lock (callQueue) // Lock the call queue to prevent concurrent modifications
                    {
                        callQueue.Enqueue(new Tuple<int, int, int>(floor, numPassengers, DestinationFloor));
                        Console.WriteLine($"No Available Elevator Found,  call has been Queued");
                    }
                }
            
        }

        public void ProcessQueue()
        {
            while (true)
            {
                Tuple<int, int, int> call;
                lock (callQueue) // Lock the call queue to prevent concurrent modifications
                {
                    if (callQueue.Count == 0)
                        break; // Exit the loop if the call queue is empty

                    call = callQueue.Dequeue();
                }

                CallElevator(call.Item1, call.Item2, call.Item3);
            }

        }


    }
}
