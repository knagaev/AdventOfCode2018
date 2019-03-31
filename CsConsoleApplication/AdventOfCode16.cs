using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsConsoleApplication
{


    class AdventOfCode16
    {
        public static Dictionary<string, Func<int[], int[], int[]>> Operations = new Dictionary<string, Func<int[], int[], int[]>>
        {
            {"addr", (before, operation) => { int[] after = before.ToArray(); after[operation[3]] = before[operation[1]] + before[operation[2]]; return after; } },
            {"addi", (before, operation) => { int[] after = before.ToArray(); after[operation[3]] = before[operation[1]] + operation[2]; return after; } },

            {"mulr", (before, operation) => { int[] after = before.ToArray(); after[operation[3]] = before[operation[1]] * before[operation[2]]; return after; } },
            {"muli", (before, operation) => { int[] after = before.ToArray(); after[operation[3]] = before[operation[1]] * operation[2]; return after; } },

            {"banr", (before, operation) => { int[] after = before.ToArray(); after[operation[3]] = before[operation[1]] & before[operation[2]]; return after; } },
            {"bani", (before, operation) => { int[] after = before.ToArray(); after[operation[3]] = before[operation[1]] & operation[2]; return after; } },

            {"borr", (before, operation) => { int[] after = before.ToArray(); after[operation[3]] = before[operation[1]] | before[operation[2]]; return after; } },
            {"bori", (before, operation) => { int[] after = before.ToArray(); after[operation[3]] = before[operation[1]] | operation[2]; return after; } },

            {"setr", (before, operation) => { int[] after = before.ToArray(); after[operation[3]] = before[operation[1]]; return after; } },
            {"seti", (before, operation) => { int[] after = before.ToArray(); after[operation[3]] = operation[1]; return after; } },

            {"gtir", (before, operation) => { int[] after = before.ToArray(); after[operation[3]] = operation[1] > before[operation[2]] ? 1 : 0; return after; } },
            {"gtri", (before, operation) => { int[] after = before.ToArray(); after[operation[3]] = before[operation[1]] > operation[2] ? 1 : 0; return after; } },
            {"gtrr", (before, operation) => { int[] after = before.ToArray(); after[operation[3]] = before[operation[1]] > before[operation[2]] ? 1: 0; return after; } },

            {"eqir", (before, operation) => { int[] after = before.ToArray(); after[operation[3]] = operation[1] == before[operation[2]] ? 1 : 0; return after; } },
            {"eqri", (before, operation) => { int[] after = before.ToArray(); after[operation[3]] = before[operation[1]] == operation[2] ? 1 : 0; return after; } },
            {"eqrr", (before, operation) => { int[] after = before.ToArray(); after[operation[3]] = before[operation[1]] == before[operation[2]] ? 1: 0; return after; } },
        };

        public static void Run1(bool isTest = true)
        {
            var samples = PrepareInput(isTest);

            var behaviors = new List<(int, List<string>)>();

            foreach (var (sample, i) in samples.Select((s, i) => (s, i)))
            {
                var behavior = new List<string>();
                foreach (var op in Operations)
                {
                    var after = op.Value(sample.Before, sample.Operation);
                    if (after[0] == sample.After[0] && after[1] == sample.After[1] && after[2] == sample.After[2] && after[3] == sample.After[3])
                        behavior.Add(op.Key);
                }
                behaviors.Add((i, behavior));
            }

            var threeAndMoreOps = behaviors.Where(b => b.Item2.Count() > 2).Count();

            Console.WriteLine(String.Format("{0} samples behave like three or more opcodes", threeAndMoreOps));
            Console.ReadLine();
        }

        public static List<Sample> PrepareInput(bool isTest)
        {
            var inputs = isTest ? ReadTestInput() : ReadInput();

            var samples = new List<Sample>();

            var sample = new Sample();
            foreach (var input in inputs)
            {
                if (input.StartsWith("Before"))
                {
                    sample = new Sample { Before = input.Substring(9, 10).Split(new string[] { ", " }, StringSplitOptions.None).Select(c => int.Parse(c)).ToArray() };
                    continue;
                }

                if (input.StartsWith("After"))
                {
                    sample.After = input.Substring(9, 10).Split(new string[] { ", " }, StringSplitOptions.None).Select(c => int.Parse(c)).ToArray();
                    samples.Add(sample);
                    continue;
                }

                if (string.IsNullOrWhiteSpace(input)) continue;

                sample.Operation = input.Split().Select(c => int.Parse(c)).ToArray();
            }

            return samples;
        }
        public static List<string> ReadInput()
        {
            var input = new List<string>();

            const Int32 BufferSize = 128;
            using (var fileStream = System.IO.File.OpenRead(@"..\..\Input\AdventOfCode16.txt"))
            using (var streamReader = new System.IO.StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                String line;
                bool emptyLine = false;
                while ((line = streamReader.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        if (emptyLine) break;
                        emptyLine = true;
                    }
                    else
                        emptyLine = false;

                    input.Add(line);
                }
            }
            return input;
        }
        public static List<string> ReadTestInput()
        {
            var input = new List<string>
            {
                @"Before: [3, 2, 1, 1]",
                @"9 2 1 2",
                @"After:  [3, 2, 2, 1]",
            };
            return input;
        }

    }


    internal class Sample
    {
        public int[] Before = new int[4];
        public int[] Operation = new int[4];
        public int[] After = new int[4];
    }
}

