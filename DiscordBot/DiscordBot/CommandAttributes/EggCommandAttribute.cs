using System;
using System.Collections.Generic;
using System.Linq;
using DiscordBot.CustomCommands;
using DiscordBot.EggCommands;

namespace DiscordBot.CommandAttributes
{
    /// <summary>
    /// The EggCommandAttribute allows hiding the default command name, and provides
    /// the ability to have whitespaces in the command triggers. The default command name
    /// is hidden by default.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class EggCommandAttribute : Attribute
    {
        private readonly string[] _triggers;
        private readonly string _internalTrigger;
        public bool ShowInternalTrigger { get; }

        /// <summary>
        /// Marks this command as an EggCommand.
        /// Hides the default command name.
        /// Allows whitespaces in the command triggers.
        /// </summary>
        /// <param name="internalTrigger"> The method name, hidden by default.</param>
        /// <param name="triggers"> The command triggers to be used to invoke this command. Not case-sensitive. Any triggers with white spaces will have an alias without white spaces generated. </param>
        public EggCommandAttribute(string internalTrigger, params string[] triggers)
        {
            _internalTrigger = internalTrigger;
            
            List<string> tempTriggers = new List<string>();
            
            foreach (var trigger in triggers)
            {
                if (!tempTriggers.Contains(trigger))
                    tempTriggers.Add(trigger);

                if (!trigger.Contains(' ', StringComparison.InvariantCulture))
                    continue;
                
                var tempTrigger = trigger.Replace(" ", "");
                    
                if (!tempTriggers.Contains(tempTrigger))
                    tempTriggers.Add(tempTrigger);
            }

            _triggers = tempTriggers.ToArray();
            
            EggCommandsManager.RegisterCommand(this);
        }

        public EggCommandAttribute(string internalTrigger, bool showInternalTrigger, params string[] triggers) : this(internalTrigger, triggers)
        {
            ShowInternalTrigger = showInternalTrigger;
        }

        public bool BestMatchInMessage(string message, out string bestMatch)
        {
            bestMatch = "";
            
            List<string> matches = _triggers.Where(trigger => message.StartsWith(trigger, StringComparison.InvariantCultureIgnoreCase)).ToList();

            if (matches.Count == 0)
                return false;
            
            matches.Sort((a, b) => b.Length - a.Length);
            
            bestMatch = matches[0];
            
            return true;
        }

        public bool MatchesAny(string triggerToCheck)
        {
            return _triggers.Any(trigger => string.Equals(trigger, triggerToCheck, StringComparison.InvariantCultureIgnoreCase));
        }

        public string GetPreferredTrigger()
        {
            return _triggers[0];
        }

        public string[] GetExclusiveAliases()
        {
            List<string> aliases = new List<string>();
            
            for (int i = 1; i < _triggers.Length; i++)
            {
                aliases.Add(_triggers[i]);
            }

            return aliases.ToArray();
        }
        
        public string GetInternalTrigger()
        {
            return _internalTrigger;
        }
    }
}