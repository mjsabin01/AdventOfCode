using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Avent2020
{
    public class Day23
    {
        public void Run()
        {
            Part2();
        }

        class CupNode
        {
            public CupNode(long id)
            {
                Id = id;
            }

            public long Id { get; set; }
            public CupNode Next { get; set; }
        }

        Regex regInput = new Regex(@"(\d)+");

        public void Part1()
        {
            var numMoves = 100;
            var input = Input;
            var match = regInput.Match(input);
            CupNode current = null;
            CupNode start = null;
            CupNode printNode = null;
            Dictionary<long, CupNode> cupDict = new Dictionary<long, CupNode>();
            long maxNodeId = -1;
            for (int i = 0; i < match.Groups[1].Captures.Count; i++)
            {
                CupNode newNode = new CupNode(long.Parse(match.Groups[1].Captures[i].Value));
                maxNodeId = Math.Max(maxNodeId, newNode.Id);
                cupDict[newNode.Id] = newNode;
                if (i != 0)
                {
                    current.Next = newNode;
                }
                else
                {
                    start = newNode;
                }

                current = newNode;
            }
            current.Next = start;
            current = start;

            Console.Write($"\r\nInitial Order of cups: ({current.Id})");
            printNode = current.Next;
            while (printNode != current)
            {
                Console.Write(printNode.Id);
                printNode = printNode.Next;
            }

            PlayRounds(current, numMoves, maxNodeId, cupDict);

            while (current.Id != 1)
            {
                current = current.Next;
            }

            Console.Write("\r\nFinal Order of cups after 1: ");
            printNode = current.Next;
            while (printNode != current)
            {
                Console.Write(printNode.Id);
                printNode = printNode.Next;
            }
        }

        public void Part2()
        {
            var numMoves = 10000000;
            var input = Input;
            var match = regInput.Match(input);
            CupNode current = null;
            CupNode start = null;
            CupNode node1 = null;
            long maxNodeId = -1;
            Dictionary<long, CupNode> cupDict = new Dictionary<long, CupNode>();
            for (int i = 0; i < match.Groups[1].Captures.Count; i++)
            {
                CupNode newNode = new CupNode(long.Parse(match.Groups[1].Captures[i].Value));
                maxNodeId = Math.Max(maxNodeId, newNode.Id);
                cupDict[newNode.Id] = newNode;
                if (newNode.Id == 1)
                {
                    node1 = newNode;
                }

                if (i != 0)
                {
                    current.Next = newNode;
                }
                else
                {
                    start = newNode;
                }

                current = newNode;
            }

            for (long i = maxNodeId + 1; i <= 1000000; i++)
            {
                CupNode newNode = new CupNode(i);
                current.Next = newNode;
                current = newNode;
                cupDict[newNode.Id] = newNode;
            }
            maxNodeId = current.Id;

            current.Next = start;
            current = start;

            PlayRounds(current, numMoves, maxNodeId, cupDict);

            var val1 = node1.Next.Id;
            var val2 = node1.Next.Next.Id;
            Console.WriteLine($"Values after 1 are: {val1} and {val2} for a product of {val1 * val2}.");

        }

        void PlayRounds(CupNode current, long numRounds, long maxNodeId, Dictionary<long, CupNode> cupDict)
        {
            for (int i = 0; i < numRounds; i++)
            {
                /// remove the next node and set the currents next to 4 nodes down
                var removeNode = current.Next;
                current.Next = removeNode.Next.Next.Next;
                removeNode.Next.Next.Next = null;

                var destId = current.Id - 1;
                CupNode dest = null;
                while (dest == null)
                {
                    if (destId == 0)
                    {
                        destId = maxNodeId;
                    }

                    if (removeNode.Id == destId || removeNode.Next.Id == destId || removeNode.Next.Next.Id == destId)
                    {
                        destId--;
                        continue;
                    }

                    dest = cupDict[destId];
                }

                removeNode.Next.Next.Next = dest.Next;
                dest.Next = removeNode;

                current = current.Next;
            }
        }

        string TestInput = @"389125467";
        string Input = @"562893147";
    }
}
