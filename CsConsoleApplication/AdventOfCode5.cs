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

            Console.WriteLine(String.Format("Polymer length {0}", polymer.Length));
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
