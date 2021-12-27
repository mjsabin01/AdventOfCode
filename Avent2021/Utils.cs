using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avent2021
{
    internal static class Utils
    {
        public static void PrintMatrix<T>(T[,] matrix)
        {
            var rows = matrix.GetLength(0);
            var cols = matrix.GetLength(1);

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    Console.Write(matrix[row, col]);
                }
                Console.WriteLine();
            }
        }

        public static void PrintBoolMatrix(bool[,] matrix, char trueChar = '#', char falseChar = ' ')
        {
            var rows = matrix.GetLength(0);
            var cols = matrix.GetLength(1);

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    Console.Write(matrix[row, col] ? trueChar : falseChar);
                }
                Console.WriteLine();
            }
        }

        public static T2 GetOrAdd<T1, T2>(this Dictionary<T1,T2> dict, T1 key, T2 addVal) where T1 : notnull
        {
            if (!dict.ContainsKey(key))
            {
                dict[key] = addVal;
            }
            return dict[key];
        }

        public static int[,] BuildMatrixWithPadding(string[] lines, int paddingVal)
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
                matrix[0, i] = paddingVal;
                matrix[rows + 1, i] = paddingVal;
            }
            for (int i = 0; i < rows + 2; i++)
            {
                matrix[i, 0] = paddingVal;
                matrix[i, cols + 1] = paddingVal;
            }

            return matrix;
        }
    }

    public struct Coordinate
    {
        public int Row;
        public int Col;

        public Coordinate(int row, int col)
        {
            Row = row;
            Col = col;
        }

        public override string ToString()
        {
            return $"Row: {Row} Col: {Col}";
        }
    }

    public struct Coordinate3d
    {
        public long Row;
        public long Col;
        public long Height;

        public Coordinate3d(long row, long col, long height)
        {
            Row = row;
            Col = col;
            Height = height;
        }

        public override string ToString()
        {
            return $"Row: {Row} Col: {Col} Height: {Height}";
        }
    }

    public class Node<T>
    {
        public T Val { get;}

        public Node<T> Next { get; set; }

        public Node<T> Previous { get; set;}

        public Node(T val)
        {
            Val = val;
        }
    }
}
