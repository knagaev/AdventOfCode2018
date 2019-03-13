using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsConsoleApplication
{
    class AdventOfCode7
    {
        public static void Run1(bool isTest = true)
        {
            var stepPairs = PrepareInput(isTest);

            var allSteps = stepPairs.Select(sp => sp.before).Union(stepPairs.Select(sp => sp.after)).Distinct();

            var conditions = stepPairs
                .Union(allSteps.Except(stepPairs.Select(sp => sp.after)).Select(al => ('\0', al)).Cast<(char before, char after)>())
                .GroupBy(sp => sp.after)
                .Select(group => new { step = group.Key, dependsOn = group.Select(sp => sp.before).ToHashSet() })
                .GroupBy(deps => deps.dependsOn, HashSet<char>.CreateSetComparer())
                .Select(group => new { dependsOn = group.Key, possible = group.Select(sp => sp.step).ToHashSet() })
                .ToList();

            var instruction = new List<char> { '\0' };

            while (true)
            {
                var nextStep = conditions
                    .Where(c => !c.dependsOn.Except(instruction.ToHashSet()).Any())
                    .SelectMany(c => c.possible)
                    .Except(instruction.ToHashSet())
                    .OrderBy(c => c)
                    .FirstOrDefault();

                if (nextStep == 0) break;

                instruction.Add(nextStep);
            }

            Console.WriteLine(String.Format("Instruction {0}", String.Join("", instruction)));
            Console.ReadLine();
        }
        public static void Run2(bool isTest = true)
        {
            var stepPairs = PrepareInput(isTest);

            var allSteps = stepPairs.Select(sp => sp.before).Union(stepPairs.Select(sp => sp.after)).Distinct();

            var conditions = stepPairs
                .Union(allSteps.Except(stepPairs.Select(sp => sp.Item2)).Select(al => ('\0', al)).Cast<(char before, char after)>())
                .GroupBy(sp => sp.after)
                .Select(group => new { step = group.Key, dependsOn = group.Select(sp => sp.before).ToHashSet() })
                .GroupBy(deps => deps.dependsOn, HashSet<char>.CreateSetComparer())
                .Select(group => new { dependsOn = group.Key, possible = group.Select(sp => sp.step).ToHashSet() })
                .ToList();

            var instruction = new List<char> { '\0' };

            int numWorkers = isTest ? 2 : 5;
            var workers = new (char step, int ready)[numWorkers];

            int clock = 0;
            while (true)
            {
                for (int i = 0; i < numWorkers; i++)
                {
                    var worker = workers[i];
                    if (worker.ready == clock)
                    {
                        instruction.Add(worker.step);
                        worker = ('\0', 0);
                    }
                }

                var nextStep = conditions
                    .Where(c => !c.dependsOn.Except(instruction.ToHashSet()).Any())
                    .SelectMany(c => c.possible)
                    .Except(instruction.ToHashSet())
                    .OrderBy(c => c)
                    .FirstOrDefault();

                if (!workers.Select(w => w.step).Contains(nextStep))
                {

                }

                if (nextStep == 0) break;

                clock++;
            }

            Console.WriteLine(String.Format("Instruction {0}", String.Join("", instruction)));
            Console.ReadLine();
        }
        public static List<(char before, char after)> PrepareInput(bool isTest)
        {
            var steps = isTest ? ReadTestInput() : ReadInput();

            // Step C must be finished before step A can begin.
            var r = new System.Text.RegularExpressions.Regex(@"Step (\w) must be finished before step (\w) can begin.", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            var stepPairs = new List<(char, char)>();

            foreach (var step in steps)
            {
                var match = r.Match(step);
                if (match.Success)
                    stepPairs.Add((match.Groups[1].Value.ToCharArray()[0], 
                                    match.Groups[2].Value.ToCharArray()[0]));
            }

            return stepPairs;
        }
        public static List<string> ReadTestInput()
        {
            var input = new List<string>
            {
                "Step C must be finished before step A can begin.",
                "Step C must be finished before step F can begin.",
                "Step A must be finished before step B can begin.",
                "Step A must be finished before step D can begin.",
                "Step B must be finished before step E can begin.",
                "Step D must be finished before step E can begin.",
                "Step F must be finished before step E can begin.",
            };

            return input;
        }
        public static List<string> ReadInput()
        {
            var input = new List<string>();

            const Int32 BufferSize = 128;
            using (var fileStream = System.IO.File.OpenRead(@"..\..\AdventOfCode\AdventOfCode7.txt"))
            using (var streamReader = new System.IO.StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                String line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    input.Add(line);
                }
            }

            return input;
        }
    }
}
