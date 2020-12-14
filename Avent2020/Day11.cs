using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avent2020
{
    public class Day11
    {
        enum SeatState
        {
            Floor,
            Empty,
            Occupied
        }

        public void Run()
        {
            Part2();
        }

        public void Part1()
        {
            var input = Input;
            var lines = input.Split("\r\n");
            var cols = lines[0].Length;
            var rows = lines.Length;
            var seats = new SeatState[rows, cols];
            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    if (lines[i][j] == 'L')
                    {
                        seats[i, j] = SeatState.Empty;
                    }
                    else
                    {
                        seats[i, j] = SeatState.Floor;
                    }
                }
            }

            var isLastRoundDifferent = true;
            while (isLastRoundDifferent)
            {
                var nextRoundSeats = new SeatState[rows, cols];
                isLastRoundDifferent = false;
                var numOccupied = 0;

                for (int row = 0; row < rows; row++)
                {
                    //Console.Write("\r\n");
                    for (int col = 0; col < cols; col++)
                    {
                        var numAdjOccumpied = NumOfAdjacentOccupied(seats, rows, cols, row, col);
                        if (seats[row, col] == SeatState.Empty && numAdjOccumpied == 0)
                        {
                            nextRoundSeats[row, col] = SeatState.Occupied;
                        }
                        else if (seats[row, col] == SeatState.Occupied && numAdjOccumpied >= 4)
                        {
                            nextRoundSeats[row, col] = SeatState.Empty;
                        }
                        else
                        {
                            nextRoundSeats[row, col] = seats[row, col];
                        }

                        isLastRoundDifferent = isLastRoundDifferent | nextRoundSeats[row, col] != seats[row, col];
                        if (nextRoundSeats[row, col] == SeatState.Occupied)
                        {
                            numOccupied++;
                        }

                        //Console.Write(nextRoundSeats[row, col] == SeatState.Occupied ? "#" : nextRoundSeats[row, col] == SeatState.Empty ? "L" : ".");
                    }
                }

                Console.WriteLine($"\r\nRound is different than last: {isLastRoundDifferent}. Num occupied is: {numOccupied}.");

                seats = nextRoundSeats;
            }
            
        }

        int NumOfAdjacentOccupied(SeatState[,] seats, int totalRow, int totalCol, int row, int col)
        {
            var adjacentOccupied = 0;
            if (row != 0)
            {
                if (col != 0 && seats[row - 1, col -1] == SeatState.Occupied)
                {
                    adjacentOccupied++;
                }

                if (seats[row - 1, col] == SeatState.Occupied)
                {
                    adjacentOccupied++;
                }

                if (col != totalCol -1 && seats[row - 1, col + 1] == SeatState.Occupied)
                {
                    adjacentOccupied++;
                }
            }

            if (col != 0 && seats[row, col - 1] == SeatState.Occupied)
            {
                adjacentOccupied++;
            }

            if (col != totalCol - 1 && seats[row, col + 1] == SeatState.Occupied)
            {
                adjacentOccupied++;
            }


            if (row != totalRow - 1)
            {
                if (col != 0 && seats[row + 1, col - 1] == SeatState.Occupied)
                {
                    adjacentOccupied++;
                }

                if (seats[row + 1, col] == SeatState.Occupied)
                {
                    adjacentOccupied++;
                }

                if (col != totalCol - 1 && seats[row + 1, col + 1] == SeatState.Occupied)
                {
                    adjacentOccupied++;
                }
            }

            return adjacentOccupied;
        }

        public void Part2()
        {
            var input = Input;
            var lines = input.Split("\r\n");
            var cols = lines[0].Length;
            var rows = lines.Length;
            var seats = new SeatState[rows, cols];
            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    if (lines[i][j] == 'L')
                    {
                        seats[i, j] = SeatState.Empty;
                    }
                    else
                    {
                        seats[i, j] = SeatState.Floor;
                    }
                }
            }

            var isLastRoundDifferent = true;
            while (isLastRoundDifferent)
            {
                var nextRoundSeats = new SeatState[rows, cols];
                isLastRoundDifferent = false;
                var numOccupied = 0;

                for (int row = 0; row < rows; row++)
                {
                    //Console.Write("\r\n");
                    for (int col = 0; col < cols; col++)
                    {
                        if (seats[row, col] == SeatState.Floor)
                        {
                            nextRoundSeats[row, col] = SeatState.Floor;
                        }
                        else
                        {
                            var numAdjOccumpied = NumOfAdjacentOccupied2(seats, rows, cols, row, col);
                            if (seats[row, col] == SeatState.Empty && numAdjOccumpied == 0)
                            {
                                nextRoundSeats[row, col] = SeatState.Occupied;
                            }
                            else if (seats[row, col] == SeatState.Occupied && numAdjOccumpied >= 5)
                            {
                                nextRoundSeats[row, col] = SeatState.Empty;
                            }
                            else
                            {
                                nextRoundSeats[row, col] = seats[row, col];
                            }

                            isLastRoundDifferent = isLastRoundDifferent | nextRoundSeats[row, col] != seats[row, col];
                            if (nextRoundSeats[row, col] == SeatState.Occupied)
                            {
                                numOccupied++;
                            }
                        }                        

                        //Console.Write(nextRoundSeats[row, col] == SeatState.Occupied ? "#" : nextRoundSeats[row, col] == SeatState.Empty ? "L" : ".");
                    }
                }

                Console.WriteLine($"\r\nRound is different than last: {isLastRoundDifferent}. Num occupied is: {numOccupied}.");

                seats = nextRoundSeats;
            }

        }

        int NumOfAdjacentOccupied2(SeatState[,] seats, int totalRow, int totalCol, int row, int col)
        {
            var rowRightOccupied = false;
            var rowLeftOccupied = false;
            var colUpOccupied = false;
            var colDownOccupied = false;
            var diagUpRightOccupied = false;
            var diagUpLeftOccupied = false;
            var diagDownLeftOccupied = false;
            var diagDownRightOccupied = false;
            var adjacentOccupied = 0;
            for (int i = 1; i < Math.Max(totalRow, totalCol); i++)
            {
                // left
                if (!rowLeftOccupied && (col - i) >= 0  && seats[row, col - i] != SeatState.Floor)
                {
                    rowLeftOccupied = true;
                    if (seats[row, col - i] == SeatState.Occupied)
                    {
                        adjacentOccupied++;
                    }
                }

                // right
                if (!rowRightOccupied && (col + i) < totalCol && seats[row, col + i] != SeatState.Floor)
                {
                    rowRightOccupied = true;
                    if (seats[row, col + i] == SeatState.Occupied)
                    {
                        adjacentOccupied++;
                    }
                }

                // up
                if (!colUpOccupied && (row - i) >= 0 && seats[row - i, col] != SeatState.Floor)
                {
                    colUpOccupied = true;
                    if (seats[row - i, col] == SeatState.Occupied)
                    {
                        adjacentOccupied++;
                    }
                }

                // down
                if (!colDownOccupied && (row + i) < totalRow && seats[row + i, col] != SeatState.Floor)
                {
                    colDownOccupied = true;
                    if (seats[row + i, col] == SeatState.Occupied)
                    {
                        adjacentOccupied++;
                    }
                }

                // diag left up
                if (!diagUpLeftOccupied && (col - i) >= 0 && (row - i) >= 0  && seats[row - i, col - i] != SeatState.Floor)
                {
                    diagUpLeftOccupied = true;
                    if (seats[row - i, col - i] == SeatState.Occupied)
                    {
                        adjacentOccupied++;
                    }
                }

                // diag left down
                if (!diagDownLeftOccupied && (col - i) >= 0 && (row + i) < totalRow && seats[row + i, col - i] != SeatState.Floor)
                {
                    diagDownLeftOccupied = true;
                    if (seats[row + i, col - i] == SeatState.Occupied)
                    {
                        adjacentOccupied++;
                    }
                }

                // diag right up
                if (!diagUpRightOccupied && (col + i) < totalCol && (row - i) >= 0 && seats[row - i, col + i] != SeatState.Floor)
                {
                    diagUpRightOccupied = true;
                    if (seats[row - i, col + i] == SeatState.Occupied)
                    {
                        adjacentOccupied++;
                    }
                }

                // diag right down
                if (!diagDownRightOccupied && (col + i) < totalCol && (row + i) < totalRow && seats[row + i, col + i] != SeatState.Floor)
                {
                    diagDownRightOccupied = true;
                    if (seats[row + i, col + i] == SeatState.Occupied)
                    {
                        adjacentOccupied++;
                    }
                }
            }

            return adjacentOccupied;
        }

        string TestInput1 = @"L.LL.LL.LL
LLLLLLL.LL
L.L.L..L..
LLLL.LL.LL
L.LL.LL.LL
L.LLLLL.LL
..L.L.....
LLLLLLLLLL
L.LLLLLL.L
L.LLLLL.LL";

        string Input = @"LLLLL.LLLLLL.LLLLLLLLL.LLL.LL.LLLLLLL.LLLLLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLLLLLL.LLLLLLLLLL.LLLLLLL
LLLLL.LLLLLL.LLLLLLLLLLLLLLL.LLLLLLLL.LLLLL.LLL.LLLLLL.LLLL.LLLLL.LLLLLLLLLLL.LLLLLLLL.LLLLLLLLL
LLLLL.LLLLLL.LLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLLLL.LLLLLL.LLLL.LLLLL.LLLLLLLLLLLLLLLLLLLLL.LLLLLLLL
LLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL.LLLLLL.LLLLLLLLL.LLLL.L.LLLLLLLLLLLLLLLLLLLLLLLLL.LLL.LLLL
LLLLL.L.LLLL.LLLLLLLLL.LLLLLL.LLLLLLLLLLLLLLLLL.LLLL.L.LLLLLLLLLL.LLLLLLLLLLLLLLLLLLLLL.LLLLLLLL
LLLLLLLLLLLL.LLLLLLLLL.LLLLLL.LLLLLLL.LLLLLLLLL.LLLLLLLLLLLLLLLLL.LLLLLLLLLLL.LLLLLLLLL.LLLLLLLL
L.LL.L.L.LL.....L.L..L.LL.L.L.L.....L..LLL.L.....L.L...LL..L.....L...L..L.LLLL..L.LL......L.L.L.
LLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLLLL.LLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLLLLLLLLL.LLLLLLLL
LLLLL.LLLLLLLLLLLLLLLL.LLLLLL.LLLLLLL.LLLLLLLLL.LLLLLLLLLLL.LLLLL.LLLLLL.LLLLLLLLLLLLLLLLLLLLLLL
LLLLL.LLLLLL.LLLLLLLLLLLLLLLL.LLLLLLL.LLLLLLLLL.LLLLLL.LLLLLLLLLL.LLLLLL.LLLL.LLLLLLLLL.LLLLLLLL
LLLLL.LLLLLL.LLLLLLLLL.LLLLLLLLLLLLLLLLLLLLLLLL.L.LLLLLLLLL.LLLLL.LLLLLLLLLLL.LLLLLLLLL.LLLLLLL.
LLLLL.LLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLLLLLLLLLLL.LLLL.LLLLLLLLLLLL.LLLLLLLLLLLLLLLLLLLLLLL
LLLLLLLLLLLLLLLLLLLLLL.LLLLLLLLLLLLLL.LLLLLLLLLLLLLLLLLLLLL.LLLLLLLLLLLL.LLLL.LLLLLLLLL.LLLLLLLL
......LL..LLLL.L...LL.....L.L.......L..L.LL....L.LLL........L.L.L.LL.....L.........L....L......L
LLLLL.LLLLLL.LLLLLLLLL.LLLLLL.LLLLLLL.LLLLLLLLLLLLLLLLLL.LL.LLLLL.LLLLLLLLLLL.LLLLLLLLL.LLLLLLLL
LLLLL.LLLLLL..LLLLLLL..LLLLLL.LLLLLLLLLLLLLLLLL.LLLLLLLLLLLLLLLLLLLL.LLL.LLLL.LLLLLLLLLLLLLLLLLL
LLLLL.LLLLLL.LLLLLLLLL.L.LLLL.LLLLLLLLLLLLLLLLL.LLLLLL.LLLL.L.LLL.LLLLLLLLLLLLLLLLLLLLL.LLLLLLLL
LLLLL.LLLLLLLLLLLLLLLLLLLLLLL.LLLLLL..LLLLL.LLL.LLLLLL.LLLL.LLLLL.LLLLLL.LLLLLLLLLLLLLL.LLLLLLLL
LLLLL.LLLLLL.LLLLLLLLL.LLLL.L.LLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLLLLL.LLLLLLLLLLL.LLLLLLLLL.LLLLLLLL
LLLLLLLLLLLL.LLLLLLLLL.LLLLLL.LLLLLLL.LLLLLLLLL.LLLLLL.LLLL.LLLLLLLLLLLL.LLLLLLLLLLLLLLLLLLLLLLL
.L...L...L.....L...L....L.LL.LLL.LL...LL.L..L...L......L..LL..L..LL..L.L......L.L.LLL......L.L..
LLLLLLLLLLLL.LLLLLLLLL.LLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLLLLLLLLLLLLLLLL.LLLLLLLL
LLLLL.LLLLL.LLLLLLLLLLLLLLLLL.L.LLLLL.LLLLLLLLLLLLLLLL.LLLL.LLLLL.LLLLLL.LLLLLLLLLLLLLLLLLLLLLLL
LLLLLLLLLLLL.LLLLLLLLL.LLLLLLLLLLLLLL.LLLLLLLLL.LLLLLL.LLLL.LLL.L.LLLLLL.LLLLLLLLLLLLLL.LLLLLLLL
LLLLL.LLLLLL.LLLLLLLLL.LLLLLL.LLLLLLL.LLLLLLLLL.LLLLLLL.LLL.LLLLL.LLLLLL.LLLL.LLLLLLLLL.LLLLLLLL
LLLLL.LLLLLL.LLLLLLLLL.LLLLLL.LLLLLLL.LLLLLLLLL.LLLLLLLLLLL.LL.LL.LLLLLL.LLLL.LLLLLLLLL.LLLLLLLL
...........LLL.....LLL.....LL.L..........L..L.L.L...L.L.....L........L......L..L..L...LL....L.L.
LLLLL.LLLLLL.LLLLLLLLL.LLLLLL.LLLLLLL.LLLLLLLLL.LLLLLL.LLLL.LLLLL.LLLLL.LLLLL.LLLLLLLLL.LLLLLLLL
LLLLL.LLLLLL.LLLLLLLLL.LLLLL.LLLL.L.LLLL.LLLLLL.LLLLLL.LLLL.LLLLL.LLLLLL.LLLL.LLLLLLLLL.LLLLLLLL
LLLLL.LLLLLLLLLLLL.LLL.LLLLLL.LLLLLLL.LLLLLLLLL.LLLLLL.LLLL.LLLLLLLLLLLL.LLLL.LLLLLLL.L.LLLLLLLL
LLLLLLLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLLLLLLLLLLLL.LLLLLLLLLLLLLLLLL.LLLLLL.LLLL.LLLLLLLLLLLLLLLLLL
LLLLL.LLLLLL.L.LLLLLLL.LLLLLL.LLLLLLL.LLLLLLLLLLLLLLLL.LLLL.LLLLLLLLLLLL.LLLL.LLLLLLLLL.LLLLLLLL
.LLLL.LLLLLLLLLLLLLLLL.LLLLLL.LLLLLLLLLLLLLLLLL.LLLLLL.LLLL.LLLLL.LLLLLL.LLLL.LLLL.LLLLLLLLLLLLL
L.......LL...LL...L..L.....L..LL..L.L.LLL.L.L..L.LL.L.........L....LLLL..LL..L.L..L..LLLL..LL.L.
LLLLL.LLLLLL.LLL.LLLLLLLLLLLL.LLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLLLLL.LLLLLL..LLL.LLLLLLLLL.LLLLLLLL
LLLLL.LLLLLL.LLLLLLLLLLLLLLLL.LLLLLLL.LLLLLLLLL.LLLLLL.LLLLLLLLLLLLLLLLL.LLLL.LLLLLLLLLLLLLLLLLL
LLLLL.LLLLLL.LLLLLLLLL.LLLLLL.LLLLLLL.LLLLLLLLL.LLLLLLLLLLL.LLLLL.LLLLLL.LLLLLLLLLLLLLL.LLLLLLLL
LLLLL.LLLLLL.LLLLLLLLLL.LLLLLLLLLLLLL.LLLLLLLLL.LLLLLL.LLLL.LLLLL.LLLLLL.LLLL.LLLLLLLLL.LLLLLLLL
LLLLL.LLLLLLLLLLLLLLLL.LLLLLL.LLLLLLL.LLLLLLLLL.LLLLLL.LLLLLLLLLLLLLLLLLLLLLL.LLLLLL.LL.LLLLLLLL
LLLLLLLLLLLL.LLLLLLLLL.LLLLLL.LLLLLLL.LLLLLLLLL.LLLLLL.LLLLLLLLLL.LLLLLLLLLLL.LLLLLLLLL.LLLLLLLL
....LLL.LL....L.L.........L.L............L..L...L.LLLL......L...L.L..L....L.......L..L..........
LLLLLLLLLLLL.LLLLLLLLLLLLLLLL.LLLLLLL..LLLLLLLL.LLLLLL.LL.L.LLLLL.LLLLLL.LLLLLL.LLLLLLL.LLLLLLLL
LLLLLLLLLLLL.LL.L.LLLL.LLLLLL.LLLLLLL.LLLLLLLLL.LLLLLL.LLLLLLLLLL.LLLLLL.LLLL.LLLLLLLLL.LLLLLLLL
LLLLL.LLLLLL.LLLLLLLLLLLLLLLL.L..LLL..LLLLLLLLL.LLLLLL.LLLL.L.LLL.LLLLLL.LLLLLLLLLLLLLLLLLLLLLLL
LLLLL.LLLLLL.LLLLLLLLL.LLLLLL.LLLLLLLLLLLLLLLLL.LLLLLL.LLLL.LLLLL..LLLLL.LLLLLLLLL.LLLL.LLLLLLLL
LLLLL.LLLLLL.LLLLLLLLLLLLLLLL.LLLLLLL.LLLLLLLLL.LLLLLL.LLLLLLLLLLLLLLLLL.LLLLLLLLLLLLLL.LLLLLLLL
.L..LLLL..L.L.LLL.......LL.LL.LL......L.L....L.LLL.......L...L.L.LL...LL.....L.L....L.L...L.L...
LLLLL.LLLLLL.LLLLLLLLLLLLLLLL.LLLLLLL.LLLLLLLLLLLLLLLLLLLLL.LLLLL.LLLLLL.LLLL.LLLLLLLLL.LLLLLLLL
LLLLLLLLLLLL.LLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLLLLLL.LLLLL.LLLLLL.LLLLLLLLLLLLLLLLLL.LLLL
LLLLL.LL.LLL.LLLLLLLLL.LLLLLLLLLLLLLL.LLLLLLLLL.LLLLLLLLLLL.LLLL..LLLLLL.LLLLLLLLLLLLLLLLLLLLLLL
LLLLL.LLLLLL..LLLLLLLL.LLLLLL.LLLLLLL.LLLLLLLLLLLLLLLLLLLLL.LLLLL.LLLLLL.LLLL.LLLLLLLLL.LLLLL.LL
LLL.LLLLLLLL.LLLLLLLLL.LLLLLL.LLLLLLL.LLLLLLLLL.LLLLLLLLLLLLLLLLL.LLLLLL.LLLLLLL.LLLLLLLLLLLLLLL
LLLL.LLLLLLL.LLLLLLLLLLLLLLLL.LLLLLLL.LLLLLLLLLLLLLLLL.LLLL.LLLLL.LLLLLLLLLL.LLLLLLLLLLLLLLLLLLL
.L..L..LLL....LL......LL.L.L..LL..LL....L...L..L.L.L...L..L..L.LLLL.L....L.....L..L...L...LLL...
LLLLL.LLLLLL.LLLLLLLLL.LLLLLL.LLLLLLL.LLLLLLLLL.LLLLLL.LLLL.LLLLL.LLLLLLLLLLL.LLLLLLLLL.LLLLLLLL
LLLLLLLLLLLLL.LLLLLLLL.LLLLLL.LLLLLLLLLLLLLLL.L.LLLLLL.LLLL.LLL.LLLLLLLL.LLLL.LLLLLLLLL.LLLLLLLL
LLLLLLLLLLLL.LLLLLLL.LLLLLLLL.LLLLLLLLLLLLLLLLL.LLLLLL.LLLL.LLLLLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLLL
LLLLL.LLLLLL.LLL.LLLLL.LLLLLLLLLLLLLLLLLLLLLLLL.LL.LLL.LLLLLLLLLL.LLLLLL.LLLL.LLLLLLLLLLLLLLLLLL
LLLLL.LLLLLLLLLLLLLLLL.LLLLLL.LLLLLLL.LLLLLLLLLLLLLLLL.LLLL.LLLLLLLLLLLL.LLLLLLLL.LLLLL.LLLLLLLL
LLLLL.LLLLLLLLLLLLLLL..LLLLLLLLLLLLLL.LLLLLL.LL.LLLLLLLLLLL.L.LLLLLLLLLL.LLLLLLLLLLLLLL.LLLLLLLL
..LL.LL.L.LL.L...L..L..L..L...LL........L......LL.LLLL.......L..LLL.L.L....L.....L..LL..L.LL.LL.
LLLLL.LLLL.L.LLLLL.LLL.LLLLLL.LLLLLLL.LL.LLLLLL.LLLLLL.LLLL.LLLLL.LLLLLL.LLLLLLLLLLLLLL.LLLLLLLL
LLLLL.LLLLLL.LLLLLLLLL..LLLLL.LLLLLLL.LLLL.LLLLLLLLLLL.LLLL..LLLL.LLLLLLLLLLL.LLLLLLLLLLLLL.LLLL
LLLLL.LLLLLLLLLLLLLLL.L.LLLLL.LLLLLLLLLLLLLLLLL.LLLLLL.LLLL.LLLLLLLLL.LL.LLLL.LLLLLLLLL.LLLLLLLL
LLLLL.LLLLLL.LLLLLLLLL.LLLLL..LLLLLLLLLLLLLLL.L.LLLLLLLLLLL.LLLLLLLLLLLL.LLLL.LLLLLLLLLLLLLLLLLL
LLLLL.LLLLLLLLLLLLLLLLLLLLLLL.LLLLLLL.LLLLLLLLLLLLLLLL.LLLL.LL.LL.LLL.LL.LLLLLLLLLLLLLL.LLLLLLLL
LLLLL.LLLLLL.LLLLLLLLL.LLLLLLLLLLLLLL.LLLLLLLLL.LLLLLLLLLLLLLLLLL.LLLLLL.LLLL.LLLLLLLLL.LLLLLLLL
LLLLL..LLLLL.LLLLLLLLL.LLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLLLLLL..LLLLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLLL
.LL..LL.L.LL......L.L.LL...L..LL..LL...L..L.L..LL...L.L..L.....L.LLL..L..LL.L..L.......L..LLL...
LLLLL.LLLLLL.LLLLLLLLL.LLLLLLLLLLLLLL.LLLLLLLLL.LLLLLLLLLLLLLLLLL.LLLLLLLLLLLLLLLLLLLLL.LLLLLLLL
LLLLLLLLLLLL.LLLLLLLLLLLLLLLL.LLLLLLL.LLLLLLLLLLLLLLLLLLLLL.LLLLL.LLLLLL.LLLLL.LLLLLLLLLLLLLLLLL
LLLLL.LLLLLLLLLLLLLLLL.LLLLLLLL.LLLLL.LLLLLLLLL.LLLLLL.LLLL.LLLLL.LLLLLL.LLLLLLLLLLLLLLLLLLLLLLL
LLLLL..LLLLLLLLLLLLLLLLLLLLLL.LLLLLLL.LLLLLLLLLLLLLLLLLLLLL.LLLLLLLLLLL..LLLL.LLLLLLLLL.LLLLLLLL
LLLLL.LLLLLLLLLLLLLLLL.LLLLLL.LLLLLLLLL.LLLLLLLLLLLLLL.LLLL.LLLLL.LLLLLL.LLLLLLLLLLLLLLLLLLLLLLL
LLLLL.LLLLLL.LLLLLLLLL.LLLLLL.LLLLLLL.LLLLLL.LLLLLLLLL.LLLL.LLL.L.LLLLLL.LLLLLLLLLLLLLL.LLLLLLLL
LLLLLLLLLLLLLLLLLLLLLL.LLLLLL.LLLLLLL.LLLLLLLLL.LLLLLL.LLLL.LLLLLLLLLLLL.LLLLLLLLLLLLLLLLLLLLLLL
LLLLL.LLL.LL.LLLLLLLLL.LLLLLLLLLLLLLL.LLLLLLLLL.LL.LLL.LLLL.LLLLL.LLLLLL.LL.L.LLLLLLLLL.LLLLLLLL
LLLLL.LLLLLL.LLLLLLLLL.L.LLLLLLLLLLLLLLLLLLLLLL.LLLLLLLLLLLLLLLLL.LLLLLL.LLLL.LLLLLLLLL.LLLLLLLL
.L.L....LLL.L.L........L..L....L..L...LL..L..LLL..L.L....L..L.LL....LL.L.L.....L........L..L.L..
LLLLLLLLLLLL.LLLLLLLLL.LLLLLLLLLLLLLL.LLLLLLLLL.LLLLLL.LLLLLLLLLL.LLLLLL.LLLL.LLLLLLLLL.LLLLLLLL
LLLLLLLLLL.LLLLLLLLLLL.LLLLLLLLLLLLLL.LL.LLLLLLLLLLLLL.LLLL.LLLLL.LLLLLL.LLLLLLLLLLLLLL.LLLLLLLL
LLLLL.LLLLLL.LLLLLLLLL.LLLLLL.LLLLLLL.LLLLLLLLL.LLLLLL.LLLL.LLLLL.LLLLLLLLLLL.LLLLLLLLLLLLLL.LLL
LLLLL.LLLLLL.LLLLLLLLLLLLLL.L.LLLLLLL.LLLLLLLL.LLLLLLL.LLLL.LLLLL.LLLLLL.LLLL.LLLLL.LL..LLLLLLLL
LLLLL.LLLLLL.LLLLLLLLL.L.LLLL.LLLLLLL.LLLLLLLLL.LLLLLLLLLLL.LLLLL.LLLLLLLLLLL.LLLLLLLLL.LLLLLLLL
LLLLLLLLLLLL.LLLLLLLLLLLLL.LL.LLLLLLLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLLLLLLLLLLLL.LLLLLLLLL.LLLLLLLL
LLLLL.LLLLLLLLLLLLLLLL.LLLLLL.LLLLLLL.LLLLLLLLLLLLLLLLLLLLLLLLLLL.LLLLLL.LLLL.LLLLLLLLL.LLLLLLLL
LLLLL.LLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL.LLLLL.LLLLLLLLLLLLLLL.LLLLL.LLLLLL.LLLL.LLLLLLLLLLLLLLLLLL
LLLL..L.LLLL.LLLLLLLLL.LLLLLL.LLLLLLL.LLLLLLLLLLLLLLLL.LLLL.LLLLL.LLLLLLLLLLL.LLLLLLLLL.LLLLLLLL
LLLLL..LLLLL.LLLLLLLLL.LLLLLL.LLLLLLL.LLLLLLLLL.LLLLLL.LLLL.LLLLLLLLL.LLLLLL..LLLLLLLLL.LLLLLLLL
LLLLL.LLLLLL.LLLLLLLLL.LLLLLL.LLLL.LLLLLLLLLLLLLLLLLLL.L.LL..LLLLLLLLLLL.LLLL.LLLLLLLLL.LLLLLLLL";
    }
}
