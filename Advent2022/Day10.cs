using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avent2022;

internal class Day10
{
    public void Run()
    {
        var lines = Input.Split("\r\n");
        Part2(lines);
    }

    public void Part1(string[] lines)
    {
        long x = 1;
        int cycle = 0;
        long signalStrengthSum = 0;
        var checkCycles = new HashSet<int>() { 20, 60, 100, 140, 180, 220 };
        

        for (int i = 0; i < lines.Length; i++)
        {
            var instructionParts = lines[i].Split(' ');            

            if (checkCycles.Contains(++cycle))
            {
                signalStrengthSum += cycle * x;
            }

            if (string.Equals("addx", instructionParts[0]))
            {
                if (checkCycles.Contains(++cycle))
                {
                    signalStrengthSum += cycle * x;
                }

                x += long.Parse(instructionParts[1].Trim());
            }
        }

        Console.WriteLine($"Sum of six signal strengths is: {signalStrengthSum}.");
    }

    public void Part2(string[] lines)
    {
        int spritePos = 1;
        int cycle = 0;
        var matrix = new bool[6, 40];


        for (int i = 0; i < lines.Length; i++)
        {
            var instructionParts = lines[i].Split(' ');

            var (row, col) = GetPosition(cycle++);
            if (Math.Abs(spritePos - col) < 2)
            {
                matrix[row, col] = true;
            }

            if (string.Equals("addx", instructionParts[0]))
            {
                (row, col) = GetPosition(cycle++);
                if (Math.Abs(spritePos - col) < 2)
                {
                    matrix[row, col] = true;
                }

                spritePos += int.Parse(instructionParts[1].Trim());
            }
        }

        Utils.PrintBoolMatrix(matrix, trueChar: '#', falseChar: ' ');
    }

    (int row, int col) GetPosition(int cycle) => (cycle / 40, cycle % 40);



    #region TestInput

    public string TestInput =
        @"addx 15
addx -11
addx 6
addx -3
addx 5
addx -1
addx -8
addx 13
addx 4
noop
addx -1
addx 5
addx -1
addx 5
addx -1
addx 5
addx -1
addx 5
addx -1
addx -35
addx 1
addx 24
addx -19
addx 1
addx 16
addx -11
noop
noop
addx 21
addx -15
noop
noop
addx -3
addx 9
addx 1
addx -3
addx 8
addx 1
addx 5
noop
noop
noop
noop
noop
addx -36
noop
addx 1
addx 7
noop
noop
noop
addx 2
addx 6
noop
noop
noop
noop
noop
addx 1
noop
noop
addx 7
addx 1
noop
addx -13
addx 13
addx 7
noop
addx 1
addx -33
noop
noop
noop
addx 2
noop
noop
noop
addx 8
noop
addx -1
addx 2
addx 1
noop
addx 17
addx -9
addx 1
addx 1
addx -3
addx 11
noop
noop
addx 1
noop
addx 1
noop
noop
addx -13
addx -19
addx 1
addx 3
addx 26
addx -30
addx 12
addx -1
addx 3
addx 1
noop
noop
noop
addx -9
addx 18
addx 1
addx 2
noop
noop
addx 9
noop
noop
noop
addx -1
addx 2
addx -37
addx 1
addx 3
noop
addx 15
addx -21
addx 22
addx -6
addx 1
noop
addx 2
addx 1
noop
addx -10
noop
noop
addx 20
addx 1
addx 2
addx 2
addx -6
addx -11
noop
noop
noop";

    #endregion

    #region Input

    public string Input =
        @"noop
noop
addx 5
addx 29
addx -28
addx 5
addx -1
noop
noop
addx 5
addx 12
addx -6
noop
addx 4
addx -1
addx 1
addx 5
addx -31
addx 32
addx 4
addx 1
noop
addx -38
addx 5
addx 2
addx 3
addx -2
addx 2
noop
addx 3
addx 2
addx 5
addx 2
addx 3
noop
addx 2
addx 3
noop
addx 2
addx -32
addx 33
addx -20
addx 27
addx -39
addx 1
noop
addx 5
addx 3
noop
addx 2
addx 5
noop
noop
addx -2
addx 5
addx 2
addx -16
addx 21
addx -1
addx 1
noop
addx 3
addx 5
addx -22
addx 26
addx -39
noop
addx 5
addx -2
addx 2
addx 5
addx 2
addx 23
noop
addx -18
addx 1
noop
noop
addx 2
noop
noop
addx 7
addx 3
noop
addx 2
addx -27
addx 28
addx 5
addx -11
addx -27
noop
noop
addx 3
addx 2
addx 5
addx 2
addx 27
addx -26
addx 2
addx 5
addx 2
addx 4
addx -3
addx 2
addx 5
addx 2
addx 3
addx -2
addx 2
noop
addx -33
noop
noop
noop
noop
addx 31
addx -26
addx 6
noop
noop
addx -1
noop
addx 3
addx 5
addx 3
noop
addx -1
addx 5
addx 1
addx -12
addx 17
addx -1
addx 5
noop
noop
addx 1
noop
noop";

    #endregion 
}

