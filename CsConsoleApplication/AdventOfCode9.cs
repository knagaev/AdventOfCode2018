using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsConsoleApplication
{
    class AdventOfCode9
    {
        public static void Run1(bool isTest = true)
        {
            var parsedResults = PrepareInput(isTest);

            Console.WriteLine(String.Format("Sum of all metadata entries {0}", 1));
            Console.ReadLine();
        }

        public static List<(int players, int points, int highScore)> PrepareInput(bool isTest)
        {
            var results = isTest ? ReadTestInput() : ReadInput();

            var r = new System.Text.RegularExpressions.Regex(@"(\d+) players; last marble is worth (\d+) points(: high score is (\d+))*", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            var parsedResults = new List<(int players, int points, int highScore)>();

            foreach (var result in results)
            {
                var match = r.Match(result);
                if (match.Success)
                    parsedResults.Add((
                                    int.Parse(match.Groups[1].Value),
                                    int.Parse(match.Groups[2].Value),
                                    (string.IsNullOrWhiteSpace(match.Groups[4].Value)) ? 0 : int.Parse(match.Groups[4].Value)
                                    ));
            }

            return parsedResults;
        }
        public static List<string> ReadTestInput()
        {
            var input = new List<string>
            {
                "10 players; last marble is worth 1618 points: high score is 8317",
                "13 players; last marble is worth 7999 points: high score is 146373",
                "17 players; last marble is worth 1104 points: high score is 2764",
                "21 players; last marble is worth 6111 points: high score is 54718",
                "30 players; last marble is worth 5807 points: high score is 37305",
            };
            return input;
        }
        public static List<string> ReadInput()
        {
            const Int32 BufferSize = 128;
            using (var fileStream = System.IO.File.OpenRead(@"..\..\Input\AdventOfCode9.txt"))
            using (var streamReader = new System.IO.StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                string input = streamReader.ReadLine();
                return new List<string>{input};
            }

        }
    }
}
