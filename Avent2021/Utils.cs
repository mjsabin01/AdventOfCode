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
