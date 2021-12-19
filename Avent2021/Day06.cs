using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avent2021
{
    internal class Day06
    {
        public void Run()
        {
            var lines = Input.Split("\r\n");
            Part2(lines);
        }

        public void Part1(string[] lines)
        {
            var lanternFish = lines[0].Split(',').Select(x => int.Parse(x)).ToList();
            var numDays = 80;
            for (int i = 1; i <= numDays; i++)
            {
                var additionFish = new List<int>();
                for (int j = 0; j < lanternFish.Count; j++)
                {
                    if (lanternFish[j] == 0)
                    {
                        additionFish.Add(8);
                        lanternFish[j] = 6;
                    }
                    else
                    {
                        lanternFish[j]--;
                    }
                }

                lanternFish.AddRange(additionFish);
            }

            Console.WriteLine($"After {numDays} there are {lanternFish.Count} lantern fish.");

        }

        public void Part2(string[] lines)
        {
            var lanternFish = lines[0].Split(',').Select(x => int.Parse(x)).ToList();
            var numDays = 256;
            var fishDays = new long[9];
            foreach (var fish in lanternFish)
            {
                fishDays[fish]++;
            }

            for (int i = 1;i <= numDays; i++)
            {
                var nextFishDays = new long[9];
                for (int j = 8; j >= 0; j--)
                {
                    if (j == 0)
                    {
                        var count = fishDays[j];
                        nextFishDays[6] += count;
                        nextFishDays[8] = count;
                    }
                    else
                    {
                        nextFishDays[j - 1] = fishDays[j];
                    }
                }
                fishDays = nextFishDays;
            }
           
            var total = fishDays.Sum();

            Console.WriteLine($"After {numDays} there are {total} lantern fish.");
        }

        #region TestInput

        public string TestInput =
            @"3,4,3,1,2";

        #endregion

        #region Input

        public string Input =
            @"2,1,2,1,5,1,5,1,2,2,1,1,5,1,4,4,4,3,1,2,2,3,4,1,1,5,1,1,4,2,5,5,5,1,1,4,5,4,1,1,4,2,1,4,1,2,2,5,1,1,5,1,1,3,4,4,1,2,3,1,5,5,4,1,4,1,2,1,5,1,1,1,3,4,1,1,5,1,5,1,1,5,1,1,4,3,2,4,1,4,1,5,3,3,1,5,1,3,1,1,4,1,4,5,2,3,1,1,1,1,3,1,2,1,5,1,1,5,1,1,1,1,4,1,4,3,1,5,1,1,5,4,4,2,1,4,5,1,1,3,3,1,1,4,2,5,5,2,4,1,4,5,4,5,3,1,4,1,5,2,4,5,3,1,3,2,4,5,4,4,1,5,1,5,1,2,2,1,4,1,1,4,2,2,2,4,1,1,5,3,1,1,5,4,4,1,5,1,3,1,3,2,2,1,1,4,1,4,1,2,2,1,1,3,5,1,2,1,3,1,4,5,1,3,4,1,1,1,1,4,3,3,4,5,1,1,1,1,1,2,4,5,3,4,2,1,1,1,3,3,1,4,1,1,4,2,1,5,1,1,2,3,4,2,5,1,1,1,5,1,1,4,1,2,4,1,1,2,4,3,4,2,3,1,1,2,1,5,4,2,3,5,1,2,3,1,2,2,1,4";

        #endregion 
    }
}
