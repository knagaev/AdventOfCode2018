using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsConsoleApplication
{
    class AdventOfCode12
    {
        private const int NoteLength = 5;

        public static void Run1(bool isTest = true)
        {
            Imitate(20, isTest);
        }
        public static void Run2(bool isTest = true)
        {
            Imitate(50000000000, isTest);
        }

        public static void Imitate(long generationsQuantity, bool isTest)
        {
            var input = PrepareInput(isTest);

            var curGen = input.InitialState;
            int offset = 0;

            for (long g = 0; g < generationsQuantity; g++)
            {
                var nextGen = curGen;

                int firstPlantPosition = nextGen.IndexOf('#');

                int curOffset = firstPlantPosition - (NoteLength - 1);

                if (curOffset < 0)
                    nextGen = new string('.', -curOffset) + nextGen;

                if (curOffset > 0)
                    nextGen = curGen.Substring(curOffset);

                offset += curOffset;

                int lastPlantPosition = nextGen.LastIndexOf('#');

                int extraTail = (nextGen.Length - NoteLength) - lastPlantPosition;

                if (extraTail < 0)
                    nextGen += new string('.', -extraTail);

                if (extraTail > 0)
                    nextGen = nextGen.Substring(0, nextGen.Length - extraTail);


                var test = nextGen
                            .Select((c, i) => new { c, i })
                            .Skip(NoteLength - 1)
                            .Select(ci => curGen.Substring(ci.i - (NoteLength - 1), NoteLength))
                            .ToList();

                curGen = ".." + string.Join("", nextGen
                                            .Select((c, i) => new { c, i })
                                            .Skip(NoteLength - 1)
                                            .Select(ci => input.Notes.ContainsKey(curGen.Substring(ci.i - (NoteLength - 1), NoteLength))
                                            ? input.Notes[curGen.Substring(ci.i - (NoteLength - 1), NoteLength)]
                                            : '.')) + "..";

                Console.WriteLine(curGen + " " + curOffset + " " + offset);
                if (curGen == nextGen) break;

                curGen = nextGen;
           }

            var plantQuantity = curGen.Select((c, i) => new { c, i }).Where(ci => ci.c == '#').Sum(ci => ci.i + offset);

            Console.WriteLine(String.Format("Sum of the numbers of all pots which contain a plant after 20 generations {0}", plantQuantity));
            Console.ReadLine();
        }

        public static (string InitialState, Dictionary<string, char> Notes) PrepareInput(bool isTest)
        {
            var input = isTest ? ReadTestInput() : ReadInput();

            // initial state: #.##.##.##.##.......###..####..#....#...#.##...##.#.####...#..##..###...##.#..#.##.#.#.#.#..####..#
            var initialState = input.InitialStateLine.Substring("initial state: ".Length);

            //..### => .
            var notes = input.NotesLines
                .ToDictionary(nl => nl.Substring(0, NoteLength), nl => nl.Substring(9, 1).ToArray()[0]);

            return (initialState, notes);
        }

        public static (string InitialStateLine, List<string> NotesLines) ReadTestInput()
        {
            string initialStateLine = "initial state: #..#.#..##......###...###";
            var notes = new List<string>
            {
                "...## => #",
                "..#.. => #",
                ".#... => #",
                ".#.#. => #",
                ".#.## => #",
                ".##.. => #",
                ".#### => #",
                "#.#.# => #",
                "#.### => #",
                "##.#. => #",
                "##.## => #",
                "###.. => #",
                "###.# => #",
                "####. => #",
            };
            return (initialStateLine, notes);
        }
        public static (string InitialStateLine, List<string> NotesLines) ReadInput()
        {

            const Int32 BufferSize = 128;
            using (var fileStream = System.IO.File.OpenRead(@"..\..\Input\AdventOfCode12.txt"))
            using (var streamReader = new System.IO.StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                string initialStateLine = streamReader.ReadLine();
                streamReader.ReadLine();

                var notesLines = new List<string>();
                String line;
                while ((line = streamReader.ReadLine()) != null)
                    notesLines.Add(line);

                return (initialStateLine, notesLines);
            }
        }
    }
}
