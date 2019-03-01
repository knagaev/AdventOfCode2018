using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsConsoleApplication
{
    class AdventOfCode4
    {
        public class GuardRecord
        {
            public DateTime timestamp;
            public int GuardId;
            public bool WakeUp;
        };

        public static void Run2(bool isTest = true)
        {
            var sleepRecords = PrepareInput(isTest);

            var mostSleepMinuteForGuard = sleepRecords.GroupBy(sr => new { sr.GuardId, sr.timestamp.Minute })
                                            .Select(gsr => new { gsr.Key.GuardId, gsr.Key.Minute, MinuteCount = gsr.Count() })
                                            .Aggregate((msg, next) => next.MinuteCount > msg.MinuteCount ? next : msg);

            Console.WriteLine(String.Format("Guard#{0} minute:{1} count:{2} result:{3}",
                                            mostSleepMinuteForGuard.GuardId, mostSleepMinuteForGuard.Minute, mostSleepMinuteForGuard.MinuteCount, mostSleepMinuteForGuard.GuardId * mostSleepMinuteForGuard.Minute));
            Console.ReadLine();
        }

        public static void Run1(bool isTest = true)
        {
            var sleepRecords = PrepareInput(isTest);

            var mostSleepGuard = sleepRecords.GroupBy(sr => sr.GuardId)
                                            .Select(gsr => new { GuardId = gsr.Key, MinuteCount = gsr.Count() })
                                            .Aggregate((msg, next) => next.MinuteCount > msg.MinuteCount ? next : msg);

            Console.WriteLine(String.Format("Guard#{0} count:{1}", mostSleepGuard.GuardId, mostSleepGuard.MinuteCount));

            var mostSleepMinute = sleepRecords.Where(ss => ss.GuardId == mostSleepGuard.GuardId)
                                            .GroupBy(sr => new { sr.timestamp.Minute })
                                            .Select(gsr => new { gsr.Key.Minute, MinuteCount = gsr.Count() })
                                            .Aggregate((msm, next) => next.MinuteCount > msm.MinuteCount ? next : msm);

            Console.WriteLine(String.Format("Guard#{0} minute:{1} count:{2} result:{3}",
                                            mostSleepGuard.GuardId, mostSleepMinute.Minute, mostSleepMinute.MinuteCount, mostSleepGuard.GuardId * mostSleepMinute.Minute));
            Console.ReadLine();
        }

        public static List<GuardRecord> PrepareInput(bool isTest)
        {
            var guardRecords = isTest ? ReadTestInput(): ReadInput();

            guardRecords = guardRecords.OrderBy(gr => gr.timestamp).ToList();

            var sleepRecords = new List<GuardRecord>();

            int guardId = 0;
            DateTime timestampBeginSleep = DateTime.MinValue;

            foreach (var guardRecord in guardRecords)
            {
                if (!guardRecord.WakeUp)
                {
                    timestampBeginSleep = guardRecord.timestamp;
                }
                else
                {
                    if (guardRecord.GuardId != 0)
                    {
                        guardId = guardRecord.GuardId;
                    }
                    else
                    {
                        for (DateTime ts = timestampBeginSleep; ts < guardRecord.timestamp; ts = ts.AddMinutes(1))
                        {
                            sleepRecords.Add(new GuardRecord { timestamp = ts, GuardId = guardId });
                        }
                    }
                }
            }
            return sleepRecords;
        }


        public static List<GuardRecord> ReadInput()
        {
            var guardRecords = new List<GuardRecord>();

            const Int32 BufferSize = 128;
            using (var fileStream = System.IO.File.OpenRead(@"..\..\AdventOfCode\AdventOfCode4.txt"))
            using (var streamReader = new System.IO.StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                String line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    guardRecords.Add(ParseLine(line));
                }
            }

            return guardRecords;
        }

        public static List<GuardRecord> ReadTestInput()
        {
            var testRecords = new List<string>
            {
                "[1518-11-01 00:00] Guard #10 begins shift",
                "[1518-11-01 00:05] falls asleep",
                "[1518-11-01 00:25] wakes up",
                "[1518-11-01 00:30] falls asleep",
                "[1518-11-01 00:55] wakes up",
                "[1518-11-01 23:58] Guard #99 begins shift",
                "[1518-11-02 00:40] falls asleep",
                "[1518-11-02 00:50] wakes up",
                "[1518-11-03 00:05] Guard #10 begins shift",
                "[1518-11-03 00:24] falls asleep",
                "[1518-11-03 00:29] wakes up",
                "[1518-11-04 00:02] Guard #99 begins shift",
                "[1518-11-04 00:36] falls asleep",
                "[1518-11-04 00:46] wakes up",
                "[1518-11-05 00:03] Guard #99 begins shift",
                "[1518-11-05 00:45] falls asleep",
                "[1518-11-05 00:55] wakes up",
            };

            var guardRecords = new List<GuardRecord>();

            foreach (var line in testRecords)
            {
                guardRecords.Add(ParseLine(line));
            }

            return guardRecords;
        }

        public static GuardRecord ParseLine(string line)
        {
            var guardRecord = new GuardRecord();

            //[1518-11-01 00:00] Guard #10 begins shift
            //[1518-11-01 00:05] falls asleep
            //[1518-11-01 00:25] wakes up
            var reGuardRecord = new System.Text.RegularExpressions.Regex(@"\[(\d+)-(\d+)-(\d+) (\d+):(\d+)\] (.+)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            var reGuardNum = new System.Text.RegularExpressions.Regex(@"Guard #(\d+)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            var match = reGuardRecord.Match(line);
            if (match.Success)
            {
                guardRecord.timestamp = new DateTime(int.Parse(match.Groups[1].Value),
                                                     int.Parse(match.Groups[2].Value),
                                                     int.Parse(match.Groups[3].Value),
                                                     int.Parse(match.Groups[4].Value),
                                                     int.Parse(match.Groups[5].Value),
                                                     0);

                string recordType = match.Groups[6].Value;

                switch (recordType.First())
                {
                    case 'G':
                        var matchNum = reGuardNum.Match(recordType);
                        if (matchNum.Success)
                        {
                            guardRecord.WakeUp = true;
                            guardRecord.GuardId = int.Parse(matchNum.Groups[1].Value);
                        }
                        break;
                    case 'f':
                        guardRecord.WakeUp = false;
                        break;
                    case 'w':
                        guardRecord.WakeUp = true;
                        break;

                }
            }

            return guardRecord;
        }
    }
}
