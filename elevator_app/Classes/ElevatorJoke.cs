using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace elevator_app.Classes
{
    public class ElevatorJoke // Elevator Jokes Class 😆
    {
        public int jokeID { get; set; }
        public string jokeText { get; set; }
        public static ElevatorJoke GetJoke()
        {
            Random random = new Random();
            int selectedJoke = random.Next(0, 6);
            var Jokes = new ElevatorJoke[]
            {
                new ElevatorJoke { jokeID = 1 , jokeText = "I've recently developed a severe phobia of elevators....\nI'm taking steps to avoid them."},
                new ElevatorJoke { jokeID = 2 , jokeText = "I like elevator jokes....\nThey're very uplifting"},
                new ElevatorJoke { jokeID = 3 , jokeText = "My twin brother prefers to take the stairs, but I like the elevator....\nI guess we are raised differently."},
                new ElevatorJoke { jokeID = 4 , jokeText = "What did the American elevator say to the British elevator?...\nYou lift bro?"},
                new ElevatorJoke { jokeID = 5 , jokeText = "My first time using an elevator was an uplifting experience....\nThe second time let me down"},
                new ElevatorJoke { jokeID = 6 , jokeText = "You should get a job as an elevator....\nIt’s easy to get a raise!"},
            };
            return Jokes[selectedJoke];
        }
    }
}
