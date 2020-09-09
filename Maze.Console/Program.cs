﻿using Maze.Core.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using Maze.Core.Models;
using static Maze.Core.Models.Helpers;
using Maze.Core.Services;
using static Maze.Core.Services.LoggingService;

namespace Maze.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            // Ask for the file path
            System.Console.WriteLine(@"Welcome to the Maze app.");
            System.Console.WriteLine(string.Empty);

            while (true)
            {
                // Get user input
                System.Console.WriteLine("Enter the path of the definition file. Type 'sample.txt' to run the sample or 'exit'.  Add -l to log path.");
                string input = System.Console.ReadLine();

                // Set up logging
                LogLevel logLevel = LogLevel.None;
                if (input.Contains("-l"))
                {
                    logLevel = LogLevel.Info;
                    input = input.Replace("-l",string.Empty).Trim();
                }
                LoggingService loggingService = new LoggingService(logLevel);

                // Exit if that is what the user typed
                if (input == "exit")
                    break;

                try
                {
                    // Read definition file
                    string[] lines = File.ReadAllLines(input);

                    // Run it through our converter
                    MazeDefinitionConverter mazeDefinitionConverter = new MazeDefinitionConverter(loggingService);
                    var coordinates = mazeDefinitionConverter.GetLaserCoordinates(lines);

                    // Display results
                    System.Console.WriteLine("Laser will exit at {0},{1} going {2}", coordinates.X, coordinates.Y, coordinates.OutDirection.ToString());
                    System.Console.WriteLine("");
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine(ex.Message);
                    System.Console.WriteLine("");
                }
            }
        }
      
    }
}
