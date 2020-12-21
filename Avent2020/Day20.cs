﻿using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Avent2020
{
    public class Day20
    {
        static Regex regTitle = new Regex(@"Tile (\d+):");

        public class Tile
        {
            public long Id { get; set; }
            public bool[,] Image { get; set; }

            public Tile(string input)
            {
                var lines = input.Split("\r\n");
                var match = regTitle.Match(lines[0]);
                if (!match.Success)
                {
                    throw new Exception("Bad format for title...");
                }
                Id = long.Parse(match.Groups[1].Value);
                Image = new bool[lines.Length - 1, lines.Length - 1];
                for (int row = 1; row < lines.Length; row++)
                {
                    for (int col = 1; col < lines.Length; col++)
                    {
                        Image[row - 1, col - 1] = lines[row][col - 1] == '#';
                    }
                }
            }

            public override string ToString()
            {
                var sb = new StringBuilder();
                sb.AppendLine($"ID: {Id}");
                sb.AppendLine();
                var dimension = Image.GetUpperBound(0) + 1;
                for (var row = 0; row < dimension; row++)
                {
                    for (var col = 0; col < dimension; col++)
                    {
                        sb.Append(Image[row, col] ? "#" : ".");
                    }
                    sb.Append("\r\n");
                }
                return sb.ToString();
            }


            public Tile NeighborTop { get; set; }
            public Tile NeighborLeft { get; set; }
            public Tile NeighborRight { get; set; }
            public Tile NeighborBottom { get; set; }

            public int NumberOfNeighbors
            {
                get
                {
                    var count = 0;
                    if (NeighborTop != null)
                        count++;
                    if (NeighborLeft != null)
                        count++;
                    if (NeighborRight != null)
                        count++;
                    if (NeighborBottom != null)
                        count++;
                    return count;
                }
            }
            public bool IsEdge => NumberOfNeighbors < 4;
            public bool IsCorner => NumberOfNeighbors == 2;
            public bool AllEdgesFound => NumberOfNeighbors == 4;

            public bool[] TopRow
            {
                get
                {
                    var dimension = Image.GetUpperBound(0) + 1;
                    var row = new bool[dimension];
                    for (int i = 0; i < dimension; i++)
                    {
                        row[i] = Image[0, i];
                    }
                    return row;
                }
            }

            public bool[] BottomRow
            {
                get
                {
                    var dimension = Image.GetUpperBound(0) + 1;
                    var row = new bool[dimension];
                    for (int i = 0; i < dimension; i++)
                    {
                        row[i] = Image[dimension - 1, i];
                    }
                    return row;
                }
            }

            public bool[] LeftCol
            {
                get
                {
                    var dimension = Image.GetUpperBound(0) + 1;
                    var col = new bool[dimension];
                    for (int i = 0; i < dimension; i++)
                    {
                        col[i] = Image[i, 0];
                    }
                    return col;
                }
            }

            public bool[] RightCol
            {
                get
                {
                    var dimension = Image.GetUpperBound(0) + 1;
                    var col = new bool[dimension];
                    for (int i = 0; i < dimension; i++)
                    {
                        col[i] = Image[i, dimension - 1];
                    }
                    return col;
                }
            }

            public void Rotate90()
            {
                this.Image = RotateMatrix90(Image, Image.GetUpperBound(0) + 1);
                var oldTop = NeighborTop;
                var oldBottom = NeighborBottom;
                var oldRight = NeighborRight;
                var oldLeft = NeighborLeft;

                NeighborTop = oldLeft;
                NeighborRight = oldTop;
                NeighborBottom = oldRight;
                NeighborLeft = oldBottom;
            }

            public void Flip()
            {
                this.Image = FlipMatrix(Image, Image.GetUpperBound(0) + 1);
                var oldLeft = NeighborLeft;
                NeighborLeft = NeighborRight;
                NeighborRight = oldLeft;
            }
        }

        static T[,] RotateMatrix90<T>(T[,] matrix, int n)
        {
            T[,] ret = new T[n, n];

            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < n; ++j)
                {
                    ret[i, j] = matrix[n - j - 1, i];
                }
            }

            return ret;
        }

        static T[,] FlipMatrix<T>(T[,] matrix, int n)
        {
            T[,] ret = new T[n, n];

            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < n; ++j)
                {
                    var origX = n - j - 1;
                    ret[i, j] = matrix[i, origX];
                }
            }

            return ret;
        }

        public void Run()
        {
            Part1();
        }

        void Part1()
        {
            var input = TestInput;
            var tileStrs = input.Split("\r\n\r\n");
            var tiles = new List<Tile>();
            foreach (var tileStr in tileStrs)
            {
                var tile = new Tile(tileStr);
                tiles.Add(tile);
            }
                        
            var cubeDimension = (int)Math.Sqrt(tiles.Count);
            Console.WriteLine($"There are {tiles.Count} to form a {cubeDimension} x {cubeDimension} grid.");

            for (int i = 0; i < tiles.Count; i++)
            {
                var tile1 = tiles[i];
                for (int j = i + 1; j < tiles.Count; j++)
                {
                    if (tile1.AllEdgesFound)
                        break;
                    
                    var tile2 = tiles[j];
                    if (tile2.AllEdgesFound)
                        continue;
                    
                    CheckTiles(tile1, tile2);
                }
            }

            long product = 1;
            var corners = tiles.Where(x => x.IsCorner);
            foreach (var corner in corners)
            {
                Console.WriteLine($"Tile with id: {corner.Id} is a corner.");
                product *= corner.Id;
            }
            Console.WriteLine($"Product of all corners is: {product}.");

            var firstCorner = tiles.First(x => x.IsCorner);
            Tile[,] tileCube = ConstructTileCube(tiles, firstCorner);


            //tileCube = FlipMatrix(tileCube, cubeDimension);
            //tileCube = RotateMatrix90(tileCube, cubeDimension);

            var image = ConstructImage(tileCube);
            Console.Write("\r\n");
            Console.Write("\r\n");
            var rows = image.GetUpperBound(0) + 1;

            for (var row = 0; row < rows; row++)
            {
                for (var col = 0; col < rows; col++)
                {
                    Console.Write(image[row, col] ? "#" : ".");
                }
                Console.Write("\r\n");
            }

            rows = image.GetUpperBound(0) + 1;
            image = FlipMatrix(image, rows);
            image = RotateMatrix90(image, rows);

            image = RotateMatrix90(image, rows);
            image = RotateMatrix90(image, rows);

            Console.Write("\r\n");
            Console.Write("\r\n");

            for (var row = 0; row < rows; row++)
            {
                for (var col = 0; col < rows; col++)
                {
                    Console.Write(image[row, col] ? "#" : ".");
                }
                Console.Write("\r\n");
            }
        }

        Tile[,] ConstructTileCube(List<Tile> tiles, Tile firstCorner)
        {
            var dimension = (int)Math.Sqrt(tiles.Count);
            var cube = new Tile[dimension, dimension];
            while (!(firstCorner.NeighborTop == null && firstCorner.NeighborLeft == null))
            {
                firstCorner.Rotate90();
            }
            cube[0, 0] = firstCorner;
            int currentRow = 0, currentCol = 1;
            while (currentRow < dimension)
            {
                while (currentCol < dimension)
                {
                    if (currentCol == 0)
                    {
                        var tile = cube[currentRow - 1, currentCol];
                        var neighbor = tile.NeighborBottom;
                        while (neighbor.NeighborTop != tile)
                        {
                            neighbor.Rotate90();
                        }
                        if (!tile.BottomRow.SequenceEqual(neighbor.TopRow))
                        {
                            neighbor.Flip();
                        }
                        if (!tile.BottomRow.SequenceEqual(neighbor.TopRow))
                        {
                            throw new Exception();
                        }

                        cube[currentRow, currentCol] = neighbor;
                    }
                    else
                    {
                        var tile = cube[currentRow, currentCol - 1];
                        var neighbor = tile.NeighborRight;
                        while (neighbor.NeighborLeft != tile)
                        {
                            neighbor.Rotate90();
                        }

                        if (!tile.RightCol.SequenceEqual(neighbor.LeftCol))
                        {
                            neighbor.Flip();
                            neighbor.Rotate90();
                            neighbor.Rotate90();
                        }
                        cube[currentRow, currentCol] = neighbor;
                    }
                    currentCol++;
                }
                currentCol = 0;
                currentRow++;
            }

            return cube;
        }

        bool[,] ConstructImage(Tile[,] tileCube)
        {
            var tilesPerCubeRow = tileCube.GetUpperBound(0) + 1;
            var rowsPerTile = tileCube[0, 0].Image.GetUpperBound(0) + 1;

            var imageDimension = tilesPerCubeRow * (rowsPerTile - 2);
            var image = new bool[imageDimension, imageDimension];

            int imageRowStart = 0, imageColStart = 0, finalImageRowOffset = 0, finalImageColOffset = 0;
            for (var tileRow = 0; tileRow < tilesPerCubeRow; tileRow++)
            {
                for (var tileCol = 0; tileCol < tilesPerCubeRow; tileCol++)
                {
                    imageRowStart = (tileRow) * (rowsPerTile - 2);
                    imageColStart = (tileCol) * (rowsPerTile - 2);

                    var tile = tileCube[tileRow, tileCol];

                    var minImageRow = 1;
                    var maxImageRow = rowsPerTile - 1;
                    var minImageCol = 1;
                    var maxImageCol = rowsPerTile - 1;

                    finalImageRowOffset = imageRowStart;
                    for (int imageRow = minImageRow; imageRow < maxImageRow; imageRow++)
                    {
                        finalImageColOffset = imageColStart;
                        for (int imageCol = minImageCol; imageCol < maxImageCol; imageCol++)
                        {
                            image[finalImageRowOffset, finalImageColOffset] = tile.Image[imageRow, imageCol];
                            finalImageColOffset++;
                        }

                        finalImageRowOffset++;
                    }
                }
            }

            return image;
        }

        void CheckTiles(Tile tile1, Tile tile2)
        {
            if (tile1.NeighborTop == null && CheckIfDimensionIsMatch(tile1.TopRow, tile1, tile2))
            {
                tile1.NeighborTop = tile2;
                return;
            }

            if (tile1.NeighborRight == null && CheckIfDimensionIsMatch(tile1.RightCol, tile1, tile2))
            {
                tile1.NeighborRight = tile2;
                return;
            }

            if (tile1.NeighborBottom == null && CheckIfDimensionIsMatch(tile1.BottomRow, tile1, tile2))
            {
                tile1.NeighborBottom = tile2;
                return;
            }

            if (tile1.NeighborLeft == null && CheckIfDimensionIsMatch(tile1.LeftCol, tile1, tile2))
            {
                tile1.NeighborLeft = tile2;
                return;
            }
        }

        bool CheckIfDimensionIsMatch(bool[] tile1Dimension, Tile tile1, Tile tile2)
        {
            if (tile2.NeighborTop == null)
            {
                if (DimensionsMatch(tile1Dimension, tile2.TopRow))
                {
                    tile2.NeighborTop = tile1;
                    return true;
                }
            }

            if (tile2.NeighborRight == null)
            {
                if (DimensionsMatch(tile1Dimension, tile2.RightCol))
                {
                    tile2.NeighborRight = tile1;
                    return true;
                }
            }

            if (tile2.NeighborBottom == null)
            {
                if (DimensionsMatch(tile1Dimension, tile2.BottomRow))
                {
                    tile2.NeighborBottom = tile1;
                    return true;
                }
            }

            if (tile2.NeighborLeft == null)
            {
                if (DimensionsMatch(tile1Dimension, tile2.LeftCol))
                {
                    tile2.NeighborLeft = tile1;
                    return true;
                }
            }

            return false;
        }

        bool DimensionsMatch(bool[] tile1, bool[] tile2)
        {
            var revereseTile1 = tile1.Reverse();
            var reverseTile2 = tile2.Reverse();

            var isMatch = tile1.SequenceEqual(tile2) ||
                tile1.SequenceEqual(reverseTile2) ||
                revereseTile1.SequenceEqual(tile2) ||
                revereseTile1.SequenceEqual(reverseTile2);

            return isMatch;
        }

        string SeaMonster = @"
                  # 
#    ##    ##    ###
 #  #  #  #  #  #   ";

        string TestInput = @"Tile 2311:
..##.#..#.
##..#.....
#...##..#.
####.#...#
##.##.###.
##...#.###
.#.#.#..##
..#....#..
###...#.#.
..###..###

Tile 1951:
#.##...##.
#.####...#
.....#..##
#...######
.##.#....#
.###.#####
###.##.##.
.###....#.
..#.#..#.#
#...##.#..

Tile 1171:
####...##.
#..##.#..#
##.#..#.#.
.###.####.
..###.####
.##....##.
.#...####.
#.##.####.
####..#...
.....##...

Tile 1427:
###.##.#..
.#..#.##..
.#.##.#..#
#.#.#.##.#
....#...##
...##..##.
...#.#####
.#.####.#.
..#..###.#
..##.#..#.

Tile 1489:
##.#.#....
..##...#..
.##..##...
..#...#...
#####...#.
#..#.#.#.#
...#.#.#..
##.#...##.
..##.##.##
###.##.#..

Tile 2473:
#....####.
#..#.##...
#.##..#...
######.#.#
.#...#.#.#
.#########
.###.#..#.
########.#
##...##.#.
..###.#.#.

Tile 2971:
..#.#....#
#...###...
#.#.###...
##.##..#..
.#####..##
.#..####.#
#..#.#..#.
..####.###
..#.#.###.
...#.#.#.#

Tile 2729:
...#.#.#.#
####.#....
..#.#.....
....#..#.#
.##..##.#.
.#.####...
####.#.#..
##.####...
##..#.##..
#.##...##.

Tile 3079:
#.#.#####.
.#..######
..#.......
######....
####.#..#.
.#...#.##.
#.#####.##
..#.###...
..#.......
..#.###...";
        string Input = @"Tile 1657:
...#.####.
.....#...#
#.....#.#.
.##.#.....
#.........
.........#
##......#.
..........
........#.
.#.#..##..

Tile 2749:
####.##..#
.#.#...#.#
#.#....#.#
...#.....#
.#...#..##
..#..##..#
......##.#
.#.#.###.#
#........#
..#..##.#.

Tile 1697:
#..#...###
#.#..#....
.##.#..###
...#..#.#.
#...##..#.
.......##.
##....#...
#...#....#
...#.....#
.#....#...

Tile 1619:
#.##.#.#.#
#.###..#..
##.......#
#.#.#...##
##.#.##...
....##....
#....#....
.#....#.##
........#.
....#.....

Tile 3037:
...####.##
#...#.....
...##.....
##.....#..
#.#.#.....
.#..##....
..##..##.#
..#...##..
#.#..#..#.
..#######.

Tile 1979:
.#...#####
#.#.##..#.
.#.....#..
##....#...
#.###.....
.###..####
.#..##.#..
.##..#..##
#...#..#.#
##.##.#.#.

Tile 3067:
.#.##....#
.#........
#...#.##..
#..#.###..
#..#.#...#
#.........
##.....###
........#.
...#.....#
#.##..#..#

Tile 2857:
.##..##.##
.......#.#
.###.#..#.
###.......
#..#.#..##
..#..#..#.
.#...#.#..
#..###.###
##...##.##
.#..#.##..

Tile 1447:
.###..#...
.#.#.#..#.
#....##.#.
..#.#....#
.##..#...#
#....#...#
#..##....#
#..#...#.#
..##......
#......#.#

Tile 3527:
#..#.....#
...#....#.
##..#..###
#.##...##.
####..#.##
.....##..#
#.........
#.#..#...#
..#####.#.
#.##....#.

Tile 2617:
..#.##.###
....#..#..
....#.#..#
#.......#.
##....#...
#.........
.#...#...#
..........
..#...##.#
###.#.#...

Tile 2699:
...#.##.##
.#..#.#..#
.....#...#
.##...#..#
##...#....
..#.....#.
###...#...
..#.###..#
.#.#..#..#
.#.##.#.##

Tile 2663:
.....###.#
.....#.##.
#.##....##
###...#..#
.#......##
#....#....
#.#.#...#.
....#..#..
...#.#....
#..##.#..#

Tile 2591:
####.###..
....##...#
..#.##....
##..##.#.#
....#.....
###.......
.#..#..#.#
....#..#..
##...#...#
###.###.##

Tile 1997:
.##....#..
..#..#.#..
....#.##..
..#...####
#..###....
#.....#...
##.....#..
##.##.####
###..#.#.#
######.##.

Tile 1039:
..#.#...#.
.##.##..#.
##..#..##.
##..#....#
#..#....#.
...#.#..##
........#.
.#.......#
##....#.##
#..##..###

Tile 1307:
..#.###...
.##.#.##.#
..#..#.###
.##.#...##
#.........
#.........
..##.....#
...#.#....
#...###...
..#.##....

Tile 2297:
#.##..##..
#..#..#..#
##.##...##
#..##.####
###......#
...#.##...
...#....##
#....#####
#...#.#...
#####..##.

Tile 2347:
..###.###.
.#..#...##
..#.#.....
#...#....#
###.....#.
.#..#....#
#...#..#.#
..#...#.##
#.#..#...#
##...##.#.

Tile 2777:
#...#..#.#
..#......#
.#.....#..
.........#
....#....#
###...##.#
..##.#..#.
#.####.#..
#...##....
...###..#.

Tile 1613:
.#..##.#..
...##.#...
#.#.#....#
#....#...#
..#..#.#.#
....##.#.#
##...#.#..
........#.
.....#.###
#..#.#.#..

Tile 1289:
.##...#...
#....#.#.#
#..#...##.
#..##.#.#.
.#..####..
.....##..#
.....#...#
.....#.##.
#....#..##
..##.#....

Tile 1087:
.#.#...#..
##....#.##
#..#..#..#
..#..##.##
##.##.#..#
##........
....##...#
##.....#.#
#....#..##
#.....#.##

Tile 3659:
#######...
...##....#
.##.##..#.
.....#...#
##..#.#..#
......#..#
#....#....
##..#..#.#
.#...#....
.#.##.####

Tile 2711:
...#....##
#..#.##..#
........#.
#...#.##..
##..#..#.#
##.##.##.#
#.#..#..##
..####....
#..#.#...#
#...######

Tile 3851:
#.#.######
.#.....##.
......#...
..#.......
...#......
..##....##
##..######
....#.#...
........#.
....#..#..

Tile 2917:
.##..#....
#..#.....#
......#..#
#.##...###
#..#.#...#
###....#.#
#.#.##..#.
#.#....#.#
##......##
#.#.###.#.

Tile 1723:
.#####.#.#
#....#...#
#.........
......#.##
....##.#.#
..##..###.
###.#.#...
..#.......
##....#.#.
.##....###

Tile 3023:
#..##.#...
###...#.##
.......##.
#....#.###
#........#
.#.##...##
#..#...#..
#.#....#.#
.#.#......
##.##.###.

Tile 3389:
#.#..#....
.#...#.###
#.#....#..
.#........
...#..##..
#.........
##.....#.#
#....#.#.#
...#..##..
.####.###.

Tile 2713:
#####.....
#.####.#..
..#.#.....
.........#
..........
#..#.....#
#........#
#....#....
..###.....
#.#..##...

Tile 3109:
..#...#.##
....#....#
#..#.#.#.#
....##..#.
#..##....#
....#.....
...##.....
#....##...
#....#.#.#
####.##...

Tile 1279:
.##.##..#.
#....#....
....###.##
#.....##..
##.#..#..#
..#..##...
#.#.#....#
##....#...
.#.##....#
..#......#

Tile 1013:
##...##.##
.###....#.
#..###.#..
...##.##.#
..###.#.#.
...##.....
.##..###..
.#.#......
.#..#.###.
.#...##..#

Tile 1889:
##..#.####
...##.....
.....#.#.#
.......##.
...##...##
.....#.##.
##.....#.#
#..#...#.#
.#..#.....
..#.#..##.

Tile 1669:
#.#.######
##..##..#.
........##
..##.#....
.........#
.....#.###
..###.....
#..#.#....
.#..#.....
###..#####

Tile 2689:
####.##...
###.......
.#.#.####.
.#####....
#..#....#.
...#....##
#..##.###.
...#..#...
.......#..
.##.##.#.#

Tile 1487:
..##..#.#.
#.###..##.
#.###.....
.#.#..##..
#.....#...
.....#....
##.....##.
...#....##
...#.#.#.#
.#.#.#....

Tile 2287:
##.#.##.#.
...#.....#
#.#.#.....
#...#.#...
##......##
#...#.###.
##.##.....
#.##..#..#
..####....
.#.###.##.

Tile 3517:
#.####....
.#..#...#.
..#.......
.#....##..
..#..##.##
....#..###
...#......
#.....#..#
........##
####.#.#.#

Tile 1559:
##.##.#..#
#.#.....#.
##.##.....
#..#..#..#
...#.###..
#...##...#
#.#.......
#.....#...
......##..
.###.##...

Tile 2053:
#.#.#...#.
##.#......
..........
##..#..#..
#.#..#..##
#....#..##
##.#....##
#..#....#.
.#......##
##..##.#..

Tile 2341:
##.####.#.
.....#.##.
#..#..####
###...####
..#....#..
.#.#...#.#
.........#
#........#
.#.#......
..###...##

Tile 3191:
##..###.#.
..#....#.#
.......#..
.###.#.#..
.##..#...#
#..#..#..#
#..#....##
..#..#....
#.##......
##.#.#....

Tile 3571:
###..#..##
#..##...#.
#..#.#..#.
#..##...##
.#...#...#
......#.##
##.......#
.....#.#..
###.......
.#.##.###.

Tile 1621:
.#####....
.##.......
.....##...
.....#....
#...##....
.#.##.#.#.
##.....#.#
#.....#...
.....#..##
##...####.

Tile 2903:
#..#.##..#
#.#...####
#....#..#.
#.....#.#.
#.........
#...#..##.
.#..#....#
#.#.....##
##...#.#.#
..##.##...

Tile 3691:
#.##.##.##
.#.#.#.#..
#...#....#
##......##
#..#.#.#.#
###....#..
.........#
.#..#.#..#
#....#...#
.##..###.#

Tile 1693:
.##....###
......###.
.#...#.#.#
.#....##.#
##...#.#..
..#....###
.#.#.##..#
##.#.#.#.#
#.#...#..#
#...##..##

Tile 2677:
###.##..#.
#.....###.
##...#....
..##..#.##
.##....#..
..#.#.###.
.#...#..#.
#....#..#.
..####...#
#.###.####

Tile 2131:
.#.#.#.###
####...#..
#....#....
##..##...#
#.#....#.#
.#.......#
.##....###
...#..#..#
.........#
####.##.##

Tile 2377:
.#.#..####
........##
..#..##...
...###...#
....##.#..
#..##..#..
....#.....
#.#......#
#.#....#.#
.###...#..

Tile 1019:
.#####.#.#
..##..#..#
####...#.#
#....#.#..
#..#..#...
..##..##..
#..#......
#..#..#...
..#....#..
..#..#.##.

Tile 1831:
.....##...
##.#.....#
###.......
.#.......#
..#...##.#
##.....##.
...#....##
......#.##
#....###.#
.#######.#

Tile 3163:
..###.....
#.#.#.#..#
........#.
.#...#...#
#..##.##.#
........#.
##....#...
......#.##
...#...###
......#...

Tile 1051:
##..###.##
..#..#.#.#
#..#...###
#....#.###
#.#..##.##
.....#.###
........#.
#..#......
#..#..#..#
..####..#.

Tile 2089:
..#####.#.
.....#..##
.###..#..#
#...###..#
#..#.#....
....#.#..#
##...#.##.
..#...#..#
#.#...#...
##.##..#..

Tile 1489:
.#.##..##.
#..#####.#
#.#.#..#..
#.......##
#......#..
.......#.#
.##..#...#
#.#......#
..#.#.###.
.#..#.###.

Tile 2969:
##.##.....
#...##.###
##.......#
..#.......
#........#
##.##.##..
.##.#.##.#
.....#...#
...#......
#...#.#.#.

Tile 2801:
.##.#.#...
####...##.
..#.#..#..
#..#.#..#.
.##..#....
.#.#.....#
#...#...#.
##.#....#.
.....#..##
#..##..##.

Tile 3719:
#.####....
#........#
#...#.....
....#####.
..##.#.#.#
##........
..#......#
#.#..#....
#.#......#
.##..####.

Tile 3323:
#####...##
....#.#...
##..#..#..
#....###..
......##.#
.#...##...
#.#.......
#...###.#.
.####.##.#
...######.

Tile 2143:
#......##.
.##...#.#.
.#....#...
#.#.#.#.#.
.##.......
#.#..#....
..##...#..
#...#....#
..#....#..
..#.####..

Tile 1721:
...#.##..#
#.#.......
......#..#
#..#.#..#.
.....#....
##.#....#.
##.....#..
#.#.###..#
##......##
....#.####

Tile 2371:
.#.#####..
#.#....#.#
#..#.#...#
..#.#....#
#####...#.
##.#...###
...###..#.
##.......#
...#....##
##..#####.

Tile 3251:
..##.#....
...##.#...
#..###....
#...##..#.
#....#....
.#...#..#.
.......#..
..#.##..#.
#..##...#.
..##.#...#

Tile 3001:
.#.......#
#..###..#.
#.##...##.
..###.##.#
..#.......
..#.....#.
##..##....
...#....##
#........#
.#..#.#.##

Tile 1951:
#.#.#.#.#.
#...#.#.#.
..###..#.#
...##.#..#
..#...#...
#.#......#
.#.#..#..#
..#..##.##
#...#.#.##
..#.##.##.

Tile 2129:
.#.#...#.#
.#....#.#.
#.####..#.
#.##.##...
.##..#.#.#
..........
#........#
##....#...
#...#.##..
...##.#..#

Tile 1063:
########.#
#....#....
...#.#...#
.........#
#.......##
.........#
#..##.....
...#......
#..##..#.#
.##....#.#

Tile 1787:
####...#..
.........#
....#.....
..##..#..#
.....###.#
...#...#.#
..#....#.#
#..##...#.
#.......#.
.#.#...#..

Tile 3709:
##.#..####
.#....#..#
.......##.
...#..##..
#.#.......
#.#..#.#..
..##...#.#
##..#...##
##.....#.#
.#..#.#...

Tile 2539:
#...##.###
...##.#.#.
......#.#.
##..#.##..
....##..#.
#..#..#...
#.##..#..#
..#.#...#.
#..##....#
##.#.#..##

Tile 3373:
.##.#.##..
..##.#.##.
#.#.#..###
.#..#....#
#.##.##...
#....##...
.##..#....
..#....#..
#..#.##..#
#....##.#.

Tile 2647:
#######.##
#...#.#..#
....###...
......#.#.
...###....
##..#..#.#
#......###
#...##....
.......#.#
####....##

Tile 3593:
.#...###..
..#....##.
..#.##.#.#
##.#..##.#
##.....#.#
..#.##...#
..#.......
.#.#.....#
..#.#....#
..###.#.#.

Tile 1303:
#...#..#..
#.....###.
...#....##
....#.....
#..#....#.
.###.##.##
##.#......
.#.####.##
...##...##
##.#.#..##

Tile 3331:
.##.#.###.
#...#.....
.......#.#
....#....#
##.#.#...#
........#.
##.#.#.#.#
#...#...##
...##.##.#
..#.###.#.

Tile 1373:
..#..##...
..#..#.#..
##..#...##
#.....#...
.........#
#..#......
#..#..##.#
......#..#
#.......##
..#...#.##

Tile 1327:
#.###..###
.#....#...
##.......#
#.#....##.
#..#..#...
.#...###..
#.#...#..#
##........
.#...#...#
.#..##.###

Tile 3217:
.##..#.#.#
.#.....#.#
#####...#.
..##.#.##.
#.#.....##
...#..#...
#..#.....#
.#...#.#.#
#...#.#...
#..###.#..

Tile 1277:
...####.##
.#..#..#.#
..##..#.#.
.#.#.#.#..
...#..#...
..#.##..#.
.....#....
.#......##
.####.....
##.#....##

Tile 1301:
.#.......#
.#..#.#...
#.....#..#
#.#..###..
#...#....#
..##.#..#.
#..##.....
#...#..#.#
.....#...#
.#.#....#.

Tile 1811:
..#..#..#.
###..#...#
##...#...#
.##.....##
#.#.....#.
.#......#.
..###.....
..#.##.##.
.#......#.
..#.....#.

Tile 3079:
.##..#.##.
..........
#..#.#....
.##.......
#.......#.
.#.....#..
###.#....#
#.#...#...
#....#..##
#.####.##.

Tile 1229:
.#.####..#
##.#...#..
#..#..#..#
#.#..###..
##....#...
#...##.#.#
#..#.....#
.#........
#.#.....#.
..#.#.#.#.

Tile 2719:
##.#..#...
#......#..
.........#
#..##...#.
#........#
###.###...
..##.#.#..
......#...
.#.#.#....
...#...#..

Tile 3547:
.#....#.#.
#.#.#....#
.....#..##
...#..#.##
#.....##..
#....##...
.##......#
#...#.....
##.......#
..#.#..###

Tile 1123:
##.##.....
#.##.....#
##...###..
...#.##..#
#........#
##.##..###
..##.#..#.
##........
#....#..#.
.......#..

Tile 3361:
#..#.#....
..#..#...#
...##.....
.#.#.#.##.
#.#..#.##.
..#.#..###
#....#....
......#...
#....#...#
.###.####.

Tile 1759:
...##..###
###..#...#
..........
#...#....#
#.###.#.#.
##.#.#.#..
#........#
#.#..#...#
#..#..#.##
..##.##..#

Tile 1783:
.#.##.....
.#....##..
#..#..#.#.
.....####.
.....#####
#...##..#.
#.....#...
#..#....##
.#........
###..#.##.

Tile 2551:
.##.#.#.##
#.#.#.....
#....#...#
..#.......
#.....#..#
..##.#..##
#..#....#.
.#.#.#...#
#..#..#...
....##.#..

Tile 1553:
.###.....#
...##..#.#
....#..#..
#.#..#..##
.#...##.##
..#..#....
.##.#....#
###.#.#..#
##...#...#
#..#..##.#

Tile 1609:
#.#..##.#.
#.#...#..#
#...####.#
.##.......
...#.....#
#..#..#...
...#...###
#..#.#..#.
......#.##
#.#####...

Tile 1847:
...###.##.
..##....##
.##..#...#
#..#..#...
#...##....
#..#.##..#
#..#..#.##
##.......#
##.......#
##.#....##

Tile 3433:
####..###.
..#..#####
##..##...#
#.##.....#
####......
.##....#.#
#..#.#..##
...#.#..#.
###.......
.##..##..#

Tile 1471:
#.##.#....
.#.#.....#
#.##.....#
.......#..
........##
#.#......#
#.....#...
..........
#.##..##..
...##.#.#.

Tile 2239:
##.#.#..#.
##..#...#.
..#.......
..#.....#.
#........#
#.........
#......#..
##..#...##
.#.#..####
#.#..#.#.#

Tile 3137:
.####...#.
.#..#..#.#
.#...##..#
.##..#...#
#.........
.#.##.#...
..#..##.##
.....#.###
.##.#.....
..#.#...#.

Tile 3911:
...###.##.
##.......#
#..#......
.......#.#
##........
####.....#
#.##.##.#.
#....###.#
#.##.##...
..##.....#

Tile 2069:
.#....####
##.#.#....
#........#
#.....#.#.
#..#.....#
...###....
....##...#
...##....#
#.#.....##
###..#.#.#

Tile 2389:
#.##.#.#.#
.###.#..#.
##..#.#...
..#.......
#......#.#
##.#.....#
##......##
.###....##
##.##.##.#
#..#####.#

Tile 1129:
.###.##.##
#.........
#..##....#
.....#...#
......##.#
##...#....
.##..#....
.#.......#
#.#.####..
##....#..#

Tile 3797:
#...##.#.#
#.###...##
#..#..####
#...#..#.#
#.....#.##
...#####.#
...###.#..
..##.#..#.
#.###....#
#..#....##

Tile 1871:
.#....###.
...##...#.
#.....#..#
##.###...#
##.#..#...
#.#.###.##
.#.......#
#.#....#..
##..#.....
...#......

Tile 1733:
##..#####.
#.#.#..#.#
##..##...#
....#.#...
..#..##...
##.##.#..#
#.##.....#
...#...#..
#..#..#...
.#..#.#.#.

Tile 2851:
.##.####.#
#......#.#
........##
...#.##...
.###...###
.....#....
#...#.#...
##.#......
##.#..#.##
####.#..#.

Tile 1823:
.##..#..#.
#.#...#...
###...#.#.
#.##.#...#
.#..#....#
##.#...###
....#....#
....#.....
#..#......
......#.##

Tile 3299:
#####.#.#.
#....##.#.
.......##.
.........#
.##.#..#..
#...##....
#......#.#
.#.....#..
....#...##
#.#.##.###

Tile 1907:
.....#...#
#....#...#
....##...#
.....##.#.
#.....##..
..##..#..#
#....#...#
#..#......
#...#.#..#
###.##....

Tile 2039:
...###.###
#...#....#
#.#......#
#.........
#####.#.##
.##..##...
#.........
#...#...#.
##.##.#...
###.#..#..

Tile 1597:
###.....#.
.#....#..#
.......#..
###.#.#...
##...#....
#...#..#..
#....##...
##..#.##.#
##..#.....
##.#.#####

Tile 2753:
#####..#..
......#...
##..#...##
.##.#.#.##
##...#..##
#...#.....
.#....#..#
..........
#.#......#
..#..#...#

Tile 3643:
........#.
#....#....
#......##.
#...###...
..#..#..#.
.####...##
...#...#..
...#..#..#
......##..
##.#.#####

Tile 3019:
##.##.#...
.#..#.#...
#..#.....#
......#..#
#.......#.
##.#.#...#
..#..#....
...#.#...#
###.......
.....#.###

Tile 1439:
.#..#...##
#........#
..........
#.........
##..##.###
##......#.
#.##.....#
.##..#..##
.##.#.##.#
...###.###

Tile 1511:
..####.#..
#.......##
..#..#.#..
..####....
...##....#
#.#.###..#
.#..#....#
##.....#.#
.....#...#
#..#####.#

Tile 2467:
.#.#######
###..##...
...#...#.#
.........#
...##.####
.........#
........##
##.#..####
.#...#####
.#.##.###.

Tile 2767:
.#...#...#
##...###..
.#....####
..#.#.#.#.
#.....##..
#......#.#
..#......#
##.#.....#
.#.##..#..
.#..####.#

Tile 2113:
..#..#####
##.#.#####
##...#...#
##.#....##
..#.##..#.
##.######.
#.........
.#.##.#..#
.#.#.##..#
#.##...###

Tile 1193:
##..###.#.
.#........
#.........
###......#
...#...#..
#.#.......
#.........
#..#.....#
#....##...
#.##..###.

Tile 2441:
#.#..#....
..........
.....#..##
..#......#
#...#....#
#........#
.#......##
.#...##.#.
...#.##...
.####....#

Tile 1861:
.##..##.##
#.......##
#.....#...
#.....#...
##.......#
.........#
.#.....#.#
#.#....#.#
#....#.#.#
...##....#

Tile 2791:
.##.###.#.
##.......#
#........#
#..#.##..#
.##...##.#
......##..
.##..##..#
#......#..
.......#..
...#.###..

Tile 3329:
...#..#.#.
#.#.#.....
.#.#..#..#
#.........
#.#...#..#
..#..#....
#.....#...
..#....##.
...#.#....
...##.#..#

Tile 3727:
####.#....
#.#......#
..#.......
##...#.##.
..#..##.#.
..#...#..#
...#.###.#
###.....##
..#.....##
###.###..#

Tile 1171:
...###.#..
#.#....#.#
....#....#
#....#....
#..#.###..
.#....###.
.#.##.##..
..###...##
....#...#.
.#######..

Tile 2953:
#..###.###
.#.#...#.#
.##.###...
..#.#.....
#........#
..#....#.#
.........#
#.###.##..
........#.
###.##.#.#

Tile 3701:
..####....
##...#..#.
##...#.#..
#......##.
#......#..
....#..##.
.####...#.
....#..##.
#..##..#.#
...#.##.#.

Tile 2671:
##..#.#...
###......#
.....#.###
#.#......#
...##.#...
...#.#...#
#.#.#..##.
#......#.#
#.##.#.##.
..#..#.##.

Tile 1913:
###..###.#
#.#..###..
...##...#.
.#.#.....#
#.#...##.#
##......#.
#...#..#..
..#.#....#
...#...#.#
##..##.#.#

Tile 2417:
.......###
...#.#...#
#.........
..#..#...#
.#....##..
##....#..#
#.....##..
...#.#.##.
#.##.#..#.
##.##.#.#.

Tile 1523:
....#...#.
#.........
..##.##..#
##.##..#.#
......####
..#......#
##.......#
##.#.#.#.#
..#.#.#...
#.##...###

Tile 2503:
######....
.###...##.
.##.#..##.
...#....#.
.....#.#.#
#..#.#.#.#
#..##..#..
##........
.#...###.#
#.##....#.

Tile 3041:
.....###.#
##..##....
....##..##
#.....#.##
##..###.#.
##.###...#
#.####...#
.#.#.##..#
......#...
####....##

Tile 3863:
#..##.#.#.
....#....#
....#.....
#....#...#
#...#.....
..##...#..
#.#..#...#
##..#...#.
##..#.#...
#.#.#.....

Tile 2269:
#...###...
.#.#.....#
##..#...#.
....#..##.
###...#...
##.......#
.#....#..#
..#.##.#..
#........#
#..##...##

Tile 3739:
####..###.
#.#.##....
...##...##
.##.#....#
#.#..#...#
..##.###..
..#..#.#.#
....###...
..###..#.#
##.###.###

Tile 2243:
..##..#..#
#......###
###...##.#
#.....#..#
#......#..
#....##...
##.....##.
.......#.#
...##.##..
.##.##.#..

Tile 3011:
.###......
.#.##.##.#
...#..#..#
##...###..
..........
###.....##
.#.....##.
.#.#..##..
..#..#....
.##..#.#..

Tile 3209:
##.#.#.##.
........#.
.....#.#..
###..##..#
##.......#
.##..##...
.##.##.#.#
..##...#..
#.#.#..###
...###....

Tile 1049:
##.#####.#
#.#.##....
.##.#.....
#.........
#....#..##
##..#.#.#.
.....#...#
#.#.......
.#.##....#
#.##.##..#

Tile 3449:
..####....
...#.#.##.
.......#..
#..##..###
...#.#..#.
#....#.#..
##.#..#.#.
###.....##
#...#...##
#.##.##..#";
    }
}
