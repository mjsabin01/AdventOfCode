using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avent2020
{
    public class Day15
    {
        public void Run()
        {
            //DoRun(2020);
            DoRun(30000000);
        }

        public void DoRun(int numTurns)
        {
            var input = new long[] { 18, 8, 0, 5, 4, 1, 20 };
            var turnNo = 1;
            var lastTurn = numTurns;
            long lastSpoken = -1;
            var numDict = new Dictionary<long, List<int>>();
            while (turnNo <= lastTurn)
            {
                if (turnNo <= input.Length)
                {
                    lastSpoken = input[turnNo - 1];
                }
                else if (numDict.ContainsKey(lastSpoken) && numDict[lastSpoken].Count >= 2)
                {
                    lastSpoken = numDict[lastSpoken][0] - numDict[lastSpoken][1];
                }
                else
                {
                    lastSpoken = 0;
                }

                turnNo++;
                if (!numDict.ContainsKey(lastSpoken))
                {
                    numDict[lastSpoken] = new List<int>(2) { turnNo, 0 };
                }

                var list = numDict[lastSpoken];
                list.Insert(0, turnNo);
                if (list.Count > 2)
                {
                    list.RemoveAt(2);
                }
                //Console.WriteLine($"On turn: {turnNo} spoken word was: {lastSpoken}");
            }

            Console.WriteLine($"On turn: {turnNo - 1} spoken word was: {lastSpoken}");
        }
    }
}
