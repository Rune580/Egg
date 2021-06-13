using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiscordBot.Helpers;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.Entities;

namespace DiscordBot.Reminders
{
    public class MessageToDateTime : IArgumentConverter<DateTime>
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
        
        public Task<Optional<DateTime>> ConvertAsync(string value, CommandContext ctx)
        {
            string[] args = value.Split(' ');

            return new Task<Optional<DateTime>>(() => FromRelative(args));
        }

        private DateTime FromRelative(string[] args)
        {
            int currentNum = -1;
            TimeAlias currentTime = TimeAlias.Invalid;
            
            long relativeUnixTime = 0;
            
            for (int i = 0; i < args.Length; i++)
            {
                if (Int32.TryParse(args[i], out int num))
                {
                    currentNum = num;
                }
                else if (TryGetAlias(args[i], out TimeAlias alias))
                {
                    currentTime = alias;
                }

                if (currentNum > -1 && currentTime != TimeAlias.Invalid)
                {
                    DateTime.pa
                }
            }
        }

        private Tuple<long, TimeAlias>[] GetTimes(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                args[i]
            }
        }

        private bool TryGetAlias(string value, out TimeAlias timeAlias)
        {
            timeAlias = TimeAlias.Invalid;
            
            int bestMatch = 0;

            foreach (var aliasPair in _aliases.Where(aliasPair => value.Contains(aliasPair.Key) && aliasPair.Key.Length > bestMatch))
            {
                timeAlias = aliasPair.Value;
                bestMatch = aliasPair.Key.Length;
            }

            return timeAlias != TimeAlias.Invalid;
        }
        
        private long TimeToSeconds()
        
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