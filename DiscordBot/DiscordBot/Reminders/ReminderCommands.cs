using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;


namespace DiscordBot.Reminders
{
    public class ReminderCommands : BaseCommandModule
    {
        [Command]
        [Description("Create a reminder")]
        [Aliases("CreateReminder")]
        public async Task RemindMe(CommandContext ctx, [Description("When remind")] DateTime message)
        {
            
        }
    }
}