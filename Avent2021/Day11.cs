using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avent2021
{
    internal class Day11
    {
        public void Run()
        {
            var lines = Input.Split("\r\n");
            Part2(lines);
        }

        public void Part1(string[] lines)
        {
            var matrix = BuildInputMatrix(lines);
            var numSteps = 100;
            long totalFlashes = 0;
            Console.WriteLine("Initial matrix:");
            Utils.PrintMatrix(matrix);

            for (int i = 1; i <= numSteps; i++)
            {
                totalFlashes += RunStep(matrix);
                Console.WriteLine($"\r\nAfter step {i}:");
                Utils.PrintMatrix(matrix);
            }

            Console.WriteLine($"After {numSteps} there are {totalFlashes} flashes.");
        }

        long RunStep(int[,] matrix)
        {
            long flashes = 0;

            var rows = matrix.GetLength(0);
            var cols = matrix.GetLength(1);
            
            // First increment all values (except the border ones)
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    if (matrix[row, col] != -1)
                    {
                        matrix[row, col]++;
                    };
                }
            }

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    if (matrix[row, col] == 10)
                    {
                        flashes += HandleCellFlash(matrix, row, col);
                    }
                }
            }
            return flashes;
        }

        private long HandleCellFlash(int[,] matrix, int row, int col)
        {
            long flashCount = 0;
            if (matrix[row, col] != 10)
            {
                return flashCount;
            }

            flashCount = 1;
            matrix[row, col] = 0;

            for (int x = row -1; x <= row +1; x++)
            {
                for (int y = col -1; y <= col +1; y++)
                {
                    if (x == row && y == col || matrix[x,y] % 10 <= 0)
                    {
                        continue;
                    }

                    matrix[x, y]++;
                    flashCount += HandleCellFlash(matrix, x, y);
                }
            }

            return flashCount;
        }


        public void Part2(string[] lines)
        {
            var matrix = BuildInputMatrix(lines);

            var totalCells = lines.GetLength(0) * lines[0].Length;
            var stepNumber = 0;
            while (true)
            {
                stepNumber++;
                var stepFlashes = RunStep(matrix);
                Console.WriteLine($"Step {stepNumber} had a total of {stepFlashes}.");
                if (stepFlashes == totalCells)
                {
                    return;
                }
            }
        }

        int[,] BuildInputMatrix(string[] lines)
        {
            var rows = lines.GetLength(0);
            var cols = lines[0].Length;

            var matrix = new int[rows + 2, cols + 2];
            for (int row = 0; row < lines.Length; row++)
            {
                for (int col = 0; col < lines[row].Length; col++)
                {
                    matrix[row + 1, col + 1] = int.Parse(lines[row][col].ToString());
                }
            }

            for (int i = 0; i < cols + 2; i++)
            {
                matrix[0, i] = -1;
                matrix[rows + 1, i] = -1;
            }
            for (int i = 0; i < rows + 2; i++)
            {
                matrix[i, 0] = -1;
                matrix[i, cols + 1] = -1;
            }

            return matrix;
        }


        #region TestInput

        public string TestInput =
            @"5483143223
2745854711
5264556173
6141336146
6357385478
4167524645
2176841721
6882881134
4846848554
5283751526";

        #endregion

        #region Input

        public string Input =
            @"4721224663
6875415276
2742448428
4878231556
5684643743
3553681866
4788183625
4255856532
1415818775
2326886125";

        #endregion 
    }
}
