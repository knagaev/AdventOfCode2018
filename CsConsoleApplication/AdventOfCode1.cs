using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsConsoleApplication
{
    class AdventOfCode1
    {
        public static void Run1()
        {
            const Int32 BufferSize = 128;

            int frequency = 0;

            using (var fileStream = System.IO.File.OpenRead(@"..\..\Input\AdventOfCode1.txt"))
            using (var streamReader = new System.IO.StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                String line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    frequency += Int32.Parse(line);
                    Console.WriteLine(frequency);
                }

                Console.ReadLine();
            }            
        }

        public static void Run2()
        {
            const Int32 BufferSize = 128;

            int frequency = 0;
            var frequencies = new HashSet<int> { frequency };

            using (var fileStream = System.IO.File.OpenRead(@"..\..\AdventOfCode\AdventOfCode1.txt"))
            using (var streamReader = new System.IO.StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                String line;
                while (true)
                {
                    line = streamReader.ReadLine();

                    if (line == null)
                    {
                        fileStream.Position = 0;
                        streamReader.DiscardBufferedData();
                        line = streamReader.ReadLine();
                    }

                    frequency += Int32.Parse(line);
                    Console.WriteLine(frequency);

                    if (frequencies.Contains(frequency))
                    {
                        Console.ReadLine();
                        return;
                    }
                    else
                        frequencies.Add(frequency);
                }
            }
        }

        public static List<string> ReadInput()
        {
            var boxIds = new List<string>();

            const Int32 BufferSize = 128;
            using (var fileStream = System.IO.File.OpenRead(@"C:\_in\AdventOfCode\AdventOfCode1.txt"))
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
