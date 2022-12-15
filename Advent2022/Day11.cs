using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avent2022;

internal class Day11
{
    public void Run()
    {
        var lines = Input.Split("\r\n");
        Part2(lines);
    }

    public void Part1(string[] lines)
    {
        List<Monkey> allMonkeys = new();
        var totalMonkeys = (lines.Length + 1) / 7;
        for (int i = 0; i < totalMonkeys; i++)
        {
            var monkey = new Monkey(i, lines, allMonkeys);
            allMonkeys.Add(monkey);
        }

        var numRounds = 20;
        for (int i = 0; i < numRounds; i++)
        {
            for (int m = 0; m < totalMonkeys; m++)
            {
                var monkey = allMonkeys[m];
                while (monkey.Items.Any())
                {
                    monkey.NumberItemsInspected++;

                    var item = monkey.Items.Dequeue();
                    long l = monkey.OpLhsVal == null ? item : monkey.OpLhsVal.Value;
                    long r = monkey.OpRhsVal == null ? item : monkey.OpRhsVal.Value;

                    // perform the operation
                    item = monkey.IsAdditionOperation ? l + r : l * r;

                    // divide by 3
                    item /= 3;

                    if (item % monkey.Divisor == 0)
                    {
                        allMonkeys[monkey.DivisorTrueMonkeyId].EnqueItem(item);
                    }
                    else
                    {
                        allMonkeys[monkey.DivisorFalseMonkeyId].EnqueItem(item);
                    }
                }
            }
        }

        allMonkeys.Sort((x, y) => y.NumberItemsInspected.CompareTo(x.NumberItemsInspected));
        var monkeyBusiness = allMonkeys.Take(2).Select(x => x.NumberItemsInspected).Aggregate((x, y) => x * y);
        Console.WriteLine($"Monkey business for {numRounds} rounds is {monkeyBusiness}.");
    }

    public void Part2(string[] lines)
    {
        List<Monkey> allMonkeys = new();
        var totalMonkeys = (lines.Length + 1) / 7;
        for (int i = 0; i < totalMonkeys; i++)
        {
            var monkey = new Monkey(i, lines, allMonkeys);
            allMonkeys.Add(monkey);
        }

        // Since you are no longer div by 3 each turn you need to get LCM of all modulo operations to keep under control
        var moduloOps = allMonkeys.Select(x => x.Divisor).ToArray();
        var lcm = Utils.LeastCommonMultiple(moduloOps);

        var numRounds = 10000;
        for (int i = 0; i < numRounds; i++)
        {
            for (int m = 0; m < totalMonkeys; m++)
            {
                var monkey = allMonkeys[m];
                while (monkey.Items.Any())
                {
                    monkey.NumberItemsInspected++;

                    var item = monkey.Items.Dequeue();
                    long l = monkey.OpLhsVal == null ? item : monkey.OpLhsVal.Value;
                    long r = monkey.OpRhsVal == null ? item : monkey.OpRhsVal.Value;

                    // perform the operation
                    item = monkey.IsAdditionOperation ? l + r : l * r;

                    // get modulo of lcm to reduce
                    item %= lcm;

                    if (item % monkey.Divisor == 0)
                    {
                        allMonkeys[monkey.DivisorTrueMonkeyId].EnqueItem(item);
                    }
                    else
                    {
                        allMonkeys[monkey.DivisorFalseMonkeyId].EnqueItem(item);
                    }
                }
            }
        }

        
        allMonkeys.Sort((x, y) => y.NumberItemsInspected.CompareTo(x.NumberItemsInspected));
        var monkeyBusiness = allMonkeys.Take(2).Select(x => x.NumberItemsInspected).Aggregate((x, y) => x * y);
        Console.WriteLine($"Monkey business for {numRounds} rounds is {monkeyBusiness}.");
    }

    

    class Monkey
    {
        public Queue<long> Items { get; } = new Queue<long>();
        public bool IsAdditionOperation { get; }
        public long? OpLhsVal { get; }
        public long? OpRhsVal { get; }
        public long Divisor { get; }
        public int DivisorTrueMonkeyId { get; }
        public int DivisorFalseMonkeyId { get; }
        public int Id { get; }
        public long NumberItemsInspected { get; set; }

        public Monkey(int monkeyId, string[] lines, List<Monkey> allMonkeys)
        {
            Id = monkeyId;

            var startLine = 7 * monkeyId;

            // starting line + 1 has items
            var l1 = lines[startLine + 1].Substring("  Starting items:".Length + 1);
            var items = l1
                .Split(',')
                .Select(x => long.Parse(x.Trim()));
            foreach (var item in items) { Items.Enqueue(item); }

            // starting line + 2 has the operation
            var l2 = lines[startLine + 2].Substring("  Operation:".Length + 1).Trim();
            var opParts = l2
                .Split(" ")
                .Select(x => x.Trim())
                .ToList();
            OpLhsVal = string.Equals(opParts[2], "old") ? null : long.Parse(opParts[2]);
            IsAdditionOperation = string.Equals(opParts[3], "+");
            OpRhsVal = string.Equals(opParts[4], "old") ? null : long.Parse(opParts[4]);

            // starting line + 3 has divisor
            var l3 = lines[startLine + 3].Substring("  Test: divisible by ".Length).Trim();
            Divisor = long.Parse(l3);

            // starting line + 4 has monkey to throw if true
            var l4 = lines[startLine + 4].Substring("    If true: throw to monkey ".Length).Trim();
            DivisorTrueMonkeyId = int.Parse(l4);

            // starting line + 4 has monkey to throw if true
            var l5 = lines[startLine + 5].Substring("    If false: throw to monkey ".Length).Trim();
            DivisorFalseMonkeyId = int.Parse(l5);
        }

        

        public void EnqueItem(long item)
        {
            Items.Enqueue(item);
        }

    }

    #region TestInput

    public string TestInput =
        @"Monkey 0:
  Starting items: 79, 98
  Operation: new = old * 19
  Test: divisible by 23
    If true: throw to monkey 2
    If false: throw to monkey 3

Monkey 1:
  Starting items: 54, 65, 75, 74
  Operation: new = old + 6
  Test: divisible by 19
    If true: throw to monkey 2
    If false: throw to monkey 0

Monkey 2:
  Starting items: 79, 60, 97
  Operation: new = old * old
  Test: divisible by 13
    If true: throw to monkey 1
    If false: throw to monkey 3

Monkey 3:
  Starting items: 74
  Operation: new = old + 3
  Test: divisible by 17
    If true: throw to monkey 0
    If false: throw to monkey 1";

    #endregion

    #region Input

    public string Input =
        @"Monkey 0:
  Starting items: 63, 84, 80, 83, 84, 53, 88, 72
  Operation: new = old * 11
  Test: divisible by 13
    If true: throw to monkey 4
    If false: throw to monkey 7

Monkey 1:
  Starting items: 67, 56, 92, 88, 84
  Operation: new = old + 4
  Test: divisible by 11
    If true: throw to monkey 5
    If false: throw to monkey 3

Monkey 2:
  Starting items: 52
  Operation: new = old * old
  Test: divisible by 2
    If true: throw to monkey 3
    If false: throw to monkey 1

Monkey 3:
  Starting items: 59, 53, 60, 92, 69, 72
  Operation: new = old + 2
  Test: divisible by 5
    If true: throw to monkey 5
    If false: throw to monkey 6

Monkey 4:
  Starting items: 61, 52, 55, 61
  Operation: new = old + 3
  Test: divisible by 7
    If true: throw to monkey 7
    If false: throw to monkey 2

Monkey 5:
  Starting items: 79, 53
  Operation: new = old + 1
  Test: divisible by 3
    If true: throw to monkey 0
    If false: throw to monkey 6

Monkey 6:
  Starting items: 59, 86, 67, 95, 92, 77, 91
  Operation: new = old + 5
  Test: divisible by 19
    If true: throw to monkey 4
    If false: throw to monkey 0

Monkey 7:
  Starting items: 58, 83, 89
  Operation: new = old * 19
  Test: divisible by 17
    If true: throw to monkey 2
    If false: throw to monkey 1";

    #endregion 
}

