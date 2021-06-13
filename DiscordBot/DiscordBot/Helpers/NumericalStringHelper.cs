using System;
using System.Collections.Generic;
using System.Linq;

namespace DiscordBot.Helpers
{
    public static class NumericalStringHelper
    {
        private static Dictionary<string, float> _numberTable = new Dictionary<string, float>
        {
            {"zero", 0}, {"one", 1}, {"a", 1}, {"an", 1}, {"two", 2}, {"three", 3}, {"four", 4},
            {"five", 5}, {"six", 6}, {"seven", 7}, {"eight", 8}, {"nine", 9}, {"ten", 10}, {"eleven", 11}, 
            {"twelve", 12}, {"thirteen", 13}, {"fourteen", 14}, {"fifteen", 15}, {"sixteen", 16},
            {"seventeen", 17}, {"eighteen", 18}, {"nineteen", 19}, {"twenty", 20}, {"thirty", 30},
            {"forty", 40}, {"fifty", 50}, {"sixty", 60}, {"seventy", 70}, {"eighty", 80}, {"ninety", 90}, 
            {"hundred", 100}, {"thousand", 1000}, {"half", 0.5f}
        };

        public static bool TryToFloat(this string value, out float toFloat)
        {
            toFloat = -1;
            
            foreach (var number in _numberTable.Where(number => string.Equals(value, number.Key, StringComparison.InvariantCultureIgnoreCase)))
            {
                toFloat = number.Value;
            }

            return toFloat >= 0;
        }
    }
}