using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using Maze.Core.Services;
using static Maze.Core.Models.Helpers;

namespace Maze.Core.Objects
{
    public class Board
    {
        public Room[,] Rooms { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        private List<Coordinates> history;
        private LoggingService _loggingService;
        public Board(int width, int height, LoggingService loggingService)
        {
            Width = width;
            Height = height;
            _loggingService = loggingService;
            Rooms = new Room[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Rooms[x, y] = new Room(x, y);
                }
            }
            history = new List<Coordinates>();
        }

        public void AddMirror(Mirror mirror, int x, int y)
        {
            _loggingService.LogInfo("Adding mirror to " + x.ToString() + "," + y.ToString());
            Rooms[x, y].Mirror = mirror;
        }

        public Coordinates ShootLaser_GetExitCoordinates(int X, int Y, Directions direction)
        {
            Coordinates coordinates = null;
            Room currentRoom;
            _loggingService.LogInfo("Laser entering at " + X.ToString() + "," + Y.ToString() + " going " + direction.ToString().ToLower() + ".");
            while (true)
            {
                // Validate indes
                if (X >= Height || Y >= Height)
                {
                    _loggingService.LogError(" " + X.ToString() + "," + Y.ToString() + " is outside of our board");
                    return null;
                }

                // Calculate our coordinates
                currentRoom = Rooms[X, Y];
                if (currentRoom.HasMirror)
                    coordinates = new Coordinates(X, Y, direction, currentRoom.Mirror);
                else
                    coordinates = new Coordinates(X, Y, direction);

                // Check for a loop, if we already came in and out the same way of this square
                var qry = from h in history
                        where h.X == coordinates.X &&
                            h.Y == coordinates.Y &&
                            h.InDirection == coordinates.InDirection &&
                            h.OutDirection == coordinates.OutDirection
                        select h;
                if (qry.Any())
                {
                    _loggingService.LogInfo("Laser is in a loop, processing has stopped.");
                    return null;
                }

                // Track history
                history.Add(coordinates);

                // Get our new direction and coordinates
                direction = coordinates.OutDirection;
                X = coordinates.NextX;
                Y = coordinates.NextY;

                // Log our coordinates
                if (coordinates.InDirection != coordinates.OutDirection)
                    _loggingService.LogInfo("Laser reflected off mirror, now going " + coordinates.OutDirection.ToString().ToLower() + ".");
                _loggingService.LogInfo("Laser moving to " + X.ToString() + "," + Y.ToString());

                // Check if we have exitted
                if (X < 0 || X >= Width)
                    break;
                if (Y < 0 || Y >= Height)
                    break;
            }
            return coordinates;
        }
    }
}
