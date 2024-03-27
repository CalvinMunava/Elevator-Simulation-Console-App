using elevator_app.Emuns;
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









       



    }
}
