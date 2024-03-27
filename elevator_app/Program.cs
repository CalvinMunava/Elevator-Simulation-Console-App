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

            // Authenticate User
            try
            {
                if (AuthenticateUser().Result) // Check User Access 
                {
                    // User has been authorized
                }
                else
                {
                    Console.WriteLine("Authentication failed. Exiting...");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("-------------------------------------------------------------");
                Console.WriteLine("An Error Has Occoured whilst proccessing your Request");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.ToString());
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("-------------------------------------------------------------");
            }

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

        // Console Admin Login Method
        static async Task<bool> AuthenticateUser()
        {
            List<string> validLoginCodes = new List<string> { "1234", "5678", "abcd" };
            bool isAuthenticated = false;

            // Request Login Code
            int attempts = 0;
            string loginCode;

            do
            {
                Console.WriteLine("Please enter your login code:");
                loginCode = ReadPassword();

                // Check if the entered code is valid
                if (validLoginCodes.Contains(loginCode))
                {
                    return true;
                }
                else
                {
                    attempts++;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Incorrect login code. Please try again.");
                    Console.ResetColor();
                }
            } while (attempts < 3);

            // If authentication failed after 3 attempts, exit the application
            if (!isAuthenticated)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Too many incorrect attempts. System will Shut Down in 5 Seconds");
                Console.ResetColor();
                // Countdown for 5 seconds
                for (int i = 5; i > 0; i--)
                {
                    Console.WriteLine($"Shutting down in {i} seconds...");
                    Thread.Sleep(1000); // Wait for 1 second
                }

                // Exit the application
                Environment.Exit(0);
            }
            return isAuthenticated;

        }
        static string ReadPassword()
        {
            string password = "";
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);

                // Ignore any key that is not a valid character
                if (!char.IsControl(key.KeyChar))
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }
                else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    // If Backspace is pressed, remove the last character from password
                    password = password.Remove(password.Length - 1);
                    Console.Write("\b \b");
                }
            } while (key.Key != ConsoleKey.Enter);

            Console.WriteLine();
            return password;
        }









    }
}
