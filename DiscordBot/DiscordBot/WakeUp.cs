using Discord;
using Discord.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot
{
    class WakeUp
    {
        bool working = false;
        public void StartUp(IGuildUser user)
        {
            if (!working)
            {
                working = true;
                if (user.VoiceChannel != null)
                {
                    if (user.IsSelfMuted || user.IsSelfDeafened)
                    {
                        var start = user.VoiceChannel;
                        var yeet = user.VoiceChannel.Guild.CreateVoiceChannelAsync("Baka");
                        while (user.IsSelfMuted || user.IsSelfDeafened)
                        {
                            if (user.VoiceChannel == start)
                            {
                                user.ModifyAsync(x =>
                                {
                                    x.ChannelId = (yeet as IVoiceChannel).Id;
                                });
                            }
                            else
                            {
                                user.ModifyAsync(x =>
                                {
                                    x.ChannelId = start.Id;
                                });
                            }
                        }
                        if (user.VoiceChannel != start)
                        {
                            user.ModifyAsync(x =>
                            {
                                x.ChannelId = start.Id;
                            });
                        }
                        (yeet as IVoiceChannel).DeleteAsync();
                    }
                }
                working = false;
            }
        }
    }
}
