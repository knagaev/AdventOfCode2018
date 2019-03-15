using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsConsoleApplication
{
    class AdventOfCode10
    {
        public static void Run1(bool isTest = true)
        {
            var points = PrepareInput(isTest);
            int counter = 0;
            while (true)
            {
                counter++;
                for (int i = 0; i < points.Length; i++)
                {
                    points[i].X += points[i].velocityX;
                    points[i].Y += points[i].velocityY;
                }

                var minX = points.Select(p => p.X).Min();
                var maxX = points.Select(p => p.X).Max();
                var minY = points.Select(p => p.Y).Min();
                var maxY = points.Select(p => p.Y).Max();


                if (maxX - minX < 80 && maxY - minY < 40)
                {
                    var canvas = new int[maxX - minX + 1, maxY - minY + 1];
                    foreach (var point in points)
                    {
                        canvas[point.X - minX, point.Y - minY] = 1;
                    }
                    VisualizeCanvas(canvas, counter);
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
                }
            }
        }
        public static void VisualizeCanvas(int[,] canvas, int counter)
        {
            Console.WriteLine();
            for (int row = 0; row < canvas.GetLength(1); row++)
            {
                for (int col = 0; col < canvas.GetLength(0); col++)
                {
                    Console.Write(canvas[col, row] > 0 ? "#" : ".");
                }
                Console.WriteLine();
            }
            Console.WriteLine(counter);
        }

        public static (int X, int Y, int velocityX, int velocityY)[] PrepareInput(bool isTest)
        {
            var lines = isTest ? ReadTestInput() : ReadInput();

            // position=< 9,  1> velocity=< 0,  2>
            var r = new System.Text.RegularExpressions.Regex(@"position=<[ ]*([-]{0,1}\d+),[ ]*([-]{0,1}\d+)> velocity=<[ ]*([-]{0,1}\d+),[ ]*([-]{0,1}\d+)>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            var points = new List<(int X, int Y, int velocityX, int velocityY)>(lines.Count);
            foreach(var line in lines)
            {
                var match = r.Match(line);
                if (match.Success)
                {
                    points.Add((
                                    int.Parse(match.Groups[1].Value),
                                    int.Parse(match.Groups[2].Value),
                                    int.Parse(match.Groups[3].Value),
                                    int.Parse(match.Groups[4].Value)
                               ));
                }
            }
            return points.ToArray();
        }
        public static List<string> ReadTestInput()
        {
            var input = new List<string>
            {
                "position=< 9,  1> velocity=< 0,  2>",
                "position=< 7,  0> velocity=<-1,  0>",
                "position=< 3, -2> velocity=<-1,  1>",
                "position=< 6, 10> velocity=<-2, -1>",
                "position=< 2, -4> velocity=< 2,  2>",
                "position=<-6, 10> velocity=< 2, -2>",
                "position=< 1,  8> velocity=< 1, -1>",
                "position=< 1,  7> velocity=< 1,  0>",
                "position=<-3, 11> velocity=< 1, -2>",
                "position=< 7,  6> velocity=<-1, -1>",
                "position=<-2,  3> velocity=< 1,  0>",
                "position=<-4,  3> velocity=< 2,  0>",
                "position=<10, -3> velocity=<-1,  1>",
                "position=< 5, 11> velocity=< 1, -2>",
                "position=< 4,  7> velocity=< 0, -1>",
                "position=< 8, -2> velocity=< 0,  1>",
                "position=<15,  0> velocity=<-2,  0>",
                "position=< 1,  6> velocity=< 1,  0>",
                "position=< 8,  9> velocity=< 0, -1>",
                "position=< 3,  3> velocity=<-1,  1>",
                "position=< 0,  5> velocity=< 0, -1>",
                "position=<-2,  2> velocity=< 2,  0>",
                "position=< 5, -2> velocity=< 1,  2>",
                "position=< 1,  4> velocity=< 2,  1>",
                "position=<-2,  7> velocity=< 2, -2>",
                "position=< 3,  6> velocity=<-1, -1>",
                "position=< 5,  0> velocity=< 1,  0>",
                "position=<-6,  0> velocity=< 2,  0>",
                "position=< 5,  9> velocity=< 1, -2>",
                "position=<14,  7> velocity=<-2,  0>",
                "position=<-3,  6> velocity=< 2, -1>",
            };
            return input;
        }
        public static List<string> ReadInput()
        {
            var input = new List<string>();

            const Int32 BufferSize = 128;
            using (var fileStream = System.IO.File.OpenRead(@"..\..\Input\AdventOfCode10.txt"))
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
