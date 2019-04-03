using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsConsoleApplication
{
    class AdventOfCode17
    {
        public const int BorderWidth = 1;

        public static void Run1(bool isTest = true)
        {
            var (undergroundMap, minX, minY) = PrepareInput(isTest);
            //var inputs = isTest? PrepareInput1(ReadTestInput1) : PrepareInput1(ReadInput1);

            PrintUndergroundMap(undergroundMap, minX, minY);

            var (filledTiles, remainedTiles) = FillWater(undergroundMap, minX, minY, isTest);

            Console.WriteLine(String.Format("Water can reach {0} tiles, stay in {1} tiles", filledTiles, remainedTiles));
            Console.ReadLine();
        }

        public static (int FilledWater, int RemainedWater) FillWater(char[][] undergroundMap, int minX, int minY, bool isTest)
        {
            var fillWaterMap = undergroundMap.Select(l => l.ToArray()).ToArray();

            var springPosition = fillWaterMap[0].Select((c, j) => (c, j)).Where(cj => cj.c == '+').Select(cj => cj.j).Single();
            fillWaterMap[1][springPosition] = '~';

            /*
            for (int i = fillWaterMap.Count() - 1; i > 0; i--)
            {
                for (int j = 1; j < fillWaterMap[i].Count() - 1; j++)
                {
                    if (fillWaterMap[i][j] )
                }
            }*/

            var fillWaterMapDeep = fillWaterMap.Count();
            var fillWaterMapWidth = fillWaterMap[0].Count();

            bool updated = false;
            while (true)
            {
                if (isTest)
                    PrintUndergroundMap(fillWaterMap, minX, minY);
                //Console.ReadLine();

                updated = false;
                for (int i = fillWaterMapDeep - 2; i > 0; i--)
                {
                    for (int j = 0; j < fillWaterMapWidth; j++)
                    {
                        if (fillWaterMap[i][j] == '~')
                        {
                            // check for flow down
                            if (fillWaterMap[i + 1][j] == '.')
                            {
                                fillWaterMap[i + 1][j] = '~';
                                updated = true;
                            }
                            else
                            {
                                if (fillWaterMap[i + 1][j] == '#' ||    // over clay
                                    (fillWaterMap[i + 1][j] == '~' &&   // or over clay-bounded water  
                                        (fillWaterMap[i + 1][j] == 'o' || IsFilled(fillWaterMap, i + 1, j))))
                                {
                                    if (j < fillWaterMapWidth - 1 && fillWaterMap[i][j + 1] == '.')
                                    {
                                        fillWaterMap[i][j + 1] = '~';
                                        updated = true;
                                    }
                                    if (j > 0 && fillWaterMap[i][j - 1] == '.')
                                    {
                                        fillWaterMap[i][j - 1] = '~';
                                        updated = true;
                                    }
                                }
                            }
                        }
                    }
                    if (updated) break; // if there were changes on the deepest line
                }
                if (!updated) break;
            }

            for (int i = fillWaterMapDeep - 1; i > 0; i--)
            {
                for (int j = 0; j < fillWaterMapWidth; j++)
                {
                    if (fillWaterMap[i][j] == '~')
                    {
                        if (!IsFilled(fillWaterMap, i, j))
                            /*((j > 0 && (fillWaterMap[i][j - 1] == '.' || fillWaterMap[i][j - 1] == '|')) ||
                            (j < fillWaterMapWidth - 1 && (fillWaterMap[i][j + 1] == '.' || fillWaterMap[i][j - 1] == '|')))*/
                            fillWaterMap[i][j] = '|';
                    }
                }
            }


            Console.WriteLine("Finish");
            PrintUndergroundMap(fillWaterMap, minX, minY);

            var filledWater = fillWaterMap.Select((l, i) => (l, i)).Where(li => li.i >= minY).SelectMany(li => li.l.Where(c => c == '~' || c == '|')).Count();
            
            var remainedWater = fillWaterMap.Select((l, i) => (l, i)).Where(li => li.i >= minY).SelectMany(li => li.l.Where(c => c == '~')).Count();
            return (filledWater, remainedWater);
        }

        private static bool IsFilled(char[][] fillWaterMap, int i, int j)
        {
            int leftBound = -1;
            for (int k = j - 1; k > -1; k--)
            {
                if (fillWaterMap[i][k] == '.')
                    return false;

                if (fillWaterMap[i][k] == '#')
                {
                    leftBound = k;
                    break;
                }
            }

            if (leftBound < 0) return false;

            int rightBound = -1;
            for (int k = j + 1; k < fillWaterMap[i].Count(); k++)
            {
                if (fillWaterMap[i][k] == '.')
                    return false;

                if (fillWaterMap[i][k] == '#')
                {
                    rightBound = k;
                    break;
                }
            }

            if (leftBound > -1 && rightBound > -1)
            {
                for (int k = leftBound + 1; k < rightBound; k++)
                {
                    if (fillWaterMap[i][k] == '.')
                        fillWaterMap[i][k] = 'o';
                }
                return true;
            }
            else
                return false;
        }

        public static (char[][] UndergroundMap, int MinX, int MinY) PrepareInput(bool isTest)
        {
            var veinsLines = isTest ? ReadTestInput() : ReadInput();
            var veins = new List<(int fromX, int toX, int fromY, int toY)>();

            var r = new System.Text.RegularExpressions.Regex(@"(\w)=(\d+), (\w)=(\d+)\.\.(\d+)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            foreach (var veinsLine in veinsLines)
            {
                var match = r.Match(veinsLine);
                if (match.Success)
                {
                    if (match.Groups[1].Value == "x")
                        veins.Add((
                            int.Parse(match.Groups[2].Value),
                            int.Parse(match.Groups[2].Value),
                            int.Parse(match.Groups[4].Value),
                            int.Parse(match.Groups[5].Value)));
                    else
                        veins.Add((
                            int.Parse(match.Groups[4].Value),
                            int.Parse(match.Groups[5].Value),
                            int.Parse(match.Groups[2].Value),
                            int.Parse(match.Groups[2].Value)));
                }
            }

            var minX = veins.Select(v => v.fromX).Min();
            var maxX = veins.Select(v => v.toX).Max();
            var minY = veins.Select(v => v.fromY).Min();
            var maxY = veins.Select(v => v.toY).Max();

            var undergroundMap = new char[maxY + 1][];
            for (int i = 0; i < undergroundMap.Count(); i++)
            {
                undergroundMap[i] = Enumerable.Repeat('.', BorderWidth * 2 + maxX - minX + 1).ToArray();
            }

            foreach (var vein in veins)
            {
                for (int y = vein.fromY; y < vein.toY + 1; y++)
                {
                    for (int x = BorderWidth + vein.fromX - minX; x < BorderWidth + vein.toX - minX + 1; x++)
                    {
                        undergroundMap[y][x] = '#';
                    }
                }
            }

            undergroundMap[0][500 - minX + 1] = '+';

            return (undergroundMap, minX, minY);
        }
        public static List<string> ReadTestInput()
        {
            var veinsLines = new List<string>
            {
                "x=495, y=2..7",
                "y=7, x=495..501",
                "x=501, y=3..7",
                "x=498, y=2..4",
                "x=506, y=1..2",
                "x=498, y=10..13",
                "x=504, y=10..13",
                "y=13, x=498..504",
            };
            return veinsLines;
        }
        public static List<string> ReadInput()
        {
            var veinsLines = new List<string>();

            const Int32 BufferSize = 128;
            using (var fileStream = System.IO.File.OpenRead(@"..\..\Input\AdventOfCode17.txt"))
            using (var streamReader = new System.IO.StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                String line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    veinsLines.Add(line);
                }
            }
            return veinsLines;
        }
        public static void PrintUndergroundMap(char[][] undergroundMap, int minX, int minY)
        {
            var memConsoleForegroundColor = Console.ForegroundColor;

            foreach (var n in new List<int> { 100, 10, 1 })
            {
                Console.Write("".PadRight((int)Math.Log10(undergroundMap.Length) + 1));
                foreach (var i in Enumerable.Range(minX - BorderWidth, undergroundMap.Select(l => l.Length).Max()))
                    Console.Write((i / n == 0) ? " " : (i / n - i / n / 10 * 10).ToString());
                Console.WriteLine();
            }

            foreach (var (l, i) in undergroundMap.Select((l, i) => (l, i)))
            {
                Console.Write(i.ToString().PadLeft((int)Math.Log10(undergroundMap.Length) + 1, ' '));
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
        public static void SaveUndergroundMap(char[][] undergroundMap, int minX, int minY)
        {
            const Int32 BufferSize = 128;
            using (var fileStream = System.IO.File.OpenWrite(@"..\..\Input\AdventOfCode17_result.txt"))
            using (var streamWriter = new System.IO.StreamWriter(fileStream, Encoding.UTF8, BufferSize))
            {
                foreach (var n in new List<int> { 100, 10, 1 })
                {
                    streamWriter.Write("".PadRight((int)Math.Log10(undergroundMap.Length) + 1));
                    foreach (var i in Enumerable.Range(minX - BorderWidth, undergroundMap.Select(l => l.Length).Max()))
                        streamWriter.Write((i / n == 0) ? " " : (i / n - i / n / 10 * 10).ToString());
                    streamWriter.WriteLine();
                }

                foreach (var (l, i) in undergroundMap.Select((l, i) => (l, i)))
                {
                    streamWriter.Write(i.ToString().PadLeft((int)Math.Log10(undergroundMap.Length) + 1, ' '));
                    foreach (var (c, j) in l.Select((c, j) => (c, j)))
                    {
                        streamWriter.Write(c);
                    }
                    streamWriter.WriteLine();
                }

                streamWriter.WriteLine();
            }
            return;
        }

    }
}
