using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avent2020
{
    public class Day22
    {
        public void Run()
        {
            Part2();
        }

        public void Part1()
        {
            var input = Input;
            var deckLines = input.Split("\r\n\r\n");
            if (deckLines.Length != 2)
            {
                throw new Exception();
            }

            var p1Cards = LoadDeck(deckLines[0]);
            var p2Cards = LoadDeck(deckLines[1]);

            var round = 1;
            while (p1Cards.Any() && p2Cards.Any())
            {
                var p1Card = p1Cards.First();
                p1Cards.RemoveAt(0);
                var p2Card = p2Cards.First();
                p2Cards.RemoveAt(0);

                if (p1Card > p2Card)
                {
                    p1Cards.Add(p1Card);
                    p1Cards.Add(p2Card);
                }
                else
                {
                    p2Cards.Add(p2Card);
                    p2Cards.Add(p1Card);
                }
                round++;
            }

            var winningCards = p1Cards.Any() ? p1Cards.ToList() : p2Cards.ToList();
            long sum = 0;
            for (int i = winningCards.Count - 1; i >= 0; i--)
            {
                sum += winningCards[i] * (winningCards.Count - i);
            }
            Console.WriteLine($"Winning player sum is: {sum}");
        }

        public void Part2()
        {
            var input = Input;
            var deckLines = input.Split("\r\n\r\n");
            if (deckLines.Length != 2)
            {
                throw new Exception();
            }

            var p1Cards = LoadDeck(deckLines[0]);
            var p2Cards = LoadDeck(deckLines[1]);

            var winner = PlayRecursiveCombat(p1Cards, p2Cards);

            var winningCards = winner == 1 ? p1Cards : p2Cards;
            long sum = 0;
            for (int i = winningCards.Count - 1; i >= 0; i--)
            {
                sum += winningCards[i] * (winningCards.Count - i);
            }
            Console.WriteLine($"Winning player sum is: {sum}");
        }

        int PlayRecursiveCombat(List<long> p1Cards, List<long> p2Cards)
        {
            var round = 1;
            List<long[]> previousP1Cards = new List<long[]>();
            List<long[]> previousP2Cards = new List<long[]>();

            while (p1Cards.Any() && p2Cards.Any())
            {
                if (DoesDeckMatchPrevious(previousP1Cards, p1Cards))
                {
                    return 1;
                }
                previousP1Cards.Add(p1Cards.ToArray());

                if (DoesDeckMatchPrevious(previousP2Cards, p2Cards))
                {
                    return 1;
                }
                previousP2Cards.Add(p2Cards.ToArray());

                var p1Card = p1Cards.First();
                p1Cards.RemoveAt(0);
                var p2Card = p2Cards.First();
                p2Cards.RemoveAt(0);

                var roundWinner = 0;
                if (p1Card <= p1Cards.Count && p2Card <= p2Cards.Count)
                {
                    var p1Rec = new List<long>();
                    p1Rec.AddRange(p1Cards.GetRange(0, (int)p1Card));
                    var p2Rec = new List<long>();
                    p2Rec.AddRange(p2Cards.GetRange(0, (int)p2Card));
                    roundWinner = PlayRecursiveCombat(p1Rec, p2Rec);
                }
                else
                {
                    roundWinner = p1Card > p2Card ? 1 : 2;
                }

                if (roundWinner == 1)
                {
                    p1Cards.Add(p1Card);
                    p1Cards.Add(p2Card);
                }
                else
                {
                    p2Cards.Add(p2Card);
                    p2Cards.Add(p1Card);
                }
                round++;
            }

            var winner = p1Cards.Any() ? 1 : 2;
            return winner;
        }

        bool DoesDeckMatchPrevious(List<long[]> previousCards, List<long> currentCards)
        {
            var arr = currentCards.ToArray();
            foreach (var previous in previousCards)
            {
                if (arr.SequenceEqual(previous))
                    return true;
            }
            return false;
        }

        List<long> LoadDeck(string deckInput)
        {
            var deck = new List<long>();
            var deckLines = deckInput.Split("\r\n");
            for (int i = 1; i < deckLines.Length; i++)
            {
                deck.Add(long.Parse(deckLines[i]));
            }
            return deck;
        }

        string TestInput1 = @"Player 1:
9
2
6
3
1

Player 2:
5
8
4
7
10";

        string Input = @"Player 1:
41
26
29
11
50
38
42
20
13
9
40
43
10
24
35
30
23
15
31
48
27
44
16
12
14

Player 2:
18
6
32
37
25
21
33
28
7
8
45
46
49
5
19
2
39
4
17
3
22
1
34
36
47";
    
    }
}
