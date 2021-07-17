using System.Collections.Generic;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.CommandsNext.Entities;

namespace DiscordBot.Legacy
{
    public class LegacyCommandHelpFormatter : BaseHelpFormatter
    {
        public LegacyCommandHelpFormatter(CommandContext ctx) : base(ctx)
        {
        }

        public override BaseHelpFormatter WithCommand(Command command)
        {
            throw new System.NotImplementedException();
        }

        public override BaseHelpFormatter WithSubcommands(IEnumerable<Command> subcommands)
        {
            throw new System.NotImplementedException();
        }

        public override CommandHelpMessage Build()
        {
            throw new System.NotImplementedException();
        }
    }
}