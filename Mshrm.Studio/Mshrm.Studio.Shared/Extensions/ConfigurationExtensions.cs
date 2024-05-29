using Mshrm.Studio.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Shared.Extensions
{
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Parses frequency string ie. 1s -> 1000, 1m -> 60000, 1h -> 3600000.. if conversion is set to milliseconds
        /// </summary>
        /// <param name="frequency"></param>
        /// <param name="returnFormat"></param>
        /// <returns>Frequency</returns>
        public static long ParseFrequency(string frequency, TimeUnit returnFormat = TimeUnit.MilliSeconds)
        {
            if (string.IsNullOrEmpty(frequency))
                throw new Exception("No value defined to parse (frequency)");

            var strTimePeriod = frequency.Substring(frequency.Length - 1, 1);
            var value = frequency.Substring(0, frequency.Length - 1);
            var timePeriodInSeconds = 0L;

            switch (strTimePeriod.ToLower())
            {
                case "s":
                    timePeriodInSeconds = long.Parse(value);
                    break;
                case "m":
                    timePeriodInSeconds = long.Parse(value) * 60;
                    break;
                case "h":
                    timePeriodInSeconds = long.Parse(value) * 60 * 60;
                    break;
                case "d":
                    timePeriodInSeconds = long.Parse(value) * 60 * 60 * 24;
                    break;
                case "w":
                    timePeriodInSeconds = long.Parse(value) * 60 * 60 * 24 * 7;
                    break;
            }

            return ConvertSecondsTo(returnFormat, timePeriodInSeconds);
        }

        /// <summary>
        /// Parses frequency string ie. 1s -> 1000, 1m -> 60000, 1h -> 3600000.. if conversion is set to milliseconds
        /// </summary>
        /// <param name="frequency"></param>
        /// <param name="returnFormat"></param>
        /// <returns>Frequency</returns>
        public static long ParseFrequencyConfig(this string frequency, TimeUnit returnFormat = TimeUnit.MilliSeconds)
        {
            return ParseFrequency(frequency, returnFormat);
        }

        /// <summary>
        /// Convert seconds to another format
        /// </summary>
        /// <param name="period"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        private static long ConvertSecondsTo(TimeUnit period, long seconds)
        {
            switch (period)
            {
                case TimeUnit.Minutes:
                    return seconds / 60;
                case TimeUnit.Hours:
                    return seconds / (60 * 60);
                case TimeUnit.Days:
                    return seconds / (60 * 60 * 24);
                case TimeUnit.MilliSeconds:
                    return seconds * 1000L;
                case TimeUnit.Seconds:
                    return seconds;
            }

            throw new Exception("Period not supported");
        }
    }
}
