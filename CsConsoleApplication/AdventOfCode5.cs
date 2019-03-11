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

            //while (true)
            //{
            //    //polymerList = polymerList
            //    //        .Zip(polymerList.Skip(1), (f, s) => ((char.IsUpper(f) ? char.ToLower(f) : char.ToUpper(f)) == s) ? '\0' : f)
            //    //        .Where(c => c != '\0')
            //    //        .ToList();

            //    polymerList = new List<char> {'0'}.Concat(polymerList)
            //            .Zip(polymerList, (l, m) => new {l, m})
            //            .Zip(polymerList.Skip(1).Concat(new List<char> {'0'}), (lm, r) => new {lm.l, lm.m, r})
            //            .Where(lmr => ((char.IsUpper(lmr.l) ? char.ToLower(lmr.l) : char.ToUpper(lmr.l)) != lmr.m) && ((char.IsUpper(lmr.m) ? char.ToLower(lmr.m) : char.ToUpper(lmr.m)) != lmr.r))
            //            .Select(lmr => lmr.m)
            //            .ToList();

            //    if (polymerList.Count == polymerLength)
            //        break;

            //    polymerLength = polymerList.Count;
            //} 
            /*
            int curIndex = 0;

            while (true)
            {
                if (curIndex >= polymerList.Count) break;

                var nextIndex = curIndex + 1;
                if (nextIndex >= polymerList.Count) break;

                var curChar = polymerList[curIndex];
                var nextChar = polymerList[nextIndex];

                if ((char.IsUpper(curChar) ? char.ToLower(curChar) : char.ToUpper(curChar)) == nextChar)
                {
                    polymerList.RemoveRange(curIndex, 2);
                    if (curIndex > 0) curIndex--;
                }
                else
                    curIndex++;
            }
            polymerList = polymerList.Aggregate(new List<char>(), (total, next) =>
                total.Count > 0 && total.Last() == (char.IsUpper(next) ? char.ToLower(next) : char.ToUpper(next)) ?
                    total.Take(total.Count - 1).ToList() :
                    total.Concat(new List<char> { next }).ToList()
            );
            */
            polymerList = ReactPolymer(polymerList);

            Console.WriteLine(String.Format("Polymer {0} length {1}", String.Join("", polymerList), polymerList.Count));
            Console.ReadLine();
        }
        public static void Run2(bool isTest = true)
        {
            var unitTypes = new List<char> {'a', 'b', 'c', 'd'};

            var polymer = PrepareInput(isTest);


            var result = unitTypes.Select(ut => ReactPolymer(polymer.ToList(), ut).Count).Min();
            Console.WriteLine(String.Format("length {0}", result));
            Console.ReadLine();


        }
        public static List<char> ReactPolymer(List<char> polymer, char unitType = '0')
        {
            int curIndex = 0;

            while (true)
            {
                if (curIndex >= polymer.Count) break;

                var nextIndex = curIndex + 1;
                if (nextIndex >= polymer.Count) break;

                var curChar = polymer[curIndex];
                var nextChar = polymer[nextIndex];

                if ((char.IsUpper(curChar) ? char.ToLower(curChar) : char.ToUpper(curChar)) == nextChar && (unitType == '0' || (unitType == curChar || unitType == nextChar)))
                {
                    polymer.RemoveRange(curIndex, 2);
                    if (curIndex > 0) curIndex--;
                }
                else
                    curIndex++;
            }

            return polymer;
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
