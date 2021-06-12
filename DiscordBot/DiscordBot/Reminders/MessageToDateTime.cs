using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.Entities;

namespace DiscordBot.Reminders
{
    public class MessageToDateTime : IArgumentConverter<DateTime>
    {
        private static Dictionary<string, string> _aliases = new Dictionary<string, string>();
        
        public Task<Optional<DateTime>> ConvertAsync(string value, CommandContext ctx)
        {
            string[] args = value.Split(' ');
            
            
        }
    }
}