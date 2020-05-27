using Discord;
using Discord.Rest;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DiscordBot
{
    public class NonExistantUsers
    {
        public NonExistantUsers(ISocketMessageChannel _channel, SocketUser _user)
        {
            channel = _channel;
            user = _user;
        }
        public ISocketMessageChannel channel;
        public int timer = 60 * 30;
        public SocketUser user;
        public void update()
        {
            timer--;
            if (timer <= 0)
            {
                OverwritePermissions perms = new OverwritePermissions(sendMessages: PermValue.Allow);
                (channel as SocketTextChannel).AddPermissionOverwriteAsync(user: user, perms);
            }
            else if (timer == (60 * 30) - 1)
            {
                OverwritePermissions perms = new OverwritePermissions(sendMessages: PermValue.Deny);
                (channel as SocketTextChannel).AddPermissionOverwriteAsync(user: user, perms);
                user.SendMessageAsync($"You just vanished from {channel.Name}! You will not be able to send messages for 30 minutes. If you wish to undo this early reply with [I have a small pp] without the square brackets.");
            }
        }
    }
}
