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
            // Find Nearest Available Elevator with capacity that is suffcient
            Elevator nearestElevator = null;
            int minDistance = int.MaxValue;
            foreach(var elevator in elevators) //Lcoate Nearest Elevator
            {
                if(!elevator.IsMoving && (elevator.CurrentCapacity + numPassengers) <= elevator.CurrentCapacity)
                {
                    int distance = Math.Abs(elevator.CurrentFloor - floor);
                    if(distance < minDistance)
                    {
                        minDistance = distance;
                        nearestElevator = elevator;
                    }
                }
            }

            if( nearestElevator != null )
            {
                Console.WriteLine($"Elevator {nearestElevator.ElevatorNumber} called to floor {floor}");
                nearestElevator.AddTo(floor, numPassengers);
            }
            else
            {
                // If no Elevator Available , enque the elevator
                callQueue.Enqueue(new Tuple<int, int>(floor, numPassengers));
                Console.WriteLine($"No Available Eevator Found,  call has been Queued");
            }
        }

        public void ProcessQueue(int floor, int numPassengers)
        {
            while(callQueue.Count > 0)
            {
                var call = callQueue.Dequeue();
                CallElevator(call.Item1, call.Item2);
            }


        }


    }
}
