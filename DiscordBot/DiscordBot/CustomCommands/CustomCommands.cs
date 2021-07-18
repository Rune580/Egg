using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiscordBot.CommandAttributes;
using DiscordBot.CustomCommands.Storage;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace DiscordBot.CustomCommands
{
    public class CustomCommands : BaseCommandModule
    {
        [Command]
        [EggCommand("AddCustomCommand" ,"Add Command", "Create Command")]
        public async Task AddCustomCommand(CommandContext ctx, DiscordMessage discordMessage)
        {
            string message = discordMessage.Content;
            if (!message.Contains("^"))
                return;

            message = message.Replace("AddCustomCommand ", "", StringComparison.InvariantCultureIgnoreCase);

            var messageBuilder = new DiscordMessageBuilder();

            var messageList = message.Split("^", StringSplitOptions.TrimEntries).ToList();

            var trigger = messageList[0];

            if (CustomCommandsManager.IsTriggerRegistered(trigger))
            {
                await messageBuilder
                    .WithContent($"The trigger {trigger} is already in use by another command!")
                    .SendAsync(ctx.Channel);

                return;
            }
            
            messageList.RemoveAt(0);

            CustomCommand command = new CustomCommand
            {
                owner = ctx.User.Id
            };

            var fuzzy = messageList[0];
            if (string.Equals(fuzzy, "true", StringComparison.InvariantCultureIgnoreCase) || string.Equals(fuzzy, "false", StringComparison.InvariantCultureIgnoreCase))
            {
                command.fuzzy = string.Equals(fuzzy, "true", StringComparison.InvariantCultureIgnoreCase);
                
                messageList.RemoveAt(0);
            }

            var disguiseAsOwner = messageList[0];
            if (string.Equals(disguiseAsOwner, " true", StringComparison.InvariantCultureIgnoreCase))
            {
                command.disguises = new List<ulong> { ctx.User.Id };
                
                messageList.RemoveAt(0);
            }

            command.content = messageList.ToArray();
            command.returnType = CustomCommandReturnType.Message;
            command.requirePrefix = false;
            
            CustomCommandsManager.AddCustomCommand(command);

            await messageBuilder
                .WithContent("Command successfully added!\n" +
                             "Show all custom commands with ShowCustomCommands.")
                .SendAsync(ctx.Channel);
        }
    }
}