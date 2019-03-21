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
            {"^r",  "<l"},
            {">r",  "^l"},
            {"vr",  ">l"},
            {"<r",  "vl"},
            {"^l",  "^s"},
            {">l",  ">s"},
            {"vl",  "vs"},
            {"<l",  "<s"},
            {"^s",  ">r"},
            {">s",  "vr"},
            {"vs",  "<r"},
            {"<s",  "^r"},
        };

        private static char[][] _cartsMap;
        private static List<CartState> _cartStates;

        public static void Run1(bool isTest = true)
        {
            if (isTest)
                PrepareInput(ReadTestInput1);
            else
                PrepareInput(ReadInput);

            if (isTest)
                PrintMapState();

            var crashes = GetCrashes(isTest).FirstOrDefault();

            //var firstCrash = crashes.OrderBy(c => c.i).ThenBy(c => c.j).FirstOrDefault();
            var firstCrash = crashes.FirstOrDefault();

            Console.WriteLine(String.Format("First crash at {0},{1}", firstCrash.j, firstCrash.i));
            Console.ReadLine();

        }
        public static void Run2(bool isTest = true)
        {
            if (isTest)
                PrepareInput(ReadTestInput2);
            else
                PrepareInput(ReadInput);

            if (isTest)
                PrintMapState();

            foreach (var crashes in GetCrashes(isTest))
            {
                foreach (var crash in crashes)
                {
                    Console.WriteLine(String.Format("Crash at {0},{1}", crash.j, crash.i));
                }
                if (_cartStates.Count() < 2)
                {
                    var theLastCart = _cartStates.Single();
                    Console.WriteLine(String.Format("The last cart at {0},{1}", theLastCart.j, theLastCart.i));
                    Console.ReadLine();
                    return;
                }
            }
        }

        public static IEnumerable<List<(int i, int j)>> GetCrashes(bool isTest)
        {
            while (_cartStates.Count() > 0)
            {
                foreach (var cartState in _cartStates.OrderBy(cs => cs.i).ThenBy(cs => cs.j))
                {
                    if (cartState.Direction == 'X') continue;

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
                        _cartStates.Where(cs => cs.i == crash.i && cs.j == crash.j).ToList()
                            .ForEach(cs => cs.Direction = 'X');
                    }
                }

                if (_cartStates.Any(cs => cs.Direction == 'X'))
                {
                    PrintMapState();

                    var crashes = _cartStates.Where(cs => cs.Direction == 'X').Select(cs => (cs.i, cs.j)).ToList();
                    _cartStates.RemoveAll(cs => cs.Direction == 'X');

                    yield return crashes;
                }

                if (isTest) PrintMapState();

                //if (!isTest) PrintCartStates();
            }
        }

        public static void PrintMapState()
        {
            var memConsoleForegroundColor = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.White;
            foreach (var n in new List<int> { 100, 10, 1 })
            {
                Console.Write("".PadRight((int)Math.Log10(_cartsMap.Length) + 1));
                foreach (var i in Enumerable.Range(0, _cartsMap.Select(l => l.Length).Max()))
                    Console.Write((i / n == 0) ? " " : (i / n - i / n / 10 * 10).ToString());
                Console.WriteLine();
            }

            var copyCartsMap = _cartsMap.Select(l => l.ToList()).ToList();

            foreach (var cartState in _cartStates)
            {
                //if ("^>v<".Contains(copyCartsMap[cartState.i][cartState.j]))
                //    copyCartsMap[cartState.i][cartState.j] = 'X';
                //else
                    copyCartsMap[cartState.i][cartState.j] = cartState.Direction;
            }

            foreach (var (line, i) in copyCartsMap.Select((line, i) => (line, i)))
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(i.ToString().PadLeft((int)Math.Log10(_cartsMap.Length) + 1, ' '));
                foreach (var c in line)
                {
                    if ("^>v<X".Contains(c))
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    else
                        Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write(c);
                }
                Console.WriteLine();
            }

            Console.WriteLine();
            Console.ForegroundColor = memConsoleForegroundColor;
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
        public static void PrepareInput(Func<List<string>> readInput)
        {
            //var input = isTest ? ReadTestInput() : ReadInput();
            var input = readInput();

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
                                    LastTurn = 'r'
                                })     // option on the previous cross: l - left, s - straight, r - right
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

        public static List<string> ReadTestInput1()
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
        public static List<string> ReadTestInput2()
        {
            var input = new List<string>
            {
                @"/>-<\  ",
                @"|   |  ",
                @"| /<+-\",
                @"| | | v",
                @"\>+</ |",
                @"  |   ^",
                @"  \<->/",
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
