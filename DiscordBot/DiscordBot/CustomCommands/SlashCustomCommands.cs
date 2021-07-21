using System.Threading.Tasks;
using DiscordBot.CustomCommands.Storage;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace DiscordBot.CustomCommands
{
    public class SlashCustomCommands : SlashCommandModule
    {
        [SlashCommand("newcommand", "Add a new custom command to Egg!")]
        public async Task AddCustomCommand(InteractionContext ctx, [Option("trigger", "The trigger for the new command.")] string trigger)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
            
            var responseBuilder = new DiscordWebhookBuilder()
                .WithContent($"Command Trigger: `{trigger}`");

            CustomCommand customCommand = new CustomCommand()
            {
                triggers = { trigger },
                returnType = CustomCommandReturnType.Message
            };

            DiscordSelectComponentOption[] fuzzyOptions =
            {
                new DiscordSelectComponentOption("Start With", "0", "The message must start with the trigger.", true),
                new DiscordSelectComponentOption("Contains", "1", "The trigger can be anywhere inside a message.")
            };
            
            var fuzzySelectComponent = new DiscordSelectComponent("fuzzy_select", "Choose trigger option", fuzzyOptions);
            var addAliasButton = new DiscordButtonComponent(ButtonStyle.Primary, "add_alias", "Add Alias", false, new DiscordComponentEmoji(DiscordEmoji.FromName(ctx.Client, ":heavy_plus_sign:", false)));
            var addContentButton = new DiscordButtonComponent(ButtonStyle.Primary, "add_content", "Add Content", false, new DiscordComponentEmoji(DiscordEmoji.FromName(ctx.Client, ":lips:", false)));

            responseBuilder.AddComponents(addAliasButton, addContentButton);

            responseBuilder.AddComponents(fuzzySelectComponent);
            
            await ctx.EditResponseAsync(responseBuilder);

            ctx.Client.ComponentInteractionCreated += async (sender, args) =>
            {
                await args.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);

                if (args.User != ctx.User)
                {
                    await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                        new DiscordInteractionResponseBuilder().AsEphemeral(true)
                            .WithContent("You are not the user who started this."));
                    return;
                }

                switch (args.Id)
                {
                    case "fuzzy_select":
                        customCommand.fuzzy = args.Values[0] == "1";
                        break;
                    case "add_content":
                        
                        break;
                }
            };
        }
    }
}