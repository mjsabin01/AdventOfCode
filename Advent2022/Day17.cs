﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avent2022;

internal class Day17
{
    public void Run()
    {
        Stopwatch s = new Stopwatch();
        s.Start();

        var lines = Input.Split("\r\n");
        Part2(lines);

        s.Stop();
        Console.WriteLine("Took: {0} to complete.", s.Elapsed);
    }

    public void Part1(string[] lines)
    {
        var movements = lines[0].ToCharArray();
        var maxY = GetHeight(2022, movements);
        Console.WriteLine($"Rocks are {maxY} units tall.");
    }

    public void Part2(string[] lines)
    {

        // ONLY WORKS FOR TEST INPUT, NOT REAL INPUT
        var movements = lines[0].ToCharArray();
        (int cycleStartRock, int cycleRockLength, int cycleRockHeight) = GetCycle(movements);

        long totalRocks = 1000000000000; 
        var numCycles = (totalRocks - cycleStartRock) / cycleRockLength;
        var postCycleRocks = (totalRocks - cycleStartRock) % (long)cycleRockLength;

        
        long totalHeight = numCycles * cycleRockHeight;
        var cycleStartHeight = GetHeight(cycleStartRock, movements);
        var remainderHight = GetHeight(cycleStartRock + (int)postCycleRocks, movements) - cycleStartHeight;
        totalHeight += cycleStartHeight + remainderHight;
        
        Console.WriteLine($"Height after all cycles is {totalHeight}");
    }


    public (int startRock, int cycleRockLength, int cycleRockHeight) GetCycle(char[] movements)
    {
        HashSet<Point> settledRocks = new();
        for (int i = 1; i <= 7; i++)
        {
            settledRocks.Add(new Point(i, 0));
        }

        int maxY = 0;
        var movementIdx = -1;

        // cache maps the (rock index, initial movement) to the (occurence, height)
        var cache = new Dictionary<(int, int), List<(int, int)>>();
        int numRocks = -1;
        while (true)
        {
            maxY = Math.Max(maxY, settledRocks.Max(x => x.Y));

            var cacheKey = (++numRocks % 5, (movementIdx + 1) % movements.Length);
            if (!cache.ContainsKey(cacheKey))
                cache[cacheKey] = new();

            var l = cache[cacheKey];
            l.Add((numRocks, maxY));

            // want to find when cycle stabalizes, since the first time cycle occurs may be skewed since the floor changes pattern
            if (l.Count > 2 && (l.Last().Item2 - l[l.Count - 2].Item2) == (l[l.Count - 2].Item2 - l[l.Count - 3].Item2))
            {                
                
                var cycleRockLength = l[l.Count - 2].Item1 - l[l.Count - 3].Item1;
                var cycleRockHeight = l[l.Count - 2].Item2 - l[l.Count - 3].Item2;
                var cycleStartIdx = l[l.Count - 3].Item1;
                return (cycleStartIdx, cycleRockLength, cycleRockHeight);
            }

            PlaceRock(numRocks, settledRocks, movements, ref movementIdx, maxY);

            maxY = Math.Max(maxY, settledRocks.Max(x => x.Y));

        }
    }

    public int GetHeight(int numRocksToPlace, char[] movements)
    {
        HashSet<Point> settledRocks = new();
        for (int i = 1; i <= 7; i++)
        {
            settledRocks.Add(new Point(i, 0));
        }

        int maxY = 0;
        var movementIdx = -1;

        int numRocks = -1;
        while (++numRocks < numRocksToPlace)
        {
            PlaceRock(numRocks, settledRocks, movements, ref movementIdx, maxY);

            maxY = Math.Max(maxY, settledRocks.Max(x => x.Y));
        }

        return maxY;
    }


    void PlaceRock(int rockCount, HashSet<Point> settledRocks, char[] movements, ref int movementCount, int maxY)
    {
        var fallingRock = (rockCount % 5) switch
        {
            0 => BuildHorizLine(maxY),
            1 => BuildT(maxY),
            2 => BuildBackwardsL(maxY),
            3 => BuildVerticalLine(maxY),
            4 => BuidSquare(maxY),
        };

        while (true)
        {
            var movement = movements[++movementCount % movements.Length];
            fallingRock = MoveRockHoriz(fallingRock, movement, settledRocks);
            if (!CanRockMoveDown(fallingRock, settledRocks))
                break;

            fallingRock = fallingRock.Select(x => new Point(x.X, x.Y - 1)).ToList();
        }

        foreach (var point in fallingRock)
        {
            settledRocks.Add(point);
        }
    }

    void PrintRocks(HashSet<Point> rocks, int maxY)
    {
        var matrixHeight = maxY + 2;
        var matrix = new int[matrixHeight, 9];

        for (int i = 0; i < matrixHeight - 1; i++)
        {
            matrix[i, 0] = 2;
            matrix[i, 8] = 2;
        }

        foreach(var point in rocks)
        {
            matrix[matrixHeight - point.Y - 1, point.X] = 
                point.Y == 0  ? 3 :
                (point.X == 0 || point.X == 8) ? 2 : 1;
        }

        Utils.PrintMatrix(matrix, (x) => x switch
        {
            1 => "#",
            2 => "|",
            3 => "-",
            _ => "."
        });

        Console.WriteLine("\r\n\r\n");
    }

    List<Point> MoveRockHoriz(List<Point> fallingRock, char direction, HashSet<Point> settledRocks)
    {
        List<Point> updated;
        if (direction == '>')
        {
            updated = fallingRock.Select(r => new Point(r.X + 1, r.Y)).ToList();            
        }
        else
        {
            updated = fallingRock.Select(r => new Point(r.X - 1, r.Y)).ToList();            

        }

        // ensure we don't move into a wall or an existing rock
        if (updated.Any(r => r.X == 0 || r.X == 8 || settledRocks.Contains(r)))
        {
            return fallingRock;
        }
        return updated;
    }

    bool CanRockMoveDown(List<Point> fallingRock, HashSet<Point> rocks)
    {
        foreach (var point in fallingRock)
        {
            if (rocks.Contains(new Point(point.X, point.Y - 1)))
                return false;
        }

        return true;
    }


    List<Point> BuildHorizLine(int currentHeight)
    {
        return new List<Point>()
        {
            new Point(3, currentHeight + 4),
            new Point(4, currentHeight + 4),
            new Point(5, currentHeight + 4),
            new Point(6, currentHeight + 4),
        };
    }

    List<Point> BuildT(int currentHeight)
    {
        return new List<Point>()
        {
            new Point(4, currentHeight + 4),
            new Point(3, currentHeight + 5),
            new Point(4, currentHeight + 5),
            new Point(5, currentHeight + 5),
            new Point(4, currentHeight + 6)
        };
    }

    List<Point> BuildBackwardsL(int currentHeight)
    {
        return new List<Point>()
        {
            new Point(3, currentHeight + 4),
            new Point(4, currentHeight + 4),
            new Point(5, currentHeight + 4),
            new Point(5, currentHeight + 5),
            new Point(5, currentHeight + 6)
        };
    }

    List<Point> BuildVerticalLine(int currentHeight)
    {
        return new List<Point>()
        {
            new Point(3, currentHeight + 4),
            new Point(3, currentHeight + 5),
            new Point(3, currentHeight + 6),
            new Point(3, currentHeight + 7),
        };
    }


    List<Point> BuidSquare(int currentHeight)
    {
        return new List<Point>()
        {
            new Point(3, currentHeight + 4),
            new Point(4, currentHeight + 4),
            new Point(3, currentHeight + 5),
            new Point(4, currentHeight + 5),
        };
    }

    #region TestInput

    public string TestInput =
        @">>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>";

    #endregion

    #region Input

    public string Input =
        @">>><>>>><<<>>>><<>><>>><<<<>>>><<<<>>><<<>><>>>><>>><><<<><<<>>><>>><<<><<<>>><<>>>><<<><<<<><<<<>>><>>><<<>>>><>><<<>>><<<<>><>>>><<<><<>><><<<><<>>><<<>><<>>><>>>><<<>>>><<<>><<<<>>><>>><<<<>><<<><<<<>>><>>><>>>><<<<><<<<>>><<>><<<<>>>><><<>><<<<>>>><<<<>>><<>>><>><>><>><<<<>>>><<>><<<>>>><>>><<>>><<><<<>><<<<>>><<<<>>>><<<<>>>><<<<>>><<>><<<<>><<<><>><><<>>>><>>>><>><<<<>>>><<<>>><>><<>><<<<>><<<<>>><<<<>><<<<>>><<<>><<><<<<><<>><>>><<<<><>>><<><<>><<>>>><<<<>>>><>>><<<<>>>><<<>><>>>><<<<><>>><<<><<<<>>><>>>><<>>><<>><<<<>><<<>>>><<<<>>><>>><<<<>>><<>><<<<>>><<<>><<>>><<<<><<<>><<>><>>>><<>>><><>>><>>>><<>><<>><<><><<<<>>>><<>><<<><<<>>>><<<>>><<<>>><>><<<<><<>><<<<>><><<>><>><>><<<>><><<>><<<<>>><>><<<>>>><><>>>><<<<>>><<<><<<>>>><<<>>><<<<>><<<>>>><<<>>><>>>><><<<>>><<>><><<<<>>>><<><<>>>><><>><<>>><<>>><<<>><<<>>><<<>>>><<<<>><<<>>><<<<>>><<<<>>><<<<>>><<<<>>><<<>><<>>><>>>><<<>>><<<<><<<<>>>><<<<><<<>>><<><<>>>><>>>><<<<>><><<<>><<<><<<<>><<<>>>><<>>>><<>>>><>>><<>><<<>><<>>><<<><<<<>><<>>>><<>>>><<>>>><<<<>>>><<>>>><<<><<>><<>><<>>><<<>>>><>>>><<<>>>><<<<>>><>>><<<>>>><<<<><>><>>><<>>>><<<<>><<<<>><<<<>>><<<<>>><><<>><<>>>><>>>><<>><<<>>>><<<><>><<<<>>><<<<>>><>>>><<<<>>><>><<><<>>>><<><<><>>><<<<>><>>>><<<<>><<>>><<>>><<>>>><>>><<><><>>>><>>><<<<>><<<<>>><>><<<<>>><>>><>><<<<>>>><<><<<>>>><<<<>>>><<>><<>>><<<<>>><<>>><<<>><<><<<<>><<<>>>><<<<>>><<<>><<<>>>><<<>>><<<<><<<>><>>>><<<><<<<>>><<<<><<<<>><>>>><<<>>><<<>>>><<<<>>>><<<<>>><<<>><<>><<<><<<<><<>>>><<<<>>>><>><>><<<>>><>><<><<<<>><<<>>><>>><<><<<<>>>><>><<<<><<<<><<<<>>><<><<>>>><>><<<><<>>>><>><<<<>>><<<<>>>><<>>><<<<>><<<<>>>><<<<>>>><<<<>><>><>><<<<>>>><<<>>><<>>><><<>>><<<>>>><<<>>>><<>>><<<<>>><<<><<<<>>>><<>>><<<<>><<<>>>><<<>>><<>>><<<<><>><<<>><><<>>>><<>><<><>><><>>>><>>><<<<>><<<<>><><<<<>>>><><<<<>>><<<<>>>><<<<>>><<<>><<>>><<<<>><<<<>><<>>>><<>>>><<<>><<<>>><<<<>><<>>>><<<<><<<<>>>><>>><<<<>>>><<<><<><>><<>>><<<<><>>>><>>>><<<>><<<<>><<>><<<<>>><<>>><>><<<>>><<<>><>>>><<>>><<<>>><<>>>><<>>>><><<<>>><<<<><>>>><<><<<<>>>><<<>><<>><<<<>>>><>>><<<>><<<>>><>>><<>>>><<<><<>>><<<>>>><>>><<<<>><<<<><<<>>>><<>>><<<<>><>>>><>><<<<>><<<>>><>><<>>>><<<><<<><<<<>>>><<>><><<<<>>>><<>>>><<<<>>><><<<<>>>><<>><>><<>>>><<>>>><<<>>>><<<<><<<>>>><>>><<<><<<<><<<<><>>><<<>><<<>><<>>><<<<><<<>>>><>><<><<>><<>>>><<<>>><<<<>><>>><<<><<<<>>>><<>><<><>>>><<<<>>><>><<>>>><<<>>>><<<<>>><<<>>>><<<>><<<<>><<<<>><<<><<<>>>><<<><<>>>><<<<><<<>>><><<<>><<><<<<><<<><<>><<<><><<><<<<>><<>><<>>><<<<><<<>>>><<<>>>><<<<>>><<>>>><<<<><<<<><<<<>>>><<>>>><<<><<<>>>><<><<>>>><<<<>>><><<>>>><<>><>>>><<<<><>>><<<>>>><<<>><>><>><<<<>>><<<>>><><>><>>><>>>><>>><>>><>>>><>>><<>>><<<<>>>><<<><<<<>><<<<>><<<>>><<<<>>><<>>><<<>>>><<<>><<<><<<<>><<<<>>>><<><>>><<<<><>>>><>><<>><<>>><<><<<<>><<><<<<>>>><<>>><<<>>><>>>><<<>>>><>>><<<>>><<<<>>><<<<>><<<>>>><<<<>><<<>><<<<>>><>>>><<>>><<<<>>><<<><>>>><><>><<><>>>><<<>><<>><<<>><<<>>>><<<>>><<<>><<<<>><>>><<<<>>>><<>>>><>>>><>><<<>>><<<<>>><<<>>><<<>>><<<<><>>>><<<>>><<>>>><><<<>>>><<<<>><<<<><<<>>>><<>>><>>>><<<<>>>><<<>>>><<>><>>>><<>>>><<<<>>>><<><<<<><<<<><<<>>>><>>>><<>><<><<>><>><<>>><<<<>><><<<>>>><>><<<<><>><<>>><>><<<>>><<>>>><><><<>><><>><<<>>><<>><<<<>><>>>><<>>><<<>>><<>>><<<<>>><<<><<<>>>><<<><>><<>><<<<>><<>>>><<<<>><<<<>>>><<<<>><>>>><<<>>>><<<<>>>><>>>><>>><<<<>>><><>>><<<>><>>>><<>><<>><<<><<<<>><<>>><><<>><<>><<<>><<<>>>><<<<>>>><<<<>>>><<>>><>>>><<>><><<<<>><<><>>><<<<>>><>>>><><<<>>><<>><<<<>>>><>>><>>>><<<<>><><<<<>>>><<<><><<<<><<<<>>>><<<<>><>>><<<>><<<>>>><<>>><<<><<<>><<<>>><<<><>><>>>><>>>><<<<>>>><><<<>>>><<<<><<<<>>><>>><>><<<>>>><<<>>>><><<<<><<<>>>><<<>><>>><<<<>>>><<>><<>>>><>>>><>>><>><<<>>><<<>>>><>><>>>><>><<<><<>>><<>>>><>><><>>>><<<<>><<<>><<<>><<><<<>>><<<>>>><<<<>><<<>>><>><<<><<<><<>>>><<><<<<>>><<<<>>>><<><><<>><<>>><>>>><><<<<>>><<><>>>><<>><<<>>><<<>>><<<<>><<>>><>><<<><<<>>>><<<<>>><><><><>>>><<<<><<<>>>><<>>>><<<>><<<<>>><>><<<<>><<<<><<<>><<><<<<>><>><>><<<><<><<<<>>><<<>><<<<>>>><<<<>><<<<>><<<<>>>><<<><<<<>>><<<><<<<>><<<<>><<<>>>><<>>><<<<><<>><<<>>>><<>>><<<<>>>><<<<><<<<>><<>>><>>>><>><<<<>>><<><<<<><<<>>>><<<<><<<<>><<<<><<>><>><<>>>><<><<<<>><><<<><>>><<<>>><<><><<<>>>><>><>>><<>>>><<<>>><<<<>>><>>><<<<>>><<<>>>><<<>>><>>>><>><><<<<>><<<<><>>>><<><>>><<>><<<<>><<<<>><<<<>><<<>><>>>><<<<>><<><>>><<>>>><<>>><<<>>>><<<<><<<<>>>><><<<<>>>><<><<<<>>>><<<<><<>><>>><<>>>><<<><<<<>>>><<<>>><<>>><<<<>>>><<<>><><<<<>>>><<>><<<<><<<>><<>><>>><<<>>><<<<>>>><><<<>>>><<<>><<><<<>><<><>>>><<>><>><>>>><<>><<<>>>><<<<>>><>>>><<<>><<<><<>><>>>><<>>>><<<><>>><<<>>>><><<<<>><<<<>><<>><<<>>><<<>>>><<<<>>>><<>>>><>><>>>><<<<>>><<>><<<<>>>><<<<>><<>>>><>>><><>>>><<>><<<<>>><>>><>><><><<<>><<<>>>><<<<><<<>><>><<<<>>><<<>>><<<<><><<>>><<<<>>><<<>>>><<><<>>><><<<<>><><<<<>>><<<>>><>><<<<>><<<<>>>><<<<>>><<>>>><<<<>>><<<>><<<<>>>><<<<>><<<>><<<<>>><<<<>>><<<>>><<<<>>>><<<>>>><<<>><>>>><<><>>>><<>>>><<<<>>><<><<<<>>>><<<<>><<<>><>>><<<<>>><>>>><<<><>>><<<><<<>>>><>>>><<<<>>>><<<<><<<>><>><<><<>>><<>><<>>><<<>>>><<<<>>><<>><<<>>><<>>><<><<<<>>>><<<>>><<>>><<<<>><>>><<>>><<><<>>><<>>><<>>><<<<><<<>>>><<>><<<<>>><<>>>><<<>>><<<<><<<<><<>><<<><<<<>>>><<<>>><<>><<><<<>>><<<>><<<>>>><<<<>>><<>>><<<><>><<<><<>><<<><<<<>><<<<>>>><<<<>><<<<>>>><<><>>><<<>>>><<<<><><><<<><<<<><<<>>><<<>>><<<<>>><<<>>>><<<>><<<>>><<<<>><<<><<<<>>><<>><><>><<<>><<><><<<<>>><<>>>><<>><<>>><<><<>>>><<<<><<>><<>>><<>><<<>>><<<<>>><<>>>><>><<<><<<>><<<<>>>><<<>>><<<<>>><<><<<<><<><>>><<<>><<<<><<<<><><<>><>>><>>>><<<<><<<>><<<><>><<>>>><<>>><<<<><<>>><>>>><<<<>>><>>><<>>><<<>>>><<<>>><<>>><<<<>>>><>><<<<>><<>>><<>>><<<<>>><>><<<<>>>><<<>>>><>>>><<>><<<>>><>><<<<>>><<<>>>><<<<>>>><<<>>><>>><<<<>>><>>><<>><<<<>><<>><<<>><<>><>><<<<>>>><<<><<><<<<>><<>><><>>><>><>>><>>><<<>>>><<>><<<><<>>>><<>>>><>><>>>><><<><<><>>><<<<>><><<<>>><<<<>>><>>><<>><<<>>><<<<>>><<<>><<><<>><><<<<><<<<>><<>>>><<>><<>>>><<<<>>><<<>>>><<><<>>>><<<>><<>>><<>>><<<<>>>><<<>>><><<>><<>><<<<>><<<<>><<>>>><<<>><<<>><<<>><<<<>>><<>>><<<>>>><>><<><><>><><<<>><>>>><>><>><<>>>><<<>>>><>>><<<>><>>>><<><><<><<<><<>>>><>>><<<<>>><<><>>><<<<><>>>><<>><<<><<<>><<>><<<>>>><><<>>><<<>>>><<<<>>><<<>>><<<<>>>><<>>><>>><<<<>>><<><<<>><<<<>>><><<>>>><<>><<>>>><<<<>>>><>>>><<<<>><<<<>>><<>><<<>><<<<><<<>><<><<<<><><<<<><<<><<>>><<<>>><<<<>>><>>>><>>>><<<<>>>><>>>><<<>>>><<<<>>>><<<<>><<<>><><<<<>>>><<<>>><>>><<<>>>><<>>><<<>>><>>>><<<>>><>>>><><<>>>><<<>><<<<>>>><<<<>><>>><<>><<><<><<>><<<>><<>>><<>>>><<<>>>><<<>>><>><<<<>>><<<><<>>>><<>><>>><<>>><<<><<<<><<<><<<<><<<<>>>><<>><<<<>><<<>>><<<>>>><>>>><<<><<<>><<<<>>><<>><<<<>>><<<>>>><<><><<>>>><>><>><<<>>>><>>><<<<><<>>><<>>>><<<>>><><>><<>><<<>>>><<<<>><<<<>>>><<><<<><<<<>><>>>><<<<>><<>>><>>><<<>><<>><<<<>>>><><<>><<<>>>><<>><<<<><>>>><<>>><<><<<>><<>>>><<>>>><>><<<>>><<>><>>>><<<<>><<<><<>>><<<<><<<<>>>><<><>><<<>>><<<>>><<>>>><<<><<<><<<<>>>><<><<<>>>><>><<<>>>><>>><<<<>><<><>><<<><<>><<>><<<><>>>><<<<>>><>>><<<>>><<><<>>>><<>><<<<>>><<<<>><<<><<<<>>>><<<><<<><<<>>><>>><<<<>>>><<<<>>>><<<<><>>>><<<<>><<>><<<<>>><>><<<>>><<><<>>><>><<<>>><<<<>><>>>><<><<<>>><<<>>><<>>>><<><<<<>>>><>>>><>><<<<>><<<><<><<>>>><<<><<<<><<>>><<<<>>>><>>>><<<>>><<<>>><>>>><<<<>>><<<<><<<>>><<<>><>>><>>><>>>><>><<>><<<>>><<<<><<<<>>>><<<<><<<>>>><<<>><<<><<<>>>><<<>><<>><<<<>><<<>>>><>>>><<>>>><<<>>><><<<>>><<<>><<<>>><<>><<<<>>><>>><<>>>><>>>><>>><><<<<>>><<<><<<<>>>><>>>><<<<><<<>>><<<>>>><<<<>>>><<<<>>><<>>><<>>>><<>>>><<<<><<<<>><<<<><<>>><>>><<>>>><>>>><<<<>>><<<<>>>><<<><<<>>><<<>><<<<>>><><<<<>>><>><<>>>><<<>><<>>>><<<<>><<><<<>><<<<><<<<><<<<>><>>><<<><<<><<>>><<<<>><<>>><>>>><<<<>>><<<<>>>><<<>>>><<<>>>><<>><<>>><<<>>><<<>>><<>>>><<<<><<<>>>><<<><<<<>>><<<>><<>>>><<>><<<>>>><<<<><<<>>><<>><<<<>><<<<>><<>>>><>>><<><<>>><<><<<>><><>>>><>><>><<>>>><>>>><<<<><<><<<<>>>><<><<<>>><><<<>>><>>><<<<>><<<<>><<<<><<>>>><<<<>><<<>>><>><<>><<<>><<>>><<<<><<<>>><<>>>><<>>><<<<>>>><<><<<<>><<<>><<<>><<><>>><<>><<<>>>><<>>>><<>><<<>><<<<>>>><<>><<<<>>><<<>>>><<<<><<<><<<>>><<<><<<<>>>><<<<><<<<>>><<><<<><<>>><<>><<<<>>><<<>>><>><<<<><<>>><>>>><<<<>>>><<>>><><<<<>>>><<<>>>><<<>><<<>>><<<<>><>><<><<<<>><<<>>><<<<>><>>><<<<>>>><<<<>>><>>>><<<<>><<><><<<><<>>>><<>>><<<><><<>>><<<<>>><<>><<<><>>>><<<<>>><<><><<<<><<>>><<>>>><<<<>>><<><>>>><><>>>><<>><<<<><<>>><<><<<<><><<>>><<<<>><<<>><><<<><<<<><<<<><<><>>>><<>><<><<>>>><>>><<<><>><>>>><<><<<<>>><<<<><><><><<<<>>><<<><<<<>>><<<<><<<<><<<<>><<<<><<<<><<<<>>>><<>>>><><<>>>><>>>><<>><<<>>>><><<>>><<<<><>>>><<>>>><<<>>>><<<>><<<<>><<<>><><><<>>>><<>><<><><<><<>><<<<><><<><>>><<<>>><<<<>>>><<>>>><<<><<<<><<>><<<<><>><<<<><<><<<>><<<<>>><<<>><<><<<<>>>><>>><<<<><<<<>><<<>>>><<><><>>>><<<><<<<><<<<><<>><<><<<><<>>>><<<>><>><<>><<<>><<<<>><>><<>>><>>>><>>><>>><<<<>>><<<<><<<<>><<<<>><<>>><>>><<<<>><<>><>>>><<<>>>><<><<<><<<>>>><>>><>><<>>>><<<>><<>>><<>>>><<<>>>><<>>><<<<>><>>><<>>>><>><<>><<<<>>><<>>>><<<<>>><<<<>><<<>>>><<<<>>>><<<><<<>><<><<<<><<<>><<<>>>><>>><<<<>>><><<<><<<<>><<><<<<>>>><>>>><<<<>>>><<<<>>><<<><<<>><>><<<<>>><<<><>>><<>>>><>>><<<>>><<<>>>><>><<<>>><<<><>>><<<>>><<<>>><<><<<<>><>>>><<<<>>><>>><>><>><<<<><<<<>>>><<<<>>>><>>>><<>>>><<<>>><>><<<>>>><<>><><<>><>><<<>><><<>>>><<>>><<<<><<>><<<>>>><><>>>><<><<<>>>><><<<>>>><<>><<<>>>><<<>>><<<<><>>>><<>><<><>>>><<<>>>><><<<<>>><<<<>><<>>>><>>>><>><<<>><<<<>>><<>>>><<>><<<<><<<>>>><>><<><>>><<<<>><<<<><>><><<><<<>>><<<<>>>><<>>><>>><<<>>><<<<>><<>><<><<<><<<<>>><<<<>>>><>><<<<>>><<<<>>><<<<><<>>>><<<>><<>><<>><>><>><<<>>>><<>><<>>>><>>><<<>>><>>>><<<>><<<<>>>><<<<><<<<>>><<>><<<<><<<>>><<>>><<<<>>>><<<>>><<><<<><>>><<>>>><<<>>>><<<>>><<<<>><<<>><>>>><<<>>><<<>>><<<>>><<<>><<<<>>><<<><>>><<<<>>>><<><<>><><><<>><<>><<<><<<<>>>><<>><>><<><<<<>>><><<>>><<<>><<<><<<>><<>><<><>>>><<<>>>><<>>>><<<<>><>>>><<>>><<><<<>>>><>><<<>>><<>>><<<<><<>>>><<<>>>><<>><>>>><<<<>><<<>>><><<><<>>>><<>>><<<>><<<>>><<<<><<>>>><<>>>><<><<<>><<<<>>>><>><<<>>>><<<<>>><>>><<<<><<><<>>><><<<><<<>><<<<>><<>>><>><<<>>>><<<<>>><<<<>><<<<><<<>>><<<>>><<<<>>>><<<><>>>><<<<>>><<>><<<>>><<<>><<<<>>><<><<<<>>><<<><<<><<<>><<>>>><<<<>>>><<<>><<<>>>><<<<><>>>><>>>><<<>>><<<<>>><<>><><<<>><<<><<<>>><<";

    #endregion 
}
