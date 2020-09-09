using Maze.Core.Objects;
using System;
using System.Collections.Generic;
using System.Text;
using static Maze.Core.Models.Helpers;

namespace Maze.Core.Services
{
    public class MazeDefinitionConverter
    {
        private LoggingService _loggingService;
        public MazeDefinitionConverter(LoggingService loggingService)
        {
            _loggingService = loggingService;
        }
        #region Public
        public Coordinates GetLaserCoordinates(string[] lines)
        {
            // Get our lines for each section
            int section = 0;
            string boardLine = string.Empty, laserLine = string.Empty;
            List<string> mirrorLines = new List<string>();
            foreach (var line in lines)
            {
                if (line.Trim() == "-1")
                    section++;
                else
                {
                    if (section == 0)
                        boardLine = line;
                    else if (section == 1)
                        mirrorLines.Add(line);
                    else if (section == 2)
                        laserLine = line;
                }
            }

            // Set up our board
            var boardInfo = GetHeightAndWidth(boardLine);
            Board board = new Board(boardInfo.Item1, boardInfo.Item2);
            
            // Add the mirrors
            foreach (var mirror in GetMirrors(mirrorLines))
                board.AddMirror(mirror, mirror.X, mirror.Y);

            // Shoot the laser and return coordinates
            var laserInfo = GetLaserEntry(laserLine);
            return board.ShootLaser_GetExitCoordinates(laserInfo.Item1, laserInfo.Item2, laserInfo.Item3, _loggingService);
        }

        #endregion

        #region Private
        private Tuple<int,int> GetHeightAndWidth(string line)
        {
            /*
            The board size is provided in X,Y coordinates.


            Example:
             5,4 
            */
            int height = Convert.ToInt32(line.Split(',')[0]);
            int width = Convert.ToInt32(line.Split(',')[1]);
            return new Tuple<int, int>(height, width);
        }

        private List<Mirror> GetMirrors(List<string> lines)
        {
            /*
                The mirror placement will be in X,Y coordinates indicating which room the mirror is located. 
            It will be followed by an R or L indicating the direction the mirror is leaning 
            (R for Right and L for Left). That will be followed by an R or L 
            indicating the side of the mirror that is reflective if it’s a 1-way mirror 
            (R for Right Side or L for Left Side) or nothing if both sides are reflective
            and it’s a 2-way mirror.
             
            Example:
                1,2RR 
                3,2L
             */

            List<Mirror> mirrors = new List<Mirror>();
            foreach(var line in lines)
            {
                string mirrorLocation;

                // Figure out the index of where the mirror type starts 
                int idx = 0;
                int idxR = line.IndexOf("R");
                int idxL = line.IndexOf("L");
                if (idxR > 0)
                {
                    if (idxL == -1 || idxR < idxL)
                        idx = idxR;
                }
                else if (idxL > 0)
                    idx = idxL;

                AngleTypes angleType = AngleTypes.Other;
                bool leftSideReflective = true, rightSideReflective = true;
                string mirrorDirection, mirrorType;
                if (idx > 0)
                {
                    // Parse our line
                    mirrorLocation = line.Substring(0, idx);
                    mirrorDirection = line.Substring(idx, 1);
                    if (line.Length > idx + 1)
                    {
                        mirrorType = line.Substring(idx + 1, 1);
                        if (mirrorType == "R")
                            leftSideReflective = false;
                        if (mirrorType == "L")
                            rightSideReflective = false;
                    }

                    int x = Convert.ToInt32(mirrorLocation.Split(',')[0]);
                    int y = Convert.ToInt32(mirrorLocation.Split(',')[1]);
                    if (mirrorDirection == "R")
                        angleType = AngleTypes.Right;
                    if (mirrorDirection == "L")
                        angleType = AngleTypes.Left;
                    mirrors.Add(new Mirror(angleType, leftSideReflective, rightSideReflective, x, y));
                }
            }
            return mirrors;
        }
    
        private Tuple<int,int, Directions> GetLaserEntry(string line)
        {
            /*
             The laser entry room is provided in X,Y coordinates followed by an H or V 
                (H for Horizontal or V for Vertical) to indicated the laser orientation. 

            Example: 
                1,0V
            */
            Directions laserDirection = Directions.Up;
            string laserLocations = line.Replace("V", string.Empty).Replace("H", string.Empty);
            int x = Convert.ToInt32(laserLocations.Split(',')[0]);
            int y = Convert.ToInt32(laserLocations.Split(',')[1]);
            if (line.IndexOf("V") >= 0)
            {
                if (y == 0)
                    laserDirection = Directions.Up;
                else
                    laserDirection = Directions.Down;
            }
            if (line.IndexOf("H") >= 0)
            {
                if (x == 0)
                    laserDirection = Directions.Right;
                else
                    laserDirection = Directions.Left;
            }
            return new Tuple<int, int, Directions>(x, y, laserDirection);
        }
        #endregion
    }
}
