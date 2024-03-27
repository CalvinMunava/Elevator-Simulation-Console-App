using elevator_app.Classes;
using elevator_app.Emuns;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace elevator_app
{
    class Program
    {

        public static BuildingManager elevatorManager = new BuildingManager(4); //Initialise Building to contain only 4 Elevators
        public static Thread SimulateMovementThread = new Thread(() => SimulateElevatorMovement());
        public static Thread UserInputThread = new Thread(() => HandleUserInput());
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
                    Console.WriteLine("Admin User : " + "\x1B[32mlogged in\x1B[0m");

                    // Display List of Elevator Controls
                    DisplayCommandKey();

                    // Start Simulation Thread
                    SimulateMovementThread.Start();

                    // Start User Input Listen Thread
                    UserInputThread.Start();


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
       
        // Read User Input for password
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
        
        // Display Command Key 
        static void DisplayCommandKey()
        {
           
            Console.WriteLine("\nCommand Key:");
            Console.WriteLine("----------------------------------------------------------------------------------------");
            Console.WriteLine("| Command                                | Description                                 |");
            Console.WriteLine("----------------------------------------------------------------------------------------");
            Console.WriteLine("| status                                 | Display real time Elevator Status           |");
            Console.WriteLine("| call - [floor]:[passangers]            | Call an elevator to the specified floor     |");
            Console.WriteLine("| send - [floor]                         | Send an elevator to the specified floor     |");
            Console.WriteLine("| exit                                   | Exit the Elevator Management System         |");
            Console.WriteLine("----------------------------------------------------------------------------------------");
        }

        // Default Elevator Movement Threads
        static void SimulateElevatorMovement()
        {
            foreach (Elevator elevator in elevatorManager.elevators)
            {
                Task.Run(() => MoveElevator(elevator));
            }
        }
        static async Task MoveElevator(Elevator elevator)
        {
            Random random = new Random();
            while (true)
            {
                if (!elevator.IsMoving && elevator.CurrentCapacity == 0)
                {
                    // Set direction to stationary when elevator is not moving
                    elevator.Direction = Direction.stationary;

                    int randomFloor = random.Next(1, 16); // Random floor between 1 and 15
                    int randomUsers = random.Next(1, 12); // Random floor between 1 and 12
                    int randomDestination = random.Next(1, 12); // Random floor between 1 and 12
                    elevator.MoveTo(randomFloor);
                    elevator.AddTo(randomUsers, randomDestination);
                    elevator.RemoveFrom(elevator.CurrentCapacity);
                }
                Thread.Sleep(1500); // Delay for 1.5 Seconds                                
            }
        }
       
        
        // User Input Threads
        static void HandleUserInput()
        {
            while (true)
            {
                string userInput = Console.ReadLine();
                ProcessUserInput(userInput);
            }
        }
        static void ProcessUserInput(string userInput)
        {
            if (userInput.ToLower() == "help")
            {
                Console.Clear();
                DisplayCommandKey();
            }
            if (userInput.ToLower() == "status")
            {
                Console.Clear();
                DisplayElevatorStatus();
            }
            else if (userInput.ToLower() == "exit")
            {
                Console.WriteLine("Exiting the Elevator Management System.");
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("Invalid command. Type 'help' to see available commands.");
            }


        }

        // Display Elevator Status 
        static void DisplayElevatorStatus()
        {
            DisplayCommandKey();
            Console.WriteLine("\nElevator Status:");
            Console.WriteLine("------------------------------------------------------------------------------");
            Console.WriteLine("| Elevator No. | Current Floor | Direction     | Is Moving | Passenger Count |");
            Console.WriteLine("------------------------------------------------------------------------------");
            lock (elevatorManager)
            {
                foreach (Elevator elevator in elevatorManager.elevators)
                {
                    string isMovingStatus = elevator.IsMoving ? "\x1B[32mtrue\x1B[0m" : "\x1B[31mfalse\x1B[0m"; // Green for true, red for false
                    Console.WriteLine($"| {elevator.ElevatorNumber,-13} | {elevator.CurrentFloor,-13} | {elevator.Direction,-13} | {isMovingStatus,-17} | {elevator.CurrentCapacity,-15} |");
                }
            }
            Console.WriteLine("------------------------------------------------------------------------------");
        }




    }
}
