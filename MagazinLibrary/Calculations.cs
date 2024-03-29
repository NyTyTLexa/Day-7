using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagazinLibrary
{
    public class Calculations
    {
        public static string[] AvailablePeriods(TimeSpan[] startTimes, int[] durations, TimeSpan beginWorkingTime, TimeSpan endWorkingTime, int consultationTime)
        {
            List<string> availablePeriods = new List<string>();

            TimeSpan currentTime = beginWorkingTime;
            TimeSpan endTime = endWorkingTime;

            for (int i = 0; i < startTimes.Length; i++)
            {
                TimeSpan busyPeriodStart = startTimes[i];
                TimeSpan busyPeriodEnd = startTimes[i] + TimeSpan.FromMinutes(durations[i]);

                if (busyPeriodStart >= currentTime)
                {
                    TimeSpan freePeriodEnd = busyPeriodStart;
                    TimeSpan freePeriodStart = freePeriodEnd - TimeSpan.FromMinutes(consultationTime);

                    if (freePeriodStart >= beginWorkingTime && freePeriodEnd <= endTime)
                    {
                        availablePeriods.Add($"{freePeriodStart:hh\\:mm} - {freePeriodEnd:hh\\:mm}");
                    }
                }

                currentTime = busyPeriodEnd;
            }

            if (currentTime + TimeSpan.FromMinutes(consultationTime) <= endTime)
            {
                availablePeriods.Add($"{currentTime:hh\\:mm} - {endTime:hh\\:mm}");
            }

            if (availablePeriods.Count == 0)
            {
                availablePeriods.Add($"{beginWorkingTime:hh\\:mm} - {endWorkingTime:hh\\:mm}");
            }

            return availablePeriods.ToArray();
        }
    }
}
