﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avent2020
{
    class Day03
    {
        #region input
        string TestInput = @"..##.........##.........##.........##.........##.........##.......
#...#...#..#...#...#..#...#...#..#...#...#..#...#...#..#...#...#..
.#....#..#..#....#..#..#....#..#..#....#..#..#....#..#..#....#..#.
..#.#...#.#..#.#...#.#..#.#...#.#..#.#...#.#..#.#...#.#..#.#...#.#
.#...##..#..#...##..#..#...##..#..#...##..#..#...##..#..#...##..#.
..#.##.......#.##.......#.##.......#.##.......#.##.......#.##.....
.#.#.#....#.#.#.#....#.#.#.#....#.#.#.#....#.#.#.#....#.#.#.#....#
.#........#.#........#.#........#.#........#.#........#.#........#
#.##...#...#.##...#...#.##...#...#.##...#...#.##...#...#.##...#...
#...##....##...##....##...##....##...##....##...##....##...##....#
.#..#...#.#.#..#...#.#.#..#...#.#.#..#...#.#.#..#...#.#.#..#...#.#";

        string Input = @"..#.#...#.#.#.##.....###.#....#
...........##.#...#.#..........
....#.....#..#.............#...
.#....###..##...#...##...#.#..#
#.......#.........#..#.......#.
...#.##..##...#.#......#.##.#..
#.#..##.....#.....#..##........
...#.####...#.##...#...........
.#...#..#..#....#.#.#.#.##.....
##.#..#.##..#......#..##.#.#..#
.#.##.....#.#...............#.#
..##.#.....#.....##..##.#....#.
#..#..........#...##........#..
#..##.#.#...............#..#...
..#....#...#.......#.......#...
.........#.#.##.#........#.....
#...##....#..#.........#.#...##
...#.#...#...........#..#...#..
...#..#........#...#...........
.#....##.#...#.#....#....##....
...#...#......#.#.......#...##.
####..........##....#..........
#..#...........................
#....#...####..##.#......#.#...
..#..#.....##.....#...#....#..#
#.##......#..##........#.......
..........#.....#...#.#.#....##
....##...##..#........#...#..#.
#..#..#...##..............##...
###.##..##.###...#....##.#..#..
.#......#.................#.#..
#.#..#.##.#.#.#.....#.........#
..##......#.......##........#..
#..............#.##.#.....#....
............................##.
..#.##......#..........#....#..
..##.....#..##.#....#.......##.
..#.#.##.#.........#...........
...........##.#.#...#......###.
#....#...#........#.#...#.#.###
..............#...#.....##....#
#...#...#..............#..#...#
.##..#.........#.##.#..#...##..
.....#.........#..#..#.......#.
.#......#.#.#....##..#...#..##.
#....................#.#....#..
......#.....##............#....
.#.....#......####.....#....##.
##.####.#..#..........#......#.
##....................#..##....
.....#...#.#.##.#.###.....#....
.#..#...####.#.#...#.#.....#...
#.....##.........##.##.##.....#
....#....##.###.........#...###
.......#........#.##.....#####.
...#.##..#...#...####.....##...
..#....#....#......#......#.#..
...#.#.#.........#.......#..#..
.....#...........#.#........##.
..##...#.#.##.#.#.#...###.#....
..##.............###....#.#....
#.......#....#..#...#..##..#...
....##..#.......####....#..#.##
##....#...#.#.#...#...#........
....#.#................#...#...
...#.....#.#.......##....#.#..#
#....##.#...#.#..#.#.........#.
#..##.........##.....#...#.....
....#.....#.#..#..##..##.##...#
#.....#...#.#.#.##....#.#.##...
.#.#........#..##.......#...#.#
..###.....#..#.##....#...#....#
...#..###...#...#.......#..#...
.#....##.......#.#..........##.
...#.#.............##.....##...
..#..#...#.....#...#...........
.#.#......#.##....#.....#......
........#.#.....#.#...#..#.#..#
#.....#.#.....#.##..#.#....#.#.
..#..###.#.#........#.....##..#
#.#....#......#.#....###..#...#
...#.#....#..#.##.....#...#....
....##....#.#...#.........#..##
.#......#...#.............#..#.
#........#........#.#.....##...
..##..#.##..#........#.........
.....#...#...#..#.....#.#.##.#.
..#..#..#.........#...#.......#
....#.....#.......#.##.#.##..##
......#.......##...#......#....
....#....##.......###.#......#.
.....#..#.#........#....#.....#
#...#...#....#...###........#..
#...........####.......#.#..#.#
..###....#..........#...#.###..
....#.#.....#....#..#.....#.##.
...##.#..#..#.......#......#.#.
....#......###..#.....#.....#..
.....#.#.#.....#.##.#....####..
.##....#.....#.#....##..#......
#..#.....#..#...#....#.#.......
.##.#..####..#.##.#......#.....
......#....#.......##.##....#..
...#....#....#..##.......##.###
..##..........##.............#.
.#...#.#...##..##.....#..#.....
....#.#.##...................#.
.......#.#..#....#.....#.......
.#.#..#....####...#.#.##....#..
.#.##...#..#..#...#.#.......#..
##.#.....##.........#.......#..
.##...#.....#.........##.#....#
.............#..#............##
...##.......#.....#.......#.##.
##..##.........................
.##.#........#........#........
.....#................#.#......
.............#....#....##....#.
#..##...##...#..#.#............
.......#...####.#..#..#.....##.
..#.#..#......#.....#.#.#.....#
...#..##........#..#.#....#.#..
.#.....#..###..#....#.##.#...#.
#.#..#.##.#..#......#.###...#..
##..#.#..###....##.#...#...##.#
##..#.........#...##......#....
#.#...#.#..#..........#.......#
.......#.#.......#.....##..#...
........#..##............##.#..
........##.....#........#..#...
#..##.#..###......##...........
..#.....#.#.#....#...#.#..#..##
#...............#.......#.#.##.
#..#.....#....#............#.#.
...#....#...#....#..#..###.....
..#....#.#.....#..#......##.#.#
.#.#....#..#...#....#........#.
..##....##....#.....#.#........
.#...#....##..##.....##.....##.
.#...........#....##...##.#....
...#.....#......###.##.#.......
......#.#..##.#.#....#...#...##
....#...###.##....#.#.....#....
.......#.....#......#.....##..#
.####.#...##..#....#...........
................###...#....#..#
...#...#.....###.#.##.......#..
..#....#...##...#.###......#.#.
#...#......#............#.....#
#.........#...............#..#.
...#.##.....#............#.....
........#......##..#..#..#.#..#
....#....#.....#.#.....##..#...
.....#....#..##.....#..........
.##....#..#...........##.......
#......##.....#...#.....#......
...#.....#......#.#....#.......
...#................##...#..#..
........#..........#....#......
......#....#.#.#...........#.#.
.#............#....##.......##.
#.......#.....#...##.#..##.....
.#.....#.##..#..#....#.#..#.#.#
....#...............###........
#####...........#..#.......#..#
...#.......#...#.#............#
#...#..#.#...#.#...#.##.....##.
.#..#..#..#.....#....#...#.....
.#...#......#.......#.........#
.#....#.....#...#...#..#....#..
#....#....#.......#.....##.....
.#...#.#.##.#....#..##........#
..##...#............#..........
..........#..#..#...#....#.....
..#.......#....#.....##..##....
.#...#......#...#..###...#...#.
..##...#......#...#.#.#...#....
.....#..#.#.#.#.#...#....##..#.
##..#..##....#.#........##.#...
.##..#.#...##..#....#..#.......
.....#...#...#..#.#..#......#..
.#.....##.##..#....####..#....#
......##.................#....#
....##.......###...#.##...##.#.
...#...#.................##.#..
.#.....##...#...#.....#.....##.
##.........####..#...#...#....#
...##.....#......#.###..#......
.....###..##.#.......###..##...
#....#...#.#...#...#.#....#..#.
#...#.........##.#.........###.
#....#..###..........##........
.###.....#.#.....#........##..#
....#.........##..#..#.#.#..#..
..#......#...........#..##...#.
...#.#..#..#...#.##..#..#.....#
.#...#...#....................#
..#..##..#.............#.....#.
.....###.#.#.#...##..#.##....#.
..#...####..##.#....#...#...#..
.....#..#........#.#.#..#.##...
#.#.........####..#...#.#......
..............#..#........#....
....#........#......#.........#
#..#.##......#.#.......#....#..
....#..............#.#.#..#....
#.#......#.....##.......#..##..
.#.#..........#....#......#....
.....#.......#.##.....#......##
...#...#.##.............####...
..#....##...#...##..#.#..##.#..
..#.........##.......###.#.....
..#.........#####..##...#......
..#.#...#.......#.####......##.
......#.#.#....#......####....#
.###...........#...#..#..#..##.
..#...#..##.##...#.#.##.....#..
.....#..#....##.......#...#....
......#.....#.........#..#..#..
...#..#.........##.....##.#...#
....##...#......#..#.....#.....
....#..#....#....#........##...
##.....#.......#.....#.#.#..#..
.....#..##.....##.##.#.........
.#.#..##.............#.#.......
......#.##.#.....#.#......#..#.
..........#.#..#....#.#.#.#..##
...##.....#..#...#...#...##....
........#.#......#..###..#.....
..#.##......#.......#.......#..
...#....##.##.........#.#......
......#....#.#.........#......#
.....#...#....#...#......#..#..
.##...#......#.........#...#.#.
..#.#.#......#....#............
..#.....##.............#.##.##.
#......#......#...##.......#.#.
##........#.....#..............
.#.###.................#.#....#
........##.#..##........#.#....
.......###...#...##.#..#....#..
.#..#....#..#......##......#...
.#...#....#..........##..##.#..
.#..###.......#............#...
...#.....###.#..#........#.#.#.
...#....#..#.##..........#.#.#.
.#..##..#.....#...........#....
#...#...##....#..#....##.......
#..#......#................#...
#..##....#.#..#......#.#.#.....
##.#..#...#.....#.#...#......##
#....#.#.#....#.....##.....##..
....#...##.#...####.#.#.#.#..#.
.....#.#....#..#.....#..#......
.........#.#...................
........#.....####......#..#..#
.#.#.##.#...#.#......#...##.##.
.#......#.#.#...#..#.......#...
..#......#.##.##.#.#....#......
..........#.#...###............
.##..#..#.#.#..#.....#..#.#....
......#.......#.#..#.#....#...#
.#.......###......#...#.#.#....
.............##..#..#...#....#.
....#......#.#...#.#...#...#...
..#....#.......#.#..#..#.#..#.#
.#..#.#...#.....#.#...#####...#
.##............#....#..........
#.......####...#.#.#...........
...#.......##.#..........#....#
..#.#......#.......##.....#..##
#......#.###..#......#......#.#
##....#..#....#.##....#..#.....
...##...#.#....#.#.......#.....
#...####....#..#.#..#.##....###
.....#..#..........###..#......
.#..#..#...#....#.##..#..#.....
#..#.....#....#..#.##...##.....
.....###.#..#.......#...###.##.
#..#........#.#..#.#.........#.
....##........................#
.#....#.#.#.#.#...#......#....#
#....#...#.##.......#.#.###....
..........###..##....#..##.#...
...##..###...#.#.#.......##...#
##.#...#..#.....###....#.......
..#..##....###........##....###
.....##..#...#..#.....#..#....#
#................#....#...#..##
#....#.#....#..###.#.#...#..#.#
........##.#...#.#.#.#...#.....
..#..###....#......##.#...##...
..#..##....#.##..#.....#.....#.
.#.#...#.....#..#..#......##.#.
........#.#...#..##....#..#....
...##...#...#...#...##...##..#.
.......#..#..#....#.#..#...##..
.#.....#.##........#...#.#.....
##.#..#....#.#....#.#....#...#.
..#.#......#.......##...#....#.
#.#..####..#........#.......###
....#.......#.......##.#...#.#.
..#..#.#.............#..#......
........###.....##....#.......#
...#.....#...#...#....#.###....
#...##.#........#..#...##..#..#
...##..#....#....#.#.#...#.#...
#......#.....#....###......##..
.....#.........####...##..#....
.......#...##...#..#..#.#......
.#.#....#.....#.......#........
...##...#....##..#.....###.....
.#....#........##......#....#.#
.........#.#.#.#...........#.#.
....#.#..##......#.#.#..##.....
.........#.....##....#.........
....#.............#...........#
...#..##........#.....###......
#....#....#......#..#..#..#.#..
#......##.....#..#....#..#.#...
#..............#....#.#....###.
..##..#..#...#...##........##..
..#.##....#..#......###..#.....";

        char Tree = '#';
        #endregion

        public void Prob1()
        {
            var lines = Input.Split("\r\n");
            int index = 0;
            int movementCount = 3;
            var treeCount = 0;
            var lineCount = 0;
            foreach (var line in lines)
            {
                var checkIndex = index % line.Length;
                var isTree = line[checkIndex] == Tree;
                if (isTree)
                {
                    treeCount++;
                }
                Console.WriteLine($"On line {lineCount} hit a tree: {isTree} (index: {index}). Total Trees Hit: {treeCount}");
                index += movementCount;
                lineCount++;
            }

            Console.WriteLine($"Total Trees hit: {treeCount}.");
        }

        long PerformRun(int down, int right)
        {
            var lines = Input.Split("\r\n");
            int index = 0;
            var treeCount = 0;
            var lineCount = 0;
            for (int i = 0; i < lines.Length; i+= down)
            {
                var line = lines[i];
                var checkIndex = index % line.Length;
                var isTree = line[checkIndex] == Tree;
                if (isTree)
                {
                    treeCount++;
                }
                index += right;
                lineCount++;
            }

            return treeCount;
        }

        public void Part2()
        {
            var run1 = PerformRun(1, 1);
            Console.WriteLine($"Run hit {run1} trees.");
            var run2 = PerformRun(1, 3);
            Console.WriteLine($"Run hit {run2} trees.");
            var run3 = PerformRun(1, 5);
            Console.WriteLine($"Run hit {run3} trees.");
            var run4 = PerformRun(1, 7);
            Console.WriteLine($"Run hit {run4} trees.");
            var run5 = PerformRun(2, 1);
            Console.WriteLine($"Run hit {run5} trees.");
            var mult = (long)(run1 * run2 * run3 * run4 * run5);
            Console.WriteLine($"Product of run hits is: {mult}.");
        }

    }
}
