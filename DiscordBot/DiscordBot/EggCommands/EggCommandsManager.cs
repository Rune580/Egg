using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;

namespace DiscordBot.EggCommands
{
    public static class EggCommandsManager
    {
        private static Dictionary<string, string> _triggerAliases = new();
        
        public static void RegisterCommand(string[] triggers, string baseTrigger)
        {
            foreach (var trigger in triggers)
                _triggerAliases.Add(trigger.ToLower(CultureInfo.InvariantCulture), baseTrigger.ToLower(CultureInfo.InvariantCulture));
        }

        private static string GetCommandString(string message)
        {
            List<string> matches = new List<string>();

            foreach (var (key, _) in _triggerAliases)
            {
                if (message.StartsWith(key, StringComparison.InvariantCultureIgnoreCase))
                {
                    matches.Add(key);
                }
            }
            
            if (matches.Count == 0)
                return message;
            
            matches.Sort((a, b) => b.Length - a.Length);

            int index = message.IndexOf(matches[0], StringComparison.InvariantCultureIgnoreCase);
            message = message.Remove(index, matches[0].Length);
            message = _triggerAliases[matches[0]] + " " + message;
            
            return message.Trim();
        }
        
        public static Task HandleCommand(DiscordClient client, MessageCreateEventArgs e)
        {
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