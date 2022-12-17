﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Avent2022;

internal class Day13
{
    public void Run()
    {
        var lines = Input.Split("\r\n");
        Part2(lines);
    }

    abstract class PacketBase { }
    class PacketInt : PacketBase
    {
        public int Val { get; }
        public PacketInt(int val) => Val = val;

        public override string ToString()
        {
            return Val.ToString();
        }
    }

    class PacketList : PacketBase
    {
        public bool IsConvertedFromPacketInt { get; }
        public List<PacketBase> Items { get; } = new();

        public PacketList(bool isConvertedFromPacketInt = false)
        {
            IsConvertedFromPacketInt = isConvertedFromPacketInt;
        }

        public override string ToString()
        {
            StringBuilder sb = new();
            sb.Append("[");
            var isFirst = true;
            foreach (var item in Items)
            {
                if (!isFirst)
                    sb.Append(",");
                sb.Append(item);
                isFirst = false;
            }
            sb.Append("]");
            return sb.ToString();
        }
    }


    public void Part1(string[] lines)
    {
        var packets = ParseInput(lines);
        var pairsInCorrectOrderSum = 0;
        for (int i = 0; i < packets.Count / 2; i++)
        {
            var first = packets[i * 2];
            var second = packets[i * 2 + 1];
            if (AreListsInOrder(first, second) == Result.Pass)
            {
                pairsInCorrectOrderSum += i + 1;
            }
        }

        Console.WriteLine($"Sum of pairs in correct order is: {pairsInCorrectOrderSum}.");
    }

    public void Part2(string[] lines)
    {
        var div1 = ParseLine("[[2]]");
        var div2 = ParseLine("[[6]]");

        var packets = ParseInput(lines);
        packets.Add(div1);
        packets.Add(div2);
        packets.Sort((x, y) =>
        {
            var inOrder = AreListsInOrder(x, y);
            return inOrder switch
            {
                Result.Continue => 0,
                Result.Pass => -1,
                Result.Fail => 1
            };
        });

        var idxDiv1 = packets.IndexOf(div1) + 1;
        var idxDiv2 = packets.IndexOf(div2) + 1;

        Console.WriteLine($"Decoder key is: {idxDiv1 * idxDiv2}.");
    }

    List<PacketList> ParseInput(string[] lines)
    {
        List<PacketList> list = new();        
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].Length == 0) continue;
            list.Add(ParseLine(lines[i]));
        }

        return list;
    }

    PacketList ParseLine(string line)
    {
        Stack<PacketBase> stack = new();      

        for (int i = 0; i < line.Length -1; i++)
        {
            var c = line[i];
            switch (c)
            {
                case '[':
                    stack.Push(new PacketList());
                    break;
                case ']':
                    var l = stack.Pop() as PacketList;
                    (stack.Peek() as PacketList).Items.Add(l);
                    break;
                case ',':
                    break;
                default:

                    int val = c - '0';
                    if (c == '1' && line[i + 1] == '0')
                    {
                        i++;
                        val = 10;
                    }

                    (stack.Peek() as PacketList).Items.Add(new PacketInt(val));
                    break;
            }
        }

        return stack.Pop() as PacketList;

    }

    enum Result
    {
        Continue, Pass, Fail
    }
    Result AreListsInOrder(PacketList l1, PacketList l2)
    {
        // go through the elements in second list (if more elements in l1, it is ok)
        for (int i = 0; i < Math.Min(l1.Items.Count, l2.Items.Count); i++)
        {
            var left = l1.Items[i];
            var right = l2.Items[i];                       

            if (left is PacketInt piLeft && right is PacketInt piRight)
            {
                if (piLeft.Val < piRight.Val) 
                    return Result.Pass;
                if (piLeft.Val > piRight.Val)
                    return Result.Fail;
            }
            else
            {
                var listLeft = left as PacketList;
                var listRight = right as PacketList;

                // convert single elements to a list
                if (listLeft == null)
                {
                    listLeft = new PacketList(true);
                    listLeft.Items.Add(left as PacketInt);
                    
                }
                else if (listRight == null)
                {
                    listRight = new(true);
                    listRight.Items.Add(right as PacketInt);
                }

                var result = AreListsInOrder(listLeft, listRight);
                if (result != Result.Continue)
                    return result;
            }
        }

        if (l1.Items.Count < l2.Items.Count)
            return Result.Pass;
        if (l1.Items.Count > l2.Items.Count)
            return Result.Fail;
        return Result.Continue;

    }

    #region TestInput

    public string TestInput =
        @"[1,1,3,1,1]
[1,1,5,1,1]

[[1],[2,3,4]]
[[1],4]

[9]
[[8,7,6]]

[[4,4],4,4]
[[4,4],4,4,4]

[7,7,7,7]
[7,7,7]

[]
[3]

[[[]]]
[[]]

[1,[2,[3,[4,[5,6,7]]]],8,9]
[1,[2,[3,[4,[5,6,0]]]],8,9]";

    #endregion

    #region Input

    public string Input =
        @"[[[5,1,[],[8,1,3],6],[[7]]],[10,[]],[6],[[],[[0,6,4,10,5],[2,2,9],[4,4],[2,10,4,10,8]],7,7],[[5],5,[[6],[8,2],[5],[]],[9,3,2,[9,4,8,6,8]]]]
[[5]]

[[[5,3,[],[5,3,10],2],[[5,0,9,1]]],[[[9,0],[9,6,3]]],[[[1],1,[7,6],10],4],[2]]
[[],[[2,[9,2,6,9],10],[[2,8,10],[4,6],[1,6,4,6],[2,10,1,0,6]],6,[7,6,3,[10,6,4,5],4]],[],[2,2,[]],[2,9,[[3,3,9,6],[6,8,7,9,6],10,2,[6,9,8,7,0]],[7]]]

[[[4,6,7]],[[1,10,8,[7,2]],10],[10],[[3,7,8,[6,7],[]],8,[4,[5,0,9,1],[6,1,2,6,5]]]]
[[[3,[1,8,7,6],[3,8,7,7],[],[8,5]],[[1,8,10],[10,8,9]],8,[[8],9,[],3],[7,9,8,[2,10,7]]],[8,2],[6]]

[[3]]
[[2,[],[]],[[5,8,[6]],[[0,6,8,10],[]],[[9,6],6,8]],[[],4,2,[0,[1,4,2,7],6],[0,7]]]

[[[0,2,[10,5,8]],[[],7,[],[5,2,0,1]],6,10],[],[[],8,0,3,[4,0,[8,5,8,3]]],[0,[1,5],[[8,5,2],9,[8,5,2],[9,9,7,8]],5]]
[[0,6,[[5,8,6],6,[1,8,2]],[[],[10],[],3,7],2],[[[],3],7,[6,2,[]],[[]]]]

[[6,8],[[9,[2,0,7],4,10],[[0,2,10,6,4],8,[7,7,2,8,7]]],[],[6,[]],[[[3,0,6]]]]
[[9,[[],[1,6,1,0,6],[8],[2]],[8,[2,5,10,0,10],1,[7],[6,5]]],[2,[9,0,[1,5,5,0,8]],9,2],[]]

[[7,0,1,[[2,1],[8],[9,9,4,8,6],10]],[7,[5,[10,5,6],6,[4,10,7,5,10]],[1]]]
[[1,[],[[4]],3],[[[8],[4,5]],[[10],[],[3],[6,2,9],[0,8]]]]

[[[2,[],4],[],3,[4,[8,0,1,2],[],[4,0,2],[0,7,4,5,6]]]]
[[1,[],[[6],[10],[6,4,6,1,7],[0,7,1]],[],[[1],[0,8,3],[6],[2,5,7,8,10],[5,5,3]]],[[4,5,2,2,[]],4,[[6,1,7,3],8,[0,5,0]]]]

[[[0,[2,6,4],3,[8,0,10],[5]]],[[[10,4,3,0,3],[3,0,9,3,5],5,[5,7,3]],[2,3],[]]]
[[3,[[5,5,3,10,10],4,[]],7,1],[3],[[[7,7,3,7],10,[9,8,9,9],[],8],6,1,[]],[[1,[8],1],3,[[9,5,4],9,0,[0],2]],[[[1],6],4,1,[]]]

[[[10,0,[7]],4,6,[4,5]],[[2,4,[0,3,7,10,9]]],[[[6,5,9],[6,9,6,3],[1,8,4,5],8],[9],[[9,3],3,[7,5],7]],[[[]],[[2,5,4],[],[],[0,1,2,2],[5,1,1,7,8]]],[3,6,8,6,[[6,1],[]]]]
[[6,2],[4,[],[[7,7,5,9],8,[6,10,2,0,9]],2,[1,6]],[[[1,5,10,8]],8,0,0,8],[6,4,[[6,4,6,7],4],[[1,3,7,1],[0,2,4,1,10],[1]],10],[]]

[[[5,[1,6,2,5,5],[3],[7,3]],[[4,6,2,1,2]],9,7],[0,5,10],[9]]
[[10,0],[[2,[7,4,6],9,3,[0,7,10,9,1]],5,[4,[],2,8,[0,8,6]]]]

[[],[[[],6,9],[[8],[2,5,4,6],[5,6,8],[5,4,4,6],[]],[[2,6,8,1],0,[7,1,9,6]]],[2,0,1,2],[]]
[[[[],[7,5,5],[3]]],[[[6,7,6],[7,1,1]],4,[7,[2,10],4,[2,2]],0,[[7],10,5,4,[2,5,5,2,1]]]]

[[[[3,4,6,2,6],[],6],[[1],5,[1],2,[5]],10,0],[9,[]]]
[[],[[]],[[[],2,[0,3,9,10],5]],[]]

[[[[2,6,0,9],[],[],[9,3,2,9],[9,10]],6,[[4,1,6,2],1],[[10,1],[0,5,5,4],[],4,[8,8,0,1,6]]],[[5],6,[[9,4],4,0],7],[9,[],[6,4]],[],[]]
[[[[9,3,0,2],[4,10],[0],[5,5,0]],1,7,[[0]]],[],[[[2,10,10],[6]],6,[7,0,[4,0,8],6,5],2],[],[8]]

[[1,1,3],[[[],7,[0,5,0,2],[10]]]]
[[],[[[3,4,7],[7,2],[1]],[[1],9,[3,1,3],[3],10]],[[[],5,[2],4],10],[[[2,6,6,8,3],[6,0,7],1],[[8,8,3,0,3],[4,8,10],0],6,[[10],[10,2,6,6,7]]]]

[[9,2,3]]
[[],[],[[[],[10,8,7,4],0,5],3,7,[6,[1,4,5,10]],1],[[],[1,5,[7,3]],[7,10,[10],[3]],[8],2],[]]

[1,5,3,3,5]
[1,5,3,3]

[[8,[[2,6,0,9],[4,9,5,5,3],[8],8,3],[1]],[0,[7,[8,8,8,1]],[],[[0],[8,6,9,9,1],2,6,2]],[],[1,[2,1,[1],8],[],8],[[9,7],[7,10],[[],9,9,7,3],[2,[],6],[[],5,[7,6,7,6,0],[3],[3,10,10,10]]]]
[[[[8],1,[1,1,3,5,1],[0,3]],[],4,0,5],[],[3,3,[9,4,5,[4,2,3]],7,9],[7],[[[2,1]],[[2,9,10,0,6],[3,0,8],[0,3,2,9,1]]]]

[[[[5],7],3,[9,[2,6,0,2,3]],[],3],[4]]
[[10,7,8,[[],[9,10],1],[4,5,[2]]],[[5,[3,9,6,5],[],2],[9,4],[6,[2,4,8,1,7],10],1,[2,[3,6,1,5],1,[5,10],[]]],[9,8],[7,9,4,[5,[4,4,0],[9,10]],9],[]]

[[2,0,[2,8,10],[[10,9,4,0],[7,6,8,1,9],[2,5,6,9,5],[]]],[8],[6,7,4,[[0,5],4,5,3,[8,6,4,6]]]]
[[[[3,6,5,5],[6,10,3,2,9],[8,7],[6,5,7]],[[3,10,7,10,5],[9],5,[1,9,1,5,5]],5,10,[[2,1,2,3],[1,0,10,1],[1,5,3]]]]

[[1,7,[9,[2,2,5,4,5],4],[[0]]]]
[[[[8],9,5,2]],[7],[0,8,[1,3,[6,5,10,2,6],9],9],[[[7,0,6,3]],[[2,1,5,9],7,[4,2,1,7],[3,0,0,10],10],[],[[4,9,3],[]],[]],[4]]

[[1]]
[[[],[9,[4,2],4,8],[8,8,0,[5,8,10],[4,1,7,10,0]]],[0,6]]

[[2,[[1,7,10,3,10],4,5],6,[[3,9,7,8,5],[]]]]
[[[[]]],[[[5,2,9,7,2],2,5,8],2,9,[5,3],2],[4,[[],4,7,[1,10],[2,1,0,0,8]],4,7],[8,[8,3,2,5,10],[4,[10,4],[2,10,9,5],[2,6],[3]],2]]

[[[[5,5,2,7,6]],[],[]],[10,6,[6,9,7]],[[[8,1]],[],0,[[],[1,0],[]],0]]
[[4,[[10,1,5]],1],[],[1,5,8,[[6,10]],[[6],9,[0,3],[],[6,3,7]]],[10,[],[[1,4],8,[5,8]],[3,5,2,[],0]],[10,[3,[4,1,6,5]]]]

[[6,10,[10,[6]]],[8,[[8,8],2,1]],[9,[6,2,[0,0,9,10,9]],1,1]]
[[],[10,[[7,5],[3,10,7,8]]],[[]],[[10,[2,9],8,2,4],[1,[2],[10,5,3],1],0,3],[[[0,4,6]],[[6,0,9]],[[6,4,6,10],[1,8,8],3,7,4],[3,2,[0]]]]

[[3,5],[[7,[8,2],[9,8]],[[3,1,0,6,10],[],[7,8,1,10]],[[],10,[4,6,7],5,[2,10,7,6,5]],[6,[3,7,8,2,7],5],[[6,3],[7,8,4,0,8],9,[0],[0,10,7,0,2]]],[[],[[4,6,1,9]],3,[],[8,[6,3,4],[5,10,6,0,9]]],[[],9,0,[[1,7,9,10],0,10,4],4],[]]
[[],[[4],[[],0],[5,[4,7,5,9,6],[8,10,0,5,0],10,[1,0,7,6,1]]],[],[]]

[[2,6,3],[],[[7,[3,9,2,10],2,[10,0,1,3,6]],[3,[3,8,9,7,3],6],[],[[],1,0],8],[8,4,[],0],[8]]
[[7],[10,5],[[[2,6,10,9,1],7,[]],[[4,1,10,4],10,10],0],[2,7,[[9,7],[7,3,9,9],[1,8]],[],[[7,10,4],[5,2,2,6],[],2]],[[8,9],9,4]]

[[[7,[9,10,1,4,0],[4,10,2,1]],7]]
[[10],[[4,9,3],9],[4],[],[7,[7],[[10,9,8,4,10],3]]]

[[9,6,[],[],[9]],[8,[4,[1,1,10,8]],1,9,10],[[],[6,4,[9,0,2,6]]],[[4,[0,8],7,[8,1]],5]]
[[2,8],[[[1,6,6],[6]],[8,[2,10],[1,2,6,3,9],[9],4]],[[3,3,[7,0,9,8,8]],[[9,7,7],4,[4,1],[10,3],[]],[[9,8],[2]],7],[],[10,[6,[],7],[[],[5,10],[5,4],2]]]

[[[[0],[2],3,[4,1,8,8,8]]],[],[[[7,5,3]],[8]]]
[[[[3],0,[0,3,7,1],9,2],[3,2,3,[6]],[],[],0]]

[[1,4,[]],[0,0,10,10,[]],[[0,[8],[4,0]],[10],[2]],[[[],[7,1,1,10]],[7,[],[8],4,[2,9,3]],[1],[[10,8,10,5],2,[5],[10,10]],[[3,2,1,9,5],9,[5,3,7],3]]]
[[[8],7,1,0],[6,1],[[2],1,[],[7]],[]]

[[6,2,[],[4,3,5,2,8],[[4,5,4,7],3,[2,9,1,7,0],[7,4,6,6,7]]],[],[10,[[9,10],4],7,0],[4],[3,[],9,8]]
[[[10,[7,3,4,7,6],6,4,[9]],[9,[],8,[6],[5,4,7]],[5,6],[5,2,[3,1,10,0],[9]]],[[1]]]

[[],[[[3],10,3,7]],[]]
[[],[[],4,[2,[0],0,[]],4],[],[[5,0,10,6,9],2]]

[[4],[[8,[4,4,3,2]],[],[],5],[[]],[],[[3,6],10,8,[[3],6,7,2]]]
[[7,[2,[2,0,8],[6]],[]],[],[]]

[[[[3,8,2,9],5,[5,5,5,0],[10,7,1,0]]],[[[5]],[[9,4,5]],[],0],[[],8,2]]
[[0,[8],7],[[0,0,0],[[3,7,9,2]],1],[5,6,[[9],[9,7,2,3,2],[0,8,1,4]],10,2]]

[[0,[8,[6,6],[0,7],8],4,10],[6,10,[9],[8,[5,2,8,8,6]]],[6,5,9,[]]]
[]

[[[[],10,[10,9,6,5,5],5],4,2,6],[],[],[],[]]
[[5,[[],[]],[]],[8,[[10,8,6],9,[0,5,6],5],[0,[]],4],[[]]]

[[5,5,[],6,3],[[[1,9,7,8],[6]],[[6,2,2,10],[3,10,4,1],6,1],0,3,[[1,6,7,5,2],0,[],[9,2,3],4]],[7,8,[[5,1,3],[],9],[2]],[],[[[9,1,4,7,5]],8,[9,0,[1,5,10]],9,[3,[],[],1]]]
[[],[2,[1,0,0,[5,4],[]],[[2,10,7],9,0],4],[[3,10],7,[2,1],[[6],10],[10,[7,6,3,6,9],[10,6,6],[],5]],[6,[[9,3,10,10],[0,7,10,7],5,0],3],[[9,[3],[8,8],[1,6]],[[9,9,4,8,4]],[4],3]]

[[[0,8,9,[9,4,9],[10,5,0,0]],2],[9],[[[6,8]]]]
[[[0],[[0,1]],6,7,7]]

[[[[3,9,8]]],[3,[7,[7,7,4]]],[[4],[3,7,[3,10,9,2],2],[4,[8,5,7],0]],[],[[]]]
[[[[7,7],[0,3,9]],6,[7,6],[2,5,[2]]],[],[[[0,5,7,6],[3,5,2],[4,5,6,9],[5,8,10,1,0]],2,[9,[7,10]],4,7],[9,[[9,0,5,9,10],7],9,9,[[10,2,6],[10,5,9],4,1]]]

[[7,[[0,1,2],7,3,6,[6,8,10,9]],[[1,10]]],[[9,5,3],[[2,0,4,9,8],4,[9]]]]
[[0,4,5,2]]

[[[[0,8]],[7,9,[4,1]]],[[[7]]]]
[[[[5,6],[6]],[[],[]],[0,[9,6]]],[2],[[7,6,2,5,1],[[1],[9,5,8],[]],6],[[0,2,9,4,7],[3,[5],10,[],[10]]]]

[[[[0,4,8,6]],3,[9,[9,4,1],[10,0,3],[]],6],[],[],[[[3,8,6,8,3],[],[0],0,3],10]]
[[2,4,8,3,[[10,0,0,7],[1],[10,8],[9,4],[]]],[0,10,4],[10,[2,7,3]],[7,8,4,2,3],[]]

[[6],[6,[8,[0]],8,[]]]
[[[],10],[2,[[4,10,1,2,10],[1,10,3,10],[],[4,6,1],1],[[4]],10],[4,1,2,4,[[6,0,10,8,3]]],[[[6,2]]]]

[[[8],[1,5,3],[]]]
[[],[[[7,1,5,2],[0,9],5,[6,8,8,1,4]],[6,[7],5,[],[2]],[1,7,3,[0,8,8,2,8],[4,0,0]],3,1],[8,[[]],[10,0,6,[2,8,1,4,0],8]],[],[3,[[1,6,3,10],1],[],[[5],[10,6,4],[4]]]]

[[[],[[1],[8,5,5,5,9],[3],[]],3,9],[],[[],[5,[7,4],[]]]]
[[[[1,10,2]],4,[9,[8,6,7,1,4],8],9,9],[4,10,[[4,0,9],[7],5,[]],[7,[2,9,7],[],1]],[[[9,7,9,2],4]],[[1,2,[3,1],[10,9,1,10,10],10],9,3,10,3]]

[[],[10,[0,[10,10,1,2,10]],1,[]],[[],[[7,5,1,8],2,0],4,[9,5]],[[10,6]],[]]
[[6,[1,8,1,3]],[[0,[2,2,9,8,5],6,[5,2,5]],0,1,7],[[9,[1],0,5,6],3,[9]],[[[3,4,1,0],[5,3,2,8,10],[9,6],[9,5,1,8],2]]]

[[7],[[9,[10],[4,9],5],[2,[6,0,1],4,3]],[9],[[]]]
[[[3,[],[6]],[10],0,[[6,5,8,0,1],[8,9,4,7,0]],[]]]

[[4,[[],[],[7,4,8,3]],[],[[4,7,7,0,5]],[5,7,7]],[9,[],[[8,8,8,5,9],7,[5,3,7,10]]],[[7,[7],4,[],[9,5,6,10,9]],[10]]]
[[6,[[6,10,10],[5,3,10,7]],[10,9,[4]],9],[7,[]],[[[9,2,0,3,0]],9,[[1,6,9,6],4,[9,2],[2,9,7]]]]

[[],[[[4],8],4,[8],[2,3]]]
[[10,[[0,8,10,6],2],[[8,5,6],[2,4,2,9],[2],7]],[],[[7,4,[9,5]],1,0,[[0,8,4,3,7]],8],[[4,9,2,[6,8,3,6,2],10]],[]]

[[[],0,4],[10,8,[],6,7]]
[[2,[5,7]],[10]]

[[[[3],8,4,[6,8,7,3],9],[[9,9,10,4,2],3,[0,8,2]],9,[]],[9,6],[6,[[4,9],2],6,[[],[9,0,6,6,6],[6]]],[[2,[8,9,7,8],10,10],[]],[]]
[[[[1,7],[4,8,9,1,9],[3,9,5,3],5,8],2,1,[6,5],7],[]]

[[7,0,[4],[[6,7,3],7,3,[6,4,3,5,6],4]],[],[2,[[],[0,0]],[[0,3,3,4,5],5,3,[7,9,8,3,3],6]]]
[[8,1,1,[0,[],6,[7,5]],10],[[]],[3,[[0]],8,0,2],[7,[0,[2,7,4],1]],[[],[],3]]

[[1,6,[]]]
[[[[]],[10,[6,1,4,2,10]],[[7,2,7],1],[[3,8,5,2],[3],[10],8,10],0],[2],[[[7,8,5],2,9,[4,0,0],[]]]]

[[7,[6,10,5,5],1,5,[[2,7,3],7,1,9,5]],[[2,2,[8]],[],[5,9,0]],[[9],6,[[4,7,1,0,5],5,9,[3,10]]]]
[[0],[0,9,[1,10,10],3,[[8],[0],[],[3,8]]],[4,1,1,8,[[0]]]]

[[[]],[[[1,9,8,5,8],[3,2,10,7,10]],[[1,8,4],1,[9,3,10,0,4]],0,4],[3,2],[4,4,[[10,0,9,1],[],6,[3,10,8,0]],7,4],[4,[],2,[[8],0,[10,2]]]]
[[10],[[[2],[0],[]],5,[4,[4,6,6,2],[0,9,10,8],[10,3,4],0]],[6,4],[7,[[3],[4,8,5,1,5],7],[5,[8],10],[5],[1,[4,9,10],6,10,9]]]

[[],[6,3,5,5]]
[[6,6],[[[6,9,5,0],10,[9,3,2]],[2],[]]]

[[4,[[],1],3,[[3,4,0,6,0]]],[6,9,5,4],[[6,0,2,[]],[[10,3],[5],1,[3,2],[10,10,2,0,4]]]]
[[[[0,5,7,2,1],[1,0,7],[1,8]],10],[9,10],[[0]]]

[[[5,[8,0,0],[4,1,10,3],0],[[5,4],[8,10,9,5],5,1],7,3],[],[9,[5,[4,8],[8,1,5,3,2],[1,4,6,1,5]],[[1,5,1,2,4]]],[[10],6,[],3],[[[0,9,7,4,3],9,[6,5,5,2,5]],8,9,5]]
[[[[9,10,2],[6,8],6,[8,2,3]],4,[]],[[1],[10,[10,6,9],8,[9,9,4,7]],[[3]]],[],[6,[[],[4,9,5],[],9],[[8,2,6,1,6]],[[2,8,8],10,[5,9,0],[1,1,6,6],[9]],8],[]]

[[4],[9,[7,[7,7,3,10,1],6],0],[2,5],[[],1,9,[]]]
[[[],[],[3,[6,4],[7,3],9]],[[3,[6],1],[[],8,1],4,[5,4,7,4,[5,10,1,8]],[10,7,0,4]],[[2,[9,3],[10,2,2,5,1],7,[]]]]

[[[[2,6],[4,9,6,1]],[[5,7,9,4,10],[8],7],[0],[5,8]]]
[[[1,2,4,[1,1,2,10],10],1]]

[[10,[]],[6,9,[],2,[5,[5],[4,4,1,2],[9,0,5],[]]]]
[[],[[7,10,[3,10]]]]

[[1,[[7,6,5,4,10],6,3,[10,8,1]],[[5,0,1,4],[8,8,4]],[10,[2,4,4,3,8],[10,8],2],4],[[],[6],[1,6],0],[[[4],[6,5,7,10]],9,[]]]
[[],[1,0,2,[8,[8,0],10,5]],[[[9,1,4,4,6],8,8,1,5],[3,10,8,3],[]]]

[[[],[4,7,8],[3,5,1,10,[7,2,5]]]]
[[[[8,8,10,8],[9,5,2,4,5],[5,2]],[]],[2,9],[[4,[3,0,7],[2,1,3]]]]

[[[10,7,9],3],[[2,[6,8,10,0],9,10,[]],[],1,10,[2,[8,8,10,10],10]],[[[8,7,5,4]],[[],7,[4,9,2,0,4],[0],1],7,2],[],[[8],[[0,0,7,8,1],2,0,[]],[[0,9,6,7,4]],[4]]]
[[[],[[6,1,9,1]]],[[7,[]],2,4]]

[[7,10,9,10],[[[6,8,7]],10,10,[10,[1,7,1],9,7,[7]]]]
[[[],[7,8,[0,10,7,5]],10],[8],[[],[[],6,10,[],[9,10,10]]]]

[[3,[],[]],[[[9,2,3],[6,8,9,5],[0,6,7,6,7],0,10],10],[],[],[3,[[0],[6,7],3,[1,2,10,2,9]]]]
[[],[3,[[5],6,[1,3,1,5],[5,9,4]],[4],[],9],[0,9,10],[[4,8,[8,6]]],[[]]]

[[[7,[5],[1],[4,8,4,2,2]],8,1,0],[[5,[10,7,8,0],[7,4,9],[9]],1,4],[9,[],[10],3,2]]
[[5],[[2]]]

[[1],[],[]]
[[[[9,7,5,10],1],[],[]],[4,[0,[4],5,2,8],8],[[9,[1,0],3,0]],[8,0,[6,4,[10],5]],[[9,[6,3,1,0],[8,8]]]]

[[8,5,[9]],[2],[9,[],5,5,8],[3,5],[0,6,[],8,[3,0]]]
[[10,10],[6,[[0,7,6],8,[0,4,8,1]],0,4,6],[3,10,[5,7]],[[3],[[10],1,5]]]

[[2,[[9,10],[1,8,8,6,1],[]]],[[[],2,[7,6,9,2,4],[],10],[[8,7,1,10,4],9,[5,4],0,1],[0,[2,2,9,0],9],9,4]]
[[[6,1,1,[3,8],[9,4,3,6,8]],7,4,[]],[9,[],[[10],[8,8,3,1]],3]]

[[[[8,4,5]],5,[5,2,[3,1,10,8]]],[[[2,7,8,2,0]],[[3,10,4,4],10,0],6,9,3],[[],[10,1,1,[]],10,3]]
[[[[7]],[],9,4],[[7,5],[],[[7,4,2],[8,9,2,0],4,1,[8,9,2,8]],1,[[6,5,6,5],[5],[],1]],[]]

[[[9],[8,8],10,[[6,5,0],3,6,[10,4],[10,4]],[10,[10,0,7,4,5]]],[]]
[[3,[4,[],6]],[0]]

[[],[6,[7,9,9,[],2],[[],[5],10,3,7]],[2,1],[],[[],[[],4,[3],0,1],3,10,[9,0,5,5,0]]]
[[[6,[6,2,0,10],0],[2,[9,4,8,0,6],[10,8,7,4,2],[],[]],0,0,[6,2,10,[6,7,10,1]]],[[]]]

[[[[2],4],6,8,4],[2,[[8,8,3,8]],[[9,8,10],[2,2,4,7]],[5,[2,8,7],4]],[[[]],[[2,1,2,9,2]],[[9],[6,10,0]]]]
[[[[10,6,1]],[],[],[2,[2]],6],[[5,[0],[9],0,0],[3],5,[0,6,[3],10]],[],[[[],4,1],[4,0],2,2,[4,0,[2,6,4,5],10,[10,6]]]]

[[[],7,[2],[9]],[[],[[0,9,6],[0,5,4,5,2],[],7,3]]]
[[],[10]]

[[[[],[7],[2,5]],3],[4,1,1,6],[]]
[[]]

[[2,[[],[5],10,[7,10,5,6],[9,8,7]],[[8,9,4,10],0],[],[[0,7,8]]],[[9,3,0,2,7],1,[5,[3,10,7,7,2],[],[0,8,2],[3,0,6]],[],0]]
[[[7],8,10,[[5,6,2,5]]],[[3,[]],8,[[],4],4],[[],[[9,0]]],[[[4,5,6,3,0],[6,1,4,2],[0],[7,10,3],7],[[5,2]],5,[[1,1,4,10,7],3,[1]]],[[8,[],[8],2],[8,[0,7],[10,8,9,4,4]],[5,[10]],[],5]]

[[2],[9,8,4],[7,9,4]]
[[3,[[],3,[1,6,8,1,6],[],[3,3,10,6,8]],1,8,[0,0,[1,7,2],[0,0]]]]

[[10,[0,3,[8]],10],[],[[8,[],1,[5,0,6]]],[1],[4]]
[[[[7,1,10,4,9]],1,[]],[[[4]],8,9,5]]

[[7,[3,6,6],7,[[9]],[[5,8]]]]
[[],[[6,9,4,5],[0],[],4],[[9,[3],[5,1,5,9],3],[]],[[0,10,[5,4,6]],[[0,3]]]]

[[5,[0],0,[5]],[[[],[6,9,1,4,9],6,[9,9],[2,1,6,2,2]],5,10,[[0,9,6]],7]]
[[8,1,6],[10,[4,[2,5,3,2,2],[6,1,3,3]],5,[[10,4,7,1],[9,2],1]],[9,[2]],[],[[[9,6,10],[0,0],[2]]]]

[[10,0,10],[[[0,6,1],[7,3,5,5,0]]],[0,[[4,7,3,4],[],[3],8,[9,3]],6]]
[[4,[[3],9,[1,3,5,2,5],[9]]],[[6,7],9],[],[]]

[[[[0,9,4,3],[9],1,[7,2]],[[],1,2,[9,2,1,5,5],[5,5,7]]],[]]
[[[[6,1],9],1],[[[2,6,2]],2,3],[3,6,[4,[9,1],7,[0,10,6]],[1,[10,7],[2,8,7,3,1],[6,5]],[7,2,0]]]

[[[[3],8,[10,5,9,10,5],5]],[]]
[[7,8,5,6],[[7,[1],[1,9,1],[],4],[[10,4,9,10],[7,1,1,6,3],6],10]]

[[[[8,7,5],8,[1],7]],[[3]]]
[[[8,[1,0,7,6],[],[1,3,8,3]]],[1,[8,2,[1]],[[4],7,0,5,6],[[],0,0,7,9],4],[6,6,2,6,0],[],[7]]

[[4],[[0,[9,9],4,0,[2,3]],[3,5,[7,3,8,4,9],5],[2,9,10],[6,[6],5,4]],[]]
[[[6,9,[3,4],0,[]]],[10,7,3,[[6,2,4,10],[3,7],10],5],[],[[10,[9,8,0],0,1,[8,8,9]],[[0,5,1,5,6],7,6],[[8,7,1],10,[0]]]]

[[10,[[3,4],5,5,8,[1,7,8]],7,[10,[7,1,1,7,0],[8,2,9],9,[]],[[6,9],8,3,[4,7,9]]],[8,8,[[1,1,7,5],[8],1,0,9]],[[[2,10,6,7],[6,10,3,4,6]],[]],[6]]
[[[5,3,[4]],4,4,1,[3,[],4,[4],9]],[[10,10,[2,8]]],[]]

[[5,8,0,9,6],[],[3],[[[10,3,6,9]],3,7,[0]],[[]]]
[[1,[]]]

[[9,[[],7,[2],5,[6]],[]],[[6,6,2],4],[],[0],[[5,[3,9,0,2],[],9],[[1],[1,3,1,2,4],6],1,[[7,9],0,[],5]]]
[[],[1,6,8,[4,7]],[[[1,0,4,7],[0]],[[9,8],0,[8,7,8]],[[2,3,2,6,1]]],[[],[[8,4,8],[0,6],7,5],[[7,10,2,6],[5,3,8,10,6],5,2],4,2]]

[[],[7],[5,10,[[5,10,1,7],[4],8],[[],2,9,[7,2]],4]]
[[[[7,8,3],0],8,2]]

[[7,2,[[10],[5,2,1,3,8],0,5]],[],[5,[5],3],[[[10,2,6,9],0,[5,8,6,2,3],10,[4,6,10,10,3]],[[10,1,9],1,[0,2,6],4,[7,5,10]],7,[[1,7,4,6]],10],[[[0],7,5,[9,4],2],0,[[],[4,7],[9,2,7,3,4]]]]
[[[],[[1,10,6,1,10],5,10],4],[9,[[]],1]]

[[7],[]]
[[[[9,2,10,10,7],[1,6,6],[10,0,9]]],[[8,9,4,10,0],[[],5],4],[[]]]

[[[[9,7,10],9,7,[6,10]],[[8,0,7,2]],[8,4,[10]]],[],[7,[8,[6,3,5]],[7,[0,4,4,9,9]]],[[[4,10,2],[5,7,3,3],[1,4,6,4,2],[8,7,10,7,1],4],[2],3,[[],9,10,[2,4]]],[[[]],[0,[4,7]],[]]]
[[],[7],[10,6,10,[[]]],[9,[[],6],[[4,2,5,5]],[[7,1,9],7]],[0,5,3,10,7]]

[[5],[0,[[0],10,[1,2]],10,[],8],[[[6,0],6,7,[]]],[2,[8,[7,0,5,5],[0,3,0],[],[1]],2,[2,10,[5,5,3,0,1],0,[1,2]]],[]]
[[],[[3,[],[],[9,4,2,9],[]],[6,9,8,6,[7,9,4,2]],6,0,[[],[10,5]]],[]]

[[],[]]
[[7,4,[9,3]],[2,[[]],[[4],[1,3],6,[8],8],[1,[8,5],1,3]],[6,0,8],[4,5,2,7]]

[[[[9],[5,6,7,1],10,9],7],[[[3],[9,7,1],4,[7,9]],5,6,6]]
[[],[[],[8],[9,1,[6,3,6,8,9],[5,7,9,5]]]]

[[],[5,[[8]],2,[]],[9],[10],[10]]
[[[[4,10],9,[5,9,8,5,3],1],[[5,0,7],[10,10,5,5,4],[9,10,7,6],[1,2]]],[2,7,1,[1,4]],[[],[[2]],8],[8,4,[5,8,[8],[2,10]]],[]]

[[],[1],[0],[],[6]]
[[],[[5]]]

[[[[0,6,9,5,10],6,[0],0]],[[7,[]]],[3],[3,8,[[0,3],6,10,[4],[]]]]
[[1,[],8,[[1,3],8,8],[[],1,[8,9],6]],[[[6,2,6,4],[4,5],[3,6,4,6],7,[]]],[2,5],[8,[1,[9,9,9,8,6],[10]],9,[[2,5],7,[5,4,8],3],7],[8,0,[4,9,9,9,[5,6,2]]]]

[[[5,[2,0,3]],[[],5,4],2],[3,[4],[[2,3,0,6]],9,1],[],[[0,6,1,[1,10,0],5],5,[1,1,5,9,[2,8]],10],[7,[[],[7],[4,3,4],7,[8,8]]]]
[[],[7,10,9,[[2,8,5,5],[2,1,9],4,[],[6,8]],[7,4]]]

[[[[],[1],[8,9,3,9],8],[[5],5,10,8],[3,5]],[[4,4,10,0],3,[[4,9,3],[6],[6,7,4,7,10],4,[4,9,2]],[10,7,[],[6,4],[7,7]],4]]
[[[[2,5],1,0]],[[[0],10,[2,1,9],[3,7,8,10,7]],7,[[6,7,3],10,0,[10,8,6],[9,9,5,4]],[4,4,3,[7,4,10,4,1],[1]]]]

[[[0,[9,1,6,7,10],7],3],[3],[[1,10],[[4,3,1,7,10],6,2,[9,3,3]],4],[[6,[]],[2,[9,2,6,5]]]]
[[3,7,[],2,[[],[1,4,9,1,10],0]],[8,[7,1,6],[4,[0,9,4,1]]],[4,[[9,9,2],4]],[2,8,[7,[8],[9],[6,7]],1]]

[[8,9,[[2,0],[],4,[8]],7,[[10,10],[9,3,1,1],7,6]],[[9],[[],5,[2,1,2,6]]],[[]],[[9,[7,3,2,2],10],4]]
[[[]]]

[[1,3]]
[[[2,[2,1,9,5,3],1,9],4],[[[2,3,9,5],9,[9,2,4,3],0]],[[[3]]]]

[[6,5,[[6,4,7,5,7]],[[]]],[[[5]],4],[[2]]]
[[],[5,[[2],8,1,[1,3,7,5,0],6],[[1,4,4,8,10],[2,7,9,9,4],1,0],9]]

[[[10,[6,0],10,2,7],[[6,1,7,1,6],[],3,[9,8,2,0],10],[2,[8,0],6,0,[6,2,4]],5,5],[[[6,1,5,3]],[[10],5,[]]]]
[[],[10,6,5,10,9],[1,4,8]]

[[[[1,0,10,3,8],[10],[7,8,3,6],3],[5,[10,6,9],1,8],0,[[],[5],[]],[[7,3,3],0,[0,7,9,0,10]]],[[[3]]],[10],[1,4,[[],[0,1],[9,9,4,8],10,10],[]]]
[[[1,[4,6,1]]],[6,5,8,6],[1,[[6]]],[3,[[1],[]]],[[5,[4,3,10],[6,1,10,1],[7,4]],[[9,10]],[10,3,1,[6,5],[0,5]]]]

[[1,[5,[6,5],9,10]],[[[1,5,0,5,5],[9,1,6,8],7,7,[7,5]],[[8,10,3,9],3,4],1],[10,8,3,[8,1,9,6],[4]]]
[[7,1,[8,[10,9,0],[4,2],10,9]]]

[[[[4],[7,3,8],[10,9]],[[8],4,[],[9,9,7,9,2]]],[],[[[],10,4],3,4,1,[9,5,6]]]
[[[]],[[0,[1],7,[1,4,10,7],[10,10,9]]],[[[4,7,0],3,3,2]],[9,0,0,5],[10,[6,8,0,[7,8],7]]]

[[[4,9,9,6],10,[[],[4,1,4,9,1],7,4,7],[5,[],4],[1,9,[9,1],0,0]],[10,[2,1,[7,3,7]],[0,[1,5,8,6,8],[3,5,2],[9],1]],[4,8,10],[[[],7,1,10,[8,8,5]]]]
[[[]],[[6,1],[[2]]],[[[]]]]

[0,0,1,4]
[0,0,1,4,10]

[[],[10,[[4,5,7,1],[9,0,9,0,0],[6,10,1],4],[[2,4,6,0],2,8,[]]]]
[[[7],10,[],8,[[2,7,10,0]]],[10]]

[[6],[],[9,9]]
[[[],1,[8,[5,7,5,0,10],6,[7,5]]],[[],[],7,5],[[0],[10,4,9],8,[[2],[1,7,0,9],1,[1,3]]],[10,[],[],6,[[0,6,7],10,7]]]

[[9,8,[[10,10,9,5,10],[8,2,1]],10,5],[2,[9,[1],9],[[4,10,3]],[[4,9,4],9,[4,1,5,10]]]]
[[10,0,7],[],[]]

[[1,[9,9,2,9],4,7],[10,[6,9,[7,6],8]],[],[[2,2,[6,3],[10,1]],6],[]]
[[3,10],[],[[],[[10],10,[7],[6]],[]],[4]]

[[4,7,[8,1],[[9],0,[3,6,0,7],9,9]],[10],[1,8,[]],[],[[],4]]
[[4,[1,9,5,0,2],9],[[[4,7,8],[6,0]]],[],[5,[0,[],[2,10,0,2],[1,6],8],6,7],[]]

[[2,[]],[[[10,9,8,7],5,[],[3],[10,2]],[9,[8,9],[8,1,4,3],4],7,2,6],[[[0],2,9,2,[]],[8,[6,0,7,10],1,1,5]]]
[[[3],[0],[[2,5,10,0],[],9],2],[4,[9,[7],[7,9],[9,2,5,3,6]],4,[[],1,[],1],10],[[[2,10,2,1,3],[3,8,5,3,10],[5,4,5,5],[],6],[[5],5,6]]]

[[9]]
[[[10],[[9,4,8]],[[],1,[10],[7,1]],[[1,0],[4,2],[1,5,5,3,9],1]],[5]]

[[[6,5,[2,3,10],9,8],[[],[8,1],3],[[],7,[5,4],[0,1,8,7,0],4],[3],[[2,0,0],1,[4,7,0,4],[],[]]],[4],[2,8,[[4],7,[10,10,9]],5]]
[[5,0],[[1,4,[0]],[[1],[8,7,7,8]],[[7,7,9,1],4,7,6],[10],7],[[],9],[[5,[10,8],0],8],[9,[[8],7,10],[0,[10,3],1,5,3]]]

[[0,[6,10],[[4,1,9],[],10,[10,6,2,7,3]]]]
[[[[1],[1],[],10],10,9,8,[[]]]]

[[[],1,[[8,1,9,5],0,10,[1],[9,1,5,0]],6,4],[[[8,2,7,0,6],6,1]],[[[6,3,10]],6],[[],[[5,0,8,10,9],[2]],8],[]]
[[[],5,[6,[0,6,8],[7,5,5,0,6],6,[]],[]],[[],3,0,3,[3,6]],[7,5,[[7,0,6,8],[9,5],[]]],[[[2],5,[1,0,5]],8,4,[[5,10,9],[0,2],10,[2]]],[]]

[[[2,3]]]
[[[[],[6],[4,9,10,6,3]],4,[]],[],[[],7,[],2],[]]

[[9,[5,8,9],1,1,6],[[[8,6]],3],[8,[[8,7,10],[]]]]
[[1,7,[7,7,4,[10,0,0],[10,10,10,10]]],[[[],[9,5]],[0,2,[8,4],[],[7,8,2]],[]],[[6,[7,0],[9,1],8,[]],4,1],[]]

[[],[6,1,[[2,1,10],7,4,[7]],[],0],[]]
[[[],8,[4,[9,9],3]],[]]

[[],[8],[[[9,3,10],[]],[10,[1,0,1],2]]]
[[[[7,9,3,3,10],4,8,[7,0,2,10,10],4],4,[8,10,[0,1,1]],6,[]]]

[[[3,8,10],0]]
[[1,[],3,[[5],[2,4,3,5],1,[6,8,6,2],[0,10]],[[6,6,3],[4,4]]],[9]]

[[],[],[[8,[0,7,9],2]]]
[[6,4],[[6,6,9],[[0,10,1]],[[],0,1,[],3],[[1,4,4,8,4]]],[4,[[3,1,5,7,6],7,[9,3,6,10]],[8,[0,10,1,10,10]],[[8,7,7],[8,2,7,7,8],0],[[7,7,8,5]]],[10,1,5,[[2,5,4],10,[]],1],[[4],1]]

[[1,[9,10,[7,4,7,9],0,[7]],3,[[1,2,8],[7,6,8],[4,9,8,1,6],6],9],[5,9,7,[[7,2,6,4,8]]]]
[[[[5,2,3],[0,8,2],2],[0,3,[9,1,5,2]],[2,6,6,4],1]]

[[7,[9,[0,9],5],[[4,3,6,7],7],2,2],[[[0,4,1,8,4],3,[],[]],[],5]]
[[[],[[],3,1,9],10],[]]

[[3,[8],8,4],[8,3,[[2,0,8,9,0],6,6]]]
[[[7,3,[6,5,5],5],8,6,2],[[[],1,[2,8,5],[],[4,10,9]],7,[],[8]]]

[[],[[[6,3,8,3],[6,4,7,2,9],0],6,[]],[9,[],6,8,[]],[]]
[[[[],[],1,[0,9,8],3],6,7],[[0,[5,7,3,0,10],0,[],7],3,9,[[6],[3],2],7]]

[[8,0,[[5,5],[0,2,4],3,[5],[2]],6],[0]]
[[5,10,7,0,[[7,9,10,2,6],[8,6]]],[[6,[9,4,3,1]],[8,3],[[3,6,7,8,6],3,[],[9],[1,2,4,7,10]]],[[],2,9,8]]

[[],[3,[[4,5,5,9,0]],8,[],[[3,7,8,10,2],7,10,[0,2,1,7]]],[[[2,6,2,0,10]]],[7,[]],[6,[],6,[[2,1,8],7,0],[[9,9,1],[],[9,4,3,1,5],[10,6,3,6,9]]]]
[[4,1,[],10,10],[[5,7],[],5]]

[[[[],7],4,3,[[],4,[4,8,3,3,3],2,10],[5]],[[7],[10,[],[9,9,3,9],[8,8,5,4],[5,6,4,10]],[7,10,[6,4,0,4],[7,0,0]]],[3,[1]],[3,[[9,8,3,4,9],10,[0,1]]],[[[3,6],6]]]
[[],[9,2,5,4],[[[0,9,2,3],7,[1,10,2],6,10],4,5,2],[[[4,9],[7,1,0,1],4],[[2,2,8],[4,3,10,9,2],4,9,[]],3,7,8],[]]

[[7],[8,[8,[2,7],7,3]],[],[[]]]
[[],[[8,10,2],8]]

[[5,[]],[[[4],8]]]
[[[[],7,2,3],9,[],[[1],9,[]],1]]

[[5],[6,[8,1,[6,7,9,5],9],0]]
[[6,5,2]]

[[0,[[8,6,3,8],1,6,[1,4],[2,2,1,0]],1,8],[[4,3,[],[],3],6,[],[3,[0,0,7,6]],[3,[],[9,6,10,10,2],0,[3]]],[],[[1,[]],6,[[0,9,6,1,4],2,2],8,[[7],6,8,7]],[3,1,8,9]]
[[4],[[[10,3,10,5,0],[],[5,1,1,9],[9,8],2],2,9],[6,0,[[4,4,5,6,8]]],[[1,6]]]

[[8,9,7,6],[9,[9,[9]],1],[[],[6,[]],[[2],1,[4,5,8,8,7]]],[0,4,[[4,7,7,0],[10,2],4,9],[[]],5],[4]]
[[],[9],[9],[9],[[[],[],[1,0],[5,10,6]],6,[[9,0],2,4],8]]

[[],[10],[[[4],[],[5,6],[7,10,5,2]]]]
[[[],6],[[8,3],[2,5,2,[10,7,8,4]]],[[0,1,9,[10,3,7],8],4,3,[[9],7],0]]

[[6],[],[[[6],[7,5,2,7],[3,7]],1,[[7,0,3],[],[7]],6],[5,5],[[9,1,[0,10,10],[3,0,4,0],3],[6,8,1],2]]
[[[9,10,[1,8,4],10]],[1,[]],[]]

[[2,[[9,5,4],4,[10]],[[10],[8,9,10,6,3],[6],9,[8,5,3]],[]],[[[9,8],[0,6],1,10,[0,10,4,6,8]],[[9,8,6]]],[3,4,[1,1,2,[1,4,5,2,2]]]]
[[7,[8,[1,8,1,8,8]],[[0,10],9],[],4]]

[[[[2,0,4,1],2],[3,7,[8,2,1]],4,[[10,3]]],[6,9,[[1,6,9,6,6],4,[]]],[[7,10],9],[5,[[4,2,9,1],[6,6],[4,4,0],[7,2,2]]]]
[[[0,10]],[]]

[[4,4,4],[1,[10,[],10,6,[8,8,3,7,7]],6],[[0,8,[9],4,[3,6]],[[4,5,3,6,8],4]],[1,[[3],[]]]]
[[10,4,7,[[8,9],7,7,[10,1,2],10],[[9,8],1]],[],[[4,10,[]]]]

[[[[4]],3,[[9,3,3,8,3],5,[1,3,9],5,[4,4]],[1,[5]],[[6,9,8],2,5,[1,5],4]],[[[],4]],[]]
[[],[[9,1,[5,8],[8,9],1],[[]]],[7,0,[[3],4,5],8],[],[1,[3,[0,0,6],[],[]],6,8,1]]

[[4],[8,4,[3,6,[],2]],[[[2,10,8,0,5],[7,4,9,5],[5,10,7],[10,2,5,0,7]]]]
[[],[[[3,0],0],[],[[4,4],[1]],[5,[7,7,8,9,10]]],[9]]

[[],[[[3,9,10],0],[9,10,[10,10,10,2,2],[5,10,0,6,9],6],[9,3,10,7,[2,5]]]]
[[7]]

[[[],7],[6],[2,8,[5,6,[9,0,2,10,8],3,5],7],[4,9,[[1,10,1,6],3,4],1],[2]]
[[],[[[8,3,0,10],6,0,[10,7,2]]],[5,9,0,4]]

[[[[1,4],8,10,6],[0],1],[],[5,4,0],[[6,4,6,6]],[]]
[[[8,3,[0,8,4,4]]],[],[[7,[],[3,8,1,1]],10,[[],[],6,[9,7,1,10,9]],[8,[3,2,0],4,[7,9,2,5,3],[9,6,7,6]],3]]";

    #endregion 
}

