﻿using System;
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
            var inputs = PrepareInput1(isTest);
            //var inputs = isTest? PrepareInput1(ReadTestInput1) : PrepareInput1(ReadInput1);

            foreach (var input in inputs)
            {
                var combat = new Combat(input.InitialState, false);
                var outcome = combat.GetOutcome();
                Console.WriteLine(String.Format("The outcome is {0} (must be {1})", outcome, input.Outcome));
                Console.ReadLine();
            }
        }
        public static void Run2(bool isTest = true)
        {
            var inputs = PrepareInput2(isTest);
            //var inputs = isTest ? PrepareInput2(ReadTestInput2) : PrepareInput2(ReadInput2);

            foreach (var input in inputs)
            {
                /*
                var outcome = 0;
                int elfAttackPower = 4;
                int losAttackPower = 0;
                int winAttackPower = 0;
                int winOutcome = 0;
                while (true)
                {
                    var combat = new Combat(input.InitialState, elfAttackPower, false);
                    outcome = combat.GetOutcome(elvesMustSurvive: true);

                    combat.PrintCombatState();
                    Console.WriteLine(String.Format("The outcome is {0} at elf attack power {1}", outcome, elfAttackPower));
                    Console.ReadLine();

                    if (outcome == 0)
                    {
                        losAttackPower = elfAttackPower;
                    }
                    else
                    {
                        winAttackPower = elfAttackPower;
                        winOutcome = outcome;
                    }

                    if (winAttackPower - losAttackPower == 1)
                        break;

                    if (winAttackPower == 0)
                        elfAttackPower *= 2;
                    else
                        elfAttackPower = (winAttackPower - losAttackPower) / 2 + losAttackPower;

                }
                Console.WriteLine(String.Format("The outcome is {0} (must be {1}) at elf attack power {2} (must be {3})", winOutcome, input.Outcome, winAttackPower, input.ElfAttackPower));
                Console.ReadLine();
                */

                int elfAttackPower = 4;
                var outcome = 0;
                while (elfAttackPower < 20)
                {
                    var combat = new Combat(input.InitialState, elfAttackPower, false);
                    outcome = combat.GetOutcome(elvesMustSurvive: true);

                    combat.PrintCombatState();
                    Console.WriteLine(String.Format("The outcome is {0} at elf attack power {1}", outcome, elfAttackPower));
                    elfAttackPower++;
                }
                Console.ReadLine();

                }
            }

        public static List<(Combat InitialState, int Outcome)> PrepareInput1(bool isTest)
        {
            var inputs = isTest ? ReadTestInput1() : ReadInput1();

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
        public static List<(Combat InitialState, int Outcome, int ElfAttackPower)> PrepareInput2(bool isTest)
        {
            var inputs = isTest ? ReadTestInput2() : ReadInput2();

            var combats = new List<(Combat InitialState, int Outcome, int ElfAttackPower)>();

            foreach (var input in inputs)
            {
                var combat = new Combat
                (
                    map: input.CombatMapLines.Select(cml => cml.Select(c => (c == '#') ? '#' : '.').ToArray()).ToArray(),
                    units: input.CombatMapLines.SelectMany((cml, i) => cml.Select((c, j) => new Unit(i, j, c))).Where(u => (u.Kind == 'E' || u.Kind == 'G')).ToList()
                );
                combats.Add((combat, input.Outcome, input.ElfAttackPower));
            }

            return combats;
        }
        public static List<(List<string> CombatMapLines, int Outcome)> ReadTestInput1()
        {
            var input = new List<(List<string> CombatMapLines, int Outcome)>
            {

                //(new List<string>
                //{
                //    "###########",
                //    "#G..#....G#",
                //    "###..E#####",
                //    "###########",
                //}, 10804),

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
        public static List<(List<string> CombatMapLines, int Outcome, int ElfAttackPower)> ReadTestInput2()
        {
            var input = new List<(List<string> CombatMapLines, int Outcome, int ElfAttackPower)>
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
                }, 4988, 15),

                (new List<string>
                {
                "#######",
                "#E..EG#",
                "#.#G.E#",
                "#E.##E#",
                "#G..#.#",
                "#..E#.#",
                "#######",
                }, 31284, 4),

                (new List<string>
                {
                "#######",
                "#E.G#.#",
                "#.#G..#",
                "#G.#.G#",
                "#G..#.#",
                "#...E.#",
                "#######",
                }, 3478, 15),

                (new List<string>
                {
                "#######",
                "#.E...#",
                "#.#..G#",
                "#.###.#",
                "#E#G#G#",
                "#...#G#",
                "#######",
                }, 6474, 12),

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
                }, 1140, 34),
            };
            return input;
        }

        public static List<(List<string> CombatMapLines, int Outcome)> ReadInput1()
        {
            var input = new List<(List<string> CombatMapLines, int Outcome)>
            {
//                (new List<string>
//                {
//"################################",
//"##############..###G.G#####..###",
//"#######...#####........#.##.####",
//"#######..G######.#...........###",
//"#######..G..###.............####",
//"########.GG.##.G.##.......E#####",
//"##########........#........##..#",
//"##############GG...#...........#",
//"##############.....#..........##",
//"#G.G...####....#G......G.#...###",
//"#G..#..##........G.........E.###",
//"#..###...G#............E.......#",
//"#...G...G.....#####............#",
//"#....#....#G.#######...........#",
//"#.##....#.#.#########.#..#...E.#",
//"####...##G..#########.....E...E#",
//"#####...#...#########.#.#....E##",
//"#####.......#########.###......#",
//"######......#########...######.#",
//"########.....#######..#..#######",
//"########......#####...##.#######",
//"########............E.##.#######",
//"####.........##......##..#######",
//"####....#..E...E...####.########",
//"####.....#...........##.########",
//"#####....##.#........###########",
//"#####.....#####....#############",
//"#####.#..######....#############",
//"####..######....################",
//"####..###.#.....################",
//"####...##...####################",
//"################################",
//                }, 0),
                (new List<string>
                {
"################################",
"#######.G...####################",
"#########...####################",
"#########.G.####################",
"#########.######################",
"#########.######################",
"#########G######################",
"#########.#...##################",
"#########.....#..###############",
"########...G....###.....########",
"#######............G....########",
"#######G....G.....G....#########",
"######..G.....#####..G...#######",
"######...G...#######......######",
"#####.......#########....G..E###",
"#####.####..#########G...#....##",
"####..####..#########..G....E..#",
"#####.####G.#########...E...E.##",
"#########.E.#########.........##",
"#####........#######.E........##",
"######........#####...##...#..##",
"###...................####.##.##",
"###.............#########..#####",
"#G#.#.....E.....#########..#####",
"#...#...#......##########.######",
"#.G............#########.E#E####",
"#..............##########...####",
"##..#..........##########.E#####",
"#..#G..G......###########.######",
"#.G.#..........#################",
"#...#..#.......#################",
"################################",
                }, 0),

           };
            return input;
        }
        public static List<(List<string> CombatMapLines, int Outcome, int ElfAttackPower)> ReadInput2()
        {
            var input = new List<(List<string> CombatMapLines, int Outcome, int ElfAttackPower)>
            {
                //Quitting after 48 total rounds
                //Outcome: 54096
                //Did it with attack_power 15
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
                }, 0, 0),
                //(new List<string>
                //{
                //    "################################",
                //    "#############################..#",
                //    "#############################..#",
                //    "#############################..#",
                //    "###########################...##",
                //    "##########################..####",
                //    "###########..#G.#########G..####",
                //    "#########G........#######....###",
                //    "#########...G.......G##........#",
                //    "#######G.................E...###",
                //    "#######.####...####..G..#...####",
                //    "######...#.........G..###....###",
                //    "#####....#....#####...####...###",
                //    "####.........#######.....#....##",
                //    "#.G..G.G..#G#########...G.....##",
                //    "#.###.......#########........###",
                //    "#...G.......#########..E.E..####",
                //    "#..G........#########......#####",
                //    "#....G...E.E#########......#####",
                //    "#........E...#######....########",
                //    "#......#...G..#####...E....#####",
                //    "#.........G................#####",
                //    "#..........G....####....########",
                //    "#................###....########",
                //    "#..........G......######.#######",
                //    "#.............###.##.....#######",
                //    "##......#....####E...E.....#####",
                //    "##......##...###.E.#..##.#######",
                //    "##...####..#####....############",
                //    "###..###...#######..############",
                //    "###..###..######################",
                //    "################################",
                //}, 0, 0),
           };
            return input;
        }
    }


    internal class Unit
    {
        public readonly int AttackPower;
        public const int BaseAttackPower = 3;
        public const int BaseHitPoints = 200;

        public int I;
        public int J;
        public readonly char Kind;
        public int HitPoints;

        public Unit(int i, int j, char kind, int attackPower = 3)
        {
            this.I = i;
            this.J = j;
            this.Kind = kind;
            this.HitPoints = Unit.BaseHitPoints;
            this.AttackPower = attackPower;
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
        public Combat(Combat c, int elfAttackPower, bool isDebug = false) : 
            this(c.Map, c.Units.Select(u => new Unit(u.I, u.J, u.Kind, (u.Kind == 'G') ? Unit.BaseAttackPower : elfAttackPower)).ToList(), isDebug) {}

        public int GetOutcome(bool elvesMustSurvive = false)
        {
            int rounds = 0;
            /*
            PrintCombatState();
            Console.WriteLine(String.Format("HitPoints {0} rounds {1}",
                string.Join(", ", Units.Select(u => u.Kind.ToString() + u.HitPoints)), rounds));
            Console.ReadLine();
            */

            while (true)
            {
                var unitsAliveOnTurnBegin = Units.Where(u => u.HitPoints > 0).OrderBy(u => u.I).ThenBy(u => u.J).Select((u, i) => (u, i)).ToList();
                foreach (var (unit, i) in unitsAliveOnTurnBegin)
                {
                    if (unit.HitPoints <= 0) continue;

                    bool someoneHasAttacked = AttackBy(unit);

                    if (Units.Any(u => u.Kind == 'E' && u.HitPoints < 7 && u.AttackPower > 15))
                    {
                        PrintCombatState();
                        Console.ReadLine();
                    }

                    if (someoneHasAttacked && elvesMustSurvive && Units.Any(u => u.Kind == 'E' && u.HitPoints < 1))
                        return 0;

                    if (someoneHasAttacked &&
                            (!Units.Any(u => u.HitPoints > 0 && u.Kind == 'E') ||
                            !Units.Any(u => u.HitPoints > 0 && u.Kind == 'G')))
                    {
                        if (!unitsAliveOnTurnBegin.Any(ui => ui.i > i && ui.u.HitPoints > 0)) rounds++;

                        PrintCombatState();
                        Console.WriteLine(String.Format("HitPoints {0} rounds {1}",
                            string.Join(", ", Units.Where(u => u.HitPoints > 0).Select(u => u.Kind.ToString() + u.HitPoints)), rounds));
                        return Units.Where(u => u.HitPoints > 0).Select(u => u.HitPoints).Sum() * rounds;
                    }
                }
                rounds++;
                //PrintCombatState();
                Console.WriteLine(String.Format("HitPoints {0} rounds {1}",
                    string.Join(", ", Units.Where(u => u.HitPoints > 0).Select(u => u.Kind.ToString() + u.HitPoints)), rounds));
                //Console.ReadLine();
            }
        }
        private bool MoveIfCan(Unit unit)
        {
            /*
            var shortestPathToEnemy = Units
                .Where(u => u.HitPoints > 0 && u.Kind != unit.Kind)
                .Select(e => new { Enemy = e, DaNS = GetDistanceAndNextStep(unit, e) })
                .Where(ed => ed.DaNS.Distance != int.MaxValue)
                .OrderBy(ed => ed.DaNS.Distance)
                .ThenBy(ed => ed.DaNS.Target.i)
                .ThenBy(ed => ed.DaNS.Target.j)
                .ThenBy(ed => ed.DaNS.NextStep.i)
                .ThenBy(ed => ed.DaNS.NextStep.j)
                .FirstOrDefault();

            var nextStep = GetNextStep(unit);

            if (shortestPathToEnemy == null && nextStep.i != -1)
                Console.ReadLine();

            if (shortestPathToEnemy != null && nextStep.i == -1)
                Console.ReadLine();

            if (shortestPathToEnemy != null && nextStep.i != -1)
            {
                if (shortestPathToEnemy.DaNS.NextStep.i != nextStep.i || shortestPathToEnemy.DaNS.NextStep.j != nextStep.j)
                {
                    PrintPath(nextStep.DistanceMap, nextStep.Path);
                    Console.ReadLine();
                }

                unit.I = shortestPathToEnemy.DaNS.NextStep.i;
                unit.J = shortestPathToEnemy.DaNS.NextStep.j;
                return true;
            }
            else
                return false;
                */

            var nextStep = GetNextStep(unit);
            if (nextStep.i != -1)
            {
                unit.I = nextStep.i;
                unit.J = nextStep.j;
                return true;
            }
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
                .ThenBy(u => u.I)
                .ThenBy(u => u.J);

            var adjacentEnemiesList = adjacentEnemies.ToList();

            if (adjacentEnemiesList.Count() == 0)
            {
                if (MoveIfCan(unit))
                {
                    mover = unit;
                    adjacentEnemiesList = adjacentEnemies.ToList();
                }
                else
                {
                    stopper = unit;
                }
            }

            if (adjacentEnemiesList.Count() > 0)
            {
                var adjacentEnemy = adjacentEnemiesList.FirstOrDefault();
                adjacentEnemy.HitPoints -= unit.AttackPower;

                fighter = unit;
                defender = adjacentEnemy;

                isAttacked = true;
            }

            if (IsDebug)
                PrintCombatState(mover: mover, stopper: stopper, fighter: fighter, defender: defender);

            return isAttacked;
        }

        private (int Distance, (int i, int j) NextStep, (int i, int j) Target) GetDistanceAndNextStep(Unit from, Unit to)
        {
            (int Distance, (int i, int j) NextStep, (int i, int j) Target) result = (int.MaxValue, (-1, -1), (-1, -1));

            foreach (var adjacentCell in offsets.Select(o => (to.I + o.i, to.J + o.j)))
            {
                if (Map[adjacentCell.Item1][adjacentCell.Item2] != '.' ||
                    Units.Any(u => u.HitPoints > 0 && u.I == adjacentCell.Item1 && u.J == adjacentCell.Item2))
                    continue;

                //// free cell = -1, obstacle = int.MinValue
                var distanceMap = Map.Select(l => l.Select(c => (c == '.') ? -1 : int.MinValue).ToArray()).ToArray();
                foreach (var unit in Units.Where(u => u.HitPoints > 0))
                {
                    distanceMap[unit.I][unit.J] = int.MinValue;
                }
                distanceMap[from.I][from.J] = -1;
                //distanceMap[to.I][to.J] = 0;
                distanceMap[adjacentCell.Item1][adjacentCell.Item2] = 0;

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

                                if (biggest < result.Distance)
                                    result = (biggest, previousStep, adjacentCell);
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
            }
            return result;
        }

        private (int i, int j, List<HashSet<(int i, int j)>> Path, int[][] DistanceMap) GetNextStep(Unit unit)
        {
            var distanceMap = Map.Select(l => l.Select(c => (c == '.') ? -1 : int.MinValue).ToArray()).ToArray();
            Units.Where(u => u.HitPoints > 0).ToList().ForEach(u => distanceMap[u.I][u.J] = int.MinValue);
            distanceMap[unit.I][unit.J] = 0;

            var adjacentCells = Units
                .Where(u => u.HitPoints > 0 && u.Kind != unit.Kind)
                .SelectMany(e => offsets.Select(o => (e.I + o.i, e.J + o.j)))
                .Cast<(int i, int j)>()
                .ToHashSet();

            int biggest = 0;
            var reached = new HashSet<(int i, int j)>();
            while (true)
            {
                bool updated = false;
                for (int i = 0; i < distanceMap.Count(); i++)
                {
                    for (int j = 0; j < distanceMap[i].Count(); j++)
                    {
                        if (distanceMap[i][j] == biggest)
                        {
                            foreach (var o in offsets)
                            {
                                if (distanceMap[i + o.i][j + o.j] == -1 || distanceMap[i + o.i][j + o.j] == biggest + 1)
                                {
                                    distanceMap[i + o.i][j + o.j] = biggest + 1;
                                    updated = true;
                                    if (adjacentCells.Contains((i + o.i, j + o.j)))
                                        reached.Add((i + o.i, j + o.j));
                                }
                            }
                        }
                    }
                }
                biggest++;
                if (!updated || reached.Count() > 0) break;
            }

            //var nearestSquare = Units
            //.Where(e => e.Kind != unit.Kind && e.HitPoints > 0)
            //.SelectMany(e => offsets.Select(o => new { i = e.I + o.i, j = e.J + o.j, distance = distanceMap[e.I + o.i][e.J + o.j] }))
            //.Where(sq => sq.distance > 0)
            ////.Aggregate((curMin, sq) => distanceMap[sq.i][sq.j] < distanceMap[curMin.i][curMin.j] ? sq : curMin)
            //.OrderBy(sq => sq.distance)
            //.ThenBy(sq => sq.i)
            //.ThenBy(sq => sq.j)
            //.FirstOrDefault();

            if (reached.Count() ==  0)
                return (-1, -1, null, null);

            var nearestSquare = reached.OrderBy(r => r.i).ThenBy(r => r.j).FirstOrDefault();

            var previousStep = new HashSet<(int i, int j)>{ (nearestSquare.i, nearestSquare.j) };
            var path = new List<HashSet<(int i, int j)>>{ previousStep };
            var distance = biggest;
            while (distance > 1)
            {
                var tempStep = new HashSet<(int i, int j)>();
                foreach (var square in previousStep)
                {
                    foreach (var o in offsets)
                    {
                        if (distanceMap[square.i + o.i][square.j + o.j] == distance - 1)
                            tempStep.Add((square.i + o.i, square.j + o.j));
                    }
                }
                path.Add(tempStep);
                previousStep = tempStep;
                distance--;
            }

            if (false)
                PrintPath(distanceMap, path);

            var nextStep = previousStep.OrderBy(ps => ps.i).ThenBy(ps => ps.j).FirstOrDefault();

            return (nextStep.i, nextStep.j, path, distanceMap);
        }

        private void PrintPath(int[][] distanceMap, List<HashSet<(int i, int j)>> path)
        {
            var memConsoleForegroundColor = Console.ForegroundColor;

            //var printDistanceMap = distanceMap.Select(l => l.Select(c => c.ToString()).ToArray()).ToArray();
            var hashUnits = Units.Select(u => new { IJ = (u.I, u.J), u.Kind }).ToDictionary(t => t.IJ, t => t.Kind);
            var hashPath = path.SelectMany(p => p).ToHashSet();

            Console.Write("  ");
            for (int j = 0; j < distanceMap[0].Count(); j++)
            {
                Console.Write(j.ToString().PadLeft(2));
            }
            Console.WriteLine();

            for (int i = 0; i < distanceMap.Count(); i++)
            {
                Console.Write(i.ToString().PadLeft(2));

                for (int j = 0; j < distanceMap[i].Count(); j++)
                {
                    if (hashPath.Contains((i, j)))
                        Console.ForegroundColor = ConsoleColor.Red;
                    else
                        Console.ForegroundColor = ConsoleColor.Cyan;

                    if (distanceMap[i][j] == int.MinValue)
                    {
                        if (hashUnits.Keys.Contains((i, j)))
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(hashUnits[(i, j)].ToString().PadLeft(2));
                        }
                        else
                            Console.Write(Map[i][j].ToString().PadLeft(2));
                    }
                    else
                        Console.Write(distanceMap[i][j].ToString().PadLeft(2));
                }
                Console.WriteLine();
            }
            Console.ForegroundColor = memConsoleForegroundColor;
            Console.ReadLine();
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
                    Console.ForegroundColor = ConsoleColor.Cyan;

                    if (mover != null && i == mover.I && j == mover.J) Console.ForegroundColor = ConsoleColor.Green;
                    if (stopper != null && i == stopper.I && j == stopper.J) Console.ForegroundColor = ConsoleColor.Gray;
                    if (fighter != null && i == fighter.I && j == fighter.J) Console.ForegroundColor = ConsoleColor.Red;
                    if (mover != null && fighter != null && i == mover.I && j == mover.J) Console.ForegroundColor = ConsoleColor.Magenta;
                    if (defender != null && i == defender.I && j == defender.J) Console.ForegroundColor = ConsoleColor.Yellow;

                    Console.Write(c);
                }

                foreach (var unit in Units.Where(u => u.HitPoints > 0 && u.I == i).OrderBy(u => u.J))
                {
                    Console.Write(" " + unit.Kind + "(" + unit.HitPoints + ")");
                }
                Console.WriteLine();
            }

            Console.ForegroundColor = memConsoleForegroundColor;

            if (mover != null && fighter != null && defender != null)
            {
                    Console.WriteLine(String.Format("Unit {0} goes to {1} and attacked unit {2} at {3} (HP {4})",
                        mover.Kind, (mover.I, mover.J),
                        defender.Kind, (defender.I, defender.J), defender.HitPoints));
            }

            if (mover != null && fighter == null)
            {
                Console.WriteLine(String.Format("Unit {0} goes to {1}",
                    mover.Kind, (mover.I, mover.J)));
            }

            if (stopper != null)
            {
                Console.WriteLine(String.Format("Unit {0} stops at {1}",
                    stopper.Kind, (stopper.I, stopper.J)));
            }

            if (mover == null && fighter != null && defender != null)
            {
                Console.WriteLine(String.Format("Unit {0} stays at {1} and attacked unit {2} at {3} (HP {4})",
                    fighter.Kind, (fighter.I, fighter.J),
                    defender.Kind, (defender.I, defender.J), defender.HitPoints));
            }

            Console.WriteLine();
            return;
        }
    }
}
