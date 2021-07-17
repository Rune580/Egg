using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DiscordBot.CustomCommands.Legacy;
using DiscordBot.CustomCommands.Storage;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using FlatSharp;
using File = System.IO.File;

namespace DiscordBot.CustomCommands
{
    public static class CustomCommandsManager
    {
        private static CustomCommandContainer _commandContainer;
        
        public static void Init()
        {
            //_commandContainer = new CustomCommandContainer();

            _commandContainer = LoadFromLegacy();
            
            Serialize();
        }
        
        public static Task CustomCommandHandler(DiscordClient client, MessageCreateEventArgs e)
        {
            var command = GetCommand(e.Message.Content);

            if (command != null)
            {
                RunCommand(e.Channel, command);
            }
            
            return Task.CompletedTask;
        }

        private static CustomCommandContainer LoadFromLegacy()
        {
            CustomCommandContainer container = new CustomCommandContainer()
            {
                commands = LegacyCustomCommandsHandler.LoadCommands()
            };
            return container;
        }

        private static CustomCommand GetCommand(string message)
        {
            if (_commandContainer.commands == null)
                return null;

            List<CustomCommand> matches = new List<CustomCommand>();

            foreach (var command in _commandContainer.commands)
            {
                if (string.IsNullOrEmpty(command?.trigger))
                    continue;
                
                if (command.fuzzy)
                {
                    if (message.Contains(command?.trigger, StringComparison.InvariantCultureIgnoreCase))
                    {
                        matches.Add(command);
                    }
                }
                else
                {
                    if (message.StartsWith(command?.trigger, StringComparison.InvariantCultureIgnoreCase))
                    {
                        matches.Add(command);
                    }
                }
            }

            if (matches.Count == 0)
                return null;
            
            // The commands have been checked before they were added to the list.
            // ReSharper disable twice PossibleNullReferenceException
            matches.Sort((a, b) => b.trigger.Length - a.trigger.Length);
                
            return matches[0];
        }

        private static async void RunCommand(DiscordChannel channel, CustomCommand command)
        {
            if (command.content is {Count: > 0})
            {
                Random random = new Random();

                var index = random.Next(0, command.content.Count);
                
                var msg = await new DiscordMessageBuilder().WithContent(command.content[index]).SendAsync(channel);
            }
        }

        private static void Serialize()
        {
            int maxBytesNeeded = FlatBufferSerializer.Default.GetMaxSize(_commandContainer);
            byte[] buffer = new byte[maxBytesNeeded];
            int bytesWritten = FlatBufferSerializer.Default.Serialize(_commandContainer, buffer);
            
            File.WriteAllBytes("CustomCommands", buffer);
        }
    }
}