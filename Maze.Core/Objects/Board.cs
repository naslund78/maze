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
        public Board(int width, int height)
        {
            Width = width;
            Height = height;
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
            Rooms[x, y].Mirror = mirror;
        }

        public Coordinates ShootLaser_GetExitCoordinates(int X, int Y, Directions direction, LoggingService loggingService)
        {
            Coordinates coordinates = null;
            Room currentRoom;
            while (true)
            {
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
                    return null;

                // Track history
                history.Add(coordinates);

                // Get our new direction and coordinates
                direction = coordinates.OutDirection;
                X = coordinates.NextX;
                Y = coordinates.NextY;

                // Log our coordinates
                loggingService.LogInfo(X.ToString() + "," + Y.ToString());

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
