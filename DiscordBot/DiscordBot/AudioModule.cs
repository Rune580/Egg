using System.Threading.Tasks;
using Discord.Commands;
using Discord;
using DiscordBot;
using System.Diagnostics;
using Discord.Audio;
using System;
namespace DiscordBot
{
    public class AudioModule2 : ModuleBase<ICommandContext>
    {
        public static IAudioClient client;
        private Process CreateStream(string url)
        {
            Process currentsong = new Process();
            try
            {
                currentsong.StartInfo = new ProcessStartInfo
                {
                    FileName = @"..\..\ffmpeg\bin\youtube-dl.exe",
                    Arguments = $"-f m4a {url}",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                };
            }
            catch
            {
                Console.WriteLine("big gay");
            }
            currentsong.Start();
            return currentsong;
        }
        [Command("joinme", RunMode = RunMode.Async)]
        public async Task play(string url)
        {
            IVoiceChannel channel = (Context.User as IVoiceState).VoiceChannel;
            Console.WriteLine(channel);
            IAudioClient client = await channel.ConnectAsync();
            Console.WriteLine(client);
            var output = CreateStream(url).StandardOutput.BaseStream;
            Console.WriteLine(output);
            var stream = client.CreatePCMStream(AudioApplication.Music, 128 * 1024);
            Console.WriteLine(stream);
            await output.CopyToAsync(stream);
            await stream.FlushAsync().ConfigureAwait(false);
        }
    }
}
