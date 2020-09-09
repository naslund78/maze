using System;
using System.Collections.Generic;
using System.Text;
using static Maze.Core.Models.Helpers;

namespace Maze.Core.Objects
{
    public class Coordinates
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int NextX { get; set; }
        public int NextY { get; set; }
        public Mirror Mirror { get; set; }
        public Directions InDirection { get; set; }
        public Directions OutDirection { get; set; }

        public Coordinates(int x, int y)
        {
            X = x;
            Y = y;
        }
        public Coordinates(int x, int y, Directions inDirection)
        {
            X = x;
            Y = y;
            InDirection = inDirection;
            Calculate();
        }
        public Coordinates(int x, int y, Directions inDirection, Mirror mirror)
        {
            X = x;
            Y = y;
            Mirror = mirror;
            InDirection = inDirection;
            Calculate();
        }

        private void Calculate()
        {
            CalculateNextDirection();
            CalculateNextX();
            CalculateNextY();
        }

        private void CalculateNextDirection()
        {
            // Default to the same direction
            OutDirection = InDirection;

            // If we don't have a mirror we keep going the same way
            if (Mirror == null)
                return;

            // Check if the mirror makes us go a different direction
            if (InDirection == Directions.Right)
            {
                if (Mirror.AngleType == AngleTypes.Left && Mirror.LeftSideReflective)
                    OutDirection = Directions.Down;
                if (Mirror.AngleType == AngleTypes.Right && Mirror.LeftSideReflective)
                    OutDirection = Directions.Up;
            }
            else if (InDirection == Directions.Down)
            {
                if (Mirror.AngleType == AngleTypes.Left && Mirror.RightSideReflective)
                    OutDirection = Directions.Right;
                if (Mirror.AngleType == AngleTypes.Right && Mirror.LeftSideReflective)
                    OutDirection = Directions.Left;
            }
            else if (InDirection == Directions.Up)
            {
                if (Mirror.AngleType == AngleTypes.Left && Mirror.LeftSideReflective)
                    OutDirection = Directions.Left;
                if (Mirror.AngleType == AngleTypes.Right && Mirror.RightSideReflective)
                    OutDirection = Directions.Right;
            }
            else if (InDirection == Directions.Left)
            {
                if (Mirror.AngleType == AngleTypes.Left && Mirror.RightSideReflective)
                    OutDirection = Directions.Up;
                if (Mirror.AngleType == AngleTypes.Right && Mirror.RightSideReflective)
                    OutDirection = Directions.Down;
            }
        }

        private void CalculateNextX()
        {
            if (OutDirection == Directions.Left)
                NextX = X - 1;
            else if (OutDirection == Directions.Right)
                NextX = X + 1;
            else
                NextX = X;
        }

        private void CalculateNextY()
        {
            if (OutDirection == Directions.Up)
                NextY = Y + 1;
            else if (OutDirection == Directions.Down)
                NextY = Y - 1;
            else
                NextY = Y;
        }
    }
}
