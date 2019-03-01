using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsConsoleApplication
{
    class AdventOfCode5
    {

        public static void Run1(bool isTest = true)
        {
            var polymer = PrepareInput(isTest);

            var polymerList = polymer.ToList();

            int polymerLength = polymerList.Count;

            while (true)
            {
                polymerList = polymerList
                        .Zip(polymerList.Skip(1), (f, s) => ((char.IsUpper(f) ? char.ToLower(f) : char.ToUpper(f)) == s) ? '\0' : f)
                        .Where(c => c != '\0')
                        .ToList();

                if (polymerList.Count == polymerLength)
                    break;

                polymerLength = polymerList.Count;
            } 

            Console.WriteLine(String.Format("Polymer {0} length {1}", String.Join("", polymerList), polymerLength));
            Console.ReadLine();
        }

        public static string PrepareInput(bool isTest)
        {
            var inputString = isTest ? ReadTestInput() : ReadInput();
            return inputString;
        }
        public static string ReadInput()
        {
            var inputString = new StringBuilder();

            const Int32 BufferSize = 128;
            using (var fileStream = System.IO.File.OpenRead(@"..\..\AdventOfCode\AdventOfCode5.txt"))
            using (var streamReader = new System.IO.StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                String line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    inputString.Append(line);
                }
            }

            return inputString.ToString();
        }
        public static string ReadTestInput()
        {
            return "dabAcCaCBAcCcaDA";
        }
    }
}
