using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsConsoleApplication
{

    internal class Unit
    {
        public const int AttackPower = 3;
        public const int BaseHitPoints = 200;

        public int I;
        public int J;
        public readonly char Kind;
        public int HitPoints;

        public Unit(int i, int j, char kind)
        {
            this.I = i;
            this.J = j;
            this.Kind = kind;
            this.HitPoints = Unit.BaseHitPoints;
        }
    }

    internal class Combat
    {
        public readonly List<(int i, int j)> offsets = new List<(int i, int j)> { (-1, 0), (0, -1), (0, 1), (1, 0) };

        public char[][] Map;
        public List<Unit> Units;

        public Combat() { }
        public Combat(Combat c)
        {
            this.Map = c.Map.Select(l => l.ToArray()).ToArray();
            this.Units = c.Units.ToList();
        }

        public int GetOutcome()
        {
            int rounds = 0;
            while (true)
            {

                foreach (var unit in Units.OrderBy(u => u.I).ThenBy(u => u.J))
                {
                    if (true)
                        PrintCombatState();

                    bool someoneHasAttacked = AttackBy(unit);

                    if (!Units.Any(u => u.HitPoints > 0 && u.Kind == 'E') ||
                        !Units.Any(u => u.HitPoints > 0 && u.Kind == 'G'))
                    {
                        return Units.Where(u => u.HitPoints > 0).Select(u => u.HitPoints).Sum() * rounds;
                    }

                    if (!someoneHasAttacked)
                    {
                        var enemies = Units
                            .Where(u => u.HitPoints > 0 && u.Kind != unit.Kind)
                            .Select(e => new { Enemy = e, DaNS = GetDistanceAndNextStep(unit, e) })
                            .OrderBy(ed => ed.DaNS.Distance)
                            .ThenBy(ed => ed.DaNS.NextStep.i)
                            .ThenBy(ed => ed.DaNS.NextStep.j)
                            .ToList();

                        var nearestEnemy = enemies
                            .FirstOrDefault();

                        if (nearestEnemy != null)
                        {
                            unit.I = nearestEnemy.DaNS.NextStep.i;
                            unit.J = nearestEnemy.DaNS.NextStep.j;
                        }
                    }
                }
                rounds++;
                Console.WriteLine(String.Format("Round {0}", rounds));
            }
        }

        private bool AttackBy(Unit unit)
        {
            var enemyKind = unit.Kind == 'G' ? 'E' : 'G';

            var adjacentEnemies = Units
                .Where(u => u.HitPoints > 0 
                    && u.Kind == enemyKind
                    && offsets
                    .Any(o => u.I + o.i == unit.I && u.J + o.j == unit.J))
                .Select(u => u)
                .OrderBy(u => u.I)
                .ThenBy(u => u.J)
                .ToList();

            if (adjacentEnemies.Count() > 0)
            {
                var adjacentEnemy = adjacentEnemies.FirstOrDefault();
                adjacentEnemy.HitPoints -= Unit.AttackPower;

                Console.WriteLine(String.Format("Unit {0} at {1} attacked unit {2} at {3} (HP {4})", 
                    unit.Kind, (unit.I, unit.J), 
                    adjacentEnemy.Kind, (adjacentEnemy.I, adjacentEnemy.J), adjacentEnemy.HitPoints));

                return true;
            }

            return false;
        }

        private (int Distance, (int i, int j) NextStep) GetDistanceAndNextStep(Unit from, Unit to)
        {
            // free cell = -1, obstacle = int.MinValue
            var distanceMap = Map.Select(l => l.Select(c => (c == '.') ? -1 : int.MinValue).ToArray()).ToArray();
            foreach (var unit in Units.Where(u => u.HitPoints > 0))
            {
                distanceMap[unit.I][unit.J] = -1;
            }
            distanceMap[from.I][from.J] = 0;
            distanceMap[to.I][to.J] = -1;

            int biggest = 0;
            while (true)
            {
                bool updated = false;
                foreach (var e in distanceMap.SelectMany((l, i) => l.Select((c, j) => new { c, i, j })).Where(cij => cij.c == biggest).OrderBy(cij => cij.i).ThenBy(cij => cij.j))
                {
                    foreach (var offset in offsets)
                    {
                        if (to.I == e.i + offset.i && to.J == e.j + offset.j)
                        {
                            int reverseDistance = biggest - 1;
                            var previousStep = (e.i, e.j);
                            while (reverseDistance > 0)
                            {
                                previousStep = offsets
                                    .Select(ro => new { ro.i, ro.j, d = distanceMap[previousStep.i + ro.i][previousStep.j + ro.j] })
                                    .Where(ijd => ijd.d == reverseDistance)
                                    .Select(ijd => (previousStep.i + ijd.i, previousStep.j + ijd.j))
                                    .First();

                                reverseDistance--;
                            }
                            return (biggest, previousStep);
                        }

                        if (distanceMap[e.i + offset.i][e.j + offset.j] == -1)
                        {
                            distanceMap[e.i + offset.i][e.j + offset.j] = biggest + 1;
                            updated = true;
                        }
                    }
                }
                biggest++;
                if (!updated) break;
            }
            return (int.MaxValue, (-1, -1));
        }

        public void PrintCombatState()
        {
            var printMap = Map.Select(l => l.ToArray()).ToArray();
            foreach (var unit in Units.Where(u => u.HitPoints > 0))
            {
                printMap[unit.I][unit.J] = unit.Kind;
            }

            foreach (var l in printMap)
                Console.WriteLine(string.Join("", l));
            Console.WriteLine();
        }

    }

    class AdventOfCode15
    {
        public static void Run1(bool isTest = true)
        {
            var inputs = PrepareInput(isTest);

            foreach (var input in inputs)
            {
                var combat = new Combat(input.InitialState);
                var outcome = combat.GetOutcome();
                Console.WriteLine(String.Format("The outcome is {0} (must be {1})", outcome, input.Outcome));
                Console.ReadLine();
            }
        }

        public static List<(Combat InitialState, int Outcome)> PrepareInput(bool isTest)
        {
            var inputs = isTest ? ReadTestInput() : ReadInput();

            var combats = new List<(Combat InitialState, int Outcome)>();

            foreach (var input in inputs)
            {
                var combat = new Combat
                {
                    //Map = input.CombatMapLines.Select(cml => cml.Select(c => (c == '.') ? 0 : 1).ToArray()).ToArray(),
                    Map = input.CombatMapLines.Select(cml => cml.Select(c => (c == '#') ? '#' : '.').ToArray()).ToArray(),
                    Units = input.CombatMapLines.SelectMany((cml, i) => cml.Select((c, j) => new Unit(i, j, c))).Where(u => (u.Kind == 'E' || u.Kind == 'G')).ToList(),
                };
                combats.Add((combat, input.Outcome));
            }

            return combats;
        }
        public static List<(List<string> CombatMapLines, int Outcome)> ReadTestInput()
        {
            var input = new List<(List<string> CombatMapLines, int Outcome)>
            {
                (new List<string>
                {
                "#######",
                "#.G...#",
                "#...EG#",
                "#.#.#G#",
                "#..G#E#",
                "#.....#",
                "#######",
                }, 27730),

                (new List<string>
                {
                "#######",
                "#G..#E#",
                "#E#E.E#",
                "#G.##.#",
                "#...#E#",
                "#...E.#",
                "#######",
                }, 36334),

                (new List<string>
                {
                "#######",
                "#E..EG#",
                "#.#G.E#",
                "#E.##E#",
                "#G..#.#",
                "#..E#.#",
                "#######",
                }, 39514),

                (new List<string>
                {
                "#######",
                "#E.G#.#",
                "#.#G..#",
                "#G.#.G#",
                "#G..#.#",
                "#...E.#",
                "#######",
                }, 27755),

                (new List<string>
                {
                "#######",
                "#.E...#",
                "#.#..G#",
                "#.###.#",
                "#E#G#G#",
                "#...#G#",
                "#######",
                }, 28944),

                (new List<string>
                {
                "#########",
                "#G......#",
                "#.E.#...#",
                "#..##..G#",
                "#...##..#",
                "#...#...#",
                "#.G...G.#",
                "#.....G.#",
                "#########",
                }, 18740),
            };
            return input;
        }
        public static List<(List<string> CombatMapLines, int Outcome)> ReadInput()
        {
            var input = new List<(List<string> CombatMapLines, int Outcome)>
            {
                (new List<string>
                {
                "################################",
                "##############..###G.G#####..###",
                "#######...#####........#.##.####",
                "#######..G######.#...........###",
                "#######..G..###.............####",
                "########.GG.##.G.##.......E#####",
                "##########........#........##..#",
                "##############GG...#...........#",
                "##############.....#..........##",
                "#G.G...####....#G......G.#...###",
                "#G..#..##........G.........E.###",
                "#..###...G#............E.......#",
                "#...G...G.....#####............#",
                "#....#....#G.#######...........#",
                "#.##....#.#.#########.#..#...E.#",
                "####...##G..#########.....E...E#",
                "#####...#...#########.#.#....E##",
                "#####.......#########.###......#",
                "######......#########...######.#",
                "########.....#######..#..#######",
                "########......#####...##.#######",
                "########............E.##.#######",
                "####.........##......##..#######",
                "####....#..E...E...####.########",
                "####.....#...........##.########",
                "#####....##.#........###########",
                "#####.....#####....#############",
                "#####.#..######....#############",
                "####..######....################",
                "####..###.#.....################",
                "####...##...####################",
                "################################",
                }, 0),
           };
            return input;
        }
    }
}
