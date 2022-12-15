using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avent2022;

internal class Day12
{
    public void Run()
    {
        var lines = Input.Split("\r\n");
        Part2(lines);
    }

    struct PointWithCost
    {
        public Point Point;
        public int Cost;

        public PointWithCost(Point point, int cost)
        {
            Point = point;
            Cost = cost;
        }
    }

    public void Part1(string[] lines)
    {
        var (map, start, end) = ParseInput(lines);
        var minSteps = RunPath(map, start, end, int.MaxValue);
        
        Console.WriteLine($"Min minSteps is: {minSteps}.");
    }

    public void Part2(string[] lines)
    {
        var (map, start, end) = ParseInput(lines);
        var minCostOfPaths = int.MaxValue;
        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                if (map[x,y] == 0)
                {
                    var pathCost = RunPath(map, new Point(x, y), end, minCostOfPaths);
                    minCostOfPaths = Math.Min(minCostOfPaths, pathCost);
                }
            }
        }

        Console.WriteLine($"Min minSteps is: {minCostOfPaths}.");
    }

    private List<Point> GetNeighbors(int[,] map, Point current)
    {
        var pointsToCheck = new List<Point>();
        if (current.X != 0)
        {
            pointsToCheck.Add(new Point(current.X - 1, current.Y));
        }
        if (current.X != map.GetLength(0) - 1)
        {
            pointsToCheck.Add(new Point(current.X + 1, current.Y));
        }
        if (current.Y != 0)
        {
            pointsToCheck.Add(new Point(current.X, current.Y - 1));
        }
        if (current.Y != map.GetLength(1) - 1)
        {
            pointsToCheck.Add(new Point(current.X, current.Y + 1));
        }

        return pointsToCheck;
    }

    
    private int RunPath(int[,] map, Point start, Point end, int previousMinPath)
    {
        var pathVisited = new bool[map.GetLength(0), map.GetLength(1)];

        var pqueue = new PriorityQueue<PointWithCost, int>();
        pqueue.Enqueue(new PointWithCost(start, 0), 0);
        var minSteps = int.MaxValue;
        while (pqueue.Count > 0)
        {
            var current = pqueue.Dequeue();

            // check if the current path is greater than previously found min. If so, just quit since we can't beat it.
            if (current.Cost > previousMinPath)
                return current.Cost;

            if (current.Point == end)
            {
                minSteps = current.Cost;
                break;
            }

            if (pathVisited[current.Point.X, current.Point.Y]) continue;



            pathVisited[current.Point.X, current.Point.Y] = true;

            var neighbors = GetNeighbors(map, current.Point);
            var currentHeight = map[current.Point.X, current.Point.Y];
            foreach (var neighbor in neighbors)
            {
                var pointHeight = map[neighbor.X, neighbor.Y];
                if (pointHeight > currentHeight + 1) continue;
                pqueue.Enqueue(new PointWithCost(neighbor, current.Cost + 1), current.Cost + 1);
            }
        }

        return minSteps;
    }

    
    (int[,] map, Point start, Point end) ParseInput(string[] lines)
    {
        var map = new int[lines.Length, lines[0].Length];
        Point start = new Point(0,0), end = new Point(0,0);

        for (int row = 0; row < lines.Length; row++)
        {
            for (int col = 0; col < lines[row].Length; col++)
            {
                var c = lines[row][col];
                switch (c)
                {
                    case 'S':
                        start = new Point(row, col);
                        map[row, col] = 0;
                        break;
                    case 'E':
                        end = new Point(row, col);
                        map[row, col] = 25;
                        break;
                    default:
                        map[row, col] = c - 'a';
                        break;
                }
            }
        }

        return (map, start, end);
    }

    #region TestInput

    public string TestInput =
        @"Sabqponm
abcryxxl
accszExk
acctuvwj
abdefghi";

    #endregion

    #region Input

    public string Input =
        @"abaaaaaccccccccccccccccccaaaaaaaaaaaaaccccaaaaaaaccccccccccccccccccccccccccccaaaaaa
abaaaaaaccaaaacccccccccccaaaaaaaaacaaaacaaaaaaaaaacccccccccccccccccccccccccccaaaaaa
abaaaaaacaaaaaccccccccccaaaaaaaaaaaaaaacaaaaaaaaaacccccccccccccaacccccccccccccaaaaa
abaaaaaacaaaaaacccccccccaaaaaaaaaaaaaaccaaacaaaccccccccccccccccaacccccccccccccccaaa
abccaaaccaaaaaacccaaaaccaaaaaaaaaaaaaccccaacaaacccccccccaacaccccacccccccccccccccaaa
abcccccccaaaaaccccaaaacccccaaaaacccaaaccaaaaaaccccccccccaaaaccccccccccccccccccccaac
abcccccccccaaaccccaaaacccccaaaaacccccccccaaaaaccccccccccklllllccccccccccccccccccccc
abcccccccccccccccccaaccccccccaaccccccccaaaaaaaccccccccckklllllllcccccddccccaacccccc
abaccccccccccccccccccccccccccaaccccccccaaaaaaaaccccccckkkklslllllcccddddddaaacccccc
abacccccccccccccccccccccccccccccccaaaccaaaaaaaaccccccckkkssssslllllcddddddddacccccc
abaccccccccccccccccccccccccccccccccaaaaccaaacaccccccckkksssssssslllmmmmmdddddaacccc
abcccccccccccccccaaacccccccccccccaaaaaaccaacccccccccckkkssssusssslmmmmmmmdddddacccc
abcccccccaaccccaaaaacccccccccccccaaaaaccccccaaaaaccckkkrssuuuussssqmmmmmmmmdddccccc
abcccccccaaccccaaaaaacccccccaaccccaaaaacccccaaaaacckkkkrruuuuuussqqqqqqmmmmdddccccc
abccccaaaaaaaacaaaaaacccccccaaaaccaaccaccccaaaaaacjkkkrrruuuxuuusqqqqqqqmmmmeeccccc
abcaaaaaaaaaaacaaaaaccccccaaaaaacccccaaccccaaaaajjjjrrrrruuuxxuvvvvvvvqqqmmmeeccccc
abcaacccaaaaccccaaaaaaacccaaaaacccacaaaccccaaaajjjjrrrrruuuxxxxvvvvvvvqqqmmeeeccccc
abaaaaccaaaaacccccccaaaccccaaaaacaaaaaaaacccaajjjjrrrrtuuuuxxxyvyyyvvvqqqnneeeccccc
abaaaaaaaaaaacccaaaaaaaccccaacaacaaaaaaaacccccjjjrrrttttuxxxxxyyyyyvvvqqnnneeeccccc
abaaaaaaaccaacccaaaaaaaaacccccccccaaaaaaccccccjjjrrrtttxxxxxxxyyyyyvvvqqnnneeeccccc
SbaaaaaacccccccccaaaaaaaaaccccccccaaaaacccccccjjjrrrtttxxxEzzzzyyyvvrrrnnneeecccccc
abaaaaacccccccccccaaaaaaacccccccccaaaaaaccccccjjjqqqtttxxxxxyyyyyvvvrrrnnneeecccccc
abaaacccccccccccaaaaaaaccaaccccccccccaaccaaaaajjjqqqttttxxxxyyyyyyvvrrrnnneeecccccc
abaaacccccccccccaaaaaaaccaaacaaacccccccccaaaaajjjjqqqtttttxxyywyyyywvrrnnnfeecccccc
abcaaacccccccaaaaaaaaaaacaaaaaaaccccccccaaaaaaciiiiqqqqtttxwyywwyywwwrrrnnfffcccccc
abcccccccccccaaaaaaaaaaccaaaaaacccccccccaaaaaacciiiiqqqqttwwywwwwwwwwrrrnnfffcccccc
abccccccccccccaaaaaacccaaaaaaaacccccccccaaaaaaccciiiiqqqttwwwwwswwwwrrrrnnfffcccccc
abccccccccccccaaaaaacccaaaaaaaaacccccccccaaacccccciiiqqqtswwwwssssrrrrrroofffcccccc
abccccccaaaaacaaaaaacccaaaaaaaaaaccccccccccccccccciiiqqqssswsssssssrrrrooofffaccccc
abccccccaaaaacaaccaaccccccaaacaaacccccccccccccccccciiiqqssssssspoorrrooooofffaacccc
abcccccaaaaaacccccccccccccaaacccccccccccccccccccccciiiqppssssspppooooooooffffaacccc
abcccccaaaaaacccccccccccccaacccccccccccccccccccccccciipppppppppppoooooooffffaaccccc
abcccccaaaaaaccccccccccccccccccccccccccccccccccccccciihppppppppgggggggggfffaaaccccc
abccccccaaacccccccccccccccccccccccaccccccccccccccccchhhhpppppphggggggggggfaaaaccccc
abaaaccccccccccccccccccccccaccccaaacccccccccccccccccchhhhhhhhhhgggggggggcaacccccccc
abaaccaaaccaccccccccccccccaaacccaaacaacccaaaaacccccccchhhhhhhhhgaaccccccccccccccccc
abaaacaaacaacccccccccaaaccaaaacaaaaaaaaccaaaaaccccccccchhhhhhaaaaacccccccccccccccca
abaaaccaaaaaccccccccccaaacaaaaaaaacaaaaccaaaaaaccccccccccaaacccaaaacccccccccccaccca
abcccaaaaaaccccccccccaaaaaaaaaaaaacaaaaccaaaaaaccccccccccaaaccccaaaccccccccccaaaaaa
abcccaaaaaaaacccccccaaaaaaaaaaaaaaaaaccccaaaaaacccccccccccccccccccccccccccccccaaaaa
abcccaacaaaaaccccccaaaaaaaaaaaaaaaaaaacccccaacccccccccccccccccccccccccccccccccaaaaa";

    #endregion 
}

