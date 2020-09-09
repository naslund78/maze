using System;
using System.Collections.Generic;
using System.Text;

namespace Maze.Core.Objects
{
    public class Laser
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Laser(int startX, int startY)
        {
            X = startX;
            Y = startY;
        }
    }
}
