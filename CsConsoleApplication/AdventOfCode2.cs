using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsConsoleApplication
{
    class AdventOfCode2
    {
        public static void Run1()
        {
            var boxIds = ReadInput();

            int doubleLetter = 0;
            int trippleLetter = 0;

            foreach(var boxId in boxIds)
            {
                var counts = new int[255];
                for (int i = 0; i < boxId.Length; i++)
                {
                    counts[(int)boxId[i]]++;
                }

                if (counts.Contains(2)) doubleLetter++;
                if (counts.Contains(3)) trippleLetter++;
            }

            Console.WriteLine(doubleLetter);
            Console.WriteLine(trippleLetter);
            Console.WriteLine(doubleLetter * trippleLetter);
            Console.ReadLine();
        }

        public static void Run2()
        {
            var boxIds = ReadInput();

            for (int i = 0; i < boxIds.Count - 1; i++)
            {
                var curBoxId = boxIds.Skip(i).Take(1).First();
                var correctBoxIds = boxIds
                                    .Skip(i + 1)
                                    .Take(boxIds.Count - i - 1)
                                    .Select(b =>
                                            new
                                            {
                                                id1 = b,
                                                id2 = curBoxId,
                                                diff = Encoding.ASCII.GetBytes(b)
                                                    .Zip(Encoding.ASCII.GetBytes(curBoxId), (fl, sl) => (fl == sl) ? 0 : 1)
                                                    .Sum(),
                                                removed = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(b)
                                                    .Zip(Encoding.ASCII.GetBytes(curBoxId), (fl, sl) => new { fl = fl, sl = sl })
                                                    .Where(fs => fs.fl == fs.sl)
                                                    .Select(fs => fs.fl)
                                                    .ToArray())
                                            })
                                    .Where(res => res.diff == 1)
                    //.OrderBy(res => res.diff)
                                    .Select(ids => ids.id1 + " " + ids.id2 + " " + ids.diff + " " + ids.removed)
                                    .ToArray();
                if (correctBoxIds.Length == 1)
                    Console.WriteLine(correctBoxIds[0]);
            }
            Console.ReadLine();
            return;
        }

        public static List<string> ReadInput()
        {
            var boxIds = new List<string>();

            const Int32 BufferSize = 128;
            using (var fileStream = System.IO.File.OpenRead(@"..\..\AdventOfCode\AdventOfCode2.txt"))
            using (var streamReader = new System.IO.StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                String line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    boxIds.Add(line);
                }
            }
            return boxIds;
        }
    }
}
