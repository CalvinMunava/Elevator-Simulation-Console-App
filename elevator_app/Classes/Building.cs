using elevator_app.Emuns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace elevator_app.Classes
{
    public class Building
    {
        public List<Elevator> elevators = new List<Elevator>();
        public Queue<Tuple<int, int>> callQueue;

        public Building(int totalElevators)
        {
            elevators = new List<Elevator>();
            callQueue = new Queue<Tuple<int, int>>();
            Random random = new Random();
            for (int i = 0; i < totalElevators; i++)
            {
                // Only use Paassanger Type Elevators for this Example
                int randomFloor = 1;                                                             //  Set Starting Position
                int randomPassengers = random.Next(0, 13);                                       //  Set Random Passangers for Elevator
                int randomPassengerLimit = 12;                                                   //  Set Maxmimum Passangers 
                Direction randomDirection = random.Next(2) == 0 ? Direction.up : Direction.down; //  Set Moving Direction
                bool ac = random.Next(2) == 0 ? true : false;                                    //  Set Air Conditioning
                bool music = random.Next(2) == 0 ? true : false;                                 //  Set Music Playing
                int randomDestinationFloor = random.Next(1, 16);                                 //  Set Destination Floor
                Passenger elevator = new Passenger(i + 1, randomFloor, false, randomDirection, randomPassengers, randomPassengerLimit, false, ac, music, randomDestinationFloor);
                elevators.Add(elevator);
            }
        }



    }
}
