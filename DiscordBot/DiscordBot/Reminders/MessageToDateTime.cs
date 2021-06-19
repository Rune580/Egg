using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiscordBot.Helpers;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.Entities;
using Microsoft.Extensions.Logging;

namespace DiscordBot.Reminders
{
    public class MessageToDateTime
    {
        private static Dictionary<string, TimeAlias> _aliases = new Dictionary<string, TimeAlias>();

        public static void AddAlias(string alias, TimeAlias timeAlias)
        {
            AddAlias(new []{alias}, timeAlias);
        }
        
        public static void AddAlias(string[] aliasArray, TimeAlias timeAlias)
        {
            foreach (var alias in aliasArray)
            {
                if (alias.Contains("(s)"))
                {
                    var indexOfPlural = alias.IndexOf("(s)", StringComparison.InvariantCultureIgnoreCase);

                    _aliases.Add(alias.Remove(indexOfPlural) + "s", timeAlias);
                }

                _aliases.Add(alias.Replace("(s)", ""), timeAlias);
            }
        }
        
        public static DateTime FromRelative(CommandContext ctx, string input)
        {
            ctx.Client.Logger.Log(LogLevel.Information, $"Starting conversion for {input}");
            
            // long currentNum = -1;
            // TimeAlias currentTime = TimeAlias.Invalid;
            // string currentMatch = String.Empty;

            long relativeUnixTime = 0;
            
            // Remove numbers from the string and create an array of the remaining strings, this should contain our aliases.
            string[] aliasArray = input.ReplaceNumbers().Split(new [] {','}, StringSplitOptions.RemoveEmptyEntries);

            // Iterate through the array of aliases.
            foreach (var alias in aliasArray)
            {
                // Remove aliases from the string and create an array of the numbers.
                string numString = input.Split(new[] {alias}, StringSplitOptions.RemoveEmptyEntries)[0];

                // See if the string in the alias matches any of our registered aliases.
                if (TryGetAlias(alias, out TimeAlias timeAlias, out string match))
                {
                    // Remove the matching alias from the input string.
                    input = input.Remove(input.IndexOf(match, StringComparison.InvariantCultureIgnoreCase), match.Length);
                        
                    // See if the remaining number is a valid number.
                    if (TryGetInt(numString, out long num))
                    {
                        // Remove the number from the input string.
                        input = input.Remove(input.IndexOf(num.ToString(), StringComparison.InvariantCultureIgnoreCase), num.ToString().Length);
                            
                        // Add the time converted to seconds.
                        relativeUnixTime += num.ToSecondsRelative(timeAlias);
                    }
                }
            }

            ctx.Client.Logger.Log(LogLevel.Information, $"Got Time: {relativeUnixTime}");
            
            return DateTimeOffset.FromUnixTimeSeconds(DateTimeOffset.Now.ToUnixTimeSeconds() + relativeUnixTime).DateTime;
        }

        private static bool TryGetInt(string value, out long num)
        {
            num = 0;

            try
            {
                num = value.GetNumber();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private static bool TryGetAlias(string value, out TimeAlias timeAlias, out string match)
        {
            timeAlias = TimeAlias.Invalid;
            
            int bestMatch = 0;

            match = string.Empty;

            foreach (var aliasPair in _aliases.Where(aliasPair => value.Contains(aliasPair.Key) && aliasPair.Key.Length > bestMatch))
            {
                timeAlias = aliasPair.Value;
                bestMatch = aliasPair.Key.Length;
                match = aliasPair.Key;
            }

            return timeAlias != TimeAlias.Invalid;
        }

        public enum TimeAlias
        {
            Invalid,
            Seconds,
            Minutes,
            Hours,
            Days,
            Weeks,
            Months,
            Years
        }
    }
}