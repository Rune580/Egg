using System;
using System.Threading.Tasks;
using DiscordBot.Reminders;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.Interactivity.Extensions;
using Microsoft.Extensions.Logging;

namespace DiscordBot
{
    public class Egg
    {
        private static void Main(string[] args)
        {
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private static async Task MainAsync(string[] args)
        {
            EggConfig config = ConfigManager.GetEggConfig();
            
            DiscordClient discordClient = new DiscordClient(new DiscordConfiguration
            {
                Token = config.Token,
                TokenType = TokenType.Bot,
                
                AutoReconnect = true,
                //Intents = DiscordIntents.All
            });
            
            CommandsNextExtension commands = discordClient.UseCommandsNext(new CommandsNextConfiguration
            {
                PrefixResolver = msg => Task.FromResult(0),
                CaseSensitive = false,
                EnableDms = false,
                IgnoreExtraArguments = false,
                UseDefaultCommandHandler = true,
                
            });
            
            commands.RegisterConverter(new MessageToDateTime());
            
            commands.RegisterCommands<ReminderCommands>();
            
            var icfg = new InteractivityConfiguration()
            {
                Timeout = TimeSpan.FromSeconds(15),
                ResponseBehavior = InteractionResponseBehavior.Ack,
                ResponseMessage = "That's not a valid button"
            };

            InteractivityExtension interactivity = discordClient.UseInteractivity(icfg);


            discordClient.Ready += DiscordClientOnReady;
            
            await discordClient.ConnectAsync();
            await Task.Delay(-1);
        }

        private static async Task DiscordClientOnReady(DiscordClient sender, ReadyEventArgs e)
        {
            sender.Logger.Log(LogLevel.Information, "Ready!");
        }
    }
}