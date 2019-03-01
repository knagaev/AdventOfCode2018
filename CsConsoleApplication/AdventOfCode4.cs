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
            public int Month;
            public int Day;
            public int Hour;
            public int Minute;
            public int GuardId;
            public bool WakeUp;
        };

        public static void Run1()
        {
            var guardRecords = ReadTestInput();

            guardRecords.OrderBy(gr => gr.Month).ThenBy(gr => gr.Day).ThenBy(gr => gr.Hour).ThenBy(gr => gr.Minute);

            var sleepRecords = new List<GuardRecord>();

            int guardId = 0;
            int monthBeginSleep = -1;
            int dayBeginSleep = -1;
            int hourBeginSleep = -1;
            int minuteBeginSleep = -1;

            foreach (var guardRecord in guardRecords)
            {
                if (!guardRecord.WakeUp)
                {
                    monthBeginSleep = guardRecord.Month;
                    dayBeginSleep = guardRecord.Day;
                    hourBeginSleep = guardRecord.Hour;
                    minuteBeginSleep = guardRecord.Minute;
                }
                else
                {
                    if (guardRecord.GuardId != 0)
                    {
                        guardId = guardRecord.GuardId;
                    }
                    else
                    {
                        int month = monthBeginSleep;
                        int day = dayBeginSleep;
                        int hour = hourBeginSleep;
                        int minute = minuteBeginSleep;
                        while (hour < guardRecord.Hour || minute < guardRecord.Minute)
                        {
                            sleepRecords.Add(new GuardRecord { Month = guardRecord.Month, Day = guardRecord.Day, Hour = hour, Minute = minute, GuardId = guardId });

                            minute++;
                            if (minute > 59)
                            {
                                minute = 0;
                                hour++;
                            }
                        }
                    }
                }

            }

            Console.WriteLine(guardRecords.Count);
            Console.ReadLine();
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
            var reGuardRecord = new System.Text.RegularExpressions.Regex(@"\[\d+-(\d+)-(\d+) (\d+):(\d+)\] (.+)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            var reGuardNum = new System.Text.RegularExpressions.Regex(@"Guard #(\d+)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            var match = reGuardRecord.Match(line);
            if (match.Success)
            {
                guardRecord.Month = int.Parse(match.Groups[1].Value);
                guardRecord.Day = int.Parse(match.Groups[2].Value);
                guardRecord.Hour = int.Parse(match.Groups[3].Value);
                guardRecord.Minute = int.Parse(match.Groups[4].Value);

                string recordType = match.Groups[5].Value;

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
