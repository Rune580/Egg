using System;
using System.Linq;

namespace DiscordBot.Helpers
{
    public static class StringHelper
    {
        private static readonly int[] Numbers = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        public static string ReplaceNumbers(this string current)
        {
            return Numbers.Aggregate(current, (current1, number) => current1.Replace(number.ToString(), ","));
        }

        public static int GetNumber(this string current)
        {
            char[] filteredChars = current.ToCharArray().Where(c => Numbers.Any(i => c.Equals(i.ToString().ToCharArray()[0]))).ToArray();


            if (Int32.TryParse(new string(filteredChars), out int num))
            {
                return num;
            }

            throw new Exception();
        }

        private static bool Predicate(int arg, char c)
        {
            throw new System.NotImplementedException();
        }
    }
}