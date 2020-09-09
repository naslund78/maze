using System;
using System.Collections.Generic;
using System.Text;

namespace Maze.Core.Objects
{
    public class Room
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Mirror Mirror { get; set; }

        public bool HasMirror { get {
                if (Mirror == null)
                    return false;
                else
                    return true;
            }
        }
        public Room(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
