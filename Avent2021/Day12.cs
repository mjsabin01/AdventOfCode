using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avent2021
{
    internal class Day12
    {
        public void Run()
        {
            var lines = Input.Split("\r\n");
            Part2(lines);
        }

        public void Part1(string[] lines)
        {
            var startNode = Cave.BuildCaveGraph(lines);
            var completedPaths = new List<string>();

            Stack<Cave> nodes = new Stack<Cave>();
            nodes.Push(startNode);

            var currentPath = new List<Cave>();

            var currentPathSmallCaves = new HashSet<string>();
            VisitCave(startNode, currentPath, currentPathSmallCaves, false, null, completedPaths);

            Console.WriteLine("Completed paths:");
            foreach (var path in completedPaths)
            {
                Console.WriteLine(path);
            }
            Console.WriteLine($"Total number of paths: {completedPaths.Count}");
            
        }

        public void Part2(string[] lines)
        {
            var startNode = Cave.BuildCaveGraph(lines);
            var completedPaths = new List<string>();

            Stack<Cave> nodes = new Stack<Cave>();
            nodes.Push(startNode);

            var currentPath = new List<Cave>();

            var currentPathSmallCaves = new HashSet<string>();
            VisitCave(startNode, currentPath, currentPathSmallCaves, true, null, completedPaths);

            Console.WriteLine("Completed paths:");
            foreach (var path in completedPaths)
            {
                //Console.WriteLine(path);
            }
            Console.WriteLine($"Total number of paths: {completedPaths.Count}");
        }

        void VisitCave(Cave current, List<Cave> currentPath, HashSet<string> currentPathSmallCaves, bool visitSmallCaveTwiceSupported, Cave? secondSmallCave, List<string> completedPaths)
        {
            currentPath.Add(current);
            if (string.Equals("end", current.Name))
            {
                completedPaths.Add(string.Join(",", currentPath.Select(x => x.Name)));
                currentPath.RemoveAt(currentPath.Count - 1);
                return;
            }

            if (current.IsSmallCave)
            {
                if (currentPathSmallCaves.Contains(current.Name))
                {
                    if (visitSmallCaveTwiceSupported && secondSmallCave == null)
                    {
                        secondSmallCave = current;
                    }
                    else
                    {
                        // already seen this small cave, and we can't visit it another time, so return and remove current from the path
                        currentPath.RemoveAt(currentPath.Count - 1);
                        return;
                    }
                }
                else
                {
                    currentPathSmallCaves.Add(current.Name);
                }
            }

            foreach (var node in current.ConnectedNodes.Where(x => !string.Equals(x.Name, "start")))
            {
                VisitCave(node, currentPath, currentPathSmallCaves, visitSmallCaveTwiceSupported, secondSmallCave, completedPaths);
            }

            if (current.IsSmallCave && secondSmallCave != current)
            {
                currentPathSmallCaves.Remove(current.Name);
            }
            currentPath.RemoveAt(currentPath.Count - 1);

        }

        class Cave
        {
            public string Name { get; }

            public HashSet<Cave> ConnectedNodes { get; } = new HashSet<Cave>();

            public Cave(string name)
            {
                Name = name;                
            }

            public bool IsSmallCave => Name != "start" && Name != "end" && string.Equals(Name, Name.ToLower());

            public static Cave BuildCaveGraph(string[] lines)
            {
                ConcurrentDictionary<string, Cave> nodes = new ConcurrentDictionary<string, Cave>();
                foreach (var line in lines)
                {
                    var parts = line.Split('-');
                    var nodeA = nodes.GetOrAdd(parts[0], name => new Cave(name));
                    var nodeB = nodes.GetOrAdd(parts[1], name => new Cave(name));
                    nodeA.ConnectedNodes.Add(nodeB);
                    nodeB.ConnectedNodes.Add(nodeA);
                }

                return nodes["start"];
            }
        }

        #region TestInput

        public string TestInput =
            @"dc-end
HN-start
start-kj
dc-start
dc-HN
LN-dc
HN-end
kj-sa
kj-HN
kj-dc";

        #endregion

        #region Input

        public string Input =
            @"hl-WP
vl-fo
vl-WW
WP-start
vl-QW
fo-wy
WW-dz
dz-hl
fo-end
VH-fo
ps-vl
FN-dz
WP-ps
ps-start
WW-hl
end-QW
start-vl
WP-fo
end-FN
hl-QW
WP-dz
QW-fo
QW-dz
ps-dz";

        #endregion 
    }
}
