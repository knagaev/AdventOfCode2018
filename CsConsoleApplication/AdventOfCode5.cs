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

            polymerList = ReactPolymer(polymerList);

            Console.WriteLine(String.Format("Polymer {0} length {1}", String.Join("", polymerList), polymerList.Count));
            Console.ReadLine();
        }
        public static void Run2(bool isTest = true)
        {
            var polymer = PrepareInput(isTest);

            var unitTypes = polymer.Select(i => char.ToLower(i)).Distinct();

            var result = unitTypes.Select(ut => ReactPolymer(polymer.ToList(), ut).Count).Min();

            Console.WriteLine(String.Format("Min length {0}", result));
            Console.ReadLine();


        }
        public static List<char> ReactPolymer(List<char> polymer, char unitType = '0')
        {
            char curChar, nextChar;
            int curIndex = 0;

            while (true)
            {
                while (true)
                {
                    if (curIndex >= polymer.Count) 
                        return polymer;

                    curChar = polymer[curIndex];

                    if (char.ToLower(curChar) == unitType)
                        polymer.RemoveAt(curIndex);
                    else
                        break;
                }

                var nextIndex = curIndex + 1;

                while (true)
                {
                    if (nextIndex >= polymer.Count) 
                        return polymer;

                    nextChar = polymer[nextIndex];

                    if (char.ToLower(nextChar) == unitType)
                        polymer.RemoveAt(nextIndex);
                    else
                        break;
                }
                
                if ((char.IsUpper(curChar) ? char.ToLower(curChar) : char.ToUpper(curChar)) == nextChar)
                {
                    polymer.RemoveRange(curIndex, 2);
                    if (curIndex > 0) curIndex--;
                }
                else
                    curIndex++;
            }
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
            using (var fileStream = System.IO.File.OpenRead(@"..\..\Input\AdventOfCode5.txt"))
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
