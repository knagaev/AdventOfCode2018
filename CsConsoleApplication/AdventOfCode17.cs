﻿using System;
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
            var (undergroundMap, minX) = PrepareInput1(isTest);
            //var inputs = isTest? PrepareInput1(ReadTestInput1) : PrepareInput1(ReadInput1);

            PrintUndergroundMap(undergroundMap, minX);

            var filledTiles = FillWater(undergroundMap, minX, isTest);

            Console.WriteLine(String.Format("Water can reach {0} tiles", filledTiles));
            Console.ReadLine();
        }

        public static int FillWater(char[][] undergroundMap, int minX, bool isTest)
        {
            var fillWaterMap = undergroundMap.Select(l => l.ToArray()).ToArray();

            var springPosition = fillWaterMap[0].Select((c, j) => (c, j)).Where(cj => cj.c == '+').Select(cj => cj.j).Single();
            fillWaterMap[1][springPosition] = '~';

            bool updated = false;

            while (true)
            {
                if (isTest)
                    PrintUndergroundMap(fillWaterMap, minX);
                //Console.ReadLine();

                updated = false;
                for (int i = fillWaterMap.Count() - 2; i > 0; i--)
                {
                    for (int j = 1; j < fillWaterMap[i].Count() - 1; j++)
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
                                        IsBounded(fillWaterMap, i + 1, j)))
                                {
                                    if (fillWaterMap[i][j + 1] == '.')
                                    {
                                        fillWaterMap[i][j + 1] = '~';
                                        updated = true;
                                    }
                                    if (fillWaterMap[i][j - 1] == '.')
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

            var filledWater = fillWaterMap.SelectMany(l => l.Where(c => c == '~')).Count();
            return filledWater;
        }

        private static bool IsBounded(char[][] fillWaterMap, int i, int j)
        {
            bool boundedLeft = false;
            for (int k = j - 1; k > -1; k--)
            {
                if (fillWaterMap[i][k] == '.')
                    return false;

                if (fillWaterMap[i][k] == '#')
                {
                    boundedLeft = true;
                    break;
                }
            }

            if (!boundedLeft) return false;

            bool boundedRight = false;
            for (int k = j + 1; k < fillWaterMap[i].Count(); k++)
            {
                if (fillWaterMap[i][k] == '.')
                    return false;

                if (fillWaterMap[i][k] == '#')
                {
                    boundedRight = true;
                    break;
                }
            }
            return boundedRight;
        }

        public static (char[][] UndergroundMap, int MinX) PrepareInput1(bool isTest)
        {
            var veinsLines = isTest ? ReadTestInput1() : ReadInput1();
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

            var minX = veins.Select(v => v.toX).Min();
            var maxX = veins.Select(v => v.toX).Max();
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

            return (undergroundMap, minX);
        }
        public static List<string> ReadTestInput1()
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
        public static List<string> ReadInput1()
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
        public static void PrintUndergroundMap(char[][] undergroundMap, int minX)
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

    }
}
