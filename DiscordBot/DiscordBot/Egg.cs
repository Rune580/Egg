using System;
using System.Threading.Tasks;
using DiscordBot.CustomCommands;
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
            MessageToDateTime.AddAlias("second(s)", MessageToDateTime.TimeAlias.Seconds);
            MessageToDateTime.AddAlias("sec(s)", MessageToDateTime.TimeAlias.Seconds);
            MessageToDateTime.AddAlias("s", MessageToDateTime.TimeAlias.Seconds);
            
            MessageToDateTime.AddAlias("minute(s)", MessageToDateTime.TimeAlias.Minutes);
            MessageToDateTime.AddAlias("min(s)", MessageToDateTime.TimeAlias.Minutes);
            MessageToDateTime.AddAlias("m", MessageToDateTime.TimeAlias.Minutes);
            
            MessageToDateTime.AddAlias("hour(s)", MessageToDateTime.TimeAlias.Hours);
            MessageToDateTime.AddAlias("h", MessageToDateTime.TimeAlias.Hours);
            
            MessageToDateTime.AddAlias("day(s)", MessageToDateTime.TimeAlias.Days);
            
            MessageToDateTime.AddAlias("month(s)", MessageToDateTime.TimeAlias.Months);
            
            MessageToDateTime.AddAlias("year(s)", MessageToDateTime.TimeAlias.Years);


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

            commands.RegisterCommands<ReminderCommands>();
            
            var icfg = new InteractivityConfiguration()
            {
                Timeout = TimeSpan.FromSeconds(15),
                ResponseBehavior = InteractionResponseBehavior.Ack,
                ResponseMessage = "That's not a valid button"
            };

            InteractivityExtension interactivity = discordClient.UseInteractivity(icfg);

            CustomCommandsManager.Init();

            discordClient.MessageCreated += CustomCommandsManager.CustomCommandHandler;

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