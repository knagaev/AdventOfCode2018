using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsConsoleApplication
{

    class AdventOfCode15
    {
        public static void Run1(bool isTest = true)
        {
            var inputs = PrepareInput(isTest);

            foreach (var input in inputs)
            {
                var combat = new Combat(input.InitialState, false);
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
                (
                    map: input.CombatMapLines.Select(cml => cml.Select(c => (c == '#') ? '#' : '.').ToArray()).ToArray(),
                    units: input.CombatMapLines.SelectMany((cml, i) => cml.Select((c, j) => new Unit(i, j, c))).Where(u => (u.Kind == 'E' || u.Kind == 'G')).ToList()
                );
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

        private bool IsDebug;

        public Combat(char[][] map, List<Unit> units, bool isDebug = false)
        {
            this.Map = map.Select(l => l.ToArray()).ToArray();
            this.Units = units.ToList();
            this.IsDebug = isDebug;
        }
        public Combat(Combat c, bool isDebug = false) : this(c.Map, c.Units, isDebug){}

        public int GetOutcome()
        {
            int rounds = 0;
            while (true)
            {
                var aliveUnits = Units.Where(u => u.HitPoints > 0).OrderBy(u => u.I).ThenBy(u => u.J).Select((u, i) => (u, i)).ToList();
                foreach (var (unit, i) in aliveUnits)
                {
                    if (unit.HitPoints <= 0) continue;

                    bool someoneHasAttacked = AttackBy(unit);

                    if (someoneHasAttacked &&
                            (!Units.Any(u => u.HitPoints > 0 && u.Kind == 'E') ||
                            !Units.Any(u => u.HitPoints > 0 && u.Kind == 'G')))
                    {
                        if (i == aliveUnits.Count() - 1) rounds++;

                        Console.WriteLine(String.Format("HitPoints {0} rounds {1}",
                            string.Join(", ", Units.Where(u => u.HitPoints > 0).Select(u => u.Kind.ToString() + u.HitPoints)), rounds));
                        return Units.Where(u => u.HitPoints > 0).Select(u => u.HitPoints).Sum() * rounds;
                    }
                }

                rounds++;
            }
        }
        private bool MoveIfCan(Unit unit)
        {
            var shortestPathToEnemy = Units
                .Where(u => u.HitPoints > 0 && u.Kind != unit.Kind)
                .Select(e => new { Enemy = e, DaNS = GetDistanceAndNextStep(unit, e) })
                .Where(ed => ed.DaNS.Distance != int.MaxValue)
                .OrderBy(ed => ed.DaNS.Distance)
                .ThenBy(ed => ed.DaNS.NextStep.i)
                .ThenBy(ed => ed.DaNS.NextStep.j)
                .FirstOrDefault();

            if (shortestPathToEnemy != null)
            {
                unit.I = shortestPathToEnemy.DaNS.NextStep.i;
                unit.J = shortestPathToEnemy.DaNS.NextStep.j;
                return true;
            }
            else
                return false;
        }

        private bool AttackBy(Unit unit)
        {
            bool isAttacked = false;

            Unit mover = null;
            Unit stopper = null;
            Unit fighter = null;
            Unit defender = null;

            var adjacentEnemies = Units
                .Where(u => u.HitPoints > 0
                    && u.Kind != unit.Kind
                    && offsets
                    .Any(o => u.I + o.i == unit.I && u.J + o.j == unit.J))
                .Select(u => u)
                .OrderBy(u => u.HitPoints)
                .ToList();

            if (adjacentEnemies.Count() == 0)
            {
                if (MoveIfCan(unit))
                {
                    mover = unit;
                    adjacentEnemies = Units
                                    .Where(u => u.HitPoints > 0
                                        && u.Kind != unit.Kind
                                        && offsets
                                        .Any(o => u.I + o.i == unit.I && u.J + o.j == unit.J))
                                    .Select(u => u)
                                    .OrderBy(u => u.HitPoints)
                                    .ToList();
                }
                else
                {
                    stopper = unit;
                }
            }

            if (adjacentEnemies.Count() > 0)
            {
                var adjacentEnemy = adjacentEnemies.FirstOrDefault();
                adjacentEnemy.HitPoints -= Unit.AttackPower;

                fighter = unit;
                defender = adjacentEnemy;

                isAttacked = true;
            }

            if (IsDebug)
                PrintCombatState(mover: mover, stopper: stopper, fighter: fighter, defender: defender);

            return isAttacked;
        }

        private (int Distance, (int i, int j) NextStep) GetDistanceAndNextStep(Unit from, Unit to)
        {
            //// free cell = -1, obstacle = int.MinValue
            var distanceMap = Map.Select(l => l.Select(c => (c == '.') ? -1 : int.MinValue).ToArray()).ToArray();
            foreach (var unit in Units.Where(u => u.HitPoints > 0))
            {
                distanceMap[unit.I][unit.J] = int.MinValue;
            }
            distanceMap[from.I][from.J] = -1;
            distanceMap[to.I][to.J] = 0;

            int biggest = 0;
            while (true)
            {
                bool updated = false;
                foreach (var e in distanceMap.SelectMany((l, i) => l.Select((c, j) => new { c, i, j })).Where(cij => cij.c == biggest).OrderBy(cij => cij.i).ThenBy(cij => cij.j))
                {
                    foreach (var offset in offsets)
                    {
                        if (from.I == e.i + offset.i && from.J == e.j + offset.j)
                        {
                            var previousStep = distanceMap
                                .Select((l, i) => (l, i))
                                .Skip(from.I - 1).Take(3)
                                .SelectMany(li => li.l
                                    .Select((c, j) => new { li.i, j, c })
                                    .Skip(from.J - 1).Take(3))
                                .Where(ijc => ijc.c == biggest)
                                .OrderBy(ijc => ijc.i)
                                .ThenBy(ijc => ijc.j)
                                .Select(ijc => (ijc.i, ijc.j))
                                .FirstOrDefault();

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

        public void PrintCombatState(Unit mover = null, Unit stopper = null, Unit fighter = null, Unit defender = null)
        {
            var memConsoleForegroundColor = Console.ForegroundColor;

            var printMap = Map.Select(l => l.ToArray()).ToArray();
            foreach (var unit in Units.Where(u => u.HitPoints > 0))
            {
                printMap[unit.I][unit.J] = unit.Kind;
            }

            foreach (var (l, i) in printMap.Select((l, i) => (l, i)))
            {
                foreach (var (c, j) in l.Select((c, j) => (c, j)))
                {
                    Console.ForegroundColor = ConsoleColor.Blue;

                    if (mover != null && i == mover.I && j == mover.J) Console.ForegroundColor = ConsoleColor.Green;
                    if (stopper != null && i == stopper.I && j == stopper.J) Console.ForegroundColor = ConsoleColor.Gray;
                    if (fighter != null && i == fighter.I && j == fighter.J) Console.ForegroundColor = ConsoleColor.Red;
                    if (mover != null && fighter != null && i == mover.I && j == mover.J) Console.ForegroundColor = ConsoleColor.Magenta;
                    if (defender != null && i == defender.I && j == defender.J) Console.ForegroundColor = ConsoleColor.Yellow;

                    Console.Write(c);
                }
                Console.WriteLine();
            }

            Console.ForegroundColor = memConsoleForegroundColor;
            if (mover != null)
            {
                if (fighter == null)
                    Console.WriteLine(String.Format("Unit {0} goes to {1}",
                        mover.Kind, (mover.I, mover.J)));
                else
                    Console.WriteLine(String.Format("Unit {0} goes to {1} and attacked unit {2} at {3} (HP {4})",
                        mover.Kind, (mover.I, mover.J),
                        defender.Kind, (defender.I, defender.J), defender.HitPoints));
            }
            else
            {
                if (fighter == null)
                    Console.WriteLine(String.Format("Unit {0} stops at {1}",
                        stopper.Kind, (stopper.I, stopper.J)));
                else
                    Console.WriteLine(String.Format("Unit {0} stays at {1} and attacked unit {2} at {3} (HP {4})",
                        fighter.Kind, (fighter.I, fighter.J),
                        defender.Kind, (defender.I, defender.J), defender.HitPoints));
            }

            Console.WriteLine();
            return;
        }
    }
}
