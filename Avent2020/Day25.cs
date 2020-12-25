using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avent2020
{
    public class Day25
    {
        public void Run()
        {
            var inputKeys = new long[] { 10441485, 1004920 };
            var testKeys = new long[] { 5764801, 17807724 };
            var keys = inputKeys;
            var loopSizes = new List<int>();
            foreach (var key in keys)
            {
                var loopSize = GetLoopSize(key, 7);
                loopSizes.Add(loopSize);
            }

            var encryptionKey = TransformSubject(keys[1], loopSizes[0]);
            Console.WriteLine($"The encryption key is: {encryptionKey}");
        }

        public long TransformSubject(long subjectNumber, int loopSize)
        {
            long val = 1;

            for (int i = 0; i < loopSize; i++)
            {
                val = val * subjectNumber;
                val = val % 20201227;
            }

            return val;
        }

        public int GetLoopSize(long key, long subjectNumber)
        {
            long val = 1;
            int loopSize = 0;

            while (val != key)
            {
                val = val * subjectNumber;
                val = val % 20201227;
                loopSize++;
            }

            return loopSize;
        }
    }
}
