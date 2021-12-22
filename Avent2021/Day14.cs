using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avent2021
{
    internal class Day14
    {
        public void Run()
        {
            var lines = TestInput.Split("\r\n");
            Part2(lines);
        }

        public void Part1(string[] lines)
        {
            var firstNode = BuildTemplate(lines[0]);
            var insertionDict = BuildInsertionDict(lines.Skip(2).ToArray());
            var numSteps = 5;
            var current = firstNode;
            for (int i = 0; i < numSteps; i++)
            {
                current = firstNode;
                while (current.Next != null)
                {
                    var next = current.Next;
                    var pair = $"{current.Val}{next.Val}";
                    var insertionVal = insertionDict[pair];

                    var insertNode = new Node<char>(insertionVal);
                    insertNode.Next = next;
                    insertNode.Previous = current;
                    current.Next = insertNode;
                    next.Previous = insertNode;

                    current = next;
                }

                current = firstNode;
                Console.Write($"Step {i + 1}: ");
                while (current != null)
                {
                    Console.Write(current.Val);
                    current = current.Next;
                }
                Console.WriteLine();
            }

            var frequency = new long[26];
            current = firstNode;
            while (current != null)
            {
                frequency[current.Val - 'A']++;
                current = current.Next;
            }

            var mostCommonCount = frequency.Max();
            var leastCommonCount = frequency.Where(x => x != 0).Min();
            Console.WriteLine($"Most common frequency is: {mostCommonCount}, least common frequency is : {leastCommonCount}. Subtract is: {mostCommonCount - leastCommonCount}");
        }

        public void Part2(string[] lines)
        {
            var firstNode = BuildTemplate(lines[0]);
            var insertionDict = BuildInsertionDict(lines.Skip(2).ToArray());
            var current = firstNode;

            var currentRoundPairs = new Dictionary<string, long>();
            while (current.Next != null)
            {
                var next = current.Next;
                var pair = $"{current.Val}{next.Val}";
                currentRoundPairs.GetOrAdd(pair, 0);
                currentRoundPairs[pair]++;
                current = current.Next;
            }

            var numSteps = 10;
            for (int i = 1; i <= numSteps; i++)
            {
                var nextRoundPairs = new Dictionary<string, long>();
                foreach (var key in currentRoundPairs.Keys)
                {
                    var pairCount = currentRoundPairs[key];
                    var insertChar = insertionDict[key];

                    var p1 = $"{key[0]}{insertChar}";
                    nextRoundPairs.GetOrAdd(p1, 0);
                    nextRoundPairs[p1] += pairCount;

                    var p2 = $"{insertChar}{key[1]}";
                    nextRoundPairs.GetOrAdd(p2, 0);
                    nextRoundPairs[p2] += pairCount;
                }

                currentRoundPairs = nextRoundPairs;
            }                               

            var frequency = new long[26];

            // Get the number of times the first letter in each pair occurs. Then add on the final letter of input string.
            foreach (var pair in currentRoundPairs.Keys)
            {
                frequency[pair[0] - 'A'] += currentRoundPairs[pair];
            }
            frequency[lines[0].Last() - 'A']++;

            var mostCommonCount = frequency.Max();
            var leastCommonCount = frequency.Where(x => x != 0).Min();
            var sub = (mostCommonCount - leastCommonCount);
            Console.WriteLine($"Most common frequency is: {mostCommonCount}, least common frequency is : {leastCommonCount}. Subtract is: {sub}");
        }

        private Node<char> BuildTemplate(string line)
        {
            Node<char>? previous = null;
            Node<char>? first = null;
            foreach (var c in line)
            {
                var current = new Node<char>(c);
                if (previous != null)
                {
                    previous.Next = current;
                    current.Previous = previous;
                }
                else
                {
                    first = current;
                }

                previous = current;
            }

            return first;
        }

        private Dictionary<string, char> BuildInsertionDict(string[] lines)
        {
            var dict = new Dictionary<string, char>();
            foreach (var line in lines)
            {
                var parts = line.Split(" -> ");
                dict[parts[0]] = parts[1][0];
            }
            return dict;
        }

        #region TestInput

        public string TestInput =
            @"NNCB

CH -> B
HH -> N
CB -> H
NH -> C
HB -> C
HC -> B
HN -> C
NN -> C
BH -> H
NC -> B
NB -> B
BN -> B
BB -> N
BC -> B
CC -> N
CN -> C";

        #endregion

        #region Input

        public string Input =
            @"KHSSCSKKCPFKPPBBOKVF

OS -> N
KO -> O
SK -> B
NV -> N
SH -> V
OB -> V
HH -> F
HP -> H
BP -> O
HS -> K
SN -> B
PS -> C
BS -> K
CF -> H
SO -> C
NO -> H
PP -> H
SS -> P
KV -> B
KN -> V
CC -> S
HK -> H
FN -> C
OO -> K
CH -> H
CP -> V
HB -> N
VC -> S
SP -> F
BO -> F
SF -> H
VO -> B
FF -> P
CN -> O
NP -> H
KK -> N
OP -> S
BH -> F
CB -> V
HC -> P
KH -> V
OV -> V
NK -> S
PN -> F
VV -> N
HO -> S
KS -> C
FP -> F
FH -> F
BB -> C
FB -> V
SB -> K
KP -> B
FS -> C
KC -> P
SC -> C
VF -> F
VN -> B
CK -> C
KF -> H
NS -> C
FV -> K
HV -> B
HF -> K
ON -> S
CV -> N
BV -> F
NB -> N
NN -> F
BF -> N
VB -> V
VS -> K
BK -> V
VP -> P
PB -> F
KB -> C
VK -> O
NF -> F
FO -> F
PH -> N
VH -> B
HN -> B
FK -> K
PO -> H
CO -> B
FC -> V
OK -> F
OF -> V
PF -> F
BC -> B
BN -> O
NC -> K
SV -> H
OH -> B
PC -> O
OC -> C
CS -> P
PV -> V
NH -> C
PK -> H";

        #endregion 
    }
}
