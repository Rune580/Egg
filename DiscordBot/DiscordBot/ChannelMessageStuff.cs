using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class ChannelMessageStuff
    {
        public ChannelMessageStuff(ISocketMessageChannel channel)
        {
            Channel = channel;
        }
        public ISocketMessageChannel Channel { get; private set; }
        public SocketMessage PrevMessage { get; set; }
    }
}
