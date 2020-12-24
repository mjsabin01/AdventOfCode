using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Drawing;

namespace Avent2020
{
    public class Day24
    {
        public void Run()
        {
            string input = Input;
            var flippedOrig = Part1(input);
            Part2(flippedOrig, 100);
        }

        Regex pathRegex = new Regex(@"(e|se|sw|w|nw|ne)+");

        public HashSet<Point> Part1(string input)
        {
            var lines = input.Split("\r\n");

            var flippedTiles = new HashSet<Point>();
            
            foreach (var line in lines)
            {
                var offset = new Point(0, 0);
                var match = pathRegex.Match(line);
                for (int i = 0; i < match.Groups[1].Captures.Count; i++)
                {
                    var direction = match.Groups[1].Captures[i].Value;
                    switch (direction)
                    {
                        case "e":
                            offset.X += 2;
                            break;
                        case "se":
                            offset.X++;
                            offset.Y--;
                            break;
                        case "sw":
                            offset.X--;
                            offset.Y--;
                            break;
                        case "w":
                            offset.X -= 2;
                            break;
                        case "nw":
                            offset.X--;
                            offset.Y++;
                            break;
                        case "ne":
                            offset.X++;
                            offset.Y++;
                            break;
                    }
                }
                //Console.WriteLine($"Flipping tile at offset X: {offset.X} Y: {offset.Y}");
                if (flippedTiles.Contains(offset))
                {
                    flippedTiles.Remove(offset);
                }
                else
                {
                    flippedTiles.Add(offset);
                }
            }

            Console.WriteLine($"Total flipped tiles with initial pattern: {flippedTiles.Count}");
            return flippedTiles;
        }

        public void Part2(HashSet<Point> currentBlack, int numDays)
        {
            for (int i = 1; i <= numDays; i++)
            {
                var nextDayBlack = new HashSet<Point>();
                var checkedPoints = new HashSet<Point>();
                foreach (var point in currentBlack)
                {
                    var newPointAndNeighborBlack = CheckPointAndNeighbors(currentBlack, point, checkedPoints);
                    foreach (var newBlackPoint in newPointAndNeighborBlack)
                    {
                        nextDayBlack.Add(newBlackPoint);
                    }
                }

                var same = currentBlack.Intersect(nextDayBlack).ToList();
                currentBlack = nextDayBlack;
                var list = currentBlack.ToList();
                list.Sort((x, y) => x.X == y.X ? x.Y.CompareTo(y.Y) :  x.X.CompareTo(y.X));
                Console.WriteLine($"Day {i}: {currentBlack.Count}");
            }
        }

        public HashSet<Point> CheckPointAndNeighbors(HashSet<Point> currentBlack, Point checkPoint, HashSet<Point> checkedPoints)
        {
            var nextRoundBlack = new HashSet<Point>();
            var checkPoints = new List<Point>()
            {
                new Point(checkPoint.X - 1, checkPoint.Y + 1),
                new Point(checkPoint.X - 1, checkPoint.Y - 1),
                new Point(checkPoint.X, checkPoint.Y),
                new Point(checkPoint.X + 2, checkPoint.Y),
                new Point(checkPoint.X - 2, checkPoint.Y),
                new Point(checkPoint.X + 1, checkPoint.Y + 1),
                new Point(checkPoint.X + 1, checkPoint.Y - 1),
            };

            foreach (var point in checkPoints)
            {
                if (checkedPoints.Contains(point))
                    continue;
                checkedPoints.Add(point);

                if (CheckPoint(currentBlack, point))
                {
                    nextRoundBlack.Add(point);
                }
            }

            return nextRoundBlack;
        }


        public bool CheckPoint(HashSet<Point> currentBlack, Point checkPoint)
        {
            var blackNeighbors = 0;
            var checkPoints = new List<Point>()
            {
                new Point(checkPoint.X - 1, checkPoint.Y + 1),
                new Point(checkPoint.X - 1, checkPoint.Y - 1),
                new Point(checkPoint.X + 2, checkPoint.Y),
                new Point(checkPoint.X - 2, checkPoint.Y),
                new Point(checkPoint.X + 1, checkPoint.Y + 1),
                new Point(checkPoint.X + 1, checkPoint.Y - 1)
            };
            foreach (var point in checkPoints)
            {
                if (currentBlack.Contains(point))
                {
                    blackNeighbors++;
                }
            }

            bool isPointBlackNextRound;
            var isCurrentBlack = currentBlack.Contains(checkPoint);
            if (isCurrentBlack)
            {
                // Any black tile with zero or more than 2 black tiles immediately adjacent to it is flipped to white
                isPointBlackNextRound = (blackNeighbors == 1 || blackNeighbors == 2);
            }
            else
            {
                // Any white tile with exactly 2 black tiles immediately adjacent to it is flipped to black
                isPointBlackNextRound = (blackNeighbors == 2);
            }
            return isPointBlackNextRound;
            
        }

        string TestInput = @"sesenwnenenewseeswwswswwnenewsewsw
neeenesenwnwwswnenewnwwsewnenwseswesw
seswneswswsenwwnwse
nwnwneseeswswnenewneswwnewseswneseene
swweswneswnenwsewnwneneseenw
eesenwseswswnenwswnwnwsewwnwsene
sewnenenenesenwsewnenwwwse
wenwwweseeeweswwwnwwe
wsweesenenewnwwnwsenewsenwwsesesenwne
neeswseenwwswnwswswnw
nenwswwsewswnenenewsenwsenwnesesenew
enewnwewneswsewnwswenweswnenwsenwsw
sweneswneswneneenwnewenewwneswswnese
swwesenesewenwneswnwwneseswwne
enesenwswwswneneswsenwnewswseenwsese
wnwnesenesenenwwnenwsewesewsesesew
nenewswnwewswnenesenwnesewesw
eneswnwswnwsenenwnwnwwseeswneewsenese
neswnwewnwnwseenwseesewsenwsweewe
wseweeenwnesenwwwswnew";
        string Input = @"neneeenenesweneswneesewenenwneew
eswswenweeseneseswwsenewsenw
seeweswwneswnwwwenwwwwwnenwwsesw
swsweseseseswswsenwswseswswsese
eseseseswsenweseseneseswnwneeewww
sweeeeeenweseenesweee
nwnwsewenwwnwnwsenwwnwseswnwnw
swswswwwswweswswswswswenenesesewwwe
swnwenenesenwsewswswseeeswnwwnewwsese
senwweewnwweswnenwwsw
ewneswswsweenwwnwsenwswnewsenwnwsw
nenenwwneenwnwnwwnesewsenenwnwnenwnw
nwnwwsenwwnwnwnwnwwnwnwewwnw
wnwnwnwnwseswwnwnwesewsenwnwnwnwnenew
seswewwneweswnwnwswwsw
neswesenenenewwswnenwnenenenwnenenenese
nwwswsenwnenwnesenwnwsenwnwnwwnwsenenwse
senwswwswwnweswswneswswwswnweswseswew
neeeeswnweswseeeeeneeeeenwwnene
nwnewnwnwnwswnewwsewnwwseenenwnww
eeseeeeeswneenwewee
neseseseeenwwseseeseneswnenwsewswse
wswswnwnwwswswneswwweeswswswwww
swnwnwneswswseswseswswswswswnwseswsesenesw
swneesenwnwnwwnwnwnenwnwnwnwnewnwsese
wswswswewnewnewsenewnwswseswswwse
swenwnwnwnenwnenwnwswnenenwswwnenwsenw
ewnenwnwweenwneesewnwwwwnwswse
eeewnweneneeesweesweeeneeee
nenwnwnwnwswwnwsewnwswswenwweswenw
senenenenenenwneneewnenenene
neneeneswnesewneneneeneneeenenenwe
nwswenwseswnwswenwnwnewnwneww
wnewwwweswwsww
seseswsewesesenesesewseewseseenenwe
neneseswnesenewwenenwneneneneneenew
swswswswwsweswseswsewwneswswswswneswswsw
sesenesesesenwsesesesenwsewnw
nenenenenenewnwswnwnenwsenesenwneswwne
swwnewweewwwwnwswnewwweswew
swnwwswesenewwwswwswwweseswswnesw
weeseseseneseseewwseseseseseseesese
nwenenenwwnenenenenwneneneseswnenwenesw
nwsewswswwswsweswnwwwnwswswsew
nwewnwwwewnwwnwnwwnwswwnwwsewnw
eesesesesesesewseseewneseesesesesenw
nwwenwweswnwswenwnw
sewseswswneneswnwenwneeswwnwwesenw
eseswwnwnwwenewneswnwsweenwseswne
wsewwswnwnwewnewswwwwswseswnew
eswnenwnwnenwswnenwnwnwnenwnwnwnenwsewne
neneewswnenenenenene
nwseeseseesesesesesesese
eenweneeeswswseeseeneesese
sewneeneneswseeewnwswsenewswenwwenw
nwwwweswnenwsenwwswnwwnwwwnwwnew
nwewwneswnwwwnwswswsesweswswwseww
neseswseweseeswsenwsesesesesw
nwswswseswswseeswswseswnewneseswswwsw
eweseeswseeeeseneenwenweeee
nesewwwwneeseweswwseneeseseswseswsw
nwswnwnewnewneeenwneeswwnwse
swwnwsesewneneeneenwseswseswnww
nwnwnwnwsenwnwnwnwnwnwnwnw
wwnewnwsesewwwwwwnwseewsenenew
swswswswenwswseswsweeswnwswswswswswswswnw
nwnwseeswnwnwswnenwwnwnewnwnwwnwnw
wnenenesesenwnwwnwsenenesewnewnenee
senwswnwnwwenenwnenwnenesewswenewnene
neseseeseneswswswswswwwswswnwswwsenesw
neswwsesenwnwnwneswseseseseswnesenwsese
eeweesenesweenweeneenwneeswse
wwnewswsenwnwsesenwswseneeswswseswnese
wnwsewnwnwnenwnwnwnwwnwnwwww
wwswswewwwwwwswwwwnewnwnenw
eeneseswswnewneneneenenenenenenenew
wswwnwswwswneneneswnwnwnwswseneneswsenene
nwswswewswsewswsewswsenweswswneswnwsw
wwwwesewnwnewwwnewwwwsese
nwnesenwnwnwnwswswnwwswwnwnwwwwneeenw
swseseswenwswswnwswwnenesenesesesesesese
nesewswswswsewswswnwswwswwswsw
wnwwwnwnwnwnwsewsenwsewwwwwww
wwwwwwwwswwswwwewwnw
swseseseseeswsesesenwseseswseswwsenwse
sweeeseeswnwenwneeenwneswenwneee
nwswseswswswswwsweswnesesweswswswseswse
nwnwenwnwnwnwnwswnwwnwnwwnenw
wseewnwnwnwenewnenenesenewnenwnenwne
nwsweswswswswswswswswsw
swswwswwswswswswnenewenwewweswnw
nwseeswnwnwseseseswneseswseseseenewsw
wseswseseesenweeeseeeewnwenene
newnenenwswnwnenenwneneseneneneenenesenw
wnwnwswseenwnwnwnwenwwwnwnenwnwswne
esesesesewneseswswsewswseswswneswsesesw
wsewwwnewswwnenenwnwwnwwnwnwwwsew
wsenwenwnwnwewwsesenwwwwnwnwnwnew
nwnwnenesenenenwnenewnenenwneneswswnene
eseneneewneneneeneeneneee
nenenenwnwseswswnwwnwnwwesenwsenewswne
neenwnwnenenwnwnwwnwnenwnwnwwnwswsenw
swwwswwswswwnwwwnenwseswswwseswsenw
nwsewneswnwesweeswnwwwnweswnwne
neswsewswseswseswswswneswseswneseseswsww
wsewwwnwnenwnwewwseenwwwwswnw
neseseswseswwswswswnenwsesewswswswswsw
swwsweswneseswswwswswswswweswwsw
nenwneneswneeneenene
eneweneseneeneeswswswenenesenenew
swseeseswswesenwsesenwswnwnwseeesenwswsw
nenenwnenwwwnenenweneneneseneswnenenene
neneenenwswswseswwwnweswnenesesewnewse
neneseneneseneenenenewswneeenenewnene
swsweesenesenenenwnwnwswsweneseenww
swnewwnwsewseswewsewsesenwneewnww
enenewnenenenwnenenenw
seseesesewwseseseseseswswseesesenese
eeenwneeeewneneeeeewswneneee
eswnwenweesenwseeswsweenwsenwnwesw
eesweeeneneeneeeeewnweeewe
seswswseseswswswsene
newswnewswwwneswwwse
neeswnenwewnwnesewsweneeswnwenesww
eswwwwwwswnesewwwnewwnwwww
ewsesenwseswsesenwsesesewesenwsenenese
nenwnewswneneneneeeneneseneneneenee
wwseseeeenwnwweseneewnweeeewsw
nwneswnwnwnwwwnwseenwnwnwenenwnw
swswswswswneswenwwweswswsw
swenwwwnwnewswnenwneswnwesweswwe
neswseenwseseeenwsesesew
seseeesesenwnwseeseseseswnesese
sweeeseseenwseeweesee
swneswsweswnewwswseswswsw
nwwwewsewwwwwwwwewswswnewsw
nenenewneswsenenenenesenenenenenenenwneenw
sewwwnesewswwnwwwwswewswwswew
swswseswnwnwswswswswswswnwseswenwseswswsw
nwnwnwnwnwnwnwnwnenwnwsenw
nenwwewnwwsewnewwnwsewwwewseswe
seseseswweewnwneneseseseesewsesesw
swswesenewswswswwewswsesweswsw
seseneneswwsewneeenwenenwseenwsee
nenwenwwneewnwneswnwnenwseswnwenwnwnw
sweswswswseswswswseseswseswwnesesesenw
nweeesweeeeneseeseeenwenwwe
sweewneneeeseeewswneeenwneenene
swsewseseenwsenesewseseseswswseswnese
wwwswseseswwnwwnwnenenwwswnwsenesene
seswswswseeseswseseseswnwsenwseswswsesene
nenwesenwswneneseeewsesesweeseeswse
nwseswswnwseeswnwswsenwnwnenwnenwnwnenwnw
neswnenwweesenwneswwneneswnwenenesw
nwneswnesesenwswenwsesewewsweewsw
neseeenenenewnenenweswew
eenweesweewneneseneeswee
weesenwsweenenenwswenenwneswenee
nwswswsweseswwsweswswswswnwswswswswsw
esweweseeeeseneeenweeeeee
swneswswswwswnenwewswenwswswwnew
eesweswnwseeeeeeeeneseeenwsee
senwwswnwnwnwnwnwnenwnwnwnwnwwnenenwnese
swwseeswswnwnenesewnenwsenweeeene
enenwnesenenenenwnenewneneneewswswwne
nwwneseseeseeseeseseeseseeseswnwsese
sewwswsewnwwswnwnwnewenweneswesw
eneeneeneeneewneeeeeeenwsesw
wwenwwnesenwnwnwwww
nwneneswnesenenenenwnwnenenenwnene
sweneseseswewnwswwneneneneseneesenwnw
nwewenwnwwwnwnenwsesenwnenesenwnwwwnw
nwenwnwswsenwnwnwnenwnwnwnwnwwesenenw
swswwnewweweswswnwswsenesewwwnww
nenewneswnenenweenenenwswsenene
swswswwswswswswswswswswswne
eeswswenwswneenwenweseenwnweeese
swnwnwnwwwsewnwnenwwwnww
wswwwenenwwwweswseswnwswweswsw
enewneneweseeswswenewnenwwnesene
swsenesweswswswswwswswswseseseswwsw
nwwseswnwsenwsenenwwnwnwneewwwwww
wwsesesenwnwnenesenwenenwswneneenesw
seseneweseeswseseneeseseesenwesese
nwnwwswwwweenwwnwwswsewwneww
eneneneeneswnenweneswenewneeseenenew
eseewneesenewesenwsewewwwsee
nwnwnwenenenwenwnwnwsenwnwnwswnewnenwne
neseeeesewneseseweeseseswseseesese
newwnwswwewneswseewsesweswneswwse
seseseseseeseenwseeenwsesesesese
eseswnesewseswswswsenwwswseswneswsee
eeseseseneeeeeseeeseeew
nwesweeneenwnwseneeeneeeneneneswse
swswswswseseswnwneswswseswsewse
senwnweswwnwwnwweseenwswsenwnwwse
wneneneenenesenewnwswswenwnene
eswseswneseseneseseeeeeewsenenwnew
sewseeeeenwseese
sesenwnwseseseswseseswswswseesenesesesenwse
nwwneewseswneeeneneneseenenwewneese
wwwswwnewwneswwwwwsewwwsw
seswwwwneswneswwneswswswswswseww
eewneenwsweseseweeeeneneneneesw
swswsewswswwswswnewwswsww
nwnwwswnenwwnwnenwswnwnwwsenwseeswwnw
swswsenwnewseeswseseneneswsewnwsesesenwsw
senwnwnwnwswneswnwenwnwwnesweswswnwnenw
neenenewswnwswnwsewwewswsenwnwwsw
senwnewnewnwwnwwnwseseswwnw
seeseseeneswneeswnenwwswswwswnwswwe
seswenweeneeneneneeneneneee
nwwnwwenwwwwwwwnw
swswswswwswwswwswsenenwsewnewneswswne
newswnwnwswnwenwwnenwnwnwwwwsewnwnw
eneewenweeeweeseenwseneeeswse
seseseswenwseseweseseenwnee
wnwwnenwswnwnwweeeswsw
senesesenwswseseseenwseswseswswseswsesw
seseseseseseseseseseseseenwsese
seenwnwnwswsewweeweseeneesewswene
wwwseswsesenwnwnenenwse
nwwewnwnwwneswnwwnwnwnwwnwewswne
neewneneseneneswnene
wswwswnwwwswwwewswswsww
wswwneswwswnwwnwnewseswsewswenwsw
ewnwwwwswwnwesweswswwsw
wswnwnwwnenwnewwsesewwseenewswenew
wswwnenwwswnenenwnenenenenweswseseswe
nesesenwswsweeswewwswneeneeeee
nwswwwnewwwwwwneewnwsewsewsew
nwwweswwswwwnwneswnwswenwnwneew
nwnwswnwswenenenwnwwnwnenwnenenweew
neswnwneseeneneenewswwnwnweseneene
nwswswwwwseeswnwswwsweswwnwseswww
neneseneswsesewenwseswneneswnenwwenew
neneneeneneneseneeneseneww
sweenenwenweeenesweeeenwesee
seseseseseseneesesesesesesesew
senesweewneeswenwseswseseeeesee
nesesenwwswnwnwwnewsenenwswnwnenwsee
neseewnwenenwwnewnenwnwsenwnwswswnwnw
eswnenenwswnewsweseswseewsesenwsee
enweeswenwewsweeeneenweeseesee
senwswswwswwswswwswswswswseswswswneswne
sweswwnwewseneeeenenewenweee
newnenwnwswwseseseesenwsese
enweeswseenwneeenenesweeneswenewsw
nwnwnwnwnwswenwnwnwnwnwnweswnwnwnwnwnw
nenenenweneswnesenwneswneneneneeneese
seseeenwswnweeeewswseeeswwnwsenw
esenwsewseseesewneseeseseseeeenese
seewnwsesesenwesesesesewneeeseee
eneseneewneneseweeneneeeneeee
nwwnwenwswsenwnwnwnwnwwnwswwnwnwnwnew
wnewwwwwwnwwwswnwww
wnwenwsenwswnwnwnwwnenwnwnw
eeneneswnwwneneswnwneeeneneneneewne
wsewnwwwwewwwwwwwsenwwnwwe
wwwseeseswsewsewewneswnwsewnenenw
seseneswsewneswswseswseswswswseneswsesesw
nwnwnwsenenweeneneeneswswneswneseeesw
eswseswwnwswseswswswswneenwswseswsese
eenewseneeeseneneeswwneeeenenee
nweeeeseeeseeseeswenwnenweswee
swswwneneneswenenwsenenenenenwnenwnwe
nwenwnwnwseswnwnwnwnwnwnwsenwnwnwnwnwnwnw
eswnwnweswsweseseswwswswnwseneswnenwswe
wnesewneswwneswwwwnwwwwsese
nwnenwnenenenenenenwneswnenwnenw
neswenenwswneneswnwee
wwnwwwwewwnwswnesewww
esenwnwswenenwnewswneswwenwswwnwswnww
swswwswnenwwswswnwseswswswwswswewsesw
nenwsenewnwswswneeswnwnenwnwnweenwnwne
neswwnwwwnwwwnenenwnwswwnwsenwnww
neneeneswenenenenenenee
swseweseneeeswwnenwswnwnwe
eewswweneweeneneeenenenenenenene
nwnwneneneneseneseswnenwnenwnwnenwnenene
enwnwenenwswswswnwsenwswswseswseenwe
eneneneneeeswneneenenenene
enwnenwwwsenwwwswneswnwwenwewww
neneneswnenewneneneeenwnenwnesewnene
seswsenwsesesenwewseswseseseseswsesenenwse
neneenewsesewneweeewseseenwneeene
nwwswwwsesenwwwsenewwnwnew
seseenesesesesesesesewesesee
nwwnwswswswseswwswseneswswswswswswswswe
wswwnwnwnwnwnwnwwnwnenwnwenwnwenwnw
seseswseneeswneswnewwswwwnwneesenene
nenwnwnwnenwnwnwnenwsenwnwnwnw
eswwwwwsewwnewswswwwswswneswww
esenwnwnweeewswneneswnwswnwswnenwnwnene
nenweseenwneenenwseeeneseene
newnwwsenwnwenwwwnwsewswnwnwenwww
newenenenenenenwenwswswswsene
nwsweswnwwnwewewwwenwwwwwnwwnw
sewnwsewwnwsenenwwww
swswnenwnwnewneswnewnweswsweswenwsenwse
wsweseeswnwseenewseneenee
eeeeeseeswnweeeenwseseewsese
sesesesenewsesesewesesesenwseseseene
sesewswseswsewseseseswweseeseseenese
esenwswwseseseseesesesenweseseseswe
nwnenenwnenwnwnenenesw
nwnenweneenwnenwnwnwswnenwneneswnenwnw
sewswswswswswnwswneeseswswswsw
swwswswswwwwneswneswwswswswnweswsw
neseswswesesewswwswswseswswnwseswnenese
nwswnwneewneweeseeseewewneneew
swseenenwwsenwnwnewneenwwewnwswe
swsweseswswswwwwswswswnewswsw
swnwwneneswswnwewwnwsenwenwnenwee
ewswseseneesewseneswsenwswseseswsese
eneeneeseswenwsewneeweenwnwese
swnwswswseswswswseseeswnwswswsweswnwsww
wwnwnwswnwsewwnwwnwwnwwnwsenwnee
eeseeeswneseneseeweneweseee
senesenwsesenwwsesesesesesesesesesesese
nwnewesewneswneeeeneeneeswnesenene
wesenwseswsweweenwnenwwnesese
nenwnwneeswneneeneneneswneneswneneenwnee
nwnwwsenwwnwnwwnwsenwnwenwnwnwwnwne
nwnwsenwnwsenwnwnwnenwnwwnesenwnwenwne
sewswweneesenenwneseswseswnweeeene
nwswnwnwnwnwwsenwnwwnwnwenwnwesenwnw
wwwwwwnenwwwwnesewwwse
swnenenenenenenwneneswnwneesenwnenewne
neneneswneneeneneneene
wswneseswswnewwnewswwwwwwwswsww
neeneseewnenwnwneseewneenenwwnenesww
enwseswseesesewnesweeeeneswsesenese
nwnwswnenenenwnwnwnenene
wswswswneswneswwswswsweseswswseswseswsw
sewseseesesweseneseseseseseswnwwsw
swseswswwnesenewnwwnweenweswwsenw
seseseswswseseseeseesesesesenwnwsesese
nenewnenwneswnwnese
seseneswseswswseseswswseswnwwswse
nwneeeeseseneseseeseeeeeeeeww
nwnwneenwswswnenwnenwnenwwneneenenwnwnw
nwnwenwnwnwsenwnwenwnwwnwnwnwswnwnwnw
wwnwwenesesewwwnewsesewnewwwe
wnwnwwwewnwseswwwwwnewnwswnwe
nwwwwswnesewwsewsenewnwnwsewnwnww
nweeeenwseeeseee
eneenewnwenewnwsesenewewswenenese
eswwsweswseswnwsenewnenwswseswwww
neswnesenenenwnenwenenwnwneneswneswnwne
enwseswnwnweesewnwseeseseeseseseseesw
wenwwwswnewswsesewwnwwnwnenewnw
nwwseseswswewwswwsewneswnenwnwesww
swnwnwnwswenenwnwnwnwsewenenenwnenesw
weneswwswswnwsweeesenenwsenwwwsw
eeseesenesesewnweseeenesweeeeew
eseweseseseesenwwsesenese
eneneneneneneneneeenenenenenwsw
esenwnweswenenwnwswnwwnewneenewnww
eeneneneeeeswneneenwsew
seweeseesenewswsenwewwneeeeee
newnwnwenwwnenesenewsenewswnweese
neswwwswswswneww
seswseswseneswseswswnewneswswswswsenenw
eneeneeeenwswneneeswnweenwseneswnene
neswseeswseseswswsewsese
wnewwswwneswswwenewswswwwswwnene
newswseswnwnwnwnenwnwnwnwnwnesenwne
neewswseswswnwswswseneswswswswswseswsesw
nwwneenwwwwsenwwseswenwnwswwnenw
wwwenwwnwnwwnwnwnwe
esesewsesesesesesenwsenwesenwswswswee
nwsesesenweseswseseseseswsesewseseenwse
newnwnwnenwswswwwnwwwnwwnwwnenwsw
swsewsenwseseseswwseseneesweswswsese
neeenweseeeeneswneeeeweee
nwenwnwnwnenwwnwnenenwnw
wwsewwenewwwwwwwsewwwnww
neneenesweneeneenwnenwneneseseeee
eswseneseseseeeeeeeseenwsewsese
nenenwswneeswnesenenewwnenenenwenwsese
nwwwwwewwswwswwnewwwwseeww
swswswswswswswseseswneeswswseneswwswwsww
nwneneswneeswenwseenweeneneewwwnee
swswswswneswwneseswwseswswswsw
swswseswneswwwswswwswsw
nwenweseesweeeeneeseeswseeesw
swswswsweswswnewwswwwswswwwnwswse
sewneseseswsesesesesewseseswnwseneswse
enenweeswsweswenwnwe
nenenwnwnenwnenwwnwswnwnenwsenenwne
neswseneswswnwwweneswnwseswswneewnese
nenweswsweeneeneswnweeneswneswee
wsenwseseseseeseseseseseseeseneswnese
swnenwwnwnwenwnwswnwnesenesesewnwnenenw
sweneeenenwenenenweeenenwsweeeswe
sewewnesesewnenwsesesesweseswswsesesw
neswswnenwneseseneneneneneneneswnenwnene
newnewweewwwnwseswwseswwewswww
senwseseseswswnesenweseswsewswsesesewsw
senenenenenenwnwnwsewnwnwenwnwnwwnene
nwnwenenwnwwneneswneneneneenwnwnwnese
seseseseseseswwneswseswesewswseswnwse
nenwnwnenwenenweswneneswweenwneneswnwnw
swnwseswewnewnesesesesesesesee
seswnenwwwewseneswswsenweneswnenenwe";
    }
}
