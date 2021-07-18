using System;
using System.Collections.Generic;
using System.Linq;
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

        private static List<string> _registeredTriggers;
        
        public static void Init()
        {
            _registeredTriggers = new List<string>();
            
            if (File.Exists("CustomCommands"))
            {
                _commandContainer = Deserialize();
            }
            else if (File.Exists("../../../UserCommands.txt") || File.Exists("../../UserCommands.txt"))
            {
                _commandContainer = LoadFromLegacy();
            }
            else
            {
                _commandContainer = new CustomCommandContainer();
            }

            if (_commandContainer.commands == null)
            {
                _commandContainer.commands = new List<CustomCommand>();
            }

            Serialize();
        }
        
        public static Task CustomCommandHandler(DiscordClient client, MessageCreateEventArgs e)
        {
            var command = GetCustomCommand(e.Message.Content);

            if (command != null)
            {
                RunCustomCommand(e.Channel, command);
            }
            
            return Task.CompletedTask;
        }

        public static void AddCustomCommand(CustomCommand command)
        {
            _registeredTriggers.Add(command.trigger);

            _commandContainer.commands?.Add(command);
        }

        public static void RegisterTriggers(IEnumerable<string> triggers)
        {
            _registeredTriggers ??= new List<string>();
            
            foreach (var trigger in triggers)
                _registeredTriggers.Add(trigger);
        }

        public static bool IsTriggerRegistered(string trigger)
        {
            return _registeredTriggers.Any(s => string.Equals(s, trigger, StringComparison.InvariantCultureIgnoreCase));
        }

        private static CustomCommandContainer LoadFromLegacy()
        {
            CustomCommand[] commands = LegacyCustomCommandsHandler.LoadCommands();

            foreach (var command in commands)
            {
                _registeredTriggers.Add(command.trigger);
            }
            
            CustomCommandContainer container = new CustomCommandContainer()
            {
                commands = commands
            };
            return container;
        }

        public static CustomCommand GetCustomCommand(string message)
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

        public static async void RunCustomCommand(DiscordChannel channel, CustomCommand command)
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
        
        private static CustomCommandContainer Deserialize()
        {
            var data = File.ReadAllBytes("CustomCommands");
            return FlatBufferSerializer.Default.Parse<CustomCommandContainer>(data);
        }
    }
}