using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avent2022;

internal class Day15
{
    public void Run()
    {
        var lines = Input.Split("\r\n");
        Part2(lines);
    }

    public void Part1(string[] lines)
    {
        var checkRow = 2000000;
        Stopwatch stopwatch = new();
        stopwatch.Start();
        var allScanners = new List<Scanner>();
        foreach (var line in lines)
        {
            var scanner = new Scanner(line);
            allScanners.Add(scanner);
        }

        var scannerPoints = allScanners.Select(x => x.Location).ToHashSet();
        var beaconPoints = allScanners.Select(x => x.ClosestBeacon).ToHashSet();

        var noBeaconPositions = new HashSet<Point>();
        foreach (var scanenr in allScanners)
        {
            var scannerPossiblePoints = scanenr.GetScanPointInRow(checkRow).ToList();
            if (scannerPossiblePoints.Any())
            {
                scannerPossiblePoints.Sort((x, y) => x.X.CompareTo(y.X));
                noBeaconPositions = noBeaconPositions.Union(scannerPossiblePoints).ToHashSet();
            }
        }

        noBeaconPositions = noBeaconPositions.Except(scannerPoints).ToHashSet();
        noBeaconPositions = noBeaconPositions.Except(beaconPoints).ToHashSet();

        stopwatch.Stop();
        Console.WriteLine($"There are {noBeaconPositions.Count} that cannot contain a beacon. Processing took {stopwatch.Elapsed}");
    }

    public void Part2(string[] lines)
    {
        var maxCoord = 4000000;
        Stopwatch stopwatch = new();
        stopwatch.Start();
        var allScanners = new List<Scanner>();
        foreach (var line in lines)
        {
            var scanner = new Scanner(line);
            allScanners.Add(scanner);
        }

        for (int y = 0; y <= maxCoord; y++)
        {
            var allRangesInRow = allScanners.Select(x => x.GetCoveredRangeInRow(y))
                .Where(x => x.End > int.MinValue)
                .ToList();
            allRangesInRow.Sort((x,y) => x.Start.CompareTo(y.Start));

            ScannerRange? rangeCombined = null;
            for (int i = 0; i < allRangesInRow.Count; i++)
            {
                var current = allRangesInRow[i];
                if (current.End < 0 || current.Start > maxCoord)
                    continue;

                if (rangeCombined == null)
                {
                    rangeCombined = new ScannerRange(0, current.End);
                }
                else if (current.Start <= rangeCombined?.End)
                {
                    rangeCombined = new ScannerRange(rangeCombined?.Start ?? 0, Math.Max(rangeCombined?.End ?? 0, current.End));
                }
                else
                {
                    // missing a spot in detection ranges
                    var x = rangeCombined?.End + 1;
                    ulong tuningFreq = (ulong)x * 4000000 + (ulong)y;
                    Console.WriteLine($"Beacon has a tuning frequency of {tuningFreq}. Processing took {stopwatch.Elapsed}.");
                    return;
                }
            }            
        }        
    }

    struct ScannerRange
    {
        public int Start;
        public int End;

        public ScannerRange(int start, int end)
        {
            Start = start;
            End = end;
        }

        public override string ToString()
        {
            return $"{Start}-{End}";
        }
    }

    class Scanner
    {
        public Point Location { get; }

        public Point ClosestBeacon { get; }

        private Lazy<int> _scanDistance;
        public int ScanDistance => _scanDistance.Value;

        public Scanner(string line)
        {
            _scanDistance = new Lazy<int>(() => Math.Abs(Location.X - ClosestBeacon.X) + Math.Abs(Location.Y - ClosestBeacon.Y));

            var mainParts = line.Split(": closest beacon is at ");
            var sensorXy = mainParts[0].Substring("Sensor at ".Length).Split(',');

            var scannerX = int.Parse(sensorXy[0].Trim().Substring(2));
            var scannerY = int.Parse(sensorXy[1].Trim().Substring(2));
            Location = new Point(scannerX, scannerY);

            var beaconXy = mainParts[1].Split(',');
            var beaconX = int.Parse(beaconXy[0].Trim().Substring(2));
            var beaconY = int.Parse(beaconXy[1].Trim().Substring(2));
            ClosestBeacon = new Point(beaconX, beaconY);
        }

        public HashSet<Point> GetScanPointInRow(int row)
        {
            var points = new HashSet<Point>();
            var xOffset = ScanDistance - (Math.Abs(row - Location.Y));
            for (int x = 0; x <= xOffset; x++)
            {
                points.Add(new Point(Location.X + x, row));
                points.Add(new Point(Location.X - x, row));
            }

            return points;
        }

        public ScannerRange GetCoveredRangeInRow(int row)
        {
            var xOffset = ScanDistance - (Math.Abs(row - Location.Y));
            if (xOffset < 0)
            {
                return new ScannerRange(int.MinValue, int.MinValue);
            }
            return new ScannerRange(Location.X - xOffset, Location.X + xOffset);
        }

    }


    #region TestInput

    public string TestInput =
        @"Sensor at x=2, y=18: closest beacon is at x=-2, y=15
Sensor at x=9, y=16: closest beacon is at x=10, y=16
Sensor at x=13, y=2: closest beacon is at x=15, y=3
Sensor at x=12, y=14: closest beacon is at x=10, y=16
Sensor at x=10, y=20: closest beacon is at x=10, y=16
Sensor at x=14, y=17: closest beacon is at x=10, y=16
Sensor at x=8, y=7: closest beacon is at x=2, y=10
Sensor at x=2, y=0: closest beacon is at x=2, y=10
Sensor at x=0, y=11: closest beacon is at x=2, y=10
Sensor at x=20, y=14: closest beacon is at x=25, y=17
Sensor at x=17, y=20: closest beacon is at x=21, y=22
Sensor at x=16, y=7: closest beacon is at x=15, y=3
Sensor at x=14, y=3: closest beacon is at x=15, y=3
Sensor at x=20, y=1: closest beacon is at x=15, y=3";

    #endregion

    #region Input

    public string Input =
        @"Sensor at x=2389280, y=2368338: closest beacon is at x=2127703, y=2732666
Sensor at x=1882900, y=3151610: closest beacon is at x=2127703, y=2732666
Sensor at x=2480353, y=3555879: closest beacon is at x=2092670, y=3609041
Sensor at x=93539, y=965767: closest beacon is at x=501559, y=361502
Sensor at x=357769, y=2291291: closest beacon is at x=262473, y=2000000
Sensor at x=2237908, y=1893142: closest beacon is at x=2127703, y=2732666
Sensor at x=2331355, y=3906306: closest beacon is at x=2092670, y=3609041
Sensor at x=3919787, y=2021847: closest beacon is at x=2795763, y=2589706
Sensor at x=3501238, y=3327244: closest beacon is at x=3562181, y=3408594
Sensor at x=1695968, y=2581703: closest beacon is at x=2127703, y=2732666
Sensor at x=3545913, y=3356504: closest beacon is at x=3562181, y=3408594
Sensor at x=1182450, y=1405295: closest beacon is at x=262473, y=2000000
Sensor at x=3067566, y=3753120: closest beacon is at x=3562181, y=3408594
Sensor at x=1835569, y=3983183: closest beacon is at x=2092670, y=3609041
Sensor at x=127716, y=2464105: closest beacon is at x=262473, y=2000000
Sensor at x=3065608, y=3010074: closest beacon is at x=2795763, y=2589706
Sensor at x=2690430, y=2693094: closest beacon is at x=2795763, y=2589706
Sensor at x=2051508, y=3785175: closest beacon is at x=2092670, y=3609041
Sensor at x=2377394, y=3043562: closest beacon is at x=2127703, y=2732666
Sensor at x=1377653, y=37024: closest beacon is at x=501559, y=361502
Sensor at x=2758174, y=2627042: closest beacon is at x=2795763, y=2589706
Sensor at x=1968468, y=2665146: closest beacon is at x=2127703, y=2732666
Sensor at x=3993311, y=3779031: closest beacon is at x=3562181, y=3408594
Sensor at x=159792, y=1923149: closest beacon is at x=262473, y=2000000
Sensor at x=724679, y=3489022: closest beacon is at x=2092670, y=3609041
Sensor at x=720259, y=121267: closest beacon is at x=501559, y=361502
Sensor at x=6, y=46894: closest beacon is at x=501559, y=361502
Sensor at x=21501, y=2098549: closest beacon is at x=262473, y=2000000
Sensor at x=2974083, y=551886: closest beacon is at x=4271266, y=-98555";

    #endregion 
}

