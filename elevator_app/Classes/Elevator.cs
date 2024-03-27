﻿using elevator_app.Emuns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace elevator_app.Classes
{
    public abstract class Elevator
    {

        // Properties  

        public int ElevatorNumber { get; private set; }
        public int CurrentFloor { get; private set; }
        public Direction Direction { get; private set; } = Direction.stationary;
        public bool IsMoving { get; private set; }
        public int CurrentCapacity { get; set; } = 0;
        public int MaxCapacity { get; set; }
        public bool LogMovement { get; set; } = false;

        // Default Constructors 

        // Parameterless Constructor
        public Elevator()
            {

            }
            
        // Parametered Constructor
        public Elevator(int elevatorNumber, int initialFloor, bool isMoving, Direction direction, int currentCapacity, int maxCapacity, bool logMovement)
            {
                ElevatorNumber = elevatorNumber;
                CurrentFloor = initialFloor;
                Direction = Direction.stationary;
                IsMoving = isMoving;
                CurrentCapacity = currentCapacity;
                MaxCapacity = maxCapacity;
                LogMovement = logMovement;
            }


        // Methods 

        public void MoveTo(int targetFloor)
        {
            if (targetFloor == CurrentFloor)
            {
                Console.WriteLine($"Elevator {ElevatorNumber} is already on floor {CurrentFloor}.");
                return;
            }

            if (targetFloor > CurrentFloor)
            {
                Direction = Direction.up;
                Console.WriteLine($"Elevator {ElevatorNumber} is moving up from floor {CurrentFloor} to floor {targetFloor}.");
            }
            else
            {
                Direction = Direction.down;
                Console.WriteLine($"Elevator {ElevatorNumber} is moving down from floor {CurrentFloor} to floor {targetFloor}.");
            }

            IsMoving = true;

            // Simulate elevator movement
            while (CurrentFloor != targetFloor)
            {
                // Simulate movement delay
                Thread.Sleep(1000);

                // Move the elevator
                if (Direction == Direction.up)
                    CurrentFloor++;
                else
                    CurrentFloor--;

                // Log movement if needed
                if (LogMovement)
                    Console.WriteLine($"Elevator {ElevatorNumber} moving to floor {CurrentFloor}...");
            }

            IsMoving = false;
            Console.WriteLine($"Elevator {ElevatorNumber} has arrived at floor {CurrentFloor}.");
        }

        // Add passengers to the elevator
        public abstract void AddTo(int count);

        // Remove passengers from the elevator
        public abstract void RemoveFrom(int count);

    }
}