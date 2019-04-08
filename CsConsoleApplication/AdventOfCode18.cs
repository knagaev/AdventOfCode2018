using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsConsoleApplication
{
    class AdventOfCode18
    {
        public static void Run1(bool isTest = true)
        {
            int minutes = 10;
            var lumberMap = PrepareInput(isTest);

            Imitate(lumberMap, minutes, false, isTest);
            Console.ReadLine();
        }
        public static void Run2(bool isTest = true)
        {
            int minutes = 1000000000;
            var lumberMap = PrepareInput(isTest);

            Imitate(lumberMap, minutes, true, isTest);

            Console.ReadLine();
        }

        private static void Imitate(char[][] lumberMap, int minutes, bool verbal, bool isTest)
        {
            var resourceValuesOnMinutes = new Dictionary<int, HashSet<int>>();
            var minutesWithResourceValues = new Dictionary<int, int>();
            //var resourceValues = new List<int>();

            var lumberMapHeight = lumberMap.Count();
            var lumberMapWidth = lumberMap[0].Count();

            var currentLumberMap = lumberMap.Select(l => l.ToArray()).ToArray();

            for (int m = 0; m < minutes; m++)
            {
                var nextLumberMap = currentLumberMap.Select(l => l.ToArray()).ToArray();

                for (int i = 0; i < lumberMapHeight; i++)
                {
                    for (int j = 0; j < lumberMapWidth; j++)
                    {
                        bool notEnough = true;
                        switch (currentLumberMap[i][j])
                        {
                            case '.':
                                int numberOfTrees = 0;
                                for (int k = Math.Max(0, i - 1); notEnough && k < Math.Min(lumberMapHeight, i + 2); k++)
                                {
                                    for (int l = Math.Max(0, j - 1); notEnough && l < Math.Min(lumberMapWidth, j + 2); l++)
                                    {
                                        if (currentLumberMap[k][l] == '|') numberOfTrees++;
                                        if (numberOfTrees > 2)
                                        {
                                            nextLumberMap[i][j] = '|';
                                            notEnough = false;
                                        }
                                    }
                                }
                                break;

                            case '|':
                                int numberOfLumberyards = 0;
                                for (int k = Math.Max(0, i - 1); notEnough && k < Math.Min(lumberMapHeight, i + 2); k++)
                                {
                                    for (int l = Math.Max(0, j - 1); notEnough && l < Math.Min(lumberMapWidth, j + 2); l++)
                                    {
                                        if (currentLumberMap[k][l] == '#') numberOfLumberyards++;
                                        if (numberOfLumberyards > 2)
                                        {
                                            nextLumberMap[i][j] = '#';
                                            notEnough = false;
                                        }
                                    }
                                }
                                break;

                            case '#':
                                bool lumberyardsExists = false;
                                bool treesExists = false;
                                for (int k = Math.Max(0, i - 1); notEnough && k < Math.Min(lumberMapHeight, i + 2); k++)
                                {
                                    for (int l = Math.Max(0, j - 1); notEnough && l < Math.Min(lumberMapWidth, j + 2); l++)
                                    {
                                        if (k == i && l == j) continue;
                                        if (currentLumberMap[k][l] == '|') lumberyardsExists = true;
                                        if (currentLumberMap[k][l] == '#') treesExists = true;
                                        if (lumberyardsExists && treesExists)
                                        {
                                            nextLumberMap[i][j] = '#';
                                            notEnough = false;
                                        }
                                    }
                                }
                                if (notEnough)
                                    nextLumberMap[i][j] = '.';
                                break;

                            default:
                                break;

                        }

                    }
                }
                currentLumberMap = nextLumberMap;
                if (isTest)
                    PrintLumberMap(currentLumberMap);

                if (verbal || m == minutes - 1)
                {
                    int finalNumberOfTrees = 0;
                    int finalNumberOfLumberyards = 0;
                    for (int i = 0; i < lumberMapHeight; i++)
                    {
                        for (int j = 0; j < lumberMapWidth; j++)
                        {
                            if (currentLumberMap[i][j] == '|') finalNumberOfTrees++;
                            if (currentLumberMap[i][j] == '#') finalNumberOfLumberyards++;
                        }
                    }
                    int resourceValue = finalNumberOfTrees * finalNumberOfLumberyards;
                    Console.WriteLine(String.Format("After {0} minutes the total resource value of the lumber collection area is {1}", m + 1, resourceValue));


                    if (resourceValuesOnMinutes.ContainsKey(resourceValue))
                    {
                        foreach (var resourceValuesOnMinute in resourceValuesOnMinutes[resourceValue])
                        {
                            bool repeated = true;
                            for (int i = m - 1; i > resourceValuesOnMinute; i--)
                            {
                                if (i - (m - resourceValuesOnMinute) < 0) break;

                                if (minutesWithResourceValues[i] != minutesWithResourceValues[i - (m - resourceValuesOnMinute)])
                                {
                                    repeated = false;
                                    break;
                                }
                            }

                            if (repeated)
                            {
                                int previousMinute = resourceValuesOnMinute;
                                int period = m - previousMinute;
                                Console.WriteLine(String.Format("Previous minute with total resource value of the lumber collection area {0} is {1}", resourceValue, previousMinute + 1));
                                Console.WriteLine(String.Format("Period is {0}", period));
                                int remain = (minutes - previousMinute) % period;
                                Console.WriteLine(String.Format("After {0} minutes the final resource value of the lumber collection area is {1}", minutes, minutesWithResourceValues[remain + previousMinute]));
                                return;
                            }

                        }
                    }
                    if (!resourceValuesOnMinutes.ContainsKey(resourceValue))
                        resourceValuesOnMinutes[resourceValue] = new HashSet<int>();
                    resourceValuesOnMinutes[resourceValue].Add(m);
                    minutesWithResourceValues[m] = resourceValue;

                    //resourceValues.Add(resourceValue);

                    //if (m % 1000 == 0)
                    //    Console.ReadLine();
                }
            }

            return;
        }
        public static void PrintLumberMap(char[][] lumberMap)
        {
            var memConsoleForegroundColor = Console.ForegroundColor;

            /*
            foreach (var n in new List<int> { 100, 10, 1 })
            {
                Console.Write("".PadRight((int)Math.Log10(undergroundMap.Length) + 1));
                foreach (var i in Enumerable.Range(minX - BorderWidth, undergroundMap.Select(l => l.Length).Max()))
                    Console.Write((i / n == 0) ? " " : (i / n - i / n / 10 * 10).ToString());
                Console.WriteLine();
            }
            */
            foreach (var (l, i) in lumberMap.Select((l, i) => (l, i)))
            {
                foreach (var (c, j) in l.Select((c, j) => (c, j)))
                {
                    Console.Write(c);
                }
                Console.WriteLine();
            }

            Console.ForegroundColor = memConsoleForegroundColor;

            Console.WriteLine();
            return;
        }

        public static char[][] PrepareInput(bool isTest)
        {
            var lumberLines = isTest ? ReadTestInput() : ReadInput();

            var lumberMap = new char[lumberLines.Count()][];
            foreach (var (l, i) in lumberLines.Select((l, i) => (l, i)))
            {
                lumberMap[i] = l.ToArray();
            }

            return lumberMap;
        }
        public static List<string> ReadTestInput()
        {
            var lumberLines = new List<string>
            {
                ".#.#...|#.",
                ".....#|##|",
                ".|..|...#.",
                "..|#.....#",
                "#.#|||#|#|",
                "...#.||...",
                ".|....|...",
                "||...#|.#|",
                "|.||||..|.",
                "...#.|..|.",
            };
            return lumberLines;
        }
        public static List<string> ReadInput()
        {
            var lumberLines = new List<string>();

            const Int32 BufferSize = 128;
            using (var fileStream = System.IO.File.OpenRead(@"..\..\Input\AdventOfCode18.txt"))
            using (var streamReader = new System.IO.StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                String line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    lumberLines.Add(line);
                }
            }
            return lumberLines;
        }

    }
}
