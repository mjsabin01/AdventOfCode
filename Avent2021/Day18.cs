using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avent2021
{
    internal class Day18
    {
        public void Run()
        {
            var lines = TestInput.Split("\r\n");
            Part1(lines);
        }

        public void Part1(string[] lines)
        {
            var rootPairs = lines.Select(x => new SnailFishPairEquation(x)).ToList();
            SnailFishPairEquation? root = null;
            foreach (var pair in rootPairs)
            {
                if (root == null)
                {
                    root = pair;
                    Console.WriteLine($"  {pair.Root.Print()}");
                }
                else
                {
                    Console.WriteLine($"+ {pair.Root.Print()}");

                    root.Add(pair);
                    Console.WriteLine("After addition: {0}", root.Root.Print());                    
                }

                root.Reduce();

                Console.WriteLine($"= {root.Root.Print()}");
                Console.WriteLine();
            }

            Console.WriteLine("Final sum: ");
            Console.WriteLine(root.Root.Print());

            var magnitude = root.Root.Magnitude();
            Console.WriteLine($"Magnitude is: {magnitude}");
        }

        public void Part2(string[] lines)
        {

        }


        public class SnailFishPairEquation
        {
            public SnailFishElement Root { get; set; }

            public HashSet<SnailFishElement> AllElaments { get; } = new HashSet<SnailFishElement>();

            public SnailFishPairEquation(string line)
            {
                Root = new SnailFishElement(null);
                int startCharIndex = 1;
                DoParse(Root, line, ref startCharIndex);
                AllElaments.Add(Root);
            }

            public void Add(SnailFishPairEquation other)
            {
                foreach (var element in other.AllElaments)
                {
                    AllElaments.Add(element);
                }

                foreach (var element in AllElaments)
                {
                    element.Depth++;
                }


                var newRoot = new SnailFishElement(null);
                newRoot.LeftChild = Root;
                newRoot.RightChild = other.Root;
                Root.Parent = newRoot;
                other.Root.Parent = newRoot;
                Root = newRoot;
            }

            public void Reduce()
            {
                while (true)
                {
                    var pairsNeedExploding = (AllElaments.Where(x => x.IsPair && x.Depth > 4)).ToList();
                    if (pairsNeedExploding.Any())
                    {
                        pairsNeedExploding.Sort((a, b) => a.GetPathString().CompareTo(b.GetPathString()));
                        ExplodePair(pairsNeedExploding.First());
                        //Console.WriteLine("After explode: {0}", Root.Print());
                    }
                    else
                    { 
                        var elementsNeedingSplit = (AllElaments.Where(x => x.RegularNumber >= 10)).ToList();
                        if (!elementsNeedingSplit.Any())
                        {
                            // no explosion or splitting needed
                            return;
                        }

                        elementsNeedingSplit.Sort((a, b) => a.GetPathString().CompareTo(b.GetPathString()));
                        SplitElement(elementsNeedingSplit.First());
                        //Console.WriteLine("After split: {0}", Root.Print());
                    }                   
                }                
            }

            public void ExplodePair(SnailFishElement element)
            {
                if (element.LeftChild.IsPair)
                {
                    throw new Exception("Left child of exploding pair is also a pair.");
                }
                if (element.RightChild.IsPair)
                {
                    throw new Exception("Right child of exploding pair is also a pair.");
                }

                var leftElement = element.GetRegularElementToLeft();
                if (leftElement != null)
                {
                    leftElement.RegularNumber += (element.LeftChild == null ? 0 : element.LeftChild.RegularNumber);
                }

                var rightElement = element.GetRegularElementToRight();
                if (rightElement != null)
                {
                    rightElement.RegularNumber += (element.RightChild == null ? 0 : element.RightChild.RegularNumber);
                }

                RemoveElementFromAllElements(element.LeftChild);
                RemoveElementFromAllElements(element.RightChild);

                element.RightChild = null;
                element.LeftChild = null;
                element.RegularNumber = 0;

            }

            public void SplitElement(SnailFishElement element)
            {
                var leftChild = new SnailFishElement(element);
                leftChild.RegularNumber = (int)Math.Floor((float)element.RegularNumber / 2);
                AllElaments.Add(leftChild);
                element.LeftChild = leftChild;

                var rightChild = new SnailFishElement(element);
                rightChild.RegularNumber = (int)Math.Ceiling((float)element.RegularNumber / 2);
                AllElaments.Add(rightChild);
                element.RightChild = rightChild;

                element.RegularNumber = -1;
            }

            void RemoveElementFromAllElements(SnailFishElement? element)
            {
                if (element == null)
                {
                    return;
                }

                AllElaments.Remove(element);
                if (element.IsPair)
                {
                    RemoveElementFromAllElements(element.LeftChild);
                    RemoveElementFromAllElements(element.RightChild);
                }
            }

            void DoParse(SnailFishElement parent, string line, ref int charIndex)
            {
                var leftChild = new SnailFishElement(parent);
                var currentChar = line[charIndex++];
                if (currentChar == '[')
                {
                    DoParse(leftChild, line, ref charIndex);
                }
                else if (currentChar >= '0' && currentChar <= '9')
                {
                    leftChild.RegularNumber = currentChar - '0';
                }
                else
                {
                    throw new Exception($"Expected digit or '[' but was '{currentChar}'");
                }


                if (line[charIndex++] != ',')
                {
                    throw new Exception($"Expected ',' but was {line[charIndex - 1]}");
                }

                var rightChild = new SnailFishElement(parent);
                currentChar = line[charIndex++];
                if (currentChar == '[')
                {
                    DoParse(rightChild, line, ref charIndex);
                }
                else if (currentChar >= '0' && currentChar <= '9')
                {
                    rightChild.RegularNumber = currentChar - '0';
                }
                else
                {
                    throw new Exception($"Expected digit or '[' but was '{currentChar}'");
                }

                if (line[charIndex++] != ']')
                {
                    throw new Exception($"Expected ']' but was {line[charIndex - 1]}");
                }

                parent.LeftChild = leftChild;
                parent.RightChild = rightChild;
                AllElaments.Add(leftChild);
                AllElaments.Add(rightChild);
            }
        }

        public class SnailFishElement
        {
            public SnailFishElement? Parent { get; set; }
            public SnailFishElement? LeftChild { get; set; }
            public SnailFishElement? RightChild { get; set; }
            public int Depth { get; set;  }
            public long RegularNumber { get; set; } = -1;

            public bool IsPair => LeftChild != null && RightChild != null;

            public SnailFishElement(SnailFishElement? parent)
            {
                Parent = parent;
                Depth = parent != null ? parent.Depth + 1 : 1;
            }

            private string? _pathString = null;

            public string GetPathString()
            {
                if (_pathString != null)
                {
                    return _pathString;
                }

                var chain = new Stack<SnailFishElement>();
                var node = this;
                while (node != null)
                {
                    chain.Push(node);
                    node = node.Parent;
                }

                StringBuilder path = new StringBuilder();
                var chars = "MMMMMMM".ToCharArray();
                var i = 0;
                var current = chain.Pop();
                while (chain.Any())
                {
                    var next = chain.Pop();
                    chars[i++] = next == current.LeftChild ? 'L' : 'R';
                    current = next;
                }

                _pathString = new string(chars);
                return _pathString;
            }

            public SnailFishElement? GetRegularElementToLeft()
            {
                var current = this;
                while (true)
                {
                    var previous = current;
                    current = current.Parent;
                    if (current == null)
                    {
                        return null;
                    }
                    if (current.RightChild == previous)
                    {
                        break;
                    }
                }

                // have hit a point where this node is the right child, so go the parents left child and traverse to the right.
                current = current.LeftChild;
                while (current != null && current.RightChild != null)
                {
                    current = current.RightChild;
                }

                if (current == null || current.IsPair)
                {
                    throw new Exception("Expected to be at regular element, but was a pair...");
                }

                return current;
            }

            public SnailFishElement? GetRegularElementToRight()
            {
                var current = this;
                while (true)
                {
                    var previous = current;
                    current = current.Parent;
                    if (current == null)
                    {
                        return null;
                    }
                    if (current.LeftChild == previous)
                    {
                        break;
                    }
                }

                // have hit a point where the node is the left child, so go the parents left child and traverse to the right.
                current = current.RightChild;
                while (current != null && current.LeftChild != null)
                {
                    current = current.LeftChild;
                }

                if (current == null || current.IsPair)
                {
                    throw new Exception("Expected to be at regular element, but was a pair...");
                }

                return current;
            }

            public long Magnitude()
            {
                if (this.RegularNumber != -1)
                {
                    return this.RegularNumber;
                }

                return (3 * (LeftChild?.Magnitude() ?? 0)) + (2 * (RightChild?.Magnitude() ?? 0));
            }

            public string Print()
            {
                var sb = new StringBuilder();
                Print(sb);
                return sb.ToString();
            }

            private void Print(StringBuilder sb)
            {
                if (this.RegularNumber != -1)
                {
                    sb.Append(this.RegularNumber);
                }
                else
                {
                    sb.Append("[");
                    LeftChild.Print(sb);
                    sb.Append(",");
                    RightChild.Print(sb);
                    sb.Append("]");
                }
            }

        }

        #region TestInput

        public string TestInput =
            @"[[[0,[4,5]],[0,0]],[[[4,5],[2,6]],[9,5]]]
[7,[[[3,7],[4,3]],[[6,3],[8,8]]]]
[[2,[[0,8],[3,4]]],[[[6,7],1],[7,[1,6]]]]
[[[[2,4],7],[6,[0,5]]],[[[6,8],[2,8]],[[2,1],[4,5]]]]
[7,[5,[[3,8],[1,4]]]]
[[2,[2,2]],[8,[8,1]]]
[2,9]
[1,[[[9,3],9],[[9,0],[0,7]]]]
[[[5,[7,4]],7],1]
[[[[4,2],2],6],[8,7]]";

        #endregion

        #region Input

        public string Input =
            @"[9,[[8,3],[6,9]]]
[[[0,[5,0]],[9,[1,0]]],[[8,0],[6,[3,3]]]]
[[5,[[4,1],[3,3]]],[[7,5],[[1,5],9]]]
[8,3]
[[[[0,5],3],2],[2,[6,0]]]
[[[0,8],[7,5]],6]
[[8,[9,[7,6]]],2]
[[[[2,3],9],[0,0]],[8,[[8,2],6]]]
[[[[8,7],[4,9]],[[0,1],9]],[[[2,1],[9,5]],2]]
[[5,[6,0]],[[1,[6,5]],[[3,4],2]]]
[[6,[5,6]],[6,5]]
[[[[6,0],0],3],[[[7,8],6],[[2,5],[8,8]]]]
[[[[1,4],[3,4]],[[1,3],7]],6]
[[[[9,7],[3,9]],2],9]
[[8,[1,7]],[[[9,4],3],5]]
[[[[9,9],[6,1]],5],[[2,6],[7,0]]]
[4,[[0,5],2]]
[[[1,8],7],[[5,[7,1]],[[2,6],8]]]
[[[3,[8,5]],9],[[6,1],[[8,1],3]]]
[[9,[[6,3],5]],[[[5,3],9],[[5,0],7]]]
[[[[3,4],[2,3]],[6,[2,1]]],[2,[7,1]]]
[[1,[9,[4,8]]],[4,9]]
[[0,[[5,0],0]],[[[6,6],[1,4]],8]]
[[[[0,9],[1,4]],[[3,4],3]],[1,[[7,7],[3,5]]]]
[8,[4,[[2,5],8]]]
[[7,6],[1,9]]
[[[1,[6,0]],1],[[[5,8],4],1]]
[[[6,[1,1]],[[3,0],[9,7]]],[[[2,3],[0,4]],[4,5]]]
[[3,9],[4,[6,1]]]
[[[5,2],[[4,1],2]],[[9,[9,2]],[5,[6,6]]]]
[[[2,5],[3,[5,5]]],[[8,[6,1]],[[1,3],[1,4]]]]
[[5,[3,2]],[[1,0],[[1,6],[0,3]]]]
[[[3,[3,0]],[[4,8],9]],[[[6,0],3],[1,[5,2]]]]
[[6,[8,1]],1]
[[[[8,6],4],[[2,0],[1,3]]],[8,7]]
[[[1,7],1],[[2,5],[[5,1],6]]]
[8,9]
[[6,[6,7]],[[[1,8],6],[6,[5,4]]]]
[[[[9,2],[2,4]],[[4,9],[5,0]]],[[[0,4],9],[[0,7],[6,2]]]]
[[0,4],0]
[[[[6,8],[8,9]],0],[[0,3],[[7,0],7]]]
[[6,[[9,6],6]],[[7,[5,4]],[7,6]]]
[[[7,7],[[6,8],[7,3]]],[[7,[9,8]],[[2,2],1]]]
[[[8,5],[[8,2],[7,4]]],[[9,[3,3]],[[5,1],[1,9]]]]
[2,6]
[[[3,[4,4]],[[5,4],[0,0]]],[[1,6],[1,[1,0]]]]
[[[8,9],[[0,1],[3,0]]],[[[1,8],1],[6,6]]]
[[[[9,2],[1,5]],7],[[8,2],3]]
[[[[0,5],1],[[8,1],[2,8]]],[[3,[8,4]],[[4,2],[0,9]]]]
[[[[2,8],[4,2]],9],[3,[3,[8,0]]]]
[[[2,[3,8]],[[6,8],1]],[[5,4],0]]
[[3,[7,9]],[[3,[8,6]],[2,1]]]
[[[3,6],[[4,4],[1,7]]],[4,0]]
[[7,[0,[7,6]]],[[[1,8],4],[4,[7,8]]]]
[[[[9,4],[2,9]],[[1,8],[1,4]]],[3,[0,8]]]
[[[7,2],[[0,7],[8,8]]],[3,[[5,9],3]]]
[[[[9,9],[3,1]],2],[[[2,3],1],[[8,9],2]]]
[[[[9,6],7],2],[[[0,8],7],[[6,9],2]]]
[[[[0,0],[7,7]],0],[[3,9],[0,[9,5]]]]
[[[1,[3,0]],[8,9]],[2,[[5,7],5]]]
[[[6,[7,2]],[9,0]],[6,[1,[2,7]]]]
[[[7,[0,2]],[1,[8,6]]],[[[8,9],5],[[1,5],[9,3]]]]
[[[[6,1],[2,0]],[6,8]],[8,[8,4]]]
[[[7,[2,8]],1],[[[9,4],[7,9]],[[7,6],[5,7]]]]
[[[8,3],[[4,1],9]],[[[6,7],1],[6,7]]]
[[[[2,2],4],[[0,3],[9,7]]],[[[9,0],7],0]]
[[[[8,7],[3,7]],[[1,9],0]],[[[4,8],7],[[2,0],1]]]
[[2,9],[8,1]]
[[[[6,4],[8,0]],[2,2]],[9,[5,[9,4]]]]
[[[[4,4],[7,3]],[3,0]],[[[4,5],5],[1,3]]]
[[[[2,0],5],[7,[2,1]]],[1,[[3,5],[9,5]]]]
[1,[[4,[3,6]],[[1,1],[2,3]]]]
[6,[3,[[1,6],5]]]
[[[9,5],[7,[8,3]]],[[9,8],[[6,6],5]]]
[[[[8,8],3],[7,[3,3]]],[5,[9,[2,5]]]]
[9,[[[1,0],6],[3,[3,3]]]]
[[[[4,0],[6,5]],5],[7,[8,1]]]
[7,[[[3,8],4],[[2,8],8]]]
[[[[1,0],[4,2]],[0,1]],[5,[[9,9],[6,9]]]]
[[[[1,8],[9,8]],[3,[4,2]]],0]
[[[[1,2],4],7],[[2,6],[7,3]]]
[3,[[0,[0,1]],[[3,0],[2,0]]]]
[[[8,[5,1]],[[4,8],2]],[3,7]]
[[9,[[1,3],[1,7]]],[[9,2],7]]
[[[[5,1],[2,6]],[[6,8],[7,9]]],[[[2,4],[2,0]],[6,0]]]
[[8,[4,[7,3]]],[6,[7,[2,5]]]]
[[8,[1,8]],4]
[[[2,[5,6]],[[3,0],[2,6]]],[[[2,7],6],[[6,3],0]]]
[[7,[8,6]],[[[0,0],4],[7,9]]]
[[3,[[2,7],1]],[[5,[3,4]],1]]
[[4,[[7,7],5]],[[[9,4],[6,0]],6]]
[[[8,5],[[1,0],[0,9]]],[[4,[3,2]],[1,3]]]
[1,[6,1]]
[[8,[9,[3,9]]],[[0,6],[0,[6,6]]]]
[8,[[[8,9],[5,1]],[[7,9],5]]]
[[[[4,8],0],[1,3]],[5,[1,0]]]
[[4,1],[[[7,3],4],[[0,4],[5,8]]]]
[[[[6,9],3],7],[[[5,3],[0,1]],[[7,7],[4,5]]]]
[[6,[8,5]],[[[7,8],[5,7]],[3,[7,8]]]]
[[[[1,3],2],[8,[9,5]]],[[3,[9,2]],[[9,0],[4,8]]]]";

        #endregion 
    }
}
