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
        public Queue<Tuple<int, int>> callQueue;

        public BuildingManager(int totalElevators)
        {
            elevators = new List<Elevator>();
            callQueue = new Queue<Tuple<int, int>>();
            Random random = new Random();
            for (int i = 0; i < totalElevators; i++)
            {
                bool ac = random.Next(2) == 0 ? true : false;      //  Set Air Conditioning
                bool music = random.Next(2) == 0 ? true : false;   //  Set Music Playing
                Passenger elevator = new Passenger(i + 1, 1, false, Direction.stationary, 0, 12, false, ac, music, -1);
                elevators.Add(elevator);
            }
        }

        public void CallElevator(int floor, int numPassengers)
        {
            // Find Nearest Available Elevator with sufficient capacity
            Elevator nearestElevator = null;
            int minDistance = int.MaxValue;
            foreach (var elevator in elevators)
            {
                if (!elevator.IsMoving && (elevator.CurrentCapacity + numPassengers) <= elevator.MaxCapacity)
                {
                    int distance = Math.Abs(elevator.CurrentFloor - floor);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearestElevator = elevator;
                    }
                }
            }

            if (nearestElevator != null)
            {
                Console.WriteLine($"Elevator {nearestElevator.ElevatorNumber} called to floor {floor}");
                nearestElevator.MoveTo(floor); // Move the elevator to the called floor
                nearestElevator.AddTo(numPassengers, floor); // Reversed the parameters
            }
            else
            {
                // If no Elevator Available, enqueue the call
                callQueue.Enqueue(new Tuple<int, int>(floor, numPassengers));
                Console.WriteLine($"No Available Elevator Found, call has been Queued");
            }
        }

        public void ProcessQueue(int floor, int numPassengers)
        {
            int queueCount = callQueue.Count;
            for (int i = 0; i < queueCount; i++)
            {
                var call = callQueue.Dequeue();
                CallElevator(call.Item1, call.Item2);
            }
        }


    }
}
