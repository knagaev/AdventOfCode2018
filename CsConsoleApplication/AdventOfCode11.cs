using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsConsoleApplication
{
    class AdventOfCode11
    {
        private const int GridSize = 300;

        public static void Run1(bool isTest = true)
        {
            var cellPowers = PrepareInput(isTest);

            if (isTest)
            {
                foreach (var cellPower in cellPowers)
                {
                    int calculatedPowerLevel = CalculatePowerLevel(cellPower.X, cellPower.Y, cellPower.GridSerialNumber);
                    Console.WriteLine(String.Format("Test {0},{1} serial {2} must be {3} calculated {4}",
                                                        cellPower.X, cellPower.Y, cellPower.GridSerialNumber, cellPower.PowerLevel, calculatedPowerLevel));
                }
                Console.ReadLine();
            }
            else
            {
                foreach (var cellPower in cellPowers)
                {
                    var grid = new int[GridSize + 1, GridSize + 1];

                    for (int i = 1; i <= GridSize; i++)
                    {
                        for (int j = 1; j <= GridSize; j++)
                        {
                            grid[i, j] = CalculatePowerLevel(i, j, cellPower.GridSerialNumber);
                        }
                    }

                    var summedAreaGrid = new int[GridSize + 1, GridSize + 1];

                    for (int i = 1; i <= GridSize; i++)
                    {
                        for (int j = 1; j <= GridSize; j++)
                        {
                            summedAreaGrid[i, j] = grid[i, j]
                                + summedAreaGrid[i - 1, j]
                                + summedAreaGrid[i, j - 1]
                                - summedAreaGrid[i - 1, j - 1];
                        }
                    }

                    if (true)
                    {
                        PrintGridState(summedAreaGrid, (0, 0));
                    }

                    int maxX = 0, maxY = 0;
                    int totalPower = int.MinValue;
                    var powers = new int[GridSize - 2, GridSize - 2];

                    for (int i = 3; i <= GridSize; i++)
                    {
                        for (int j = 3; j <= GridSize; j++)
                        {
                            int power = summedAreaGrid[i, j]
                                    + summedAreaGrid[i - 3, j - 3]
                                    - summedAreaGrid[i - 3, j] 
                                    - summedAreaGrid[i, j - 3];
                            if (totalPower < power)
                            {
                                totalPower = power;
                                maxX = i;
                                maxY = j;
                            }
                        }
                    }

                    Console.WriteLine(String.Format("Max power {0} at {1},{2}", totalPower, maxX - 2, maxY - 2));
                    Console.ReadLine();
                }
            }
        }
        public static void Run2(bool isTest = true)
        {
            var cellPowers = PrepareInput(isTest);

            foreach (var cellPower in cellPowers)
            {
                var grid = new int[GridSize + 1, GridSize + 1];

                for (int i = 1; i <= GridSize; i++)
                {
                    for (int j = 1; j <= GridSize; j++)
                    {
                        grid[i, j] = CalculatePowerLevel(i, j, cellPower.GridSerialNumber);
                    }
                }

                var summedAreaGrid = new int[GridSize + 1, GridSize + 1];

                for (int i = 1; i <= GridSize; i++)
                {
                    for (int j = 1; j <= GridSize; j++)
                    {
                        summedAreaGrid[i, j] = grid[i, j]
                            + summedAreaGrid[i - 1, j]
                            + summedAreaGrid[i, j - 1]
                            - summedAreaGrid[i - 1, j - 1];
                    }
                }

                if (true)
                {
                    PrintGridState(summedAreaGrid, (0, 0));
                }

                int maxX = 0, maxY = 0, maxSquareSize = 0;
                int totalPower = int.MinValue;

                for (int squareSize = 1; squareSize <= GridSize; squareSize++)
                {
                    for (int i = squareSize; i <= GridSize; i++)
                    {
                        for (int j = squareSize; j <= GridSize; j++)
                        {
                            int power = summedAreaGrid[i, j]
                                    + summedAreaGrid[i - squareSize, j - squareSize]
                                    - summedAreaGrid[i - squareSize, j]
                                    - summedAreaGrid[i, j - squareSize];
                            if (totalPower < power)
                            {
                                totalPower = power;
                                maxX = i - squareSize + 1;
                                maxY = j - squareSize + 1;
                                maxSquareSize = squareSize;
                            }
                        }
                    }
                }

                Console.WriteLine(String.Format("GridSerialNumber {0} Max power {1} at {2},{3},{4}", cellPower.GridSerialNumber, totalPower, maxX, maxY, maxSquareSize));
                Console.ReadLine();
            }
        }

        public static void PrintGridState(int[,] grid, (int Left, int Top) point)
        {
            Console.WriteLine();
            const int sizeX = 40;
            const int sizeY = 50;
            for (int row = point.Top; row < point.Top + sizeY; row++)
            {
                for (int col = point.Left; col < point.Left + sizeX; col++)
                {
                    Console.ForegroundColor = (ConsoleColor)grid[col, row] + 7;
                    Console.Write(grid[col, row] < 0 ? " " + grid[col, row] : "  " + grid[col, row]);
                }
                Console.WriteLine();
            }
            Console.ReadLine();
        }

        public static int CalculatePowerLevel(int X, int Y, int GridSerialNumber)
        {
            int powerLevel = ((X + 10) * Y + GridSerialNumber) * (X + 10) / 100 - ((X + 10) * Y + GridSerialNumber) * (X + 10) / 1000 * 10 - 5;
            return powerLevel;
        }

        public static List<(int X, int Y, int GridSerialNumber, int PowerLevel)> PrepareInput(bool isTest)
        {
            var lines = isTest ? ReadTestInput() : ReadInput();

            // Fuel cell at  3,5, grid serial number 8: power level 4.
            var r = new System.Text.RegularExpressions.Regex(@"Fuel cell at[ ]+(\d+),(\d+), grid serial number (\d+): power level[ ]+([-]{0,1}\d+)\.", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            var points = new List<(int X, int Y, int GridSerialNumber, int PowerLevel)>(lines.Count);
            foreach (var line in lines)
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
            return points.ToList();
        }

        public static List<string> ReadTestInput()
        {
            var input = new List<string>
            {
                "Fuel cell at  3,5, grid serial number 8: power level 4.",
                "Fuel cell at  122,79, grid serial number 57: power level -5.",
                "Fuel cell at 217,196, grid serial number 39: power level  0.",
                "Fuel cell at 101,153, grid serial number 71: power level  4.",
            };
            return input;
        }
        public static List<string> ReadInput()
        {
            var input = new List<string>
            {
                "Fuel cell at  3,5, grid serial number 18: power level 4.",
                "Fuel cell at  3,5, grid serial number 42: power level 4.",
                "Fuel cell at  0,0, grid serial number 9445: power level 0.",
            };
            return input;
        }
    }
}
