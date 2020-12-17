using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avent2020
{
    public class Day17Pt2
    {
        public class Coordinate
        {
            public Coordinate(int x, int y, int z, int w)
            {
                X = x;
                Y = y;
                Z = z;
                W = w;
            }

            public int X { get; set; }
            public int Y { get; set; }

            public int Z { get; set; }

            public int W { get; set; }
        }

        public void Run()
        {
            var input = Input;
            var lines = input.Split("\r\n");
            var length = lines[0].Length;
            var dimension = (int)Math.Pow(length, 2);
            var cube = new bool[dimension, dimension, dimension, dimension];
            var startPos = (dimension / 2) - (length / 2);

            Coordinate min = new Coordinate(startPos, startPos, startPos, startPos);
            Coordinate max = new Coordinate(startPos + length, startPos + length, startPos, startPos);

            var y = min.Y;
            var initialActive = 0;
            foreach (var line in lines)
            {
                var x = min.X;
                for (int i = 0; i < line.Length; i++)
                {
                    if (line[i] == '#')
                    {
                        initialActive++;
                        cube[x, y, min.Z, min.W] = true;
                    }
                    x++;
                }
                y++;
            }

            Console.WriteLine("Initial cube.");
            for (int w = min.W; w < max.W + 1; w++)
            {
                Console.WriteLine($"\r\nW = {w - startPos}.");
                for (int z = min.Z; z < max.Z + 1; z++)
                {
                    Console.WriteLine($"\r\nZ = {z - startPos}.");
                    for (y = min.Y; y < max.Y + 1; y++)
                    {
                        for (int x = min.X; x < max.X + 1; x++)
                        {
                            Console.Write(cube[x, y, z, w] ? "#" : ".");
                        }
                        Console.Write("\r\n");
                    }
                }
            }
                

            Console.WriteLine($"There are {initialActive} active at start.");

            for (int i = 0; i < 6; i++)
            {
                (cube, min, max) = RunCycle(cube, dimension, min, max);

                var totalActive = 0;

                for (int w = min.W; w < max.W + 1; w++)
                {
                    for (int z = min.Z; z < max.Z + 1; z++)
                    {
                        //Console.WriteLine($"\r\nW = {w - startPos} Z = {z - startPos}.");
                        for (y = min.Y; y < max.Y + 1; y++)
                        {
                            for (int x = min.X; x < max.X + 1; x++)
                            {
                                //Console.Write(cube[x, y, z, w] ? "#" : ".");
                                if (cube[x, y, z, w])
                                {
                                    totalActive++;
                                }
                            }
                            //Console.Write("\r\n");
                        }
                    }
                }

                Console.WriteLine($"After {i + 1} rounds, there are {totalActive} active.");
            }


        }

        (bool[,,,] newCube, Coordinate newMin, Coordinate newMax) RunCycle(bool[,,,] cube, int dimension, Coordinate min, Coordinate max)
        {
            var checkCount = 0;
            var newMin = new Coordinate(min.X, min.Y, min.Z, min.W);
            var newMax = new Coordinate(max.X, max.Y, max.Z, max.W);

            var nextCube = new bool[dimension, dimension, dimension, dimension];
            for (int w = min.W - 1; w < max.W + 2; w++)
            {
                for (int x = min.X - 1; x < max.X + 2; x++)
                {
                    for (int y = min.Y - 1; y < max.Y + 2; y++)
                    {
                        for (int z = min.Z - 1; z < max.Z + 2; z++)
                        {
                            checkCount++;
                            var numActive = NumActiveNeighbors(cube, x, y, z, w);
                            if (cube[x, y, z, w])
                            {
                                nextCube[x, y, z, w] = numActive == 2 || numActive == 3;
                            }
                            else
                            {
                                nextCube[x, y, z, w] = numActive == 3;
                            }

                            if (nextCube[x, y, z, w])
                            {
                                newMax = new Coordinate(Math.Max(newMax.X, x), Math.Max(newMax.Y, y), Math.Max(newMax.Z, z), Math.Max(newMax.W, w));
                                newMin = new Coordinate(Math.Min(newMin.X, x), Math.Min(newMin.Y, y), Math.Min(newMin.Z, z), Math.Min(newMin.W, w));
                            }
                        }
                    }
                }
            }

            Console.WriteLine($"In this round: {checkCount} cubes checked.");
            return (nextCube, newMin, newMax);
        }



        int NumActiveNeighbors(bool[,,,] cube, int x, int y, int z, int w)
        {
            var numActive = 0;
            for (int w1 = w - 1; w1 <= w + 1; w1++)
            {
                for (int x1 = x - 1; x1 <= x + 1; x1++)
                {
                    for (int y1 = y - 1; y1 <= y + 1; y1++)
                    {
                        for (int z1 = z - 1; z1 <= z + 1; z1++)
                        {
                            if (x1 == x && y1 == y && z1 == z && w1 == w)
                            {
                                continue;
                            }

                            if (cube[x1, y1, z1, w1])
                            {
                                numActive++;
                            }
                        }
                    }
                }
            }

            return numActive;
        }



        string TestInput1 = @".#.
..#
###";

        string Input = @"##..####
.###....
#.###.##
#....#..
...#..#.
#.#...##
..#.#.#.
.##...#.";
    }
}
