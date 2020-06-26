using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot
{
    [Serializable]
    struct GalaxyWord
    {
        public string word;
        public DateTime timestamp;
    }
    [Serializable]
    class GalaxyStroke
    {
        public List<ulong> boundchannels = new List<ulong>();
        public List<GalaxyWord> words = new List<GalaxyWord>();
        public void AddWords(string input)
        {
            DateTime time = DateTime.Now;
            string[] yeet = input.Split(' ');
            foreach (var item in yeet)
            {
                GalaxyWord temp;
                temp.word = item;
                temp.timestamp = time;
                words.Add(temp);
            }
        }
        public void UpdateWords()
        {
            DateTime time = DateTime.Now;
            for (int i = 0; i < words.Count; i++)
            {
                if (words[i].timestamp.AddHours(24) < time)
                {
                    words.RemoveAt(i);
                    i--;
                }
            }
        }
        public string RetrieveString(int length = 30)
        {
            List<int> used = new List<int>();
            Random rand = new Random();
            int num;
            StringBuilder sb = new StringBuilder();
            if (length > words.Count)
            {
                length = words.Count;
            }
            for (int i = 0; i < length; i++)
            {
                do
                {
                    num = rand.Next() % words.Count;
                } while (used.Contains(num));
                used.Add(num);
                sb.Append($"{words[num].word} ");
            }
            int num2 = sb.ToString().Length - 2000;
            if (num2 > -1)
            {
                sb = sb.Remove(num2-1, num2);
            }
            for (int x = 0; x < used.Count; x++)
            {
                words.RemoveAt(used[x]);
                for (int i = 0; i < used.Count; i++)
                {
                    if (used[i] > used[x])
                    {
                        used[i]--;
                    }
                }
            }
            return sb.ToString();
        }
        
        public bool GalaxyBind(ulong channel)
        {
            if (boundchannels.Contains(channel))
            {
                return false;
            }
            else
            {
                boundchannels.Add(channel);
                return true;
            }
        }
        public bool GalaxyDebug(SocketMessage m)
        {
            if (boundchannels.Contains(m.Channel.Id))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("All currently available words:\n```");
                for (int i = 0; i < words.Count; i++)
                {
                    if (sb.Length + words[i].word.Length + 3 > 2000)
                    {
                        sb.Append("```");
                        m.Channel.SendMessageAsync(sb.ToString());
                        sb.Clear();
                        sb.Append($"```");
                    }
                    if (i == words.Count-1)
                    {
                        sb.Append($"{words[i].word}```");
                        m.Channel.SendMessageAsync(sb.ToString());
                    }
                    else
                    {
                        sb.Append($"{words[i].word} ");
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
