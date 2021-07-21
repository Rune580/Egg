using System.Collections.Generic;
using System.Text;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.CommandsNext.Entities;
using DSharpPlus.Entities;

namespace DiscordBot.EggCommands
{
    public class EggCommandHelpFormatter : BaseHelpFormatter
    {
        private readonly DiscordEmbedBuilder _embedBuilder;
        //protected StringBuilder _stringBuilder;
        
        public EggCommandHelpFormatter(CommandContext ctx) : base(ctx)
        {
            _embedBuilder = new DiscordEmbedBuilder();
            //_stringBuilder = new StringBuilder();
        }

        public override BaseHelpFormatter WithCommand(Command command)
        {
            var eggCommand = EggCommandsManager.GetEggCommandFromCommand(command);

            _embedBuilder.AddField(eggCommand != null ? eggCommand.GetPreferredTrigger() : command.Name, string.IsNullOrEmpty(command.Description) ? "No description provided." : command.Description);
            
            if (eggCommand != null)
            {
                StringBuilder stringBuilder = new StringBuilder();

                stringBuilder.AppendJoin(", ", eggCommand.GetExclusiveAliases());

                _embedBuilder.AddField("Aliases", stringBuilder.ToString());
            }
            
            //_stringBuilder.AppendLine($"{command.Name} - {command.Description}");

            return this;
        }

        public override BaseHelpFormatter WithSubcommands(IEnumerable<Command> subCommands)
        {
            foreach (var command in subCommands)
            {
                var eggCommand = EggCommandsManager.GetEggCommandFromCommand(command);

                _embedBuilder.AddField(eggCommand != null ? eggCommand.GetPreferredTrigger() : command.Name, string.IsNullOrEmpty(command.Description) ? "No description provided." : command.Description);
                //_stringBuilder.AppendLine($"{command.Name} - {(string.IsNullOrEmpty(command.Description) ? "No description provided." : command.Description)}");
            }

            return this;
        }

        public override CommandHelpMessage Build()
        {
            return new(embed: _embedBuilder.Build());
        }
    }
}