using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsConsoleApplication
{
    class AdventOfCode6
    {

        public static void Run1(bool isTest = true)
        {
            var points = PrepareInput(isTest);

            var maxX = points.Select(p => p.Item1).Max();
            var maxY = points.Select(p => p.Item2).Max();

            var areaGrid = new (int, int)[maxX + 1, maxY + 1];

            if (isTest) // visualize start position
            {
                foreach (var point in points)//.Select((p, i) => new { p, i })
                    areaGrid[point.Item1, point.Item2] = point;
                VisualizeAreaGrid(areaGrid);
            }

            for (int row = 0; row < areaGrid.GetLength(0); row++)
            {
                for (int col = 0; col < areaGrid.GetLength(1); col++)
                {
                    var manhDists = points
                        .Select(p => new { p, md = ManhattanDistance(p, (row, col)) })
                        .OrderBy(pd => pd.md)
                        .Take(2)
                        .GroupBy(pd => pd.md)
                        .Select(group => new { p = group.Min(g => g.p), md = group.Key })
                        .OrderByDescending(pd => pd.md)
                        .Skip(1)
                        .Select(pd => pd.p)
                        .SingleOrDefault();

                    areaGrid[row, col] = manhDists;
                }
            }
            if (isTest)
                VisualizeAreaGrid(areaGrid);

            var xLimit = Enumerable.Range(0, areaGrid.GetUpperBound(0) + 1);
            var yLimit = Enumerable.Range(0, areaGrid.GetUpperBound(1) + 1);

            var borderPoints = xLimit
                .SelectMany(x => yLimit
                    .Where(y => (x == xLimit.First()) || (x == xLimit.Last() || y == yLimit.First()) || (y == yLimit.Last()))
                    .Select(y => areaGrid[x, y]))
                    .Distinct()
                    .ToHashSet();

            for (int row = 0; row < areaGrid.GetLength(0); row++)
            {
                for (int col = 0; col < areaGrid.GetLength(1); col++)
                {
                    if (borderPoints.Contains(areaGrid[row, col]))
                        areaGrid[row, col] = (0, 0);
                }
            }
            if (isTest)
                VisualizeAreaGrid(areaGrid);

            var maxArea = areaGrid.Cast<(int, int)>()
                .Where(p => p.Item1 != 0 && p.Item2 != 0)
                .GroupBy(p => p)
                .Select(group => new { p = group.Key, area = group.Count()})
                .Aggregate((curMaxRegion, region) => region.area > curMaxRegion.area ? region : curMaxRegion);

            Console.WriteLine(String.Format("Point {0} area {1}", maxArea.p, maxArea.area));
            Console.ReadLine();
        }
        public static void Run2(bool isTest = true)
        {
            var points = PrepareInput(isTest);

            var maxX = points.Select(p => p.Item1).Max();
            var maxY = points.Select(p => p.Item2).Max();

            var totalGrid = new int[maxX + 1, maxY + 1];

            for (int row = 0; row < totalGrid.GetLength(0); row++)
            {
                for (int col = 0; col < totalGrid.GetLength(1); col++)
                {
                    var totalManhDists = points
                        .Select(p => ManhattanDistance(p, (row, col)))
                        .Sum();

                    totalGrid[row, col] = totalManhDists;
                }
            }
            if (isTest)
                VisualizeTotalGrid(totalGrid);

            var xLimit = Enumerable.Range(0, totalGrid.GetUpperBound(0) + 1);
            var yLimit = Enumerable.Range(0, totalGrid.GetUpperBound(1) + 1);

            int maxDistance = isTest ? 32 : 10000;

            var regionSize = xLimit
                .SelectMany(x => yLimit.Where(y => totalGrid[x, y] < maxDistance))
                .Count();

            Console.WriteLine(String.Format("Region size {0}", regionSize));
            Console.ReadLine();
        }

        public static int ManhattanDistance((int, int) p1, (int, int) p2)
        {
            return Math.Abs(p1.Item1 - p2.Item1) + Math.Abs(p1.Item2 - p2.Item2);
        }

        public static void VisualizeAreaGrid((int, int)[,] grid)
        {
            Console.WriteLine();
            for (int row = 0; row < grid.GetLength(1); row++)
            {
                for (int col = 0; col < grid.GetLength(0); col++)
                {
                    Console.Write(grid[col, row]);
                }
                Console.WriteLine();
            }
            Console.ReadLine();
        }

        public static void VisualizeTotalGrid(int[,] grid)
        {
            Console.WriteLine();
            for (int row = 0; row < grid.GetLength(1); row++)
            {
                for (int col = 0; col < grid.GetLength(0); col++)
                {
                    Console.Write(grid[col, row]);
                }
                Console.WriteLine();
            }
            Console.ReadLine();
        }

        public static List<(int, int)> PrepareInput(bool isTest)
        {
            var points = isTest ? ReadTestInput() : ReadInput();

            return points;
        }
        public static List<(int, int)> ReadTestInput()
        {
            var input = new List<(int, int)>
            {
                (1, 1),
                (1, 6),
                (8, 3),
                (3, 4),
                (5, 5),
                (8, 9),
            };

            return input;
        }
        public static List<(int, int)> ReadInput()
        {
            var input = new List<(int, int)>();

            const Int32 BufferSize = 128;
            using (var fileStream = System.IO.File.OpenRead(@"..\..\AdventOfCode\AdventOfCode6.txt"))
            using (var streamReader = new System.IO.StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                String line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    var xy = line.Split(',');
                    input.Add((int.Parse(xy[0].Trim()), int.Parse(xy[1].Trim())));
                }
            }

            return input;
        }
    }
}