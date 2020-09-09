using Microsoft.VisualStudio.TestTools.UnitTesting;
using Maze.Core.Objects;
using System.Collections.Generic;
using static Maze.Core.Models.Helpers;

namespace Maze.Tests
{
    [TestClass]
    public class MazeTest
    {
        [TestMethod]
        public void ShootLaser_1_Sample1()
        {
            // Set up our board
            int height = 5;
            int width = 4;
            Board board = new Board(width, height);

            // Add our mirrors
            board.AddMirror(new Mirror(AngleTypes.Right, false, true), 1, 2);
            board.AddMirror(new Mirror(AngleTypes.Left, true, true), 3, 2);

            // Shoot laser
            int startX = 1;
            int startY = 0;
            Directions direction = Directions.Up;
            Coordinates coordinates = board.ShootLaser_GetExitCoordinates(startX, startY, direction, new Core.Services.LoggingService(Core.Services.LoggingService.LogLevel.None));

            // Validate
            Coordinates expected = new Coordinates(3, 0, Directions.Down);
            Assert.AreEqual(expected.X, coordinates.X, "Expected X=" + expected.X + " Found X=" + coordinates.X);
            Assert.AreEqual(expected.Y, coordinates.Y, "Expected Y=" + expected.Y + " Found Y=" + coordinates.Y);
            Assert.AreEqual(expected.OutDirection, coordinates.OutDirection, "Expected Direction=" + expected.OutDirection + " Found Direction=" + coordinates.OutDirection);
        }

        [TestMethod]
        public void ShootLaser_1_Sample2()
        {
            // Set up our board
            int height = 5;
            int width = 4;
            Board board = new Board(width, height);

            // Add our mirrors
            board.AddMirror(new Mirror(AngleTypes.Right, false, true), 1, 2);
            board.AddMirror(new Mirror(AngleTypes.Left, true, true), 3, 2);

            // Shoot laser
            int startX = 0;
            int startY = 2;
            Directions direction = Directions.Right;
            Coordinates coordinates = board.ShootLaser_GetExitCoordinates(startX, startY, direction, new Core.Services.LoggingService(Core.Services.LoggingService.LogLevel.None));

            // Validate
            Coordinates expected = new Coordinates(3, 0, Directions.Down);
            Assert.AreEqual(expected.X, coordinates.X, "Expected X=" + expected.X + " Found X=" + coordinates.X);
            Assert.AreEqual(expected.Y, coordinates.Y, "Expected Y=" + expected.Y + " Found Y=" + coordinates.Y);
            Assert.AreEqual(expected.OutDirection, coordinates.OutDirection, "Expected Direction=" + expected.OutDirection + " Found Direction=" + coordinates.OutDirection);
        }

        [TestMethod]
        public void ShootLaser_2()
        {
            // Set up our board
            int height = 10;
            int width = 10;
            Board board = new Board(width, height);

            // Add our mirrors
            board.AddMirror(new Mirror(AngleTypes.Right, false, true), 1, 2);
            board.AddMirror(new Mirror(AngleTypes.Left, true, true), 3, 2);
            board.AddMirror(new Mirror(AngleTypes.Right, true, true), 3, 0);
            board.AddMirror(new Mirror(AngleTypes.Left, false, false), 1, 0);

            // Shoot laser
            int startX = 0;
            int startY = 2;
            Directions direction = Directions.Right;
            Coordinates coordinates = board.ShootLaser_GetExitCoordinates(startX, startY, direction, new Core.Services.LoggingService(Core.Services.LoggingService.LogLevel.None));

            // Validate
            Coordinates expected = new Coordinates(0, 0, Directions.Left);
            Assert.AreEqual(expected.X, coordinates.X, "Expected X=" + expected.X + " Found X=" + coordinates.X);
            Assert.AreEqual(expected.Y, coordinates.Y, "Expected Y=" + expected.Y + " Found Y=" + coordinates.Y);
            Assert.AreEqual(expected.OutDirection, coordinates.OutDirection, "Expected Direction=" + expected.OutDirection + " Found Direction=" + coordinates.OutDirection);
        }


        [TestMethod]
        public void ShootLaser_2_Loop()
        {
            // Set up our board
            int height = 10;
            int width = 10;
            Board board = new Board(width, height);

            // Add our mirrors
            board.AddMirror(new Mirror(AngleTypes.Right, false, true), 1, 2);
            board.AddMirror(new Mirror(AngleTypes.Left, true, true), 3, 2);
            board.AddMirror(new Mirror(AngleTypes.Right, true, true), 3, 0);
            board.AddMirror(new Mirror(AngleTypes.Left, false, true), 1, 0);

            // Shoot laser
            int startX = 0;
            int startY = 2;
            Directions direction = Directions.Right;
            Coordinates coordinates = board.ShootLaser_GetExitCoordinates(startX, startY, direction, new Core.Services.LoggingService(Core.Services.LoggingService.LogLevel.None));

            // Validate
            Assert.IsNull(coordinates, "Expecting a loop and the coordinates to be null.");
        }

        [TestMethod]
        public void ShootLaser_3_PerformanceTestLoop()
        {
            // Set up our board
            int height = 1000;
            int width = 1000;
            Board board = new Board(width, height);

            // Add our mirrors, loop around outside
            board.AddMirror(new Mirror(AngleTypes.Right, true, true), 0, height-1);
            board.AddMirror(new Mirror(AngleTypes.Left, true, true), width-1, height-1);
            board.AddMirror(new Mirror(AngleTypes.Right, true, true), width - 1, 0);

            // Shoot laser
            int startX = 0;
            int startY = 0;
            Directions direction = Directions.Up;
            Coordinates coordinates = board.ShootLaser_GetExitCoordinates(startX, startY, direction, new Core.Services.LoggingService(Core.Services.LoggingService.LogLevel.None));

            // Validate
            Coordinates expected = new Coordinates(0, 0, Directions.Left);
            Assert.AreEqual(expected.X, coordinates.X, "Expected X=" + expected.X + " Found X=" + coordinates.X);
            Assert.AreEqual(expected.Y, coordinates.Y, "Expected Y=" + expected.Y + " Found Y=" + coordinates.Y);
            Assert.AreEqual(expected.OutDirection, coordinates.OutDirection, "Expected Direction=" + expected.OutDirection + " Found Direction=" + coordinates.OutDirection);
        }

    }
}
