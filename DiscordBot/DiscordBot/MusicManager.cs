using Discord.Audio;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot
{
    class MusicManager
    {
        DiscordSocketClient Client;
        IAudioClient Audio;
        List<string> Queue;
        Stream output;
        AudioOutStream stream;
        bool connecting = false;
        public MusicManager(DiscordSocketClient _Client)
        {
            Client = _Client;
            Queue = new List<string>();
        }

        async public Task ConnectToChannel(ISocketAudioChannel socketAudioChannel)
        {
            if (connecting == false)
            {
                connecting = true;
                try
                {
                    if (Audio.ConnectionState == Discord.ConnectionState.Connecting)
                    {
                    }
                    else if (Audio.ConnectionState == Discord.ConnectionState.Connected)
                    {
                    }
                    else
                    {
                        Audio = await socketAudioChannel.ConnectAsync();
                    }
                }
                catch
                {
                    Audio = await socketAudioChannel.ConnectAsync();
                }
                connecting = false;
            }
        }

        async public Task Leave()
        {
            Queue.Clear();
            Skip();
            foreach (var item in Client.Guilds)
            {
                foreach (var voice in item.VoiceChannels)
                {
                    try
                    {
                        if (Audio.ConnectionState == Discord.ConnectionState.Connected)
                        {
                            await voice.DisconnectAsync();
                        }
                    }
                    catch
                    {

                    }
                }
            }
            Console.WriteLine("Disconnected");
        }

        async void PlayAudio(string thesound)
        {
            try
            {
                output = CreateStream(thesound).StandardOutput.BaseStream;
                stream = Audio.CreatePCMStream(AudioApplication.Mixed, 128 * 1024);
                await output.CopyToAsync(stream);
                await stream.FlushAsync();
                Console.WriteLine("Finished Audio");
                Queue.RemoveAt(0);
                if (Queue.Count != 0)
                {
                    Console.WriteLine("Queue not empty, playing next song");
                    PlayAudio(Queue[0]);
                }
            }
            catch
            {
                try
                {
                    await stream.FlushAsync();
                    Queue.RemoveAt(0);
                    if (Queue.Count != 0)
                    {
                        Console.WriteLine("Queue not empty, playing next song");
                        PlayAudio(Queue[0]);
                    }
                }
                catch
                {
                }
                Console.WriteLine("Failed to play audio");
            }
        }

        Process CreateStream(string filePath)
        {
            return Process.Start(new ProcessStartInfo
            {
                FileName = @"..\..\ffmpeg.exe",
                Arguments = $"-hide_banner -loglevel panic -i \"{filePath}\" -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true
            });
        }

        public void Enqueue(string _Song)
        {
            Queue.Add(_Song);
            if (Queue.Count == 1)
            {
                PlayAudio(Queue[0]);
            }
        }

        public void AddToStream(string _Song)
        {
            //try
            //{
            //    Stream output2 = CreateStream(_Song).StandardOutput.BaseStream;
            //    output2.CopyTo(stream);
            //}
            //catch
            //{
            //    Console.WriteLine("Couldn't add audio");
            //    Enqueue(_Song);
            //}
            Enqueue(_Song);
        }

        public void Skip()
        {
            try
            {
                output.Close();
                Console.WriteLine("Skipped Song");
            }
            catch
            {
                Console.WriteLine("Failed to skip");
            }
        }
    }
}
