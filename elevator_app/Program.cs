using elevator_app.Classes;
using elevator_app.Enums;
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
        public static Thread StartQueueThread = new Thread(() => ProcessQueue());
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

                    // Start Queue Processing
                    StartQueueThread.Start();
                     

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
            Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("| Command                                                               | Description                                 |");
            Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("| status                                                                | Display real time Elevator Status           |");
            Console.WriteLine("| call - [callToFloor]:[WaitingPassangers]:[DestinationFloor]           | Call an elevator to the specified floor     |");
            Console.WriteLine("| exit                                                                  | Exit the Elevator Management System         |");
            Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------");
        }

        // Default Elevator Movement Threads
        static async Task SimulateElevatorMovement()
        {
            List<Task> tasks = new List<Task>();
            foreach (Elevator elevator in elevatorManager.elevators)
            {
                tasks.Add(MoveElevator(elevator));
            }

            // Wait for all elevator movements to complete
            await Task.WhenAll(tasks);
        }

        private static readonly object destinationLock = new object();
        static async Task MoveElevator(Elevator elevator)
        {
            
            while (true)
            {
                if (!elevator.IsMoving && elevator.CurrentCapacity == 0)
                {
                    lock (elevator) // Lock elevator object to prevent race conditions
                    {
                        // Set direction to stationary when elevator is not moving
                        elevator.Direction = Direction.stationary;

                        lock (destinationLock)
                        {
                            if (elevator.destinationFloors.Count > 0)
                            {
                                // Process the next pending call in the queue
                                int nextFloor = elevator.destinationFloors[0];
                                elevator.MoveTo(nextFloor);
                                elevator.destinationFloors.RemoveAt(0);
                            }
                            else
                            {
                                Random random = new Random();
                                int randomFloor = random.Next(1, 16); // Random floor between 1 and 15
                                int randomUsers = random.Next(1, 12); // Random floor between 1 and 12
                                int randomDestination = random.Next(1, 12); // Random floor between 1 and 12
                                elevator.MoveTo(randomFloor);
                                elevator.AddTo(randomUsers, randomDestination);
                                elevator.RemoveFrom(elevator.CurrentCapacity);
                            }
                        }
                    }
                }
                await Task.Delay(1500);// Delay for 0.1 Seconds                                
            }
        }     
        static void ProcessQueue()
        {
            elevatorManager.ProcessQueue(); // Process the call queue
            Thread.Sleep(1000);
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
            else if (userInput.ToLower() == "status")
            {
                Console.Clear();
                DisplayElevatorStatus();
            }
            else if (userInput.ToLower().StartsWith("call"))
            {
                // Split the user input by spaces to separate the command and its arguments
                string[] inputParts = userInput.Split(' ');

                // Check if the input has the correct number of parts and if it starts with the correct command
                if (inputParts.Length == 3 && inputParts[0].Trim().Equals("call", StringComparison.OrdinalIgnoreCase) &&
                    inputParts[1].Trim().Equals("-", StringComparison.OrdinalIgnoreCase))
                {
                    // Extract floor and passengers from the input
                    string[] argumentParts = inputParts[2].Split(':');
                    if (argumentParts.Length == 3 && 
                        int.TryParse(argumentParts[0], out int floor) && 
                        int.TryParse(argumentParts[1], out int passengers) &&
                        int.TryParse(argumentParts[2], out int destinationFloor))
                    {
                        if (floor >= 0 && floor <= 15 && 
                            destinationFloor >= 0 && destinationFloor <= 15)
                        {
                            // Call an elevator to the specified floor
                            Console.WriteLine($"Calling Nearest Elevator to floor {floor}.");
                            elevatorManager.CallElevator(floor, passengers, destinationFloor);
                            foreach(var elevator in elevatorManager.elevators)
                            {
                                elevator.LogMovement = false;
                            }
                            DisplayElevatorStatus();
                        }
                        else
                        {
                            Console.WriteLine("Invalid floor number. Please enter a floor number between 1 and 15.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid floor or passengers specified.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid command format. Please use 'call - [floor]:[passengers]'.");
                }
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
