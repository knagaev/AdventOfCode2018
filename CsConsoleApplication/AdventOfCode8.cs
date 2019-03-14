using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsConsoleApplication
{
    class AdventOfCode8
    {
        public static void Run1(bool isTest = true)
        {
            var tree = PrepareInput(isTest);

            (int sumOfMetadataEntries, int sumNodeLength) = ParseNodes1(tree);

            Console.WriteLine(String.Format("Sum of all metadata entries {0}", sumOfMetadataEntries));
            Console.ReadLine();
        }

        public static (int metadataEntries, int nodeLength) ParseNodes1(int[] node)
        {
            int childNodesQty = node[0];
            int metadataEntriesQty = node[1];

            var sumOfMetadataEntries = 0;
            var subNodeLengths = new int[childNodesQty];
            for (int i = 0; i < childNodesQty; i++)
            {
                (int subSumOfMetadataEntries, int subNodeLength) = ParseNodes1(node.Skip(2 + subNodeLengths.Take(i).Sum()).ToArray());
                sumOfMetadataEntries += subSumOfMetadataEntries;
                subNodeLengths[i] = subNodeLength;
            }

            sumOfMetadataEntries += node.Skip(2 + subNodeLengths.Sum()).Take(metadataEntriesQty).Sum();
            int sumNodeLength = 2 + subNodeLengths.Sum() + metadataEntriesQty;
            return (sumOfMetadataEntries, sumNodeLength);
        }

        public static void Run2(bool isTest = true)
        {
            var tree = PrepareInput(isTest);

            (int valueOfNode, int sumNodeLength) = ParseNodes2(tree);

            Console.WriteLine(String.Format("Value of root node {0}", valueOfNode));
            Console.ReadLine();
        }

        public static (int metadataEntries, int nodeLength) ParseNodes2(int[] node)
        {
            int childNodesQty = node[0];
            int metadataEntriesQty = node[1];

            var subNodeLengths = new int[childNodesQty];
            var subValuesOfNodes = new int[childNodesQty];

            for (int i = 0; i < childNodesQty; i++)
            {
                (int subValueOfNode, int subNodeLength) = ParseNodes2(node.Skip(2 + subNodeLengths.Take(i).Sum()).ToArray());
                subValuesOfNodes[i] = subValueOfNode;
                subNodeLengths[i] = subNodeLength;
            }

            var metadataEntries = node.Skip(2 + subNodeLengths.Sum()).Take(metadataEntriesQty).ToArray();
            var valueOfNode = (childNodesQty > 0) ? 
                metadataEntries.Select(me => me > subValuesOfNodes.Count() ? 0 : subValuesOfNodes[me - 1]).Sum() :
                metadataEntries.Sum();
            int sumNodeLength = 2 + subNodeLengths.Sum() + metadataEntriesQty;
            return (valueOfNode, sumNodeLength);
        }

        public static int[] PrepareInput(bool isTest)
        {
            var licenseFile = isTest ? ReadTestInput() : ReadInput();

            var numbers = licenseFile.Split(new char[] { ' ' }).Select(n => int.Parse(n)).ToArray();
            return numbers;
        }
        public static string ReadTestInput()
        {
            var input = "2 3 0 3 10 11 12 1 1 0 1 99 2 1 1 2";
            return input;
        }
        public static string ReadInput()
        {
            const Int32 BufferSize = 128;
            using (var fileStream = System.IO.File.OpenRead(@"..\..\Input\AdventOfCode8.txt"))
            using (var streamReader = new System.IO.StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                string input = streamReader.ReadLine();
                return input;
            }

        }
    }
}
