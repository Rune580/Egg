using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class UnSentUserMessage
    {
        public ulong ID;
        public int Timer;
        public ISocketMessageChannel Channel;
        public int Cooldown;
        public string UserName;
        public UnSentUserMessage(ulong _ID, int _Timer, ISocketMessageChannel _Channel, SocketUser _UserName)
        {
            ID = _ID;
            Timer = _Timer;
            Channel = _Channel;
            Cooldown = 0;
            UserName = _UserName.Username;
        }
    }
    public static class UnSentHandler
    {
        public static List<UnSentUserMessage> unSents = new List<UnSentUserMessage>();
        public static void Update()
        {
            for (int i = 0; i < unSents.Count; i++)
            {
                if (unSents[i].Timer == 0)
                {
                    unSents[i].Channel.SendMessageAsync($"Alright then {unSents[i].UserName}, keep your secrets.");
                    unSents[i].Cooldown = (60 * 60) + 1;
                    unSents[i].Timer = -1;
                }
                if (unSents[i].Timer > 0)
                {
                    unSents[i].Timer--;
                }
                if (unSents[i].Cooldown > 0)
                {
                    unSents[i].Cooldown--;
                }
            }
        }
        public static void ResetTimer(ulong ID, ISocketMessageChannel Channel, SocketUser user)
        {
            bool resetit = false;
            for (int i = 0; i < unSents.Count; i++)
            {
                if (unSents[i].ID == ID)
                {
                    unSents[i].Channel = Channel;
                    unSents[i].Timer = 20;
                    resetit = true;
                    if (unSents[i].Cooldown != 0)
                    {
                        unSents[i].Timer = -1;
                    }
                    break;
                }
            }
            if (!resetit)
            {
                unSents.Add(new UnSentUserMessage(ID, 20, Channel, user));
            }
        }
        public static void SentMessage(ulong ID, ISocketMessageChannel Channel, SocketUser user)
        {
            for (int i = 0; i < unSents.Count; i++)
            {
                if (unSents[i].ID == ID)
                {
                    unSents[i].Cooldown = (60 * 60) + 1;
                    unSents[i].Timer = -1;
                    break;
                }
                if (i == unSents.Count - 1)
                {
                    unSents.Add(new UnSentUserMessage(ID, 20, Channel, user));
                    unSents[i+1].Cooldown = (60 * 60) + 1;
                    unSents[i+1].Timer = -1;
                }
            }
        }
    }
}
