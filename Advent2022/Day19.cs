using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Avent2022;

internal class Day19
{
    public void Run()
    {
        Stopwatch s = new Stopwatch();
        s.Start();

        var lines = TestInput.Split("\r\n");
        Part1(lines);

        s.Stop();
        Console.WriteLine("Took: {0} to complete.", s.Elapsed);
    }

    public void Part1(string[] lines)
    {
        var blueprints = lines.Select(x => new Blueprint(x)).ToList();
        var qualityLevel = 0;
        foreach(var blueprint in blueprints)
        {
            var backpack = new Backpack() { OreRobots = 1 };
            var cache = new Dictionary<CacheKey, int>();
            var maxAtTimeCache = new Dictionary<int, int>();
            var blueprintMax = CollectGeodes(backpack, blueprint, 24, cache);//, maxAtTimeCache);
            var cacheList = cache.ToList();
            cacheList.Sort((x,y) => y.Value.CompareTo(x.Value));
            qualityLevel += blueprintMax * blueprint.Id;
        }

        Console.WriteLine("Max geodes are: {0}.", qualityLevel);
    }

    public void Part2(string[] lines)
    {

    }

    int CollectGeodes (Backpack previous, Blueprint blueprint, int remaining, Dictionary<CacheKey, int> cache)//, Dictionary<int, int> maxAtTimeCache)
    {
        if (remaining == 0)
        {
            return previous.Geode;
        }

        previous.Reduce(blueprint, remaining);

        var cacheKey = new CacheKey(remaining, previous);
        if (cache.ContainsKey(cacheKey))
        {
            return cache[cacheKey];
        }

        // start with a max of building no more robots and just let existing mine
        var maxGeode = previous.Geode + previous.GeodeRobots * remaining;
        
        if (previous.ObsidianRobots > 1)
        {
            var oreTurns = Math.Max(0, (int)Math.Ceiling((float)(blueprint.GeodeRobotOreCost - previous.Ore) / (float)(previous.OreRobots)));
            var obsidianTurns = Math.Max(0, (int)Math.Ceiling((float)(blueprint.GeodeRobotObsidianCost - previous.Obsidian) / (float)(previous.ObsidianRobots)));
            var turnsUntilFirstResource = (int)Math.Max(oreTurns, obsidianTurns);
            turnsUntilFirstResource += 2; // add 1 for turn to build machine and another for the next turn where it makes the resource
            if ((turnsUntilFirstResource) < remaining)
            {
                var robotCompleteTurns = turnsUntilFirstResource - 1;
                var next = previous.Clone(robotCompleteTurns);
                next.GeodeRobots++;
                next.Obsidian -= blueprint.GeodeRobotObsidianCost;
                next.Ore -= blueprint.GeodeRobotOreCost;

                var geode = CollectGeodes(next, blueprint, remaining - robotCompleteTurns, cache);
                maxGeode = Math.Max(maxGeode, geode);
            }
        }

        if ((previous.ObsidianRobots < blueprint.GeodeRobotObsidianCost) && previous.ClayRobots > 0)
        {
            // we can make a obsidian robot and we haven't reached the max
            var oreTurns = Math.Max(0, (int)Math.Ceiling((float)(blueprint.ObsidianRobotOreCost - previous.Ore) / (float)(previous.OreRobots)));
            var clayTurns = Math.Max(0, (int)Math.Ceiling((float)(blueprint.ObsidianRobotClayCost - previous.Clay) / (float)(previous.ClayRobots)));
            var turnsUntilFirstResource = (int)Math.Max(oreTurns, clayTurns);
            turnsUntilFirstResource += 2; // add 1 for turn to build machine and another for the next turn where it makes the resource
            if ((turnsUntilFirstResource) < remaining)
            {
                var robotCompleteTurns = turnsUntilFirstResource - 1;
                var next = previous.Clone(robotCompleteTurns);
                next.ObsidianRobots++;
                next.Ore -= blueprint.ObsidianRobotOreCost;
                next.Clay -= blueprint.ObsidianRobotClayCost;

                var geode = CollectGeodes(next, blueprint, remaining - robotCompleteTurns, cache);
                maxGeode = Math.Max(maxGeode, geode);
            }
        }

        if (previous.ClayRobots < blueprint.ObsidianRobotClayCost)
        {
            // we haven't reached max clay robots, so try making one
            var turnsUntilFirstResource = Math.Max(0, (int)Math.Ceiling((float)(blueprint.ClayRobotOreCost - previous.Ore) / (float)(previous.OreRobots)));
            turnsUntilFirstResource += 2; // add 1 for turn to build machine and another for the next turn where it makes the resource
            if ((turnsUntilFirstResource) < remaining)
            {
                var robotCompleteTurns = turnsUntilFirstResource - 1;
                var next = previous.Clone(robotCompleteTurns);
                next.ClayRobots++;
                next.Ore -= blueprint.ClayRobotOreCost;

                var geode = CollectGeodes(next, blueprint, remaining - robotCompleteTurns, cache);
                maxGeode = Math.Max(maxGeode, geode);
            }
        }

        if ((previous.OreRobots < blueprint.MaxOrePerRound))
        {
            // we haven't reached max ore robots, so try making one
            var turnsUntilFirstResource = Math.Max(0, (int)Math.Ceiling((float)(blueprint.OreRobotOreCost - previous.Ore) / (float)(previous.OreRobots)));
            turnsUntilFirstResource += 2; // add 1 for turn to build machine and another for the next turn where it makes the resource
            if ((turnsUntilFirstResource) < remaining)
            {
                var robotCompleteTurns = turnsUntilFirstResource - 1;
                var next = previous.Clone(robotCompleteTurns);
                next.OreRobots++;
                next.Ore -= blueprint.OreRobotOreCost;

                var geode = CollectGeodes(next, blueprint, remaining - robotCompleteTurns, cache);
                maxGeode = Math.Max(maxGeode, geode);
            }
        }

        cache[cacheKey] = maxGeode;
        return maxGeode;
    }

    struct CacheKey
    {
        public int Mins;

        public int Ore;
        public int Clay;
        public int Obsidian;
        public int Geode;

        public int OreRobots;
        public int ClayRobots;
        public int ObsidianRobots;
        public int GeodeRobots;

        public CacheKey(int mins, Backpack backpack)
        {
            Mins = mins;

            Ore = backpack.Ore;
            Clay = backpack.Clay;
            Obsidian = backpack.Obsidian;
            Geode = backpack.Geode;

            OreRobots = backpack.OreRobots;
            ClayRobots = backpack.ClayRobots;
            ObsidianRobots = backpack.ObsidianRobots;
            GeodeRobots = backpack.GeodeRobots;
        }

    }

    class Backpack
    {
        public int Ore;
        public int Clay;
        public int Obsidian;
        public int Geode;

        public int OreRobots;
        public int ClayRobots;
        public int ObsidianRobots;
        public int GeodeRobots;

        public Backpack Clone()
        {
            var backpack = (Backpack)MemberwiseClone();
            return backpack;
        }

        public Backpack Clone(int numberOfTurns)
        {
            var backpack = (Backpack)MemberwiseClone();
            backpack.Ore += numberOfTurns * OreRobots;
            backpack.Clay += numberOfTurns * ClayRobots;
            backpack.Obsidian += numberOfTurns * ObsidianRobots;
            backpack.Geode += numberOfTurns * GeodeRobots;   
            return backpack;
        }

        public void Reduce(Blueprint blueprint, int roundsRemaining)
        {
            Ore = Math.Min(blueprint.MaxOrePerRound * roundsRemaining, Ore);
            Clay = Math.Min(blueprint.ObsidianRobotClayCost * roundsRemaining, Clay);
            Obsidian = Math.Min(blueprint.GeodeRobotObsidianCost, Obsidian);
        }
    }

    class Blueprint
    {
        public int Id { get; }

        public int OreRobotOreCost { get; }

        public int ClayRobotOreCost { get; }

        public int ObsidianRobotOreCost { get; }

        public int ObsidianRobotClayCost { get; }

        public int GeodeRobotOreCost { get; }

        public int GeodeRobotObsidianCost { get; }

        public int MaxOrePerRound { get; }

        static Regex inputReg = new Regex(@"Blueprint (?<Id>\d+): Each ore robot costs (?<OreRobotOreCost>\d+) ore. Each clay robot costs (?<ClayRobotOreCost>\d+) ore. Each obsidian robot costs (?<ObsidianRobotOreCost>\d+) ore and (?<ObsidianRobotClayCost>\d+) clay. Each geode robot costs (?<GeodeRobotOreCost>\d+) ore and (?<GeodeRobotObsidianCost>\d+) obsidian.");
        public Blueprint(string input)
        {
            var m = inputReg.Match(input);
            Id = int.Parse(m.Groups["Id"].Value);
            OreRobotOreCost = int.Parse(m.Groups["OreRobotOreCost"].Value);
            ClayRobotOreCost = int.Parse(m.Groups["ClayRobotOreCost"].Value);
            ObsidianRobotOreCost = int.Parse(m.Groups["ObsidianRobotOreCost"].Value);
            ObsidianRobotClayCost = int.Parse(m.Groups["ObsidianRobotClayCost"].Value);
            GeodeRobotOreCost = int.Parse(m.Groups["GeodeRobotOreCost"].Value);
            GeodeRobotObsidianCost = int.Parse(m.Groups["GeodeRobotObsidianCost"].Value);

            MaxOrePerRound = Math.Max(GeodeRobotOreCost, Math.Max(ObsidianRobotClayCost, Math.Max(OreRobotOreCost, ClayRobotOreCost)));
        }
    }

    #region TestInput

    public string TestInput =
        @"Blueprint 1: Each ore robot costs 4 ore. Each clay robot costs 2 ore. Each obsidian robot costs 3 ore and 14 clay. Each geode robot costs 2 ore and 7 obsidian.
Blueprint 2: Each ore robot costs 2 ore. Each clay robot costs 3 ore. Each obsidian robot costs 3 ore and 8 clay. Each geode robot costs 3 ore and 12 obsidian.";

    #endregion

    #region Input

    public string Input =
        @"Blueprint 1: Each ore robot costs 3 ore. Each clay robot costs 4 ore. Each obsidian robot costs 3 ore and 10 clay. Each geode robot costs 2 ore and 7 obsidian.
Blueprint 2: Each ore robot costs 4 ore. Each clay robot costs 4 ore. Each obsidian robot costs 4 ore and 12 clay. Each geode robot costs 3 ore and 8 obsidian.
Blueprint 3: Each ore robot costs 4 ore. Each clay robot costs 4 ore. Each obsidian robot costs 4 ore and 17 clay. Each geode robot costs 2 ore and 13 obsidian.
Blueprint 4: Each ore robot costs 2 ore. Each clay robot costs 2 ore. Each obsidian robot costs 2 ore and 20 clay. Each geode robot costs 2 ore and 14 obsidian.
Blueprint 5: Each ore robot costs 3 ore. Each clay robot costs 4 ore. Each obsidian robot costs 4 ore and 5 clay. Each geode robot costs 3 ore and 12 obsidian.
Blueprint 6: Each ore robot costs 2 ore. Each clay robot costs 4 ore. Each obsidian robot costs 4 ore and 11 clay. Each geode robot costs 3 ore and 8 obsidian.
Blueprint 7: Each ore robot costs 3 ore. Each clay robot costs 4 ore. Each obsidian robot costs 3 ore and 17 clay. Each geode robot costs 3 ore and 7 obsidian.
Blueprint 8: Each ore robot costs 2 ore. Each clay robot costs 3 ore. Each obsidian robot costs 3 ore and 11 clay. Each geode robot costs 2 ore and 16 obsidian.
Blueprint 9: Each ore robot costs 3 ore. Each clay robot costs 3 ore. Each obsidian robot costs 3 ore and 9 clay. Each geode robot costs 3 ore and 7 obsidian.
Blueprint 10: Each ore robot costs 4 ore. Each clay robot costs 4 ore. Each obsidian robot costs 4 ore and 9 clay. Each geode robot costs 4 ore and 16 obsidian.
Blueprint 11: Each ore robot costs 4 ore. Each clay robot costs 4 ore. Each obsidian robot costs 2 ore and 16 clay. Each geode robot costs 4 ore and 16 obsidian.
Blueprint 12: Each ore robot costs 2 ore. Each clay robot costs 3 ore. Each obsidian robot costs 2 ore and 17 clay. Each geode robot costs 3 ore and 19 obsidian.
Blueprint 13: Each ore robot costs 4 ore. Each clay robot costs 4 ore. Each obsidian robot costs 2 ore and 10 clay. Each geode robot costs 3 ore and 14 obsidian.
Blueprint 14: Each ore robot costs 4 ore. Each clay robot costs 3 ore. Each obsidian robot costs 2 ore and 13 clay. Each geode robot costs 2 ore and 10 obsidian.
Blueprint 15: Each ore robot costs 3 ore. Each clay robot costs 3 ore. Each obsidian robot costs 2 ore and 19 clay. Each geode robot costs 2 ore and 12 obsidian.
Blueprint 16: Each ore robot costs 4 ore. Each clay robot costs 3 ore. Each obsidian robot costs 2 ore and 20 clay. Each geode robot costs 3 ore and 9 obsidian.
Blueprint 17: Each ore robot costs 3 ore. Each clay robot costs 4 ore. Each obsidian robot costs 2 ore and 11 clay. Each geode robot costs 2 ore and 10 obsidian.
Blueprint 18: Each ore robot costs 2 ore. Each clay robot costs 4 ore. Each obsidian robot costs 4 ore and 18 clay. Each geode robot costs 2 ore and 11 obsidian.
Blueprint 19: Each ore robot costs 4 ore. Each clay robot costs 4 ore. Each obsidian robot costs 3 ore and 14 clay. Each geode robot costs 3 ore and 8 obsidian.
Blueprint 20: Each ore robot costs 4 ore. Each clay robot costs 4 ore. Each obsidian robot costs 4 ore and 8 clay. Each geode robot costs 4 ore and 14 obsidian.
Blueprint 21: Each ore robot costs 2 ore. Each clay robot costs 4 ore. Each obsidian robot costs 3 ore and 20 clay. Each geode robot costs 2 ore and 17 obsidian.
Blueprint 22: Each ore robot costs 3 ore. Each clay robot costs 4 ore. Each obsidian robot costs 3 ore and 15 clay. Each geode robot costs 4 ore and 16 obsidian.
Blueprint 23: Each ore robot costs 4 ore. Each clay robot costs 4 ore. Each obsidian robot costs 2 ore and 17 clay. Each geode robot costs 3 ore and 11 obsidian.
Blueprint 24: Each ore robot costs 3 ore. Each clay robot costs 3 ore. Each obsidian robot costs 2 ore and 13 clay. Each geode robot costs 3 ore and 12 obsidian.
Blueprint 25: Each ore robot costs 3 ore. Each clay robot costs 3 ore. Each obsidian robot costs 3 ore and 19 clay. Each geode robot costs 3 ore and 17 obsidian.
Blueprint 26: Each ore robot costs 3 ore. Each clay robot costs 4 ore. Each obsidian robot costs 3 ore and 12 clay. Each geode robot costs 3 ore and 17 obsidian.
Blueprint 27: Each ore robot costs 3 ore. Each clay robot costs 4 ore. Each obsidian robot costs 2 ore and 15 clay. Each geode robot costs 3 ore and 7 obsidian.
Blueprint 28: Each ore robot costs 3 ore. Each clay robot costs 4 ore. Each obsidian robot costs 4 ore and 14 clay. Each geode robot costs 4 ore and 10 obsidian.
Blueprint 29: Each ore robot costs 3 ore. Each clay robot costs 3 ore. Each obsidian robot costs 2 ore and 15 clay. Each geode robot costs 3 ore and 9 obsidian.
Blueprint 30: Each ore robot costs 4 ore. Each clay robot costs 4 ore. Each obsidian robot costs 2 ore and 7 clay. Each geode robot costs 4 ore and 13 obsidian.";

    #endregion 
}

