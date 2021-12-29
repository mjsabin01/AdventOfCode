using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avent2021
{
    internal class Day21
    {
        public void Run()
        {
            var lines = Input.Split("\r\n");
            Part2(lines);
        }

        public void Part1(string[] lines)
        {
            var startPos = lines[0].Split(',');
            var player1 = new Player(startPos[0], 10);
            var player2 = new Player(startPos[1], 10);
            var dice = new Dice(100);
            long moveCount = 1;
            while (player1.Score < 1000 && player2.Score < 1000)
            {
                var moveSum = dice.Roll() + dice.Roll() + dice.Roll();
                if (moveCount++ % 2 == 1)
                {
                    player1.Move(moveSum);
                }
                else
                {
                    player2.Move(moveSum);
                }
            }

            var loserScore = Math.Min(player1.Score, player2.Score);
            Console.WriteLine("Player lost with score of {0}. Dice was rolled {1} times. Product is: {2}", loserScore, dice.RollCount, loserScore * dice.RollCount);

        }

        public void Part2(string[] lines)
        {
            var startPos = lines[0].Split(',');

            var rollOutcomes = new int[10];
            for (int i = 1; i <= 3; i++)
            {
                for (int j = 1; j <= 3; j++)
                {
                    for (int k = 1; k <= 3; k++)
                    {
                        rollOutcomes[i + j + k]++;
                    }
                }
            }

            var state = new GameState(int.Parse(startPos[0]), int.Parse(startPos[1]), 0, 0);
            var currentGameStates = new Dictionary<GameState, long>();
            var previousGameStates = currentGameStates;
            previousGameStates[state] = 1;

            long player1Wins = 0, player2Wins = 0;
            var isPlayerOneTurn = true;
            var turnNo = 1;
            while (previousGameStates.Keys.Any())
            {
                currentGameStates = new Dictionary<GameState, long>();
                foreach (var gameState in previousGameStates.Keys)
                {
                    for (int i = 3; i <= 9; i++)
                    {
                        var multiplier = rollOutcomes[i];
                        var totalAtNextPos = previousGameStates[gameState] * multiplier;
                        GameState nextGameState;
                        if (isPlayerOneTurn)
                        {
                            var nextPos = gameState.Player1Pos + i;
                            if (nextPos > 10)
                            {
                                nextPos = nextPos % 10;
                            }

                            var player1Score = gameState.Player1Score + nextPos;
                            if (player1Score >= 21)
                            {
                                player1Wins += totalAtNextPos;
                                continue;
                            }

                            nextGameState = new GameState(nextPos, gameState.Player2Pos, player1Score, gameState.Player2Score);                            
                        }
                        else
                        {
                            var nextPos = gameState.Player2Pos + i;
                            if (nextPos > 10)
                            {
                                nextPos = nextPos % 10;
                            }

                            var player2Score = gameState.Player2Score + nextPos;
                            if (player2Score >= 21)
                            {
                                player2Wins += totalAtNextPos;
                                continue;
                            }

                            nextGameState = new GameState(gameState.Player1Pos, nextPos, gameState.Player1Score, player2Score);
                        }

                        currentGameStates.GetOrAdd(nextGameState, 0);
                        currentGameStates[nextGameState] += totalAtNextPos;
                    }
                }

                previousGameStates = currentGameStates;
                turnNo++;
                isPlayerOneTurn = isPlayerOneTurn ^ true;
            }

            Console.WriteLine($"Player 1 has {player1Wins}, player 2 has {player2Wins}.");
        }

        public class GameState
        {
            public int Player1Pos { get; set; }
            public int Player2Pos { get; set; }
            public int Player1Score { get; set; }
            public int Player2Score { get; set; }

            public GameState(int player1Pos, int player2Pos, int player1Score, int player2Score)
            {
                Player1Pos = player1Pos;
                Player2Pos = player2Pos;
                Player2Score = player2Score;
                Player1Score = player1Score;
            }

            public override bool Equals([NotNullWhen(true)] object? obj)
            {
                var other = obj as GameState;
                if (other == null)
                {
                    return false;
                }

                return Player1Pos == other.Player1Pos &&
                    Player2Pos == other.Player2Pos &&
                    Player1Score == other.Player1Score &&
                    Player2Score == other.Player2Score;
            }

            public override int GetHashCode()
            {
                return Player1Pos.GetHashCode() ^ Player2Pos.GetHashCode() ^ Player1Score.GetHashCode() ^ Player2Score.GetHashCode();
            }

            public override string ToString()
            {
                return $" Score: {Player1Score}-{Player2Score}. Positions: {Player1Pos}: {Player2Pos}";
            }
        }

        public class Dice
        {
            public int CurrentValue { get; set; }
            public int MaxValue { get; set; }

            public int RollCount { get; set; }
            public Dice(int maxValue)
            {
                CurrentValue = 1;
                MaxValue = maxValue;
            }
            
            public int Roll()
            {
                RollCount++;
                if (CurrentValue > MaxValue)
                {
                    CurrentValue = 1;
                }
                return CurrentValue++;
            }
        }

        public class Player
        {
            private int MaxSpace { get; set; }
            public long Score { get; set; }

            public int CurrentPosition { get; set; }

            private Player(Player other)
            {
                MaxSpace = other.MaxSpace;
                Score = other.Score;
                CurrentPosition = other.CurrentPosition;
            }

            public Player(string startingPos, int maxSpace)
            {
                CurrentPosition = int.Parse(startingPos);
                MaxSpace = maxSpace;
            }

            public void Move(int amount)
            {
                CurrentPosition += amount;
                CurrentPosition = CurrentPosition % MaxSpace == 0 ? MaxSpace : CurrentPosition % MaxSpace;
                Score += CurrentPosition;
            }
        }

        #region TestInput

        public string TestInput =
            @"4,8";

        #endregion

        #region Input

        public string Input =
            @"4,2";

        #endregion 
    }
}
