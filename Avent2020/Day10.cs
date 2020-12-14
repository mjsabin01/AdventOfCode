using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avent2020
{
    class Day10
    {
        public void Run()
        {
            Part2();
        }

        public void Part1()
        {
            var input = Input;
            var lines = input.Split("\r\n");
            var orderedAmps = lines.Select(x => int.Parse(x.Trim())).ToList();
            orderedAmps.Sort();
            var current = 0;
            long count1 = 0;
            long count3 = 1;
            foreach (var amp in orderedAmps)
            {
                if (amp - current == 3)
                {
                    count3++;
                }
                else
                {
                    count1++;
                }
                current = amp;
            }

            Console.WriteLine($"There are '{count1}' 1 jlt difference and '{count3}' 3 jolt difference. Result value is {count1 * count3}.");
        }

        public void Part2()
        {
            var time = DateTime.Now;
            var input = Input;
            var lines = input.Split("\r\n");
            var orderedAmps = lines.Select(x => int.Parse(x.Trim())).ToList();
            orderedAmps.Sort((x, y) => y.CompareTo(x));
            var wayDict = new Dictionary<int, long>();
            var totalWays = CheckPath(orderedAmps, 0, wayDict);

            var end = DateTime.Now; 
            Console.WriteLine($"There are '{totalWays}' to get max jouts. Took {end - time}.");
        }

        long CheckPath(List<int> values, int index, Dictionary<int, long> wayDict)
        {

            Console.WriteLine($"Checking value at index: {index} of {values.Count}.");
            if (wayDict.ContainsKey(index))
            {
                return wayDict[index];
            }


            long acum = values[index] <= 3 ? 1 : 0;
            for (int i = index + 1; i <= index + 3; i++)
            {
                if (i < values.Count && values[index] - values[i] <= 3)
                {
                    acum += CheckPath(values, i, wayDict);
                }
            }

            wayDict[index] = acum;
            return acum;
        }

        string TestInput0 = @"16
10
15
5
1
11
7
19
6
12
4";
        string TestInput1 = @"28
33
18
42
31
14
46
20
48
47
24
23
49
45
19
38
39
11
1
32
25
35
8
17
7
9
4
2
34
10
3";

        string Input = @"145
3
157
75
84
141
40
20
60
48
15
4
2
21
129
113
54
28
69
42
34
1
155
63
151
8
139
135
33
81
70
132
150
112
102
59
154
53
144
149
116
13
41
156
85
22
165
51
14
125
52
64
16
134
110
71
107
124
164
160
10
25
66
74
161
111
122
166
140
87
126
123
146
35
91
106
133
26
77
19
86
105
39
99
76
58
31
96
78
88
168
119
27
45
9
92
138
38
97
32
7
98
167
95
55
65";
    }
}
