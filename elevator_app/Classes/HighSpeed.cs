﻿using elevator_app.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace elevator_app.Classes
{
    public class HighSpeed : Elevator // Elevator Child Class : Passanger
    {
        // Properties
        public int MinSpeed { get; private set; }
        public int MaxSpeed { get; private set; }


        // Default Constuctors
        public HighSpeed() : base()
        {
        }

        public HighSpeed(int elevatorNumber, int initialFloor, bool isMoving, Direction direction, int currentCapacity, int maxCapacity, bool logMovement, int destinationFloor, int minSpeed, int maxSpeed)
            : base(elevatorNumber, initialFloor, isMoving, direction, currentCapacity, maxCapacity, logMovement, destinationFloor)
        {
            MinSpeed = minSpeed;
            MaxSpeed = maxSpeed;
        }


        public override void AddTo(int numPassengers, int destinationFloor)
        {
            if (CurrentCapacity + numPassengers <= MaxCapacity)
            {
                CurrentCapacity += numPassengers;
                if (LogMovement)
                    Console.WriteLine($"{numPassengers} passengers entered elevator {ElevatorNumber}.");

                // Set destination After adding
                this.DestinationFloor = destinationFloor;

                // Add the new destination floor to the elevator
                AddDestinationFloor(destinationFloor);


                // Close Doors
                CloseDoor(null);

                if (CurrentFloor != DestinationFloor)
                {

                    MoveTo(DestinationFloor);  // Move elevator to destination floor
                }
                else
                {
                    SetRandomDestinationFloor();  // Set New Random Floor
                }
            }
            else
            {
                Console.WriteLine($"Adding {numPassengers} passengers would exceed elevator {ElevatorNumber}'s capacity of {MaxCapacity}.");
            }
        }

        public override void RemoveFrom(int numPassengers)
        {
            CurrentCapacity -= numPassengers;
            if (CurrentCapacity < 0)
            {
                CurrentCapacity = 0;
                if (LogMovement)
                    Console.WriteLine($"{numPassengers} passengers exited elevator {ElevatorNumber}.");
            }

            if (CurrentCapacity == 0)
            {
                SetRandomDestinationFloor();
            }
        }

        public void SetRandomDestinationFloor()
        {
            Random random = new Random();
            int randomFloor = random.Next(1, 16); // Only 15 floors in building
            while (randomFloor == CurrentFloor)  // Ensure destination floor is different from current Floor
            {
                randomFloor = random.Next(1, 16);
            }
            Direction = Direction.stationary;
            IsMoving = false;
            DestinationFloor = randomFloor; // Assign the random floor as the destination floor
            if (LogMovement)
                Console.WriteLine($"Elevator {ElevatorNumber} selected destination floor {randomFloor}.");

        }


    }
}
