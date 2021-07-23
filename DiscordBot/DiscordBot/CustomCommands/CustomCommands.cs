using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiscordBot.CommandAttributes;
using DiscordBot.CustomCommands.Storage;
using DiscordBot.EggCommands;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;

namespace DiscordBot.CustomCommands
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class CustomCommands : BaseCommandModule
    {
        [Command]
        [EggCommand("AddCustomCommand" ,"Add Command", "Create Command")]
        public async Task AddCustomCommand(CommandContext ctx, [RemainingText] string message)
        {
            if (!message.Contains("^"))
                return;

            var messageBuilder = new DiscordMessageBuilder();

            var messageList = message.Split("^", StringSplitOptions.TrimEntries).ToList();

            var trigger = messageList[0];

            if (EggCommandsManager.IsBotTriggerRegistered(trigger))
            {
                await messageBuilder
                    .WithContent($"The trigger {trigger} is already in use by a built-in command!")
                    .SendAsync(ctx.Channel);
                return;
            }

            messageList.RemoveAt(0);

            CustomCommand command = new CustomCommand();

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
            
            command.triggers = new List<string> { trigger };
            command.content = messageList;
            command.returnType = CustomCommandReturnType.Message;
            command.requirePrefix = false;
            
            if (CustomCommandsManager.IsCustomCommandTriggerRegistered(trigger, out var existingCommand))
            {
                bool canEdit = existingCommand.owner == ctx.User.Id || existingCommand.owner == 0;
                
                if (canEdit)
                {
                    messageBuilder.WithContent(
                        $"The trigger `{trigger}` is already in use by another command!\n" +
                        "You can either Append to this command, or Override it.\n" +
                        "Please select an option below to continue, otherwise this operation will automatically be cancelled after 30 seconds.");
                    
                    var appendButton = new DiscordButtonComponent(ButtonStyle.Primary, "append_button", "Append");
                    var overrideButton = new DiscordButtonComponent(ButtonStyle.Danger, "override_button", "Override");
                    var cancelButton = new DiscordButtonComponent(ButtonStyle.Secondary, "cancel_button", "Cancel");

                    var msg = await messageBuilder.AddComponents(appendButton, overrideButton, cancelButton)
                        .SendAsync(ctx.Channel);

                    var result = await msg.WaitForButtonAsync(TimeSpan.FromSeconds(30));

                    while (!result.TimedOut)
                    {
                        await result.Result.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
                        
                        if (result.Result.User.Id != ctx.User.Id)
                        {
                            await result.Result.Interaction.CreateResponseAsync(
                                InteractionResponseType.ChannelMessageWithSource,
                                new DiscordInteractionResponseBuilder().AsEphemeral(true)
                                    .WithContent("You are not the user who started this operation."));
                            continue;
                        }
                        
                        switch (result.Result.Id)
                        {
                            case "append_button":
                                List<string> content = existingCommand.content as List<string>;
                                content?.AddRange(messageList);
                                existingCommand.content = content;
                                await result.Result.Interaction.EditOriginalResponseAsync(new DiscordWebhookBuilder().WithContent($"Command `{trigger}` has been updated!"));
                                break;
                            case "override_button":
                                CustomCommandsManager.UpdateCustomCommand(trigger, command);
                                await result.Result.Interaction.EditOriginalResponseAsync(new DiscordWebhookBuilder().WithContent($"Command `{trigger}` has been overriden!"));
                                break;
                            case "cancel_button":
                                await result.Result.Interaction.EditOriginalResponseAsync(new DiscordWebhookBuilder().WithContent("Add Command operation cancelled."));
                                return;
                        }
                    }

                    if (result.TimedOut)
                    {
                        await result.Result.Interaction.EditOriginalResponseAsync(new DiscordWebhookBuilder().WithContent("Add Command operation cancelled."));
                    }
                }
                return;
            }

            CustomCommandsManager.AddCustomCommand(command);

            await messageBuilder
                .WithContent("Command successfully added!\n" +
                             "Show all custom commands with ShowCustomCommands.")
                .SendAsync(ctx.Channel);
        }

        [Command]
        [EggCommand("ShowAllCustomCommands", "Show Custom Commands")]
        public async Task ShowAllCustomCommands(CommandContext ctx)
        {
            var commands = CustomCommandsManager.GetAllCustomCommands();

            DiscordComponent[] components =
            {
                new DiscordButtonComponent(ButtonStyle.Secondary, "prev_page_button", "", true, new DiscordComponentEmoji(DiscordEmoji.FromName(ctx.Client ,":arrow_left:"))),
                new DiscordButtonComponent(ButtonStyle.Secondary, "next_page_button", "", false, new DiscordComponentEmoji(DiscordEmoji.FromName(ctx.Client, ":arrow_right:")))
            };

            var messageBuilder = new DiscordMessageBuilder();
            messageBuilder.AddComponents(components);
            
            var embedBuilder = new DiscordEmbedBuilder();
            embedBuilder.Title = "All Custom Commands";

            InteractivityResult<ComponentInteractionCreateEventArgs> result;

            do
            {
                if (result.Result == null)
                {
                    
                }
            } while (!result.TimedOut);
        }
    }
}