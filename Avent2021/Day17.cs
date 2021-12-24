using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avent2021
{
    internal class Day17
    {
        public void Run()
        {
            var lines = Input.Split("\r\n");
            Part2(lines);
        }

        public void Part1(string[] lines)
        {
            var targetArea = new TargetArea(lines[0]);
            long maxY = 0;

            for (long xVel = 0; xVel < targetArea.maxX; xVel++)
            {
                for (long yVel = 0; yVel < -1 * targetArea.minY; yVel++)
                {
                    long fireMaxY = 0;
                    if (SimulateFire(0, 0, xVel, yVel, targetArea, ref fireMaxY))
                    {
                        maxY = Math.Max(fireMaxY, maxY);
                    }
                }
            }
            Console.WriteLine($"Max y is: {maxY}");
        }

        public bool SimulateFire (long x, long y, long xVelocity, long yVelocity, TargetArea target, ref long maxY)
        {
            if (target.IsInTargetArea(x, y))
            {
                return true;   
            }

            if (y < target.minY)
            {
                return false;
            }

            x = x + xVelocity;
            xVelocity = xVelocity == 0 ? 0 :
                xVelocity > 0 ? xVelocity - 1 : xVelocity + 1;

            y = y + yVelocity;
            maxY = Math.Max(y, maxY);
            yVelocity--;
            return SimulateFire(x, y, xVelocity, yVelocity, target, ref maxY);
        }

        public void Part2(string[] lines)
        {
            var targetArea = new TargetArea(lines[0]);
            long numPosibilites = 0;

            for (long xVel = 0; xVel < targetArea.maxX * 2; xVel++)
            {
                for (long yVel = targetArea.minY; yVel < -1 * targetArea.minY; yVel++)
                {
                    long fireMaxY = 0;
                    if (SimulateFire(0, 0, xVel, yVel, targetArea, ref fireMaxY))
                    {
                        numPosibilites++;
                    }
                }
            }
            Console.WriteLine($"There are {numPosibilites} ways to fire.");
        }

        public class TargetArea
        {
            public long minX { get; set; }
            public long maxX { get; set; }
            public long minY { get; set; }
            public long maxY { get; set; }

            public TargetArea(string input)
            {
                var parts = input.Substring("target area: ".Length).Split(", ");
                var xParts = parts[0].Substring("x=".Length).Split("..");
                var yParts = parts[1].Substring("y=".Length).Split("..");

                minX = int.Parse(xParts[0]);
                maxX = int.Parse(xParts[1]);

                minY = int.Parse(yParts[0]);
                maxY = int.Parse(yParts[1]);
            }

            public bool IsInTargetArea(long x, long y) => x >= minX && x <= maxX && y >= minY && y <= maxY;
        }

        #region TestInput

        public string TestInput =
            @"target area: x=20..30, y=-10..-5";

        #endregion

        #region Input

        public string Input =
            @"target area: x=117..164, y=-140..-89";

        #endregion 
    }
}
