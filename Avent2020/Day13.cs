using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avent2020
{
    public class Day13
    {
        public void Run()
        {
            Part2();
        }

        public void Part1()
        {
            var input = Input;
            var arriveTime = long.Parse(input.Split("\r\n")[0]);
            var busIds = input.Split("\r\n")[1].Split(',').Where(x => x != "x").Select(x => int.Parse(x)).ToList();

            var minWaitBusId = int.MaxValue;
            var minWaitBusTime = long.MaxValue;
            for (int i = 0; i < busIds.Count(); i++)
            {
                var bus = busIds[i];
                var numWaits = arriveTime / busIds[i];
                if (numWaits * bus != arriveTime)
                    numWaits++;

                var busWaitTime = numWaits * bus - arriveTime;
                if (busWaitTime < minWaitBusTime)
                {
                    minWaitBusTime = busWaitTime;
                    minWaitBusId = bus;
                }
            }

            Console.WriteLine($"Min wait time of {minWaitBusTime} with bus {minWaitBusId} for a product of {minWaitBusId * minWaitBusTime}");
        }


        public void Part2()
        {
            var input = Input;
            var arriveTime = long.Parse(input.Split("\r\n")[0]);
            var busIds = input.Split("\r\n")[1].Split(',').Select(x => x == "x" ? -1 : long.Parse(x)).ToList();
            var mods = busIds.Where(x => x > 0).ToArray();
            var remainders = mods.Select(x => busIds.IndexOf(x) == 0 ? 0 : x - busIds.IndexOf(x)).ToArray();
            var sln = ChineseRemainderTheorem.Solve(mods, remainders); 
            Console.WriteLine($"Solution is: {sln}");
            
        }

        public static class ChineseRemainderTheorem
        {
            public static long Solve(long[] n, long[] a)
            {
                var prod = n.Aggregate((product, j) => ((product * j)));
                long p;
                long sm = 0;
                for (int i = 0; i < n.Length; i++)
                {
                    p = prod / n[i];
                    sm += (long)(a[i] * ModularMultiplicativeInverse(p, n[i]) * p);
                }
                return (long)(sm % prod);
            }

            private static long ModularMultiplicativeInverse(long a, long mod)
            {
                long b = a % mod;
                for (int x = 1; x < mod; x++)
                {
                    if ((b * x) % mod == 1)
                    {
                        return x;
                    }
                }
                return 1;
            }
        }

        string TestInput = @"939
7,13,x,x,59,x,31,19";

        string Input = @"1000417
23,x,x,x,x,x,x,x,x,x,x,x,x,41,x,x,x,37,x,x,x,x,x,479,x,x,x,x,x,x,x,x,x,x,x,x,13,x,x,x,17,x,x,x,x,x,x,x,x,x,x,x,29,x,373,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,19";
    }
}
