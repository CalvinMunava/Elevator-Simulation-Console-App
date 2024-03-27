using elevator_app.Classes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace elevator_app
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Set Console Settings
            Console.Title = "Elevator Simulation Program";

            // Call Console Greetings
            Greeting();

        }

        // Console App Greeting Method
        static void Greeting()
        {
            Console.WriteLine("Welcome to Elevator Admin Console");
            Console.WriteLine("Here's your complimentary Joke of the Day :)");
            Console.WriteLine("-------------------------------------------------------------");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(ElevatorJoke.GetJoke().jokeText); // Display App Greeting Joke lol
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("-------------------------------------------------------------");
        }








    }
}
