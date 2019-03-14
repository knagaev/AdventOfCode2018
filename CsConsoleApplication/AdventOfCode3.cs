using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsConsoleApplication
{
    class AdventOfCode3
    {
        public class Claim
        {
            public int Id;
            public int Left;
            public int Top;
            public int Width;
            public int Height;
        };

        public static void Run1()
        {
            var claims = ReadInput();

            var crosses = new HashSet<string>();

            for (int i = 0; i < claims.Count - 1; i++)
            {
                var claim1 = claims.Skip(i).Take(1).First();
                foreach (var claim2 in claims.Skip(i + 1))
                {
                    crosses.UnionWith(GetCrosses(claim1, claim2));
                }
            }
            Console.WriteLine(crosses.Count);
            Console.ReadLine();
        }

        public static void Run2()
        {
            var claims = ReadInput();

            foreach (var claim1 in claims)
            {
                bool crossed = false;
                foreach (var claim2 in claims.Where(c => c.Id != claim1.Id))
                {
                    var crosses = GetCrosses(claim1, claim2).Count;
                    if (crosses > 0)
                    {
                        crossed = true;
                        break;
                    }
                }

                if (!crossed)
                    Console.WriteLine(String.Format("{0} {1} {2} {3} {4}", claim1.Id, claim1.Left, claim1.Width, claim1.Top, claim1.Height));
            }

            Console.ReadLine();
            return;
        }

        public static List<Claim> ReadInput()
        {
            var claims = new List<Claim>();

            const Int32 BufferSize = 128;
            using (var fileStream = System.IO.File.OpenRead(@"..\..\Input\AdventOfCode3.txt"))
            using (var streamReader = new System.IO.StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                String line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    claims.Add(ParseLine(line));
                }
            }

            return claims;
        }

        public static Claim ParseLine(string line)
        {
            var claim = new Claim();

            // #1 @ 55,885: 22x10
            var r = new System.Text.RegularExpressions.Regex(@"#(\d+) @ (\d+),(\d+): (\d+)x(\d+)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            var match = r.Match(line);
            if (match.Success)
            {
                claim.Id = int.Parse(match.Groups[1].Value);
                claim.Left = int.Parse(match.Groups[2].Value);
                claim.Top = int.Parse(match.Groups[3].Value);
                claim.Width = int.Parse(match.Groups[4].Value);
                claim.Height = int.Parse(match.Groups[5].Value);
            }

            return claim;
        }

        public static List<string> GetCrosses(Claim claim1, Claim claim2)
        {
            var crosses = new List<string>();

            int crossLeft = Math.Max(claim1.Left, claim2.Left);
            int crossRight = Math.Min(claim1.Left + claim1.Width - 1, claim2.Left + claim2.Width - 1);
            int crossTop = Math.Max(claim1.Top, claim2.Top);
            int crossDown = Math.Min(claim1.Top + claim1.Height - 1, claim2.Top + claim2.Height - 1);

            for (int i = crossLeft; i <= crossRight; i++)
                for (int j = crossTop; j <= crossDown; j++)
                    crosses.Add(i + ":" + j);

            return crosses;
        }
    }
}
