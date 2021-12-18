﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avent2021
{
    internal class Day02
    {

        public void Run()
        {
            var lines = Input.Split("\r\n");
            Part2(lines);
        }

        public void Part1(string[] lines)
        {
            long x = 0, y = 0;
            foreach (var line in lines)
            {
                var parts = line.Split(' ');
                var amount = long.Parse(parts[1]);
                switch (parts[0])
                {
                    case "down": y += amount; break;
                    case "forward": x += amount; break;
                    case "up": y -= amount; break;
                }
            }

            Console.WriteLine($"Final horiz: {x}, final depth: {y}. Product = {x * y}");
        }

        public void Part2(string[] lines)
        {
            long x = 0, y = 0, aim = 0;
            foreach (var line in lines)
            {
                var parts = line.Split(' ');
                var amount = long.Parse(parts[1]);
                switch (parts[0])
                {
                    case "down": aim += amount; break;
                    case "forward": x += amount; y += aim * amount; break;
                    case "up": aim -= amount; break;
                }
            }

            Console.WriteLine($"Final horiz: {x}, final depth: {y}. Product = {x * y}");
        }

        #region TestInput

        public string TestInput =
            @"forward 5
down 5
forward 8
up 3
down 8
forward 2";

        #endregion

        #region Input

        public string Input =
            @"forward 8
down 6
down 8
forward 7
down 5
up 2
down 3
down 7
down 8
down 8
down 8
down 2
up 1
down 3
up 2
down 4
down 2
forward 6
forward 4
down 3
down 2
forward 2
forward 1
forward 4
forward 5
forward 8
down 1
down 4
up 5
up 2
forward 3
down 9
forward 7
forward 9
forward 9
forward 8
down 1
down 2
forward 7
down 3
forward 6
down 4
forward 7
down 1
up 8
forward 3
down 1
forward 7
up 1
forward 8
up 6
up 2
down 6
forward 1
up 6
forward 5
down 9
up 5
forward 7
forward 9
down 9
down 3
forward 7
forward 8
forward 3
forward 9
forward 7
down 3
down 7
down 4
forward 2
down 7
down 3
down 5
up 1
down 9
up 4
forward 1
up 9
down 2
forward 8
down 8
down 6
forward 7
down 9
down 3
forward 8
forward 3
down 6
down 7
down 4
forward 3
down 3
down 9
forward 8
forward 9
up 5
forward 1
down 3
down 3
down 3
down 9
down 2
down 9
forward 5
up 3
up 5
up 7
down 2
down 7
down 9
down 5
down 4
down 8
forward 1
up 8
up 3
forward 1
forward 5
forward 3
up 7
down 9
down 9
forward 7
down 1
forward 1
forward 8
forward 6
down 1
down 7
forward 9
up 4
forward 8
up 6
forward 3
down 3
down 9
forward 5
up 3
down 7
forward 9
forward 2
up 1
forward 7
up 8
forward 7
forward 1
up 3
up 7
down 1
forward 5
up 8
down 2
up 2
up 3
down 5
forward 6
up 8
down 7
up 8
up 4
down 8
forward 9
down 8
down 2
up 7
down 5
forward 1
up 1
down 1
forward 1
forward 1
forward 3
forward 8
down 4
down 5
forward 9
up 6
up 7
down 8
forward 8
down 2
forward 6
down 3
forward 9
forward 5
up 7
down 2
up 6
up 6
down 9
forward 3
up 1
up 2
forward 9
down 1
up 3
forward 4
forward 9
down 3
down 4
forward 4
up 6
up 5
forward 2
down 5
down 1
forward 9
down 7
up 6
up 5
forward 4
forward 9
down 6
forward 1
up 6
down 1
forward 4
up 9
down 6
forward 5
down 2
forward 8
forward 9
down 7
down 4
down 1
forward 1
down 4
down 6
forward 5
forward 2
forward 8
forward 5
down 6
up 9
forward 2
down 1
forward 6
forward 6
down 5
forward 5
down 8
forward 3
down 5
up 1
forward 4
down 5
down 4
forward 4
down 3
down 5
down 7
forward 5
forward 2
up 2
up 4
forward 7
down 3
down 1
down 7
up 8
forward 6
forward 3
forward 7
forward 5
up 5
down 3
down 6
forward 7
up 9
up 5
forward 2
down 9
forward 8
forward 6
forward 5
up 5
down 9
down 8
up 2
up 4
forward 5
forward 2
up 4
forward 3
down 7
forward 8
forward 1
forward 9
forward 6
up 7
up 2
forward 1
down 5
forward 9
down 8
down 4
down 7
up 2
down 5
forward 7
up 3
forward 6
down 2
forward 8
forward 8
up 3
forward 6
forward 9
forward 8
forward 3
up 9
forward 9
down 6
forward 5
forward 8
up 1
forward 2
forward 6
forward 8
up 6
down 3
down 9
down 6
up 7
forward 6
forward 1
forward 1
forward 7
down 5
down 9
down 3
up 3
forward 3
forward 2
down 5
up 4
forward 1
down 9
forward 9
forward 1
forward 1
down 9
down 2
forward 4
forward 9
down 5
up 5
down 6
forward 8
down 4
down 1
up 5
up 3
down 2
down 3
forward 8
forward 5
forward 9
down 4
up 9
down 1
forward 2
down 8
up 2
down 8
up 6
forward 7
down 1
up 7
down 9
forward 9
down 9
forward 7
forward 4
down 5
up 3
down 3
forward 8
down 3
down 4
down 9
forward 4
up 4
forward 6
down 1
forward 5
down 2
forward 6
down 4
down 1
forward 3
up 3
up 3
forward 8
forward 6
forward 6
down 9
forward 5
down 9
forward 6
forward 3
up 4
forward 6
down 8
up 3
down 9
down 3
forward 6
down 4
down 8
down 6
down 5
forward 1
down 3
forward 9
down 9
down 3
forward 9
down 2
forward 3
up 6
forward 2
forward 1
forward 8
down 2
down 2
down 7
up 7
forward 3
up 2
up 6
up 6
down 2
forward 2
forward 2
down 6
down 2
up 6
forward 4
down 9
up 3
down 4
forward 7
up 6
forward 3
forward 1
down 1
down 8
down 8
down 1
forward 2
down 6
down 6
forward 2
up 6
down 2
up 4
down 1
up 8
up 5
down 4
forward 2
forward 2
down 2
forward 9
down 5
down 9
forward 6
down 9
down 5
down 7
down 3
up 9
down 6
up 6
up 8
forward 8
forward 8
down 3
up 9
forward 9
forward 8
forward 6
down 4
down 6
up 9
down 9
down 5
up 2
up 2
forward 2
forward 1
down 5
down 8
up 3
forward 2
down 1
down 9
forward 7
forward 5
up 3
up 6
down 5
up 1
down 2
up 7
forward 1
down 6
up 6
up 1
up 2
forward 2
down 4
up 1
up 3
up 9
up 7
forward 4
down 5
down 9
down 8
forward 1
down 4
forward 4
forward 8
up 4
down 8
down 1
down 9
down 5
forward 3
forward 8
up 2
down 6
up 6
forward 5
down 6
down 8
forward 6
down 6
up 5
down 2
up 5
down 7
down 9
forward 3
down 8
forward 1
forward 5
forward 2
down 4
forward 2
forward 7
up 7
up 3
down 2
forward 7
up 6
forward 6
forward 1
down 4
down 2
down 6
down 1
forward 1
forward 8
down 1
up 2
down 2
down 1
down 6
forward 7
forward 6
forward 5
down 1
down 8
down 1
up 5
forward 6
forward 5
up 5
forward 5
up 8
down 3
forward 1
forward 6
up 8
up 9
down 7
down 1
forward 2
forward 1
forward 9
forward 3
forward 7
forward 8
down 6
up 5
down 1
forward 1
forward 8
down 6
forward 7
forward 8
down 7
down 5
down 7
up 7
down 5
forward 5
down 4
down 7
forward 6
forward 5
forward 6
forward 7
up 9
down 2
down 2
down 4
down 8
up 3
down 7
down 5
forward 6
down 9
down 5
down 9
down 1
forward 6
up 7
down 2
down 2
forward 8
forward 1
down 3
down 4
forward 3
forward 4
down 1
forward 9
up 7
forward 8
down 9
forward 7
forward 6
forward 2
down 8
up 9
down 2
forward 8
up 7
down 5
down 9
down 3
down 6
down 4
up 2
down 3
down 1
up 1
up 6
forward 4
down 1
forward 1
up 4
forward 4
forward 3
forward 8
forward 9
forward 9
down 2
down 5
up 8
up 1
down 9
forward 5
down 1
up 5
down 4
up 3
forward 9
up 7
forward 9
up 1
forward 4
forward 8
up 6
down 6
down 8
down 8
down 9
down 2
up 7
forward 9
up 8
down 9
up 6
forward 4
up 7
down 6
up 7
down 4
forward 2
forward 9
down 6
down 8
forward 6
forward 3
down 3
forward 3
forward 7
up 2
down 8
forward 7
down 5
down 1
down 6
down 5
down 2
up 6
forward 7
forward 6
down 1
down 5
forward 7
forward 3
down 9
down 8
forward 5
up 7
forward 1
up 5
down 7
forward 8
forward 6
forward 2
down 1
down 9
up 1
down 2
down 2
down 7
down 4
forward 1
down 3
down 5
up 8
forward 7
up 5
down 8
down 6
down 3
down 3
down 9
down 7
forward 4
up 5
forward 3
forward 7
down 3
up 6
forward 4
forward 4
down 4
down 2
up 1
forward 8
forward 3
up 1
forward 1
down 9
down 6
up 1
down 4
down 8
up 9
forward 2
down 3
forward 8
down 6
down 5
down 4
up 5
down 9
up 3
forward 4
down 9
down 7
forward 6
forward 6
forward 8
forward 6
down 9
down 1
forward 3
forward 9
forward 4
up 8
up 5
up 2
down 9
forward 9
forward 3
forward 5
up 8
down 2
down 1
forward 9
forward 7
down 7
forward 1
down 5
down 8
down 4
down 7
down 1
down 4
down 7
forward 2
down 5
forward 1
down 4
down 5
down 2
up 5
forward 9
down 5
forward 1
down 7
down 4
down 7
down 6
forward 5
down 3
down 1
up 2
forward 2
forward 2
forward 1
down 1
forward 3
forward 5
forward 4
down 7
forward 7
down 1
forward 7
forward 5
down 8
forward 6
forward 6
forward 6
forward 7
up 9
down 4
down 1
down 8
forward 7
up 4
forward 4
down 6
up 1
forward 5
forward 2
down 1
forward 7
forward 6
forward 5
forward 2
down 5
down 6
down 9
up 4
forward 6
forward 2
down 5
down 3
up 4
down 6
up 8
forward 8
up 9
forward 6
forward 6
up 5
down 7
forward 9
forward 6
down 9
down 9
up 1
forward 7
down 6
up 4
down 8
down 3
forward 9
forward 5
forward 9
down 2
forward 3
down 1
forward 9
up 4
up 8
forward 6
down 1
forward 9
forward 4
down 5
forward 2
up 3
forward 5
up 8
up 7
down 8
forward 4
down 6
forward 7
up 2
down 2
forward 4
down 9
down 8
forward 2
forward 2
down 2
down 3
forward 3
down 1
forward 8
down 7
up 9
down 4
down 2
down 5
up 7
down 8
down 2
down 4
down 4
down 8
forward 7
forward 7
down 8
up 2
up 3
forward 8
up 1
down 7
forward 7
down 6
down 8
up 6
forward 5
forward 3
down 6
forward 9
up 4
up 7
forward 4
down 1
down 8
down 1
forward 9
down 3
forward 8
forward 6
forward 4
down 9
forward 3
up 5
up 8
down 9
down 5
down 1
up 8
forward 8
up 6
forward 2
down 8
up 4
up 7
forward 7
forward 5
forward 9
forward 2
up 4
down 9
forward 7
down 6
down 6
forward 7
down 5
up 6
down 9
forward 3";

        #endregion
    }
}
