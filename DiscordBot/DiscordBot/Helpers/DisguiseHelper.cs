using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Exceptions;
using Microsoft.Extensions.Logging;

namespace DiscordBot.Helpers
{
    public static class DisguiseHelper
    {
        /// <summary>
        /// Trys to set the AvatarUrl and Username of the webhook from the guildId and memberId.
        /// If either the guild or the member can't be found, then this will fallback to using a DiscordUser
        /// rather than a DiscordMember, this means it will not use a nickname, just a username.
        /// </summary>
        /// <param name="webhookBuilder"> The webhook builder to apply this disguise to. </param>
        /// <param name="client"> The discord client, used to interact with guilds and members. </param>
        /// <param name="guildId"> Used to find the guild the member is in. </param>
        /// <param name="memberId"> Used to find the member. </param>
        /// <returns></returns>
        public static async Task<DiscordWebhookBuilder> SetDisguise(this DiscordWebhookBuilder webhookBuilder, DiscordClient client, ulong guildId, ulong memberId)
        {
            if (client.Guilds.ContainsKey(guildId))
            {
                DiscordGuild guild = client.Guilds[guildId];

                if (guild.Members.ContainsKey(memberId))
                {
                    DiscordMember member = guild.Members[memberId];

                    string name = member.Nickname;

                    if (string.IsNullOrEmpty(name))
                        name = member.Username;
                    
                    webhookBuilder.Username = name;
                    webhookBuilder.AvatarUrl = member.AvatarUrl;
                }
            }

            try
            {
                DiscordUser user = await client.GetUserAsync(memberId);

                webhookBuilder.Username = user.Username;
                webhookBuilder.AvatarUrl = user.AvatarUrl;
            }
            catch (NotFoundException e)
            {
                // Could not find the user on discord, probably an invalid Id.
                client.Logger.Log(LogLevel.Error, $"UserId: {memberId} was not found!");
            }
            
            return webhookBuilder;
        }
    }
}