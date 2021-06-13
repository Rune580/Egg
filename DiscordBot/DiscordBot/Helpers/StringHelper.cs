using System.Linq;

namespace DiscordBot.Helpers
{
    public static class StringHelper
    {
        private static readonly int[] Numbers = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        public static string RemoveNumbers(this string current)
        {
            return Numbers.Aggregate(current, (current1, number) => current1.Replace(number.ToString(), ""));
        }
    }
}