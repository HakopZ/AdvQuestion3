using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdvanceQuestions
{
    class Node
    {
        
        public int Weight
        {
            get
            {
                if (Word != "")
                {
                    return Word.Length;
                }
                if (Left.Word != "")
                {
                    return Left.Word.Length;
                }
                int x = 0;
                x += Left != null ? Left.Weight : 0;
                x += Right != null ? Right.Weight : 0;
                return x;

            }
        }
        public int WordCount
        {
            get
            {
                if (Word != "")
                {
                    return Word.Trim().Split().Length;
                }
                int x = 0;
                x += Left != null ? Left.WordCount : 0;
                x += Right != null ? Right.WordCount : 0;
                return x;
            }
        }


        public string Word { get; set; }
        public Node Left { get; set; }
        public Node Right { get; set; }

        public Node Parent { get; set; } //Remove if not used
        public Node(string word = "")
        {
            Word = word;
        }


    }
    class Rope
    {
        public Node Root;
        public int Count { get; private set; }
        
        public Rope(List<string> words)
        {
            Count = words.Count;
            Root = BuildTree(words, Root);
        }
        public Node BuildTree(List<string> words, Node curr)
        {
            if (words.Count > 1)
            {
                curr = new Node();
                curr.Left = BuildTree(words.Take(words.Count / 2).ToList(), curr.Left);
                curr.Right = BuildTree(words.Skip(words.Count / 2).ToList(), curr.Right);
                curr.Left.Parent = curr;
                curr.Right.Parent = curr;
            }
            else
            {
                curr = new Node(words[0]);
            }
            return curr;
        }

        public void Insert(int pos, string s)
        {
            Node newRope = Split(pos);
            Node temp = new Node(s);
            temp = Concatenate(temp, newRope);
            Root =  Concatenate(Root, temp);
        }
        
        public Node Concatenate(Node S1, Node S2)
        {
            if (S1.Weight == 0) return S2;
            if (S2.Weight == 0) return S1;
            Node temp = new Node();
            temp.Left = S1;
            temp.Right = S2;
            S1.Parent = temp;
            S2.Parent = temp;
            return temp;
        }
        public char Index(Node node, int i)
        {
            if (node.Weight <= i && node.Right != null)
            {
                return Index(node.Right, i - node.Weight);
            }
            if (node.Left != null)
            {
                return Index(node.Left, i);
            }
            return node.Word[i];

        }

        public (Node, char) IndexOf(Node current, int i)
        {
            if (current.Weight <= i && current.Right != null)
            {
                return IndexOf(current.Right, i - current.Weight);
            }
            if (current.Left != null)
            {
                return IndexOf(current.Left, i);
            }
            return (current, current.Word[i]);
        }
        public Node Split(int point)
        {
            (Node ind, char l) = IndexOf(Root, point);
            int index = ind.Word.IndexOf(l);
            Node startNode = ind;
            if (index != ind.Word.Length - 1)
            {
                Node temp = new Node();
                temp.Left = new Node(ind.Word.Substring(0, index));
                temp.Right = new Node(ind.Word.Substring(index + 1));
                temp.Left.Parent = temp;
                temp.Right.Parent = temp;
                ind = temp;
                startNode = temp.Left;
            }
            Queue<Node> orphans = new Queue<Node>();
            while (startNode != Root || startNode.Parent.Right != startNode)
            {
                startNode = startNode.Parent;
                if (startNode.Right != null)
                {
                    orphans.Enqueue(startNode.Right);
                }
                startNode.Right = null;
                startNode.Right.Parent = null;
            }
            Node f1 = orphans.Dequeue();
            while (orphans.Count > 0)
            {
                var f2 = orphans.Dequeue();
                f1 = Concatenate(f1, f2);
            }
            return f1; 
        }
        public string JustifyText(int width)
        {

            StringBuilder stringBuilder = new StringBuilder();
            int length = 0;
            int spaces = 0;
            GoThroughTree(Root, ref length, ref spaces, new List<string>(), stringBuilder, width);
            return stringBuilder.ToString();
        }

        public void GoThroughTree(Node curr, ref int length, ref int spaces, List<string> wordsUsed, StringBuilder sb, int width)
        {
            if (curr.Word != "")
            {
                if (length + curr.Word.Length > width)
                {
                    StringBuilder temp = new StringBuilder();
                    spaces += (width - length);
                    for (int i = 0; i < wordsUsed.Count; i++)
                    {
                        temp.Append(wordsUsed[i]);
                        if (temp.Length >= width || wordsUsed.Count == 1)
                        {
                            temp.Append("\n");
                            sb.Append(temp);
                            spaces = 0;
                            length = 0;
                            wordsUsed.Clear();
                            break;
                        }
                        int x = (int)Math.Ceiling((double)spaces / (wordsUsed.Count - i - 1));
                        temp.Append(' ', x);
                        spaces -= x;
                    }
                }
                wordsUsed.Add(curr.Word);
                length += curr.Word.Length;
                if (length < width)
                {
                    spaces++;
                    length++;
                }
                return;
            }
            else if (curr.Word == "")
            {
                SpaceCount(curr.Left, ref length, ref spaces, wordsUsed, sb, width);
                SpaceCount(curr.Right, ref length, ref spaces, wordsUsed, sb, width);
            }

        }
    }
}
