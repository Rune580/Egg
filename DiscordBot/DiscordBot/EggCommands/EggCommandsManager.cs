using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiscordBot.CommandAttributes;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;

namespace DiscordBot.EggCommands
{
    public static class EggCommandsManager
    {
        //private static readonly Dictionary<EggCommandAttribute, string> TriggerAliases = new();

        private static readonly List<EggCommandAttribute> EggCommands = new();
        
        public static void RegisterCommand(EggCommandAttribute eggCommand)
        {
            EggCommands.Add(eggCommand);
        }

        public static EggCommandAttribute GetEggCommandFromCommand(Command command)
        {
            foreach (var eggCommand in EggCommands)
            {
                if (string.Equals(eggCommand.GetInternalTrigger(), command.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    return eggCommand;
                }
            }

            return null;
        }

        public static bool IsBotTriggerRegistered(string trigger)
        {
            return EggCommands.Any(eggCommand => eggCommand.MatchesAny(trigger));
        }

        private static string GetCommandString(string message)
        {
            Dictionary<string, EggCommandAttribute> matches = new Dictionary<string, EggCommandAttribute>();

            foreach (var eggCommand in EggCommands)
            {
                if (message.StartsWith("help") && message.Length > 4)
                {
                    var nextChar = message[4];

                    if (!Char.IsWhiteSpace(nextChar))
                        break;
                    
                    var commandString = message.Remove(0, 5);
                    
                    if (eggCommand.BestMatchInMessage(commandString, out var subMatch))
                    {
                        matches.Add(subMatch, eggCommand);
                    }
                    else if (!eggCommand.ShowInternalTrigger && commandString.StartsWith(eggCommand.GetInternalTrigger(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        break;
                    }
                }
                else if (eggCommand.BestMatchInMessage(message, out var match))
                {
                    matches.Add(match, eggCommand);
                }
                else if (!eggCommand.ShowInternalTrigger && message.StartsWith(eggCommand.GetInternalTrigger(), StringComparison.InvariantCultureIgnoreCase))
                {
                    break;
                }
            }

            if (matches.Count == 0)
                return message;

            var sortedMatches = (from entry in matches orderby entry.Key.Length descending select entry).ToDictionary(pair => pair.Key, pair => pair.Value);

            var bestMatch = sortedMatches.First();

            int index = message.IndexOf(bestMatch.Key, StringComparison.InvariantCultureIgnoreCase);
            
            message = message.Remove(index, bestMatch.Key.Length);
            message = message.Insert(index, bestMatch.Value.GetInternalTrigger());
            
            return message.Trim();
        }
        
        public static Task HandleCommand(DiscordClient client, MessageCreateEventArgs e)
        {
            if (e.Author.IsCurrent)
                return Task.CompletedTask;
            
            var commandsNext = client.GetCommandsNext();
            var msg = e.Message;

            var cmdString = GetCommandString(msg.Content);

            var command = commandsNext.FindCommand(cmdString, out var args);

            var ctx = commandsNext.CreateContext(msg, "", command, args);

            Task.Run(async () => await commandsNext.ExecuteCommandAsync(ctx));
            
            return Task.CompletedTask;
        }
    }
}