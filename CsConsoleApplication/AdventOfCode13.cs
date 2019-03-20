using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsConsoleApplication
{
    class AdventOfCode13
    {
        public class CartState
        {
            public int i { get; set; }
            public int j { get; set; }
            public char Direction { get; set; }
            public char LastTurn { get; set; }

            public CartState(){}

            public CartState(CartState cartState)
            {
                this.i = cartState.i;
                this.j = cartState.j;
                this.Direction = cartState.Direction;
                this.LastTurn = cartState.LastTurn;
            }
        };

        private static Dictionary<string, char> _roads = new Dictionary<string, char>
        {
            {"^/",  '>'},
            {">/",  '^'},
            {"v/",  '<'},
            {"</",  'v'},
            {"^\\",	'<'},
            {">\\",	'v'},
            {"v\\",	'>'},
            {"<\\",	'^'},
            {"^|",  '^'},
            {">|",  '*'},
            {"v|",  'v'},
            {"<|",  '*'},
            {"^-",  '*'},
            {">-",  '>'},
            {"v-",  '*'},
            {"<-",  '<'},
        };

        private static Dictionary<string, string> _crosses = new Dictionary<string, string>
        {
            {"^s",  "<l"},
            {">s",  "^l"},
            {"vs",  ">l"},
            {"<s",  "vl"},
            {"^l",  "^f"},
            {">l",  ">f"},
            {"vl",  "vf"},
            {"<l",  "<f"},
            {"^f",  ">r"},
            {">f",  "vr"},
            {"vf",  "<r"},
            {"<f",  "^r"},
            {"^r",  "^s"},
            {">r",  ">s"},
            {"vr",  "vs"},
            {"<r",  "<s"},
        };

        private static char[][] _cartsMap;
        private static List<CartState> _cartStates;

        public static void Run1(bool isTest = true)
        {
            PrepareInput(isTest);

            if (isTest)
                PrintMapState();

            while (true)
            {
                foreach (var cartState in _cartStates.OrderBy(cs => cs.i).ThenBy(cs => cs.j))
                {
                    var newCartState = ChangeDirection(cartState, _cartsMap[cartState.i][cartState.j]);

                    cartState.Direction = newCartState.Direction;
                    cartState.LastTurn = newCartState.LastTurn;

                    switch (cartState.Direction)
                    {
                        case '^':
                            cartState.i--;
                            break;
                        case '>':
                            cartState.j++;
                            break;
                        case 'v':
                            cartState.i++;
                            break;
                        case '<':
                            cartState.j--;
                            break;
                        default:
                            throw new Exception("Wrong direction " + cartState.Direction);
                    }

                    var crash = _cartStates
                        .GroupBy(cs => new { cs.i, cs.j })
                        .Select(group => new { group.Key, Count = group.Count() })
                        .Where(grp => grp.Count > 1)
                        .Select(grp => new { grp.Key.i, grp.Key.j })
                        .SingleOrDefault();

                    if (crash != null)
                    {
                        if (true)
                            PrintMapState();

                        Console.WriteLine(String.Format("First crash at {0},{1}", crash.j, crash.i));
                        Console.ReadLine();
                    }
                }

                if (true)
                    PrintMapState();

                if (!isTest) PrintCartStates();
            }
        }
        public static void PrintMapState()
        {
            var copyCartsMap = _cartsMap.Select(l => l.ToList()).ToList();

            foreach (var cartState in _cartStates)
            {
                copyCartsMap[cartState.i][cartState.j] = cartState.Direction;
            }

            foreach (var line in copyCartsMap)
            {
                foreach (var c in line)
                {
                    if ("^>v<".Contains(c))
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    else
                        Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write(c);
                }
                Console.WriteLine();
            }

            Console.WriteLine();
            return;
        }

        public static void PrintCartStates()
        {
            Console.WriteLine(string.Join("", _cartStates.Select(cs => "(" + cs.i + "," + cs.j + "," + cs.Direction + ")")));
            return;
        }

        private static CartState ChangeDirection(CartState cartState, char roadType)
        {
            var newCartState = new CartState(cartState);

            if (roadType == '+')
            {
                string stateOnCross = "" + cartState.Direction + cartState.LastTurn;
                var newStateOnCross = _crosses[stateOnCross];
                newCartState.Direction = newStateOnCross[0];
                newCartState.LastTurn = newStateOnCross[1];
            }
            else
            {
                string stateOnRoad = "" + cartState.Direction + roadType;
                newCartState.Direction = _roads[stateOnRoad];
                if (newCartState.Direction == '*')
                    throw new Exception("Wrong stateOnRoad " + stateOnRoad);
            }

            return newCartState;
        }

        //public static (char[][] CartsMap, List<CartState> InitialCartStates) PrepareInput(bool isTest)
        public static void PrepareInput(bool isTest)
        {
            var input = isTest ? ReadTestInput() : ReadInput();

            var maxWidth = input.Max(l => l.Length);

            var cartsMap = new char[input.Count][];
            foreach (var (line, i) in input.Select((line, i) => (line, i)))
            {
                cartsMap[i] = line.ToArray();
            }

            var initialCartStates = cartsMap
                .SelectMany((l, i) => l.Select((c, j) => new { i, j, c }))
                .Where(ijc => "^>v<".Contains(ijc.c))
                .Select(ijc => new CartState
                                {
                                    i = ijc.i,
                                    j = ijc.j, 
                                    //Direction: "^>v<".Select((cc, ci) => (cc, ci)).Where(ccic => ccic.cc == ijc.c).Select(ccic => ccic.ci).Single(),
                                    Direction = ijc.c,
                                    LastTurn = 's'
                                })     // option on the last cross: l - left, f - first straight, r - right, s - second straight
                .ToList();

           var neighbours = initialCartStates
                .Select(ics => (ics,
                                CrossItems: "" + ((ics.i == 0 || "- ".Contains(cartsMap[ics.i - 1][ics.j])) ? ' ' : '*')
                                            + ((ics.j == cartsMap[ics.i].Length - 1 || "| ".Contains(cartsMap[ics.i][ics.j + 1])) ? ' ' : '*')
                                            + ((ics.i == cartsMap.Length - 1 || "- ".Contains(cartsMap[ics.i + 1][ics.j])) ? ' ' : '*')
                                            + ((ics.j == 0 || "| ".Contains(cartsMap[ics.i][ics.j - 1])) ? ' ' : '*')))
                .ToList();

            foreach (var neighbour in neighbours)
            {
                switch (neighbour.CrossItems)
                {
                    case "* * ":    // road from up to down
                        cartsMap[neighbour.ics.i][neighbour.ics.j] = '|';
                        break;

                    case " * *":    // road from right to left
                        cartsMap[neighbour.ics.i][neighbour.ics.j] = '-';
                        break;

                    case "****":    // road from all sides
                        cartsMap[neighbour.ics.i][neighbour.ics.j] = '+';
                        break;

                    case "**  ":    // road from up to right
                    case "  **":    // and from left to down
                        cartsMap[neighbour.ics.i][neighbour.ics.j] = '\\';
                        break;

                    case " ** ":    // road down to right
                    case "*  *":    // and from up to left
                        cartsMap[neighbour.ics.i][neighbour.ics.j] = '/';
                        break;

                }
            }

            _cartsMap = cartsMap;
            _cartStates = initialCartStates;

            return;
        }

        public static List<string> ReadTestInput()
        {
            var input = new List<string>
            {
                @"/->-\        ",
                @"|   |  /----\",
                @"| /-+--+-\  |",
                @"| | |  | v  |",
                @"\-+-/  \-+--/",
                @"  \------/   ",
            };
            return input;
        }
        public static List<string> ReadInput()
        {
            var input = new List<string>();

            const Int32 BufferSize = 128;
            using (var fileStream = System.IO.File.OpenRead(@"..\..\Input\AdventOfCode13.txt"))
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
