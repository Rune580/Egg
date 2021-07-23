using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiscordBot.CustomCommands.Legacy;
using DiscordBot.CustomCommands.Storage;
using DiscordBot.Helpers;
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

            _commandContainer.commands ??= new List<CustomCommand>();

            Serialize();
        }
        
        public static Task CustomCommandHandler(DiscordClient client, MessageCreateEventArgs e)
        {
            if (e.Author.IsCurrent)
                return Task.CompletedTask;
            
            var command = GetCustomCommand(e.Message.Content);

            if (command != null)
            {
                RunCustomCommand(client, e, command);
            }
            
            return Task.CompletedTask;
        }

        public static void AddCustomCommand(CustomCommand command)
        {
            _commandContainer ??= new CustomCommandContainer();
            _commandContainer.commands ??= new List<CustomCommand>();
            
            _commandContainer.commands.Add(command);
            RegisterCustomCommandTriggers(command.triggers);
            
            Serialize();
        }


        public static void UpdateCustomCommand(string trigger, CustomCommand newCommand)
        {
            if (_commandContainer.commands == null)
                return;
            
            for (int i = 0; i < _commandContainer.commands.Count; i++)
            {
                var triggers = _commandContainer.commands[i].triggers;
                
                if (triggers != null && triggers.Contains(trigger, StringComparer.InvariantCultureIgnoreCase))
                {
                    _commandContainer.commands[i] = newCommand;
                }
            }
            
            Serialize();
        }

        public static void RegisterCustomCommandTriggers(IEnumerable<string> triggers)
        {
            _registeredTriggers ??= new List<string>();
            
            foreach (var trigger in triggers)
                _registeredTriggers.Add(trigger);
        }

        public static bool IsCustomCommandTriggerRegistered(string trigger, out CustomCommand command)
        {
            command = null;

            bool isRegistered = _registeredTriggers.Any(s => string.Equals(s, trigger, StringComparison.InvariantCultureIgnoreCase));

            if (isRegistered)
            {
                command = GetCustomCommand(trigger);
            }
            
            return isRegistered;
        }

        public static CustomCommand[] GetAllCustomCommands()
        {
            return _commandContainer.commands as CustomCommand[];
        }
        
        private static CustomCommandContainer LoadFromLegacy()
        {
            CustomCommand[] commands = LegacyCustomCommandsHandler.LoadCommands();

            foreach (var command in commands)
            {
                RegisterCustomCommandTriggers(command.triggers);
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

            Dictionary<string, CustomCommand> matches = new Dictionary<string, CustomCommand>();

            foreach (var command in _commandContainer.commands)
            {
                if (command.triggers == null)
                    continue;

                foreach (var commandTrigger in command.triggers)
                {
                    if (string.IsNullOrEmpty(commandTrigger))
                        continue;
                    
                    if (command.fuzzy)
                    {
                        if (message.Contains(commandTrigger, StringComparison.InvariantCultureIgnoreCase))
                        {
                            matches.Add(commandTrigger, command);
                        }
                    }
                    else
                    {
                        if (!message.StartsWith(commandTrigger, StringComparison.InvariantCultureIgnoreCase))
                            continue;
                    
                        if (message.Length > commandTrigger.Length)
                        {
                            var nextChar = message[commandTrigger.Length];
                            if (char.IsWhiteSpace(nextChar))
                            {
                                matches.Add(commandTrigger, command);
                            }
                        }
                        else
                        {
                            matches.Add(commandTrigger, command);
                        }
                    }
                }
            }

            if (matches.Count == 0)
                return null;
            
            var sortedMatches = (from entry in matches orderby entry.Key.Length descending select entry).ToDictionary(pair => pair.Key, pair => pair.Value);

            var bestMatch = sortedMatches.First();
                
            return bestMatch.Value;
        }

        private static async void RunCustomCommand(DiscordClient client, MessageCreateEventArgs eventArgs, CustomCommand command)
        {
            if (command.content is not {Count: > 0})
                return;
            
            Random random = new Random();

            var index = random.Next(0, command.content.Count);

            if (command.disguises is {Count: > 0})
            {
                var webhookBuilder = new DiscordWebhookBuilder().WithContent(command.content[index]);

                var disguise = random.Next(0, command.disguises.Count);

                await webhookBuilder.SetDisguise(client, eventArgs.Guild.Id, command.disguises[disguise]);

                var webhook = await eventArgs.Channel.CreateWebhookAsync("Egg Disguise");
                    
                await webhookBuilder.SendAsync(webhook);

                await webhook.DeleteAsync();
            }
            else
            {
                var msg = await new DiscordMessageBuilder().WithContent(command.content[index]).SendAsync(eventArgs.Channel);
            }
        }

        private static void Serialize()
        {
            int maxBytesNeeded = FlatBufferSerializer.Default.GetMaxSize(_commandContainer);
            byte[] buffer = new byte[maxBytesNeeded];
            FlatBufferSerializer.Default.Serialize(_commandContainer, buffer);
            
            File.WriteAllBytes("CustomCommands", buffer);
        }
        
        private static CustomCommandContainer Deserialize()
        {
            var data = File.ReadAllBytes("CustomCommands");

            var customCommandContainer = FlatBufferSerializer.Default.Parse<CustomCommandContainer>(data);

            if (customCommandContainer.commands != null)
            {
                foreach (var customCommand in customCommandContainer.commands)
                {
                    RegisterCustomCommandTriggers(customCommand.triggers);
                }
            }
            
            return customCommandContainer;
        }
    }
}