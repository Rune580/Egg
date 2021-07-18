using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiscordBot.CustomCommands;
using DiscordBot.EggCommands;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace DiscordBot.CommandAttributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class EggCommandAttribute : Attribute
    {
        private readonly string[] _triggers;

        public EggCommandAttribute(string baseTrigger, params string[] triggers)
        {
            List<string> tempTriggers = new List<string>();
            
            foreach (var trigger in triggers)
            {
                if (!tempTriggers.Contains(trigger))
                    tempTriggers.Add(trigger);
                
                if (trigger.Contains(' ', StringComparison.InvariantCulture))
                {
                    var tempTrigger = trigger.Replace(" ", "");
                    
                    if (!tempTriggers.Contains(tempTrigger))
                        tempTriggers.Add(tempTrigger);
                }
            }

            _triggers = tempTriggers.ToArray();
            
            CustomCommandsManager.RegisterTriggers(_triggers);
            EggCommandsManager.RegisterCommand(_triggers, baseTrigger);
        }

        public bool Matches(string trigger)
        {
            return _triggers.Any(s => string.Equals(s, trigger, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}