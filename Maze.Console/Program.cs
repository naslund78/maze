using System;
using System.IO;
using System.Collections.Generic;
using Maze.Core.Models;
using Maze.Core.Objects;
using Maze.Core.Services;
using static Maze.Core.Models.Helpers;
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
                System.Console.WriteLine("Enter the path of the definition file. Type 'sample.txt' to run the sample or 'exit'.");
                string input = System.Console.ReadLine();

                // Set up logging
                LoggingService loggingService = new LoggingService(LogLevel.Info);

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
                    if (coordinates == null)
                    {
                        System.Console.WriteLine("Laser did not exit.");
                        System.Console.WriteLine("");
                    }
                    else
                    {
                        // Display results
                        System.Console.WriteLine("Laser will exit at {0},{1} going {2}.", coordinates.X, coordinates.Y, coordinates.OutDirection.ToString().ToLower());
                        System.Console.WriteLine("");
                    }

    
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
