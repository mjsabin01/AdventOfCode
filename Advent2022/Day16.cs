using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Avent2022.Day16;

namespace Avent2022;

internal class Day16
{
    public void Run()
    {
        Stopwatch s = new Stopwatch();
        s.Start();
        var lines = Input.Split("\r\n");
        Part1(lines);
        s.Stop();
        Console.WriteLine("Took: {0} to complete.", s.Elapsed);
    }

    public void Part1(string[] lines)
    {
        var valveDict = ParseInput(lines);
        var currentValve = valveDict["AA"];
        var cache = new Dictionary<string, long>();
        var valvesWithPressure = valveDict.Values.Where(x => x.FlowRate != 0).ToHashSet();
        FindMax(currentValve, 30, 0, new HashSet<Valve>(), valvesWithPressure, cache);
        var max = cache.Max(x => x.Value);

        Console.WriteLine($"Total presssure released is: {max}");

    }

    public void Part2(string[] lines)
    {

    }


    void FindMax(Valve current, int minRemaining, long totalRelased, HashSet<Valve> released, HashSet<Valve> valvesWithPressure, Dictionary<string, long> cache)
    {
        // keep track of max released with the specific valves. If current path has more than previous, otherwise continue
        var sortedReleased = released.ToList();
        sortedReleased.Sort((x, y) => x.Name.CompareTo(y.Name));
        var cacheKey = string.Join(',', sortedReleased);
        if (cache.ContainsKey(cacheKey) && totalRelased < cache[cacheKey])
            return;

        cache[cacheKey] = totalRelased;

        // attempt to release each valve, one at a time and find which order gives best result
        var remainingValves = valvesWithPressure.Except(released);
        foreach (var valve in remainingValves)
        {
            
            var pathToValve = PathCache.GetPath(current, valve);
            var minToOpen = pathToValve.Count + 1;
            if (minRemaining > minToOpen)
            {
                var pressureReleased = (minRemaining - minToOpen) * valve.FlowRate;
                released.Add(valve);
                FindMax(valve, minRemaining - minToOpen, totalRelased + pressureReleased, released, valvesWithPressure, cache);
                released.Remove(valve);
            }
        }
    }



    Dictionary<string, Valve> ParseInput(string[] lines)
    {
        Regex r = new Regex(@"Valve (?<ValveName>\w+) has flow rate=(?<FlowRate>\d+); tunnels? leads? to valves? (?<ConnectedValves>.*)$");
        Dictionary<string, Valve> valves = new();
        foreach (string line in lines)
        {
            var m = r.Match(line);
            var valveName = m.Groups["ValveName"].Value;
            var flowRate = long.Parse(m.Groups["FlowRate"].Value);
            var connected = m.Groups["ConnectedValves"].Value.Split(", ");

            var valve = valves.GetOrAdd(valveName, new Valve(valveName));
            valve.FlowRate = flowRate;
            foreach(var cValve in connected)
            {
                valve.ConnectedValves.Add(valves.GetOrAdd(cValve, new Valve(cValve)));
            }
        }

        return valves;
    }

    public static class PathCache
    {
        private static Dictionary<string, Dictionary<string, List<Valve>>> pathCache = new();

        public static List<Valve> GetPath(Valve source, Valve target)
        {
            if (source == target)
                return new List<Valve>();

            var sourceCache = pathCache.GetOrAdd(source.Name, () => new());
            if (sourceCache.ContainsKey(target.Name))
            {
                return sourceCache[target.Name];
            }

            var targetCache = pathCache.GetOrAdd(target.Name, () => new());

            // get optimal path to the target
            var path = DiscoverPath(source, target);

            // reverese the path so we cache the reverse way 
            var reverse = new List<Valve>();
            reverse.AddRange(path);
            reverse.Reverse();

            path.RemoveAt(0);
            reverse.RemoveAt(0);

            sourceCache[target.Name] = path;
            targetCache[source.Name] = reverse;


            return path;
        }

        private static List<Valve> DiscoverPath(Valve source, Valve target)
        {
            var visited = new HashSet<Valve>();

            var pq = new PriorityQueue<List<Valve>, int>();
            var path = new List<Valve>();
            path.Add(source);
            pq.Enqueue(path, 0);

            while (true)
            {
                var currentPath = pq.Dequeue();
                var current = currentPath.Last();
                if (visited.Contains(current))
                    continue;
                visited.Add(current);

                foreach (var neighbor in current.ConnectedValves)
                {
                    List<Valve> neighborPath = new();
                    neighborPath.AddRange(currentPath);
                    neighborPath.Add(neighbor);

                    if (neighbor == target)
                    {
                        return neighborPath;
                    }

                    pq.Enqueue(neighborPath, neighborPath.Count);
                }
            }
        }

    }

    public class Valve
    {
        public Valve(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }
        public long FlowRate { get; set; }
        public HashSet<Valve> ConnectedValves { get; set; } = new HashSet<Valve>();
        public bool IsOpen { get; set; }

        public override string ToString()
        {
            var valveNameSet = ConnectedValves.Select(x => x.Name);
            var valveNames = string.Join(",", valveNameSet);
            return $"{Name} with flow rate: {FlowRate} connects to valves: {valveNames}";
        }
    }

    #region TestInput

    public string TestInput =
        @"Valve AA has flow rate=0; tunnels lead to valves DD, II, BB
Valve BB has flow rate=13; tunnels lead to valves CC, AA
Valve CC has flow rate=2; tunnels lead to valves DD, BB
Valve DD has flow rate=20; tunnels lead to valves CC, AA, EE
Valve EE has flow rate=3; tunnels lead to valves FF, DD
Valve FF has flow rate=0; tunnels lead to valves EE, GG
Valve GG has flow rate=0; tunnels lead to valves FF, HH
Valve HH has flow rate=22; tunnel leads to valve GG
Valve II has flow rate=0; tunnels lead to valves AA, JJ
Valve JJ has flow rate=21; tunnel leads to valve II";

    #endregion

    #region Input

    public string Input =
        @"Valve EV has flow rate=0; tunnels lead to valves WG, IB
Valve IB has flow rate=0; tunnels lead to valves EW, EV
Valve KL has flow rate=0; tunnels lead to valves JH, OY
Valve QJ has flow rate=0; tunnels lead to valves TX, JH
Valve OA has flow rate=12; tunnels lead to valves SB, GI, ED
Valve BQ has flow rate=0; tunnels lead to valves NK, JJ
Valve PZ has flow rate=0; tunnels lead to valves JH, VA
Valve QO has flow rate=8; tunnels lead to valves LN, LU, CU, SQ, YZ
Valve MP has flow rate=0; tunnels lead to valves LN, GO
Valve YZ has flow rate=0; tunnels lead to valves AA, QO
Valve CU has flow rate=0; tunnels lead to valves RY, QO
Valve UE has flow rate=16; tunnel leads to valve VP
Valve HT has flow rate=0; tunnels lead to valves AA, JE
Valve EF has flow rate=0; tunnels lead to valves ES, JE
Valve JJ has flow rate=15; tunnel leads to valve BQ
Valve JX has flow rate=0; tunnels lead to valves AA, GO
Valve AA has flow rate=0; tunnels lead to valves JX, TX, HT, YZ
Valve MI has flow rate=21; tunnels lead to valves PQ, QT
Valve ES has flow rate=0; tunnels lead to valves EF, NK
Valve VC has flow rate=0; tunnels lead to valves MC, IW
Valve LN has flow rate=0; tunnels lead to valves MP, QO
Valve ED has flow rate=0; tunnels lead to valves OA, RY
Valve WG has flow rate=20; tunnels lead to valves EV, OY, KF
Valve GI has flow rate=0; tunnels lead to valves WE, OA
Valve UK has flow rate=0; tunnels lead to valves TO, JE
Valve GY has flow rate=23; tunnels lead to valves EO, QT
Valve TX has flow rate=0; tunnels lead to valves AA, QJ
Valve OE has flow rate=0; tunnels lead to valves GO, NK
Valve OQ has flow rate=9; tunnels lead to valves VP, SB
Valve NK has flow rate=25; tunnels lead to valves OE, ES, BQ
Valve LU has flow rate=0; tunnels lead to valves JH, QO
Valve RY has flow rate=18; tunnels lead to valves ED, IW, CU
Valve KF has flow rate=0; tunnels lead to valves JE, WG
Valve IW has flow rate=0; tunnels lead to valves VC, RY
Valve SQ has flow rate=0; tunnels lead to valves MC, QO
Valve PQ has flow rate=0; tunnels lead to valves MC, MI
Valve TO has flow rate=0; tunnels lead to valves UK, JH
Valve OY has flow rate=0; tunnels lead to valves KL, WG
Valve JE has flow rate=10; tunnels lead to valves EF, ND, HT, KF, UK
Valve JH has flow rate=3; tunnels lead to valves QJ, KL, PZ, TO, LU
Valve VP has flow rate=0; tunnels lead to valves OQ, UE
Valve EW has flow rate=22; tunnel leads to valve IB
Valve ND has flow rate=0; tunnels lead to valves JE, GO
Valve VA has flow rate=0; tunnels lead to valves GO, PZ
Valve QT has flow rate=0; tunnels lead to valves MI, GY
Valve EO has flow rate=0; tunnels lead to valves GY, MC
Valve MC has flow rate=11; tunnels lead to valves PQ, SQ, WE, EO, VC
Valve GO has flow rate=4; tunnels lead to valves JX, VA, OE, MP, ND
Valve SB has flow rate=0; tunnels lead to valves OQ, OA
Valve WE has flow rate=0; tunnels lead to valves MC, GI";

    #endregion 
}

