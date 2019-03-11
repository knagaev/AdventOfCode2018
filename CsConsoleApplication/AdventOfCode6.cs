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
            var grid = PrepareInput(isTest);

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

        public static int[,] PrepareInput(bool isTest)
        {
            var points = isTest ? ReadTestInput() : ReadInput();

            var maxX = points.Select(p => p.Item1).Max();
            var maxY = points.Select(p => p.Item2).Max();

            var grid = new int[maxX + 1, maxY + 1];

            foreach (var point in points.Select((p, i) => new { p, i }))
                grid[point.p.Item1, point.p.Item2] = point.i + 1;

            return grid;
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