using System;
using System.Collections.Generic;
using System.Text;
using static Maze.Core.Models.Helpers;

namespace Maze.Core.Objects
{
    public class Mirror
    {
        public AngleTypes AngleType { get; set; }
        public bool LeftSideReflective { get; set; }
        public bool RightSideReflective { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public Mirror(AngleTypes angleType, bool leftSideReflective, bool rightSideReflective)
        {
            AngleType = angleType;
            LeftSideReflective = leftSideReflective;
            RightSideReflective = rightSideReflective;
        }
        public Mirror(AngleTypes angleType, bool leftSideReflective, bool rightSideReflective, int x, int y)
        {
            AngleType = angleType;
            LeftSideReflective = leftSideReflective;
            RightSideReflective = rightSideReflective;
            X = x;
            Y = y;
        }
    }
}
