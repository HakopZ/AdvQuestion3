
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

        

        static void Main(string[] args)
        {
            Console.WriteLine("Give me a paragraph or line");
            string words = Console.ReadLine();
            Console.WriteLine("Give me a length");
            Rope rope = new Rope(new List<string>(words.Split(' ')));
            int l = int.Parse(Console.ReadLine());
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            string x = rope.JustifyText(l);
            stopwatch.Stop();
            Console.WriteLine(x);
            Console.WriteLine(stopwatch.Elapsed);
            Console.ReadKey();
        }
    }
}
