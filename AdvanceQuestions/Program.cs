
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace AdvanceQuestions
{
    class Program
    {
        static int CountOperations = 0;
        static int FindSpaces(ref Span<string> words, int width, out Span<string> Line)
        {
            Line = default;
            int Clength = 0;
            int spaces = 0;
            for (int i = 0; i < words.Length; i++)
            {
                if (Clength + words[i].Length > width)
                {
                    spaces += (width - Clength);
                    Line = words.Slice(0, i);
                    words = words[i..];
                    return spaces;
                }
                else
                {
                    spaces++;
                    Clength += words[i].Length + 1;
                }
            }

            Line = words;
            words = new Span<string>();
            return spaces;
        }
        static string JustifyText(string input, int width)
        {
            StringBuilder result = new StringBuilder();
            var words = input.Split(' ').AsSpan();
            while (!words.IsEmpty)
            {
                StringBuilder str = new StringBuilder();

                int spaces = FindSpaces(ref words, width, out Span<string> Line);
                for (int i = 0; i < Line.Length; i++)
                {
                    CountOperations++;
                    str.Append(Line[i]);
                    if (str.Length >= width || Line.Length == 1)
                    {
                        str.Append("\n");
                        break;
                    }
                    int x = (int)Math.Ceiling((double)spaces / (Line.Length == 1 ? 1 : (Line.Length - i - 1)));
                    str.Append(' ', x);
                    spaces -= x;
                }
                result.Append(str);
            }

            return result.ToString();
        }

        static string JustifyText(Rope text, int width)
        {

            StringBuilder stringBuilder = new StringBuilder();
            int length = 0;
            int spaces = 0;
            SpaceCount(text.Root, ref length, ref spaces, new List<string>(), stringBuilder, width);
            return stringBuilder.ToString();
        }
        
        static void SpaceCount(Node curr, ref int length, ref int spaces, List<string> wordsUsed, StringBuilder sb, int width)
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
                curr.Visited = true;
                
                wordsUsed.Add(curr.Word);
                length += curr.Word.Length;
                if (length < width)
                {
                    spaces++;
                    length++;
                }
                return;
            }
            else if (curr.Word == "") //Might clean up
            {
                if (!curr.Left.Visited)
                {
                    SpaceCount(curr.Left, ref length, ref spaces, wordsUsed, sb, width);
                }
                if (!curr.Right.Visited)
                {
                    SpaceCount(curr.Right, ref length, ref spaces, wordsUsed, sb, width);
                }
                curr.Visited = true;
            }

        }

        static void Main(string[] args)
        {
            Console.WriteLine("Give me a paragraph or line");
            string words = Console.ReadLine();
            Console.WriteLine("Give me a length");
            Rope rope = new Rope(new List<string>(words.Split(' ')));
            int l = int.Parse(Console.ReadLine());
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            string x = JustifyText(rope, l);
            stopwatch.Stop();
            Console.WriteLine(x);
            Console.WriteLine(stopwatch.Elapsed);
            Console.ReadKey();
        }
    }
}
