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

            foreach (var parsedResult in parsedResults)
            {
                var calculatedHighScore = CalculateHighScore(parsedResult.PlayersQty, parsedResult.Points);
                Console.WriteLine(String.Format("High score is {0} {1}", parsedResult.HighScore, calculatedHighScore));
            }
            Console.ReadLine();
        }
        public static void Run2(bool isTest = true)
        {
            var parsedResults = PrepareInput(isTest);

            foreach (var parsedResult in parsedResults)
            {
                var calculatedHighScore = CalculateHighScore(parsedResult.PlayersQty, parsedResult.Points * 100);
                Console.WriteLine(String.Format("High score is {0} {1}", parsedResult.HighScore, calculatedHighScore));
            }
            Console.ReadLine();
        }

        private static long CalculateHighScore(int playersQty, int points)
        {
            var game = new DoubleLinkedList();
            var elves = new long[playersQty];

            for (int i = 0; i < points; i++)
            {
                int marble = i + 1;
                if (marble % 23 == 0)
                {
                    var seventhCcMarble = game.GetPrevious(7);
                    elves[marble % playersQty] += marble + seventhCcMarble;
                    game.Remove(seventhCcMarble);
                    Console.WriteLine(elves.Max());
                }
                else
                    game.Insert(i + 1);

                if (false)
                {
                    game.PrintState();
                }
            }

            return elves.Max();
        }

        public static List<(int PlayersQty, int Points, int HighScore)> PrepareInput(bool isTest)
        {
            var results = isTest ? ReadTestInput() : ReadInput();

            var r = new System.Text.RegularExpressions.Regex(@"(\d+) players; last marble is worth (\d+) points(: high score is (\d+))*", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            var parsedResults = new List<(int playersQty, int points, int highScore)>();

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
                "7 players; last marble is worth 25 points: high score is 32",
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

    class DoubleLinkedList
    {
        private int _current;
        private Dictionary<int, (int Previous, int Next)> _list;

        public DoubleLinkedList()
        {
            _list = new Dictionary<int, (int Previous, int Next)> { { 0, (0, 0) } };
            _current = 0;
        }

        public DoubleLinkedList(int element, int previous, int next)
        {
            _list = new Dictionary<int, (int Previous, int Next)> { { element, (previous, next) } };
            _current = element;
        }

        public void Insert(int element)
        {
            var after = _list[_current].Next;

            var next = _list[after].Next;

            if (next == after)
            {
                _list[after] = (element, element);
                _list[element] = (after, after);
            }
            else
            {
                _list[element] = (after, _list[after].Next);
                _list[after] = (_list[after].Previous, element);
                _list[next] = (element, _list[next].Next);
            }

            _current = element;
        }

        public void PrintState()
        {
            int pointer = 0;
            while (true)
            {
                Console.Write(" " + (pointer != _current ? pointer.ToString() : "(" + pointer + ")"));
                pointer = _list[pointer].Next;
                if (pointer == 0) break;
            }

            Console.WriteLine();
        }

        public void Remove(int element)
        {
            var previous = _list[element].Previous;
            var next = _list[element].Next;

            if (previous == next)
            {
                _list[previous] = (previous, next);
            }
            else
            {
                _list[previous] = (_list[previous].Previous, next);
                _list[next] = (previous, _list[next].Next);
            }

            _current = next;
            _list.Remove(element);
        }

        public int GetNext(int offset = 1)
        {
            int pointer = _current;
            for (int i = 0; i < offset; i++)
                pointer = _list[pointer].Next;

            return pointer;
        }

        public int GetPrevious(int offset = 1)
        {
            int pointer = _current;
            for (int i = 0; i < offset; i++)
                pointer = _list[pointer].Previous;

            return pointer;
        }
    }


}
