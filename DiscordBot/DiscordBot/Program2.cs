using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord.API;
using Discord.Audio;
using System.Threading;
using CsvHelper;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using ImageProcessor;
using ImageProcessor.Imaging;
using System.Net;
using Discord.Rest;
using System.Diagnostics;
using YoutubeSearch;
using RedditSharp;
using System.Windows.Forms;
using Discord.Webhook;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace DiscordBot
{
    public struct tempcommand
    {
        public string Trigger;
        public string[] Return;
        public bool include;
        public ulong ID;
        public ulong asMeID;
    };
    class Program2
    {
        #region declarations
        public static ulong strokemessage = 0;
        public static tempcommand _tempcommand;
        public static bool One911 = true;
        public static List<int> Timers = new List<int>();
        public static List<int> Yess = new List<int>();
        public static int Timer = 0;
        public static int Yes = 0;
        public static List<SocketMessage> Messages = new List<SocketMessage>();
        public static string TheMessage = "";
        public static string TheMessageNormal = "";
        public static string TheSender = "";
        public static SocketUser TheSenderNoString;
        public static SocketChannel ogChannel;
        public static List<SocketMessage> ogMessages = new List<SocketMessage>();
        public static ulong MyID = 171749888747503617;
        public static ulong oldPlatinum = 511701588612612126;
        public static ulong Platinum = 603637484597018644;
        public static ulong Rune = 97724098423037952;
        public static ulong EpicGamer = 536038822555680770;
        public static ulong Gears = 151165627665612801;
        public static ulong Egg = 536383522068365312;
        public static bool KilledSelf = false;
        public static int every3seconds = DateTime.Now.Second;
        public static int smashindex = 0;
        public static List<string> smashmouth = CSVCONVERT(@"../../smashmouth.json");
        public static List<string> Last3Message = new List<string>();
        public static List<string> CSVCONVERT(string absolutePath)
        {
            List<string> result = new List<string>();
            string value;
            using (TextReader fileReader = File.OpenText(absolutePath))
            {
                var csv = new CsvReader(fileReader);
                csv.Configuration.Delimiter = "*";
                csv.Configuration.HasHeaderRecord = false;
                while (csv.Read())
                {
                    for (int i = 0; csv.TryGetField<string>(i, out value); i++)
                    {
                        result.Add(value);
                    }
                }
            }
            return result;
        }
        public static int trucktimer = -1;
        public static int truckseconds = DateTime.Now.Second;
        public static int RPCcountdown = -1;
        public static int RPCphasemom = 0;
        public static SocketUser RPC1;
        public static SocketUser RPC2;
        public static string RPCchoice1;
        public static ISocketMessageChannel RPCchannel;
        public static int EverySecond = DateTime.Now.Second;
        public static IAudioClient audioClient;
        public static string gunshot = @"..\..\boomheadshot.wav";
        public static bool trashing = false;
        public static UserReturnMessages CustomCommands;
        public static List<string> BannedCommands = new List<string>();
        public static MusicManager musicManager;
        public class RedditUrl
        {
            public string URL;
            public int Timer;
            public string Subreddit;
        }
        public static List<RedditUrl> UsedURLs = new List<RedditUrl>();
        public static int CurrentMinute = DateTime.Now.Minute;
        public class PreviousMessage
        {
            public PreviousMessage(string _Message, ISocketMessageChannel _Channel)
            {
                PrevMessage = _Message;
                PrevChannel = _Channel;
            }
            public string PrevMessage;
            public ISocketMessageChannel PrevChannel;
        }
        public static List<PreviousMessage> PreviousMessages = new List<PreviousMessage>();
        public static int CurrentHour = DateTime.Now.Hour;
        public static bool working = false;
        public static List<NonExistantUsers> deadpeople = new List<NonExistantUsers>();
        public static List<RemindMe> reminders = new List<RemindMe>();
        public static int cyear = DateTime.Now.Year, cmonth = DateTime.Now.Month, cday = DateTime.Now.Day, chour = DateTime.Now.Hour, cminute = DateTime.Now.Minute, csecond = DateTime.Now.Second;
        public static GalaxyStroke galaxywords = new GalaxyStroke();
        public static List<ulong> nodads = new List<ulong>();
        public static AliasManager manager = new AliasManager();
        public static string tempalias = "";
        public static List<string> tempout;
        public static bool canudont = false;
        #endregion
        static void Main(string[] args)
        {
            TopDownGame TopDown = new TopDownGame();
            #region startup
            DiscordSocketClient Client;
            Client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Info
            });

            CommandService Commands;
            Commands = new CommandService(new CommandServiceConfig
            {
                CaseSensitiveCommands = true,
                DefaultRunMode = RunMode.Async,
                LogLevel = LogSeverity.Debug
            });
            Client.Log += Log;
            Task Log(LogMessage message)
            {
                Console.WriteLine(message.ToString());
                return Task.CompletedTask;
            }
            Client.SetGameAsync($"EggHelp/CustomHelp");
            string Token;
            using (StreamReader r = new StreamReader(@"..\..\token.txt"))
            {
                Token = r.ReadToEnd();
            }

            Client.LoginAsync(TokenType.Bot, Token);
            Client.StartAsync();
            CustomCommands = new UserReturnMessages(Client);
            string line;
            using (StreamReader r = new StreamReader(@"..\..\trashdata.json"))
            {
                line = r.ReadToEnd();
            }
            string line2;
            using (StreamReader r = new StreamReader(@"..\..\RedditURLS.txt"))
            {
                line2 = r.ReadToEnd();
            }
            string[] FirstSplit = line2.Split('\n');
            foreach (var item in FirstSplit)
            {
                string[] SecondSplit = item.Split('\t');
                RedditUrl ___Temp = new RedditUrl();
                for (int i = 0; i < SecondSplit.Count(); i++)
                {
                    if (i == 0)
                    {
                        ___Temp.URL = SecondSplit[i];
                    }
                    else if (i == 1)
                    {
                        ___Temp.Timer = int.Parse(SecondSplit[i]);
                    }
                    else
                    {
                        ___Temp.Subreddit = SecondSplit[i];
                    }
                }
                UsedURLs.Add(___Temp);
            }
            StorageClass temp = JsonConvert.DeserializeObject<StorageClass>(line);
            musicManager = new MusicManager(Client);
            WakeUp wakemachine = new WakeUp();

            IFormatter f = new BinaryFormatter();
            if (File.Exists(@"..\..\reminders.sugondeez"))
            {
                using (Stream r = new FileStream(@"..\..\reminders.sugondeez", FileMode.Open, FileAccess.Read))
                {
                    reminders = (List<RemindMe>)f.Deserialize(r);
                }
            }
            foreach (var item in reminders)
            {
                item.FixTimes();
            }
            IFormatter f2 = new BinaryFormatter();
            if (File.Exists(@"..\..\galaxy.words"))
            {
                using (Stream r = new FileStream(@"..\..\galaxy.words", FileMode.Open, FileAccess.Read))
                {
                    galaxywords = (GalaxyStroke)f2.Deserialize(r);
                }
            }
            if (File.Exists(@"..\..\daddydaddy.doo"))
            {
                using (Stream r = new FileStream(@"..\..\daddydaddy.doo", FileMode.Open, FileAccess.Read))
                {
                    nodads = (List<ulong>)f2.Deserialize(r);
                }
            }
            if (File.Exists(@"..\..\aliases.yourmother"))
            {
                using (Stream r = new FileStream(@"..\..\aliases.yourmother", FileMode.Open, FileAccess.Read))
                {
                    manager = (AliasManager)f2.Deserialize(r);
                }
            }
            #endregion
            #region Banned Commands
            BannedCommands.Add("ASSIGN USER");
            BannedCommands.Add("ASSIGNUSER");
            BannedCommands.Add("ASSIGN PERSON");
            BannedCommands.Add("ASSIGNPERSON");
            BannedCommands.Add("ALIAS HELP");
            BannedCommands.Add("ALIASHELP");
            BannedCommands.Add("REMOVE ALIAS");
            BannedCommands.Add("REMOVEALIAS");
            BannedCommands.Add("ADD ALIAS");
            BannedCommands.Add("ADDALIAS");
            BannedCommands.Add("DELETE ALIAS");
            BannedCommands.Add("DELETEALIAS");
            BannedCommands.Add("SHOW ALIASES");
            BannedCommands.Add("SHOWALIASES");
            BannedCommands.Add("CREATE ALIAS");
            BannedCommands.Add("CREATEALIAS");
            BannedCommands.Add("YES DAD");
            BannedCommands.Add("YESDAD");
            BannedCommands.Add("NO DAD");
            BannedCommands.Add("NODAD");
            BannedCommands.Add("GALAXY BAN LIST");
            BannedCommands.Add("GALAXYBANLIST");
            BannedCommands.Add("GALAXYBLACKLIST");
            BannedCommands.Add("GALAXY BLACKLIST");
            BannedCommands.Add("GALAXYOPTOUT");
            BannedCommands.Add("GALAXY OPT OUT");
            BannedCommands.Add("GALAXY STROKE");
            BannedCommands.Add("GALAXYSTROKE");
            BannedCommands.Add("SAVE THE GALAXY");
            BannedCommands.Add("BIND GALAXY DEBUG");
            BannedCommands.Add("BINDGALAXYDEBUG");
            BannedCommands.Add("GALAXYDEBUG");
            BannedCommands.Add("GALAXY DEBUG");
            BannedCommands.Add("DELETE REMINDER");
            BannedCommands.Add("ERASE REMINDER");
            BannedCommands.Add("CLEAR ALL REMINDERS");
            BannedCommands.Add("DELETE ALL REMINDERS");
            BannedCommands.Add("ERASE ALL REMINDERS");
            BannedCommands.Add("REMINDER HELP");
            BannedCommands.Add("REMINDERS HELP");
            BannedCommands.Add("REMINDERSHELP");
            BannedCommands.Add("REMINDERHELP");
            BannedCommands.Add("CREATEREMINDER");
            BannedCommands.Add("CREATE REMINDER");
            BannedCommands.Add("REMIND ME");
            BannedCommands.Add("!REMINDME");
            BannedCommands.Add("REMINDME");
            BannedCommands.Add("CHECK REMINDERS");
            BannedCommands.Add("CHECKREMINDERS");
            BannedCommands.Add("SHOW REMINDERS");
            BannedCommands.Add("SHOWREMINDERS");
            BannedCommands.Add("REMINDERS");
            BannedCommands.Add("IS RUNE A FURRY");
            BannedCommands.Add("CLAIMCOMMAND");
            BannedCommands.Add("UNCLAIMCOMMAND");
            BannedCommands.Add("ADDMESSAGE");
            BannedCommands.Add("CREATEMESSAGE");
            BannedCommands.Add("APPENDMESSAGE");
            BannedCommands.Add("REMOVEMESSAGE");
            BannedCommands.Add("DELETEMESSAGE");
            BannedCommands.Add("GETMESSAGES");
            BannedCommands.Add("SHOWMESSAGES");
            BannedCommands.Add("MESSAGEHELP");
            BannedCommands.Add("MESSAGE HELP");
            BannedCommands.Add("APPEND");
            BannedCommands.Add("[APPEND]");
            BannedCommands.Add("[OVERRIDE]");
            BannedCommands.Add("OVERRIDE");
            BannedCommands.Add("REVERSESEARCH");
            BannedCommands.Add("I HAVE A SMALL PP");
            BannedCommands.Add("!VANISH");
            BannedCommands.Add("WAKE UP");
            BannedCommands.Add("WAKEUP");
            BannedCommands.Add("TRANSFERCOMMANDS");
            BannedCommands.Add("WHEN ITS");
            BannedCommands.Add("WHEN IT'S");
            BannedCommands.Add("WHEN IVE GOT");
            BannedCommands.Add("WHEN I'VE GOT");
            BannedCommands.Add("WHEN YOUVE GOT");
            BannedCommands.Add("WHEN YOU'VE GOT");
            BannedCommands.Add("WHEN I");
            BannedCommands.Add("WHEN I GOT");
            BannedCommands.Add("WHEN IM");
            BannedCommands.Add("WHEN I'M");
            BannedCommands.Add("WHEN YOU GOT");
            BannedCommands.Add("WHEN YOU");
            BannedCommands.Add("WHEN YOURE");
            BannedCommands.Add("WHEN YOU'RE");
            BannedCommands.Add("DOWNLOADCOMMAND");
            BannedCommands.Add("SHOWAREAS");
            BannedCommands.Add("SHOWWORLDS");
            BannedCommands.Add("MHELP");
            BannedCommands.Add("MINECRAFTHELP");
            BannedCommands.Add("CREATEDOOR");
            BannedCommands.Add("CREATESIGN");
            BannedCommands.Add("EDITSIGN");
            BannedCommands.Add("TOGGLESTATS");
            BannedCommands.Add("F3");
            BannedCommands.Add("CHANGEAREA");
            BannedCommands.Add("CREATEAREA");
            BannedCommands.Add("CREATEWORLD");
            BannedCommands.Add("CANCELDOOR");
            BannedCommands.Add("DELETEDOOR");
            BannedCommands.Add("REMOVEDOOR");
            BannedCommands.Add("PLEFT");
            BannedCommands.Add("PUP");
            BannedCommands.Add("PDOWN");
            BannedCommands.Add("PRIGHT");
            BannedCommands.Add("SAVEWORLD");
            BannedCommands.Add("SHOWCOMMAND");
            BannedCommands.Add("R/");
            BannedCommands.Add("GOOGLE IT");
            BannedCommands.Add("GOOGLING IT");
            BannedCommands.Add("CHANGEICON");
            BannedCommands.Add("UP");
            BannedCommands.Add("DOWN");
            BannedCommands.Add("LEFT");
            BannedCommands.Add("RIGHT");
            BannedCommands.Add("STAY");
            BannedCommands.Add("CHECKCOMMAND");
            BannedCommands.Add("GETCOMMAND");
            BannedCommands.Add("TOGGLECOMMANDTYPE");
            BannedCommands.Add("APPENDCOMMAND");
            BannedCommands.Add("REMOVEFROMCOMMAND");
            BannedCommands.Add("ADDCOMMAND");
            BannedCommands.Add("CREATECOMMAND");
            BannedCommands.Add("LEAVE");
            BannedCommands.Add("SHOWCUSTOMCOMMANDS");
            BannedCommands.Add("EGGRESTART");
            BannedCommands.Add("EGG RESTART");
            BannedCommands.Add("RESTART EGG");
            BannedCommands.Add("RESTARTEGG");
            BannedCommands.Add("CUSTOMHELP");
            BannedCommands.Add("ERASECOMMAND");
            BannedCommands.Add("REMOVECOMMAND");
            BannedCommands.Add("TRASHRESET");
            BannedCommands.Add("ROLESTEST");
            BannedCommands.Add("TRASH");
            BannedCommands.Add("RPS");
            BannedCommands.Add("Y");
            BannedCommands.Add("N");
            BannedCommands.Add("ROCK");
            BannedCommands.Add("PAPER");
            BannedCommands.Add("SCISSORS");
            BannedCommands.Add("GUN");
            BannedCommands.Add("TESTSPOILER");
            BannedCommands.Add("EGGHELP");
            BannedCommands.Add("HELPEGG");
            BannedCommands.Add("EGG HELP");
            BannedCommands.Add("HELP EGG");
            BannedCommands.Add("ROLL");
            BannedCommands.Add("2TEAMS");
            BannedCommands.Add("TRASHTIME");
            BannedCommands.Add("PLAY ");
            BannedCommands.Add("SKIP");
            BannedCommands.Add("JOIN");
            BannedCommands.Add("STORKE");
            BannedCommands.Add("SRTOKE");
            BannedCommands.Add("SUPERSTROKE");
            BannedCommands.Add("SUPER STROKE");
            BannedCommands.Add("STROKE ME DADDY");
            #endregion
            async Task MessageReceived(SocketMessage message)
            {
                bool strokeoutdab = false;
                if (message.Attachments.Count == 0 && message.Id != strokemessage)
                    try
                    {
                        bool appendrules = false;
                        //UnSentHandler.SentMessage(TheSenderNoString.Id, message.Channel, message.Author);
                        TheMessage = message.Content.ToUpper();
                        TheMessageNormal = message.Content;
                        TheSender = message.Author.ToString();
                        TheSenderNoString = message.Author;
                        SocketUser user = message.Author;
                        string CustomString = message.Content.ToUpper();
                        while (CustomString[0] == ' ')
                        {
                            CustomString = CustomString.Remove(0, 1);
                        }
                        while (CustomString[CustomString.Count() - 1] == ' ')
                        {
                            CustomString = CustomString.Remove(CustomString.Count() - 1, 1);
                        }
                        bool StartsWithBannedCommand = false;
                        bool badmessage = false;
                        foreach (var item in deadpeople)
                        {
                            foreach (var person in message.MentionedUsers)
                            {
                                if (person.Id == item.user.Id)
                                {
                                    await message.DeleteAsync();
                                    badmessage = true;
                                }
                            }
                        }
                        foreach (var item in BannedCommands)
                        {
                            if (TheMessage.StartsWith($"{item} "))
                            {
                                StartsWithBannedCommand = true;
                                break;
                            }
                        }
                        if (user.Id != Egg && !badmessage && user.Id != 674716175716057103 && !user.IsWebhook)
                        {
                            //Anywhere
                            #region Anywhere
                            if (TheMessage.Contains("I'M ") || TheMessage.Contains("IM "))
                            {
                                if (!nodads.Contains(message.Author.Id))
                                {
                                    string bigsextemp = GetIm(message);
                                    if (bigsextemp != "")
                                    {
                                        var hook = (message.Channel as SocketTextChannel).CreateWebhookAsync("Daddy");
                                        Image im = new Image($@"..\..\dad.jpg");
                                        Random r = new Random();
                                        int num = r.Next() % 100;
                                        await hook.Result.ModifyAsync(x =>
                                        {
                                            if (num == 0)
                                            {
                                                x.Name = "Daddy Bot";
                                            }
                                            else if (num == 1)
                                            {
                                                x.Name = "Father Bot";
                                            }
                                            else
                                            {
                                                x.Name = "Dad Bot";
                                            }
                                            x.Image = im;
                                        });
                                        DiscordWebhookClient d = new DiscordWebhookClient(hook.Result);
                                        await d.SendMessageAsync(bigsextemp);
                                        await d.DeleteWebhookAsync();
                                    }
                                }
                            }
                            if (TheMessage.StartsWith("PLAY "))
                            {
                                //PlaySong(TheMessageNormal, user);
                            }
                            else if (CustomCommands.IsIn(CustomString) && !StartsWithBannedCommand)
                            {
                                string CustomMessage = CustomCommands.ReturnIt(CustomString);
                                ulong userID = CustomCommands.ReturnUser(CustomString);
                                Console.WriteLine($"Detected: {CustomString} and is sending {CustomMessage.ToUpper()}");
                                if (CustomMessage.ToUpper().StartsWith("PLAY "))
                                {
                                    PlaySong(CustomMessage, user, true);
                                }
                                else if (CustomMessage.ToUpper().StartsWith("R/"))
                                {
                                    RedditBrowse(message, CustomMessage);
                                }
                                else
                                {
                                    if (userID != 0)
                                    {
                                        try
                                        {
                                            var yes = Client.GetUser(userID).GetAvatarUrl();
                                            System.Drawing.Image image = DownloadImageFromUrl(yes.Trim());
                                            image.Save($@"..\..\{userID}image.png");
                                            image.Dispose();

                                            var hook = (message.Channel as SocketTextChannel).CreateWebhookAsync("You Have Sexuals");
                                            Image im = new Image($@"..\..\{userID}image.png");
                                            bool found = false;
                                            foreach (var guild in Client.Guilds)
                                            {
                                                foreach (var channel in guild.Channels)
                                                {
                                                    if (channel == message.Channel)
                                                    {
                                                        try
                                                        {
                                                            await hook.Result.ModifyAsync(x =>
                                                            {
                                                                List<ulong> ids = new List<ulong>();
                                                                foreach (var item in guild.Users)
                                                                {
                                                                    ids.Add(item.Id);
                                                                }
                                                                if (ids.Contains(userID))
                                                                {
                                                                    x.Name = guild.GetUser(userID).Nickname;
                                                                }
                                                                else
                                                                {
                                                                    x.Name = Client.GetUser(userID).Username;
                                                                }
                                                                x.Image = im;
                                                                im.Dispose();
                                                            });
                                                        }
                                                        catch (Exception e3)
                                                        {
                                                            Console.WriteLine(e3.Message);
                                                            Console.WriteLine(e3.Source);
                                                            await hook.Result.ModifyAsync(x =>
                                                            {
                                                                x.Name = Client.GetUser(userID).Username;
                                                                x.Image = im;
                                                                im.Dispose();
                                                            });
                                                        }
                                                        DiscordWebhookClient d = new DiscordWebhookClient(hook.Result);
                                                        await d.SendMessageAsync(CustomMessage);
                                                        await d.DeleteWebhookAsync();
                                                        found = true;
                                                        break;
                                                    }
                                                }
                                                if (found)
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                        catch (Exception e3)
                                        {
                                            Console.WriteLine(e3.Message);
                                            await message.Channel.SendMessageAsync(CustomMessage);
                                        }
                                    }
                                    else
                                    {
                                        await message.Channel.SendMessageAsync(CustomMessage);
                                    }
                                }
                            }
                            else if (TheMessage.Equals("NODAD") || TheMessage.Equals("NO DAD"))
                            {
                                string t;
                                var hook = (message.Channel as SocketTextChannel).CreateWebhookAsync("Daddy");
                                Image im = new Image($@"..\..\dad.jpg");
                                await hook.Result.ModifyAsync(x =>
                                {
                                    x.Name = "Dad Bot";
                                    x.Image = im;
                                });
                                DiscordWebhookClient d = new DiscordWebhookClient(hook.Result);
                                if (!nodads.Contains(message.Author.Id))
                                {
                                    nodads.Add(message.Author.Id);
                                    t = "*slowly sets down shotgun*";
                                    SaveDaddy();
                                }
                                else
                                {
                                    t = "*the shotgun is already down*";
                                }
                                await d.SendMessageAsync(t);
                                await d.DeleteWebhookAsync();
                            }
                            else if (TheMessage.Equals("YESDAD") || TheMessage.Equals("YES DAD"))
                            {
                                string t;
                                var hook = (message.Channel as SocketTextChannel).CreateWebhookAsync("Daddy");
                                Image im = new Image($@"..\..\dad.jpg");
                                await hook.Result.ModifyAsync(x =>
                                {
                                    x.Name = "Dad Bot";
                                    x.Image = im;
                                });
                                DiscordWebhookClient d = new DiscordWebhookClient(hook.Result);
                                if (nodads.Contains(message.Author.Id))
                                {
                                    nodads.Remove(message.Author.Id);
                                    t = "*slowly picks up shotgun*";
                                    SaveDaddy();
                                }
                                else
                                {
                                    t = "*the shotgun is already prepared*";
                                }
                                await d.SendMessageAsync(t);
                                await d.DeleteWebhookAsync();
                            }
                            else if (TheMessage.StartsWith("GALAXYBLACKLIST") || TheMessage.StartsWith("GALAXY BLACKLIST"))
                            {
                                strokeoutdab = true;
                                StringBuilder sb = new StringBuilder();
                                if (message.MentionedUsers.Count != 0)
                                {
                                    foreach (var item in message.MentionedUsers)
                                    {
                                        if (item.IsBot)
                                        {
                                            if (galaxywords.optOut(item.Id))
                                            {
                                                sb.Append($"<@{item.Id}> has been added to the blacklist.\n");
                                            }
                                            else
                                            {
                                                sb.Append($"<@{item.Id}> has been removed from the blacklist.\n");
                                            }
                                        }
                                        else
                                        {
                                            sb.Append($"<@{item.Id}> has been ignored since you can't force someone to be blacklisted.\n");
                                        }
                                    }
                                    sb.Append("\nType GalaxyBanList in a debug channel to see all blacklisted users.");
                                }
                                else
                                {
                                    TheMessage = TheMessage.Remove(0, "GALAXYBLACKLIST".Length);
                                    RemoveFilledSpace(ref TheMessage);
                                    RemoveWhiteSpace(ref TheMessage);
                                    galaxywords.BanWords(TheMessage);
                                    sb.Append("Words banned/unbanned! Type GalaxyBanList in a debug channel to see all banned words.");
                                }
                                await message.Channel.SendMessageAsync(sb.ToString());
                            }
                            else if (TheMessage.StartsWith("GALAXYBANLIST") || TheMessage.StartsWith("GALAXY BAN LIST"))
                            {
                                strokeoutdab = true;
                                if (!galaxywords.GalaxyBanList(message, Client))
                                {
                                    await message.Channel.SendMessageAsync($"Channel not setup for debugging, pinging <@{MyID}> to fix this.");
                                }
                            }
                            else if (TheMessage.StartsWith("GALAXY OPT OUT") || TheMessage.StartsWith("GALAXYOPTOUT"))
                            {
                                strokeoutdab = true;
                                if (galaxywords.optOut(message.Author.Id))
                                {
                                    await message.Channel.SendMessageAsync("You have opted out of the Galaxy Stroke command. Your messages will no longer be recorded for the stroke database.\nIf this was a mistake, simply type 'Galaxy Opt Out' again.");
                                }
                                else
                                {
                                    await message.Channel.SendMessageAsync("You have opted back into the Galaxy Stroke command. Welcome back to the cool kids club.");
                                }
                            }
                            else if (TheMessage.StartsWith("GALAXY STROKE") || TheMessage.StartsWith("GALAXYSTROKE"))
                            {
                                TheMessage = TheMessage.Remove(0, "GALAXYSTROKE".Length);
                                RemoveFilledSpace(ref TheMessage);
                                RemoveWhiteSpace(ref TheMessage);
                                try
                                {
                                    int num = Int32.Parse(TheMessage);
                                    await message.Channel.SendMessageAsync(galaxywords.RetrieveString(num));
                                }
                                catch (Exception)
                                {
                                    await message.Channel.SendMessageAsync(galaxywords.RetrieveString());
                                }
                                strokeoutdab = true;
                            }
                            else if (TheMessage.StartsWith("SAVE THE GALAXY"))
                            {
                                strokeoutdab = true;
                                SaveTheGalaxy();
                                await message.Channel.SendMessageAsync("Ok, done!");
                            }
                            else if (TheMessage.StartsWith("BINDGALAXYDEBUG") || TheMessage.StartsWith("BIND GALAXY DEBUG"))
                            {
                                strokeoutdab = true;
                                if (message.Author.Id == MyID)
                                {
                                    if (galaxywords.GalaxyBind(message.Channel.Id))
                                    {
                                        await message.Channel.SendMessageAsync("Channel bound for the [GALAXY DEBUG] command!");
                                    }
                                    else
                                    {
                                        await message.Channel.SendMessageAsync("Channel unbound for the [GALAXY DEBUG] command!");
                                    }
                                }
                                else
                                {
                                    await message.Channel.SendMessageAsync("No");
                                }
                            }
                            else if (TheMessage.StartsWith("GALAXYDEBUG") || TheMessage.StartsWith("GALAXY DEBUG"))
                            {
                                strokeoutdab = true;
                                if (!galaxywords.GalaxyDebug(message))
                                {
                                    await message.Channel.SendMessageAsync($"Channel not setup for debugging, pinging <@{MyID}> to fix this.");
                                }
                            }
                            else if (TheMessage.StartsWith("REMOVEALIAS") || TheMessage.StartsWith("REMOVE ALIAS") || TheMessage.StartsWith("DELETEALIAS") || TheMessage.StartsWith("DELETE ALIAS"))
                            {
                                TheMessage = TheMessage.Remove(0, "REMOVEALIA".Length);
                                RemoveFilledSpace(ref TheMessage);
                                RemoveWhiteSpace(ref TheMessage);
                                bool yes = false;
                                for (int i = 0; i < manager.aliases.Count; i++)
                                {
                                    if (manager.aliases[i].input.ToUpper() == TheMessage)
                                    {
                                        manager.aliases.RemoveAt(i);
                                        await message.Channel.SendMessageAsync("Alias removed!");
                                        yes = true;
                                        break;
                                    }
                                }
                                if (!yes)
                                {
                                    await message.Channel.SendMessageAsync("Alias not found.");
                                }
                                SaveAlias();
                            }
                            else if (TheMessage.StartsWith("CREATEALIAS") || TheMessage.StartsWith("CREATE ALIAS") || TheMessage.StartsWith("ADDALIAS") || TheMessage.StartsWith("ADD ALIAS"))
                            {
                                TheMessageNormal = TheMessageNormal.Remove(0, "CREATEA".Length);
                                RemoveFilledSpace(ref TheMessageNormal);
                                RemoveWhiteSpace(ref TheMessageNormal);
                                Alias a = new Alias();
                                string[] s = TheMessageNormal.Split('>');
                                while (s[0][s[0].Length - 1] == ' ')
                                {
                                    s[0] = s[0].Remove(s[0].Length - 1, 1);
                                }
                                a.input = s[0];
                                a.output = s[1].Split(' ').ToList();
                                bool yes = false;
                                foreach (var item in manager.aliases)
                                {
                                    if (item.input.ToUpper() == a.input.ToUpper())
                                    {
                                        yes = true;
                                        break;
                                    }
                                }
                                if (yes)
                                {
                                    await message.Channel.SendMessageAsync("Alias already exists.");
                                }
                                else
                                {
                                    await message.Channel.SendMessageAsync("Alias added! Would you like to add the plural version of this word? Y/N");
                                    manager.aliases.Add(a);
                                    SaveAlias();
                                    canudont = true;
                                    tempalias = a.input;
                                    tempout = a.output;
                                }
                            }
                            else if (TheMessage.Equals("SHOWALIASES") || TheMessage.Equals("SHOW ALIASES"))
                            {
                                StringBuilder sb = new StringBuilder();
                                sb.Append("```\n");
                                foreach (var item in manager.aliases)
                                {
                                    if (sb.Length > 1800)
                                    {
                                        sb.Append("```");
                                        await message.Channel.SendMessageAsync(sb.ToString());
                                        sb.Clear();
                                        sb.Append("```");
                                    }
                                    sb.Append($"[ {item.input} ] expands to [");
                                    foreach (var item2 in item.output)
                                    {
                                        sb.Append($"{item2} ");
                                    }
                                    sb.Append("]\n");
                                }
                                sb.Append("```");
                                await message.Channel.SendMessageAsync(sb.ToString());
                            }
                            else if (TheMessage.Equals("ALIASHELP") || TheMessage.Equals("ALIAS HELP"))
                            {
                                string m = "When creating a reminder, you can call aliases which have been created by users. This is intended to make assigning a time to your reminder a much smoother experience. \n\nWhen creating an alias with ``Create Alias``, keep in mind the syntax is as follows: ``Create Alias input > output`` where input is a word with no spaces, and output can be anything really, bearing in mind neither of these can contain ``>`` \n\nHere is a list of suggestions/ guidelines for the output \n\n```^ == delimiter between how much time and the reminder message \nyears / months / weeks / days / hours / minutes / seconds == self explanatory I hope \n\nExamples: \ncreatealias sexual > 1 minute 2 seconds \ncreatealias kevin > 2 hours \ncreatealias browning > 69 seconds \ncreatealias BIG > 10 years 69 months 2 weeks 1 minute 7 seconds \ncreatealias like > n/a <----(n/a gets interperated as nothing at runtime)```";
                                await message.Channel.SendMessageAsync(m);
                            }
                            else if (TheMessage.Equals("Y") && tempalias != "")
                            {
                                if (tempalias.ToUpper().EndsWith("S"))
                                {
                                    tempalias += "es";
                                }
                                else if (tempalias.ToUpper().EndsWith("Y"))
                                {
                                    tempalias = tempalias.Remove(tempalias.Length - 1, 1);
                                    tempalias += "ies";
                                }
                                else
                                {
                                    tempalias += "s";
                                }
                                Alias a = new Alias();
                                a.input = tempalias;
                                a.output = tempout;
                                bool yes = false;
                                foreach (var item in manager.aliases)
                                {
                                    if (item.input.ToUpper() == a.input.ToUpper())
                                    {
                                        yes = true;
                                        break;
                                    }
                                }
                                if (yes)
                                {
                                    await message.Channel.SendMessageAsync("Plural alias already exists.");
                                }
                                else
                                {
                                    await message.Channel.SendMessageAsync("Plural alias added!");
                                    manager.aliases.Add(a);
                                    SaveAlias();
                                }
                            }
                            else if (TheMessage.StartsWith("REMINDERS HELP") || TheMessage.StartsWith("REMINDERSHELP") || TheMessage.StartsWith("REMINDER HELP") || TheMessage.StartsWith("REMINDERHELP"))
                            {
                                await message.Channel.SendMessageAsync($"```Create a Reminder!\nCreate a reminder for yourself, this reminder will be bound the the channel you create it it.\nExample: RemindMe 2 hours 1 minute 30 seconds ^ Get Laundry\n" +
                                    $"You can also create aliases for words to be interperated as for the timing portion of this command, this can be done by using CreateAlias. Type AliasHelp for more info.\n" +
                                    $"To see all reminders in this channel, use Reminders```");
                            }
                            else if ((TheMessage.Contains("REMINDME") || TheMessage.Contains("!REMINDME") || TheMessage.Contains("CREATE REMINDER") || TheMessage.Contains("CREATEREMINDER") || TheMessage.Contains("REMIND ME")))
                            {
                                while (!(TheMessage.StartsWith("REMINDME") || TheMessage.StartsWith("!REMINDME") || TheMessage.StartsWith("CREATE REMINDER") || TheMessage.StartsWith("CREATEREMINDER") || TheMessage.StartsWith("REMIND ME")))
                                {
                                    TheMessage = TheMessage.Remove(0, 1);
                                    TheMessageNormal = TheMessageNormal.Remove(0, 1);
                                }
                                int y = 0, mo = 0, d = 0, h = 0, mi = 0, s = 0;
                                bool daily = false, weekly = false, monthly = false, yearly = false;
                                string yeetem = TheMessageNormal.Remove(0, "REMINDME".Length);
                                RemoveFilledSpace(ref yeetem);
                                RemoveWhiteSpace(ref yeetem);
                                string notyeetem = manager.ParseMessage(yeetem);
                                int delimnumber1 = 0, delimnumber2 = 0;
                                int time = 0;
                                int delimeter = 0;
                                if (notyeetem.Contains('^'))
                                {
                                    for (int i = 0; i < notyeetem.Length; i++)
                                    {
                                        if (notyeetem[i] == '^')
                                        {
                                            if (i < notyeetem.Length - 1 && i > 0)
                                            {
                                                int num = i + 1;
                                                while (notyeetem[num] == ' ')
                                                {
                                                    num++;
                                                }
                                                StringBuilder tempSB = new StringBuilder();
                                                while (notyeetem[num] != ' ')
                                                {
                                                    tempSB.Append(notyeetem[num]);
                                                    num++;
                                                }
                                                int tempnum;
                                                if (Int32.TryParse(tempSB.ToString(), out tempnum))
                                                {
                                                    time = 1;
                                                    delimeter = i;
                                                }
                                                delimnumber1++;
                                            }
                                        }
                                    }
                                    for (int i = 0; i < notyeetem.Length; i++)
                                    {
                                        if (notyeetem[i] == '^')
                                        {
                                            try
                                            {
                                                if (i > 2)
                                                {
                                                    int num = i - 1;
                                                    while (notyeetem[num] == ' ')
                                                    {
                                                        if (num == -1)
                                                        {
                                                            break;
                                                        }
                                                        num--;
                                                        if (num == -1)
                                                        {
                                                            break;
                                                        }
                                                    }
                                                    while (notyeetem[num] != ' ')
                                                    {
                                                        if (num == -1)
                                                        {
                                                            break;
                                                        }
                                                        num--;
                                                        if (num == -1)
                                                        {
                                                            break;
                                                        }
                                                    }
                                                    while (notyeetem[num] == ' ')
                                                    {
                                                        if (num == -1)
                                                        {
                                                            break;
                                                        }
                                                        num--;
                                                        if (num == -1)
                                                        {
                                                            break;
                                                        }
                                                    }
                                                    StringBuilder tempSB = new StringBuilder();
                                                    while (notyeetem[num] != ' ')
                                                    {
                                                        tempSB.Insert(0, notyeetem[num]);
                                                        num--;
                                                        if (num == -1)
                                                        {
                                                            break;
                                                        }
                                                    }
                                                    int tempnum;
                                                    if (Int32.TryParse(tempSB.ToString(), out tempnum))
                                                    {
                                                        time = 0;
                                                        delimeter = i;
                                                    }
                                                    delimnumber2++;
                                                }
                                            }
                                            catch (Exception)
                                            {
                                            }
                                        }
                                    }
                                }
                                StringBuilder sb = new StringBuilder();
                                StringBuilder sb2 = new StringBuilder();
                                for (int i = 0; i < delimeter; i++)
                                {
                                    sb.Append(notyeetem[i]);
                                }
                                for (int i = delimeter + 1; i < notyeetem.Length; i++)
                                {
                                    sb2.Append(notyeetem[i]);
                                }
                                List<string> badwords = new List<string>();
                                string remindermessage;

                                if (time == 0)
                                {
                                    remindermessage = manager.ParseReminder(yeetem, time, delimnumber2);
                                    badwords = NumberStuffBetter(sb.ToString().Split(' '), ref mo, ref y, ref mi, ref d, ref h, ref s, ref daily, ref weekly, ref monthly, ref yearly);
                                }
                                else
                                {
                                    remindermessage = manager.ParseReminder(yeetem, time, delimnumber1);
                                    badwords = NumberStuffBetter(sb2.ToString().Split(' '), ref mo, ref y, ref mi, ref d, ref h, ref s, ref daily, ref weekly, ref monthly, ref yearly);
                                }

                                if (y + mo + d + h + mi + s != 0)
                                {
                                    CreateReminder(y, mo, d, h, mi, s, message.Author.Id, message.Channel, remindermessage, daily, weekly, monthly, yearly);
                                    await message.Channel.SendMessageAsync("Reminder created!");
                                }
                                else if (badwords.Count != 0)
                                {
                                    sb.Clear();
                                    sb.Append("There were some issues with setting the time. Here are the words stood out as problems to me; consider assigning them to an alias with ``CreateAlias word > alias`` AliasHelp for more info\n```");
                                    foreach (var item in badwords)
                                    {
                                        sb.Append(item + " ");
                                    }
                                    sb.Append("```");
                                    await message.Channel.SendMessageAsync(sb.ToString());
                                }
                                else
                                {
                                    #region gay
                                    //////////////////////////////////////////////////////////////////
                                    //OLD AND BAD (but i might need later so it stays)
                                    //////////////////////////////////////////////////////////////////
                                    Console.WriteLine("Uh oh, something bad happened, using the old remindme code as a last ditch effort");
                                    int tempnum = 0;
                                    StringBuilder stronks = new StringBuilder();
                                    int toremove = 0;
                                    for (int i = 0; yeetem[i] != ' '; i++)
                                    {
                                        stronks.Append(yeetem[i]);
                                        toremove++;
                                    }
                                    if (stronks.ToString().ToUpper() == "ABOUT" || stronks.ToString().ToUpper() == "THAT" || stronks.ToString().ToUpper() == "TO")
                                    {
                                        yeetem.Remove(0, stronks.Length);
                                        stronks.Clear();
                                        while (!(yeetem.ToUpper()[0] == ' ' && yeetem.ToUpper()[1] == 'I' && yeetem.ToUpper()[2] == 'N' && yeetem.ToUpper()[0] == ' ') && yeetem[0] != '^' && !(yeetem.ToUpper()[0] == ' ' && yeetem.ToUpper()[1] == 'A' && yeetem.ToUpper()[2] == 'F' && yeetem.ToUpper()[3] == 'T' && yeetem.ToUpper()[4] == 'E' && yeetem.ToUpper()[5] == 'R' && yeetem.ToUpper()[6] == ' ') && !(yeetem[0] == ' ' && !yeetem.Remove(0, 1).Contains(' ')) && !(yeetem.ToUpper()[0] == ' ' && yeetem.ToUpper()[1] == 'E' && yeetem.ToUpper()[2] == 'V' && yeetem.ToUpper()[3] == 'E' && yeetem.ToUpper()[4] == 'R' && yeetem.ToUpper()[5] == 'Y' && yeetem.ToUpper()[6] == ' ') && !(yeetem[0] == ' ' && !yeetem.Remove(0, 1).Contains(' ')))
                                        {
                                            stronks.Append(yeetem[0]);
                                            yeetem = yeetem.Remove(0, 1);
                                        }
                                        string them = stronks.ToString().Remove(0, toremove);
                                        if (yeetem.ToUpper() == " NOW" || yeetem.ToUpper() == " YESTERDAY" || yeetem.ToUpper() == " IMMEDIATELY" || yeetem.ToUpper() == " INSTANTLY")
                                        {

                                        }
                                        else if (yeetem.ToUpper() == " TOMORROW" || yeetem.ToUpper() == " TOMMORROW" || yeetem.ToUpper() == " TOMMOROW")
                                        {
                                            d = 1;
                                        }
                                        else if (yeetem.ToUpper() == " NEXT WEEK")
                                        {
                                            d = 7;
                                        }
                                        else
                                        {
                                            bool yeet = true;
                                            RemoveWhiteSpace(ref yeetem);
                                            if (yeetem.Remove(0, 1).Contains(' ') && !yeetem.ToUpper().StartsWith("EVERY"))
                                            {
                                                yeet = false;
                                                RemoveFilledSpace(ref yeetem);
                                                RemoveWhiteSpace(ref yeetem);
                                            }

                                            //amount stuff
                                            NumberStuff(ref tempnum, ref stronks, ref yeetem, ref mo, ref y, ref mi, ref d, ref h, ref s, yeet, ref daily, ref weekly, ref monthly, ref yearly);
                                        }

                                        CreateReminder(y, mo, d, h, mi, s, message.Author.Id, message.Channel, them, daily, weekly, monthly, yearly);
                                        message.Channel.SendMessageAsync("Reminder created!");
                                    }
                                    else
                                    {
                                        if (!NumberStuff(ref tempnum, ref stronks, ref yeetem, ref mo, ref y, ref mi, ref d, ref h, ref s, false, ref daily, ref weekly, ref monthly, ref yearly))
                                        {
                                            RemoveFilledSpace(ref yeetem);
                                            RemoveWhiteSpace(ref yeetem);
                                        }

                                        CreateReminder(y, mo, d, h, mi, s, message.Author.Id, message.Channel, yeetem, daily, weekly, monthly, yearly);
                                        message.Channel.SendMessageAsync("Reminder created!");
                                    }
                                    #endregion
                                }
                            }
                            else if (TheMessage.StartsWith("REMINDERS") || TheMessage.StartsWith("SHOWREMINDERS") || TheMessage.StartsWith("SHOW REMINDERS") || TheMessage.StartsWith("CHECK REMINDERS") || TheMessage.StartsWith("CHECKREMINDERS"))
                            {
                                StringBuilder s = new StringBuilder();
                                StringBuilder s2 = new StringBuilder();
                                s.Append("```");
                                for (int i = 0; i < reminders.Count; i++)
                                {
                                    if (reminders[i].channel != message.Channel.Id)
                                    {
                                        continue;
                                    }
                                    bool ye3 = false;
                                    s2.Append($"({Client.GetUser(reminders[i].ID).Username})");
                                    if (reminders[i].daily)
                                    {
                                        s2.Append("(daily)");
                                    }
                                    if (reminders[i].weekly)
                                    {
                                        s2.Append("(weekly)");
                                    }
                                    if (reminders[i].monthly)
                                    {
                                        s2.Append("(monthly)");
                                    }
                                    if (reminders[i].yearly)
                                    {
                                        s2.Append("(yearly)");
                                    }
                                    if (reminders[i].years != 0)
                                    {
                                        s2.Append($"{reminders[i].years} years, ");
                                        ye3 = true;
                                    }
                                    if (reminders[i].months != 0)
                                    {
                                        s2.Append($"{reminders[i].months} months, ");
                                        ye3 = true;
                                    }
                                    if (reminders[i].days != 0)
                                    {
                                        s2.Append($"{reminders[i].days} days, ");
                                        ye3 = true;
                                    }
                                    if (reminders[i].hours != 0)
                                    {
                                        s2.Append($"{reminders[i].hours} hours, ");
                                        ye3 = true;
                                    }
                                    if (reminders[i].minutes != 0)
                                    {
                                        s2.Append($"{reminders[i].minutes} minutes, ");
                                        ye3 = true;
                                    }

                                    string ye, ye2;
                                    if (reminders[i].message.Length > 15)
                                    {
                                        ye = $"{reminders[i].message.Remove(15, reminders[i].message.Length - 15)}...";
                                    }
                                    else
                                    {
                                        ye = reminders[i].message;
                                    }
                                    if (ye3)
                                    {
                                        ye2 = "and ";
                                    }
                                    else
                                    {
                                        ye2 = "";
                                    }
                                    s2.Append($"{ye2}{reminders[i].seconds} seconds left on reminder: {ye}\n");

                                    if (s.Length + 3 + s2.Length > 2000)
                                    {
                                        s.Append("```");
                                        message.Channel.SendMessageAsync(s.ToString());
                                        s.Clear();
                                        s.Append("```");
                                    }
                                    s.Append(s2.ToString());
                                    s2.Clear();
                                }
                                s.Append("```");
                                if (s.ToString() == "``````")
                                    message.Channel.SendMessageAsync("No reminders in this channel!");
                                else
                                    message.Channel.SendMessageAsync(s.ToString());
                            }
                            else if (TheMessage.StartsWith("DELETE REMINDER") || TheMessage.StartsWith("ERASE REMINDER"))
                            {
                                TheMessage = TheMessage.Remove(0, 9);
                                RemoveFilledSpace(ref TheMessage);
                                RemoveWhiteSpace(ref TheMessage);
                                for (int i = 0; i < reminders.Count; i++)
                                {
                                    if (reminders[i].ID == message.Author.Id && reminders[i].channel == message.Channel.Id && reminders[i].message.ToUpper().Contains(TheMessage))
                                    {
                                        reminders.RemoveAt(i);
                                        await message.Channel.SendMessageAsync("Reminder deleted!");
                                        break;
                                    }
                                }
                                SaveReminders();
                            }
                            else if (TheMessage.StartsWith("CLEAR ALL REMINDERS") || TheMessage.StartsWith("DELETE ALL REMINDERS") || TheMessage.StartsWith("ERASE ALL REMINDERS"))
                            {
                                for (int i = 0; i < reminders.Count; i++)
                                {
                                    if (reminders[i].ID == message.Author.Id && reminders[i].channel == message.Channel.Id)
                                    {
                                        reminders.RemoveAt(i);
                                        i--;
                                    }
                                }
                                SaveReminders();
                                await message.Channel.SendMessageAsync("Reminders deleted!");
                            }
                            else if (TheMessage.StartsWith("SUPERSTROKE") || TheMessage.StartsWith("SUPER STROKE") || TheMessage.StartsWith("STROKE ME DADDY"))
                            {
                                List<string> templist = new List<string>();
                                StringBuilder sb = new StringBuilder();
                                foreach (var item in Last3Message)
                                {
                                    string[] thing = item.Split(' ');
                                    foreach (var thing2 in thing)
                                    {
                                        templist.Add(thing2);
                                    }
                                }
                                Random rand = new Random();
                                while (templist.Count != 0)
                                {
                                    int t = rand.Next() % templist.Count;
                                    sb.Append($"{templist[t]} ");
                                    templist.RemoveAt(t);
                                }
                                while (sb.ToString().Length > 2000)
                                {
                                    sb.Remove(sb.ToString().Length - 1, 1);
                                }
                                strokemessage = (await message.Channel.SendMessageAsync(sb.ToString())).Id;
                                //if (!File.Exists($@"..\..\{user.Id}image.png"))
                                //{
                                //    var yes = Client.GetUser(user.Id).GetAvatarUrl();
                                //    System.Drawing.Image image = DownloadImageFromUrl(yes.Trim());
                                //    image.Save($@"..\..\{user.Id}image.png");
                                //}

                                //var hook = (message.Channel as SocketTextChannel).CreateWebhookAsync("Fuck u");
                                //Image im = new Image($@"..\..\{user.Id}image.png");
                                //await hook.Result.ModifyAsync(x =>
                                //{
                                //    x.Name = (message.Author as IGuildUser).Nickname;
                                //    x.Image = im;
                                //});
                                //DiscordWebhookClient d = new DiscordWebhookClient(hook.Result);
                                //await d.SendMessageAsync(sb.ToString());
                                //await d.DeleteWebhookAsync();
                                //await message.DeleteAsync();
                                strokeoutdab = true;
                            }
                            else if (TheMessage.StartsWith("IS RUNE A FURRY"))
                            {
                                Random rand = new Random();
                                ulong name = 0;
                                if (rand.Next() % 2 == 0)
                                {
                                    name = Rune;
                                }
                                else
                                {
                                    name = Platinum;
                                }
                                if (!File.Exists($@"..\..\{name}image.png"))
                                {
                                    var yes = Client.GetUser(name).GetAvatarUrl();
                                    System.Drawing.Image image = DownloadImageFromUrl(yes.Trim());
                                    image.Save($@"..\..\{name}image.png");
                                }

                                var hook = (message.Channel as SocketTextChannel).CreateWebhookAsync("Its true");
                                Image im = new Image($@"..\..\{name}image.png");
                                bool found = false;
                                foreach (var guild in Client.Guilds)
                                {
                                    foreach (var channel in guild.Channels)
                                    {
                                        if (channel == message.Channel)
                                        {
                                            await hook.Result.ModifyAsync(x =>
                                            {
                                                if (name == Rune)
                                                {
                                                    x.Name = (guild.GetUser(name) as IGuildUser).Nickname;
                                                }
                                                else
                                                {
                                                    x.Name = "PMM Act 2";
                                                }
                                                x.Image = im;
                                            });
                                            DiscordWebhookClient d = new DiscordWebhookClient(hook.Result);
                                            if (name == Rune)
                                            {
                                                int yeet = rand.Next() % 3;
                                                switch (yeet)
                                                {
                                                    case 0:
                                                        await d.SendMessageAsync("I am a furry yes");
                                                        break;
                                                    case 1:
                                                        await d.SendMessageAsync("Probably");
                                                        break;
                                                    case 2:
                                                        await d.SendMessageAsync("Absolutely");
                                                        break;
                                                    default:
                                                        break;
                                                }
                                            }
                                            else
                                            {
                                                int yeet = rand.Next() % 3;
                                                switch (yeet)
                                                {
                                                    case 0:
                                                        await d.SendMessageAsync("Actually I think he is");
                                                        break;
                                                    case 1:
                                                        await d.SendMessageAsync("I'm not allowed to say no so I won't say anything");
                                                        break;
                                                    case 2:
                                                        await d.SendMessageAsync("I have seen his yiff stash");
                                                        break;
                                                    default:
                                                        break;
                                                }
                                            }
                                            await d.DeleteWebhookAsync();
                                            found = true;
                                            break;
                                        }
                                    }
                                    if (found)
                                    {
                                        break;
                                    }
                                }

                            }
                            else if ((TheMessage.Contains("WAKEUP") || TheMessage.Contains("WAKE UP")) && message.MentionedUsers.Count == 1 && !working)
                            {
                                working = true;
                                foreach (var guild in Client.Guilds)
                                {
                                    foreach (var channel in guild.VoiceChannels)
                                    {
                                        for (int i = 0; i < channel.Users.Count; i++)
                                        {
                                            if (channel.Users.ElementAt(i).Id == message.MentionedUsers.ElementAt(0).Id && (guild.GetUser(channel.Users.ElementAt(i).Id).IsSelfMuted || guild.GetUser(channel.Users.ElementAt(i).Id).IsSelfDeafened))
                                            {
                                                var yeet = await (channel.Users.ElementAt(i) as IGuildUser).Guild.CreateVoiceChannelAsync("Wake");
                                                var yeet2 = await (channel.Users.ElementAt(i) as IGuildUser).Guild.CreateVoiceChannelAsync("Up");
                                                var start = (channel.Users.ElementAt(i) as IGuildUser).VoiceChannel;

                                                int thing = 0;
                                                while (true)
                                                {
                                                    //if ((guild.GetUser(channel.Users.ElementAt(i).Id).IsSelfMuted || guild.GetUser(channel.Users.ElementAt(i).Id).IsSelfDeafened) == false)
                                                    //{
                                                    //    break;
                                                    //}
                                                    if (thing == 0)
                                                    {
                                                        await (channel.Users.ElementAt(i) as IGuildUser).ModifyAsync(x =>
                                                        {
                                                            x.ChannelId = (yeet as IVoiceChannel).Id;
                                                        });
                                                        thing = 1;
                                                    }
                                                    else if (thing == 1)
                                                    {
                                                        await (channel.Users.ElementAt(i) as IGuildUser).ModifyAsync(x =>
                                                        {
                                                            x.ChannelId = (yeet2 as IVoiceChannel).Id;
                                                        });
                                                        thing = 0;
                                                    }
                                                    else if (thing == -1)
                                                    {
                                                        break;
                                                    }
                                                    foreach (var item in (yeet as SocketVoiceChannel).Users)
                                                    {
                                                        if (item.Id == message.MentionedUsers.ElementAt(0).Id && (item.IsSelfMuted || item.IsSelfDeafened) == false)
                                                        {
                                                            thing = -1;
                                                            break;
                                                        }
                                                    }
                                                }
                                                if ((channel.Users.ElementAt(i) as IGuildUser).VoiceChannel != start)
                                                {
                                                    await (channel.Users.ElementAt(i) as IGuildUser).ModifyAsync(x =>
                                                    {
                                                        x.ChannelId = start.Id;
                                                    });
                                                }
                                                await (yeet as IVoiceChannel).DeleteAsync();
                                                await (yeet2 as IVoiceChannel).DeleteAsync();
                                                working = false;
                                                break;
                                            }
                                        }
                                        if (!working)
                                        {
                                            break;
                                        }
                                    }
                                    if (!working)
                                    {
                                        break;
                                    }
                                }
                                working = false;
                            }
                            else if (TheMessage.StartsWith("TRANSFERCOMMANDS"))
                            {
                                //CustomCommands.Transfer(message.Channel);
                            }
                            else if (TheMessage.StartsWith("WHEN YOU'VE GOT") || TheMessage.StartsWith("WHEN YOUVE GOT") || TheMessage.StartsWith("WHEN I'VE GOT") || TheMessage.StartsWith("WHEN IVE GOT"))
                            {
                                try
                                {
                                    while (TheMessageNormal.ToUpper()[0] != 'T')
                                    {
                                        TheMessageNormal = TheMessageNormal.Remove(0, 1);
                                    }
                                    TheMessageNormal = TheMessageNormal.Remove(0, 1);
                                    while (TheMessageNormal[0] == ' ')
                                    {
                                        TheMessageNormal = TheMessageNormal.Remove(0, 1);
                                    }
                                }
                                catch
                                {
                                }
                                await message.Channel.SendMessageAsync($"Yeah I've got {TheMessageNormal}");
                            }
                            else if (TheMessage.StartsWith("WHEN ITS") || TheMessage.StartsWith("WHEN IT'S"))
                            {
                                try
                                {
                                    while (TheMessageNormal.ToUpper()[0] != 'S')
                                    {
                                        TheMessageNormal = TheMessageNormal.Remove(0, 1);
                                    }
                                    TheMessageNormal = TheMessageNormal.Remove(0, 1);
                                    while (TheMessageNormal[0] == ' ')
                                    {
                                        TheMessageNormal = TheMessageNormal.Remove(0, 1);
                                    }
                                }
                                catch
                                {
                                }
                                await message.Channel.SendMessageAsync($"Yeah it's {TheMessageNormal}");
                            }
                            else if (TheMessage.StartsWith("WHEN I ") || TheMessage.StartsWith("WHEN I GOT") || TheMessage.StartsWith("WHEN IM") || TheMessage.StartsWith("WHEN I'M") || TheMessage.StartsWith("WHEN YOU GOT") || TheMessage.StartsWith("WHEN YOU") || TheMessage.StartsWith("WHEN YOURE") || TheMessage.StartsWith("WHEN YOU'RE"))
                            {
                                if ((TheMessage.StartsWith("WHEN I") && TheMessage.Length == "WHEN I".Length) || (TheMessage.StartsWith("WHEN YOU") && TheMessage.Length == "WHEN YOU".Length))
                                {
                                    await message.Channel.SendMessageAsync("Yeah I");
                                }
                                else if ((TheMessage.StartsWith("WHEN I GOT") && TheMessage.Length == "WHEN I GOT".Length) || (TheMessage.StartsWith("WHEN YOU GOT") && TheMessage.Length == "WHEN YOU GOT".Length))
                                {
                                    await message.Channel.SendMessageAsync("Yeah I got");
                                }
                                else if ((TheMessage.StartsWith("WHEN YOU'RE") && TheMessage.Length == "WHEN YOU'RE".Length) || (TheMessage.StartsWith("WHEN YOURE") && TheMessage.Length == "WHEN YOURE".Length) || (TheMessage.StartsWith("WHEN I'M") && TheMessage.Length == "WHEN I'M".Length) || (TheMessage.StartsWith("WHEN IM") && TheMessage.Length == "WHEN IM".Length))
                                {
                                    await message.Channel.SendMessageAsync("Yeah I'm");
                                }
                                else if (TheMessage.StartsWith("WHEN IM") || TheMessage.StartsWith("WHEN I'M") || TheMessage.StartsWith("WHEN YOURE") || TheMessage.StartsWith("WHEN YOU'RE"))
                                {
                                    TheMessageNormal = TheMessageNormal.Remove(0, 5);
                                    while (TheMessageNormal[0] != ' ')
                                    {
                                        TheMessageNormal = TheMessageNormal.Remove(0, 1);
                                    }
                                    TheMessageNormal = TheMessageNormal.Remove(0, 1);
                                    await message.Channel.SendMessageAsync($"Yeah I'm {TheMessageNormal}");
                                }
                                else if (TheMessage.StartsWith("WHEN I GOT") || TheMessage.StartsWith("WHEN YOU GOT"))
                                {
                                    TheMessageNormal = TheMessageNormal.Remove(0, 7);
                                    while (TheMessageNormal[0] != ' ')
                                    {
                                        TheMessageNormal = TheMessageNormal.Remove(0, 1);
                                    }
                                    TheMessageNormal = TheMessageNormal.Remove(0, 1);
                                    await message.Channel.SendMessageAsync($"Yeah I got {TheMessageNormal}");
                                }
                                else if (TheMessage.StartsWith("WHEN I") || TheMessage.StartsWith("WHEN YOU"))
                                {
                                    TheMessageNormal = TheMessageNormal.Remove(0, 5);
                                    while (TheMessageNormal[0] != ' ')
                                    {
                                        TheMessageNormal = TheMessageNormal.Remove(0, 1);
                                    }
                                    TheMessageNormal = TheMessageNormal.Remove(0, 1);
                                    await message.Channel.SendMessageAsync($"Yeah I {TheMessageNormal}");
                                }
                            }
                            else if (TheMessage.StartsWith("STORKE") || TheMessage.StartsWith("SRTOKE") && TheMessage.Count() == "STORKE".Length)
                            {
                                await message.Channel.SendMessageAsync("I think you had one");
                            }
                            else if (TheMessage.StartsWith("DOWNLOADCOMMAND") && TheSenderNoString.Id == MyID)
                            {
                                if (TheMessage.StartsWith("DOWNLOADCOMMAND"))
                                {
                                    TheMessage = TheMessage.Remove(0, "DOWNLOADCOMMAND".Count());
                                }
                                else
                                {
                                    TheMessage = TheMessage.Remove(0, "DOWNLOADCOMMAND".Count());
                                }
                                while (TheMessage[0] == ' ')
                                {
                                    TheMessage = TheMessage.Remove(0, 1);
                                }
                                while (TheMessage[TheMessage.Count() - 1] == ' ' || TheMessage[TheMessage.Count() - 1] == '^')
                                {
                                    TheMessage = TheMessage.Remove(TheMessage.Count() - 1, 1);
                                }
                                CustomCommands.DownloadCommand(TheMessage);
                            }
                            else if (TheMessage.StartsWith("CHECKCOMMAND") || TheMessage.StartsWith("GETCOMMAND") || TheMessage.StartsWith("SHOWCOMMAND"))
                            {
                                if (TheMessage.StartsWith("CHECKCOMMAND"))
                                {
                                    TheMessage = TheMessage.Remove(0, "CHECKCOMMAND".Count());
                                }
                                else if (TheMessage.StartsWith("GETCOMMAND"))
                                {
                                    TheMessage = TheMessage.Remove(0, "GETCOMMAND".Count());
                                }
                                else
                                {
                                    TheMessage = TheMessage.Remove(0, "SHOWCOMMAND".Count());
                                }
                                while (TheMessage[0] == ' ')
                                {
                                    TheMessage = TheMessage.Remove(0, 1);
                                }
                                while (TheMessage[TheMessage.Count() - 1] == ' ' || TheMessage[TheMessage.Count() - 1] == '^')
                                {
                                    TheMessage = TheMessage.Remove(TheMessage.Count() - 1, 1);
                                }
                                CustomCommands.GetCommand(TheMessage, message);
                            }
                            else if (TheMessage.StartsWith("REVERSESEARCH"))
                            {
                                TheMessage = TheMessage.Remove(0, "REVERSESEARCH".Count());
                                while (TheMessage[0] == ' ')
                                {
                                    TheMessage = TheMessage.Remove(0, 1);
                                }
                                while (TheMessage[TheMessage.Count() - 1] == ' ' || TheMessage[TheMessage.Count() - 1] == '^')
                                {
                                    TheMessage = TheMessage.Remove(TheMessage.Count() - 1, 1);
                                }
                                CustomCommands.ReverseSearch(TheMessage, message);
                            }
                            else if (TheMessage.StartsWith("TOGGLECOMMANDTYPE"))
                            {
                                TheMessageNormal = TheMessageNormal.Remove(0, 17);
                                while (TheMessageNormal[0] == ' ')
                                {
                                    TheMessageNormal = TheMessageNormal.Remove(0, 1);
                                }
                                while (TheMessageNormal[TheMessageNormal.Count() - 1] == ' ')
                                {
                                    TheMessageNormal = TheMessageNormal.Remove(TheMessageNormal.Count() - 1, 1);
                                }
                                if (CustomCommands.IsIn(TheMessageNormal))
                                {
                                    string _Value = CustomCommands.CheckExisting(TheMessageNormal, "Change");
                                    if (_Value == "")
                                    {
                                        var _TempItem = CustomCommands.ToggleType(TheMessageNormal);

                                        if (_TempItem.Contains)
                                        {
                                            await message.Channel.SendMessageAsync($"[{_TempItem.Trigger}] will now trigger whenever it is used in a message.");
                                        }
                                        else
                                        {
                                            await message.Channel.SendMessageAsync($"[{_TempItem.Trigger}] will now trigger only when sent on it's own.");
                                        }
                                        CustomCommands.SaveCommands();
                                    }
                                    else
                                    {
                                        await message.Channel.SendMessageAsync($"Changing that command would interfere with [{_Value}].");
                                    }
                                }
                                else
                                {
                                    await message.Channel.SendMessageAsync("The requested trigger word was not found!");
                                }
                            }
                            else if (TheMessage.StartsWith("LEAVE") && TheMessage.Count() == 5)
                            {
                                musicManager.Leave();
                            }
                            else if (TheMessage.StartsWith("SKIP") && TheMessage.Count() == 4)
                            {
                                musicManager.Skip();
                            }
                            else if (TheMessage.StartsWith("JOIN") && TheMessage.Count() == 4)
                            {
                                musicManager.ConnectToChannel(((user as IGuildUser).VoiceChannel as SocketVoiceChannel));
                            }
                            else if ((TheMessage.StartsWith("ASSIGNPERSON") || TheMessage.StartsWith("ASSIGN PERSON") || TheMessage.StartsWith("ASSIGNUSER") || TheMessage.StartsWith("ASSIGN USER")) && TheMessage.Contains('^'))
                            {
                                TheMessage = TheMessage.Remove(0, "ASSIGNUS".Length);
                                RemoveFilledSpace(ref TheMessage);
                                RemoveWhiteSpace(ref TheMessage);
                                string[] twosides = TheMessage.Split('^');
                                for (int i = 0; i < twosides.Length; i++)
                                {
                                    RemoveWhiteSpace(ref twosides[i]);
                                    while (twosides[i][twosides[i].Length - 1] == ' ')
                                    {
                                        twosides[i] = twosides[i].Remove(twosides[i].Length - 1, 1);
                                    }
                                }
                                if (CustomCommands.IsIn(twosides[0]))
                                {
                                    if (message.MentionedUsers.Count == 0)
                                    {
                                        CustomCommands.ToggleAsMe(twosides[0], ulong.Parse(twosides[1]));
                                        await message.Channel.SendMessageAsync("User selected for command!");
                                    }
                                    else
                                    {
                                        foreach (var item in message.MentionedUsers)
                                        {
                                            CustomCommands.ToggleAsMe(twosides[0], item.Id);
                                        }
                                        await message.Channel.SendMessageAsync("User selected for command!");
                                    }
                                }
                            }
                            else if ((TheMessage.StartsWith("ADDCOMMAND") || TheMessage.StartsWith("CREATECOMMAND")) && TheMessage.Contains('^'))
                            {
                                StringBuilder _Temp = new StringBuilder();
                                string _Trigger;
                                string[] _Return;
                                bool _Contains = false;
                                ulong userID = 0;
                                if (TheMessage.StartsWith("ADDCOMMAND"))
                                {
                                    TheMessageNormal = TheMessageNormal.Remove(0, 10);
                                }
                                else
                                {
                                    TheMessageNormal = TheMessageNormal.Remove(0, "CREATECOMMAND".Length);
                                }
                                while (TheMessageNormal[0] == ' ')
                                {
                                    TheMessageNormal = TheMessageNormal.Remove(0, 1);
                                }
                                while (TheMessageNormal[0] != '^')
                                {
                                    _Temp.Append(TheMessageNormal[0]);
                                    TheMessageNormal = TheMessageNormal.Remove(0, 1);
                                }
                                _Trigger = _Temp.ToString();
                                while (_Trigger[_Trigger.Count() - 1] == ' ')
                                {
                                    _Trigger = _Trigger.Remove(_Trigger.Count() - 1, 1);
                                }
                                TheMessageNormal = TheMessageNormal.Remove(0, 1);
                                while (TheMessageNormal[0] == ' ')
                                {
                                    TheMessageNormal = TheMessageNormal.Remove(0, 1);
                                }
                                _Return = TheMessageNormal.Split('^');
                                for (int i = 0; i < _Return.Count(); i++)
                                {
                                    while (_Return[i].EndsWith(" "))
                                    {
                                        _Return[i] = _Return[i].TrimEnd(' ');
                                    }
                                    while (_Return[i].StartsWith(" "))
                                    {
                                        _Return[i] = _Return[i].TrimStart(' ');
                                    }
                                }
                                if (_Return[0].ToUpper() == "TRUE" || _Return[0].ToUpper() == "FALSE")
                                {
                                    if (_Return[0].ToUpper() == "TRUE")
                                    {
                                        _Contains = true;
                                    }
                                    List<string> _TempList = _Return.ToList();
                                    _TempList.RemoveAt(0);
                                    _Return = _TempList.ToArray();
                                }
                                if (_Return[0].ToUpper() == "TRUE")
                                {
                                    userID = message.Author.Id;
                                    List<string> _TempList = _Return.ToList();
                                    _TempList.RemoveAt(0);
                                    _Return = _TempList.ToArray();
                                }
                                if (!BannedCommands.Contains(_Trigger.ToUpper()))
                                {
                                    string _Value = CustomCommands.CheckExisting(_Trigger, "Add");
                                    if (_Value == "")
                                    {
                                        if (CustomCommands.AddCheck(message, _Trigger, _Return, _Contains, userID))
                                        {
                                            CustomCommands.AddCommand(_Trigger, _Return, _Contains, userID);
                                            await message.Channel.SendMessageAsync("Command successfully added!\nShow all custom commands with ShowCustomCommands.");
                                            CustomCommands.SaveCommands();
                                        }
                                        else
                                        {
                                            appendrules = true;
                                            _tempcommand.Trigger = _Trigger;
                                            _tempcommand.Return = _Return;
                                            _tempcommand.include = _Contains;
                                            _tempcommand.asMeID = userID;
                                        }
                                    }
                                    else
                                    {
                                        await message.Channel.SendMessageAsync($"The [{_Value}] command is entirely contained in your command. Please change the type of [{_Value}] or the spelling of your new command.");
                                    }
                                }
                                else
                                {
                                    await message.Channel.SendMessageAsync("I am already using that trigger word, please try a different trigger word!");
                                }
                            }
                            else if (TheMessage.StartsWith("APPENDCOMMAND") && TheMessage.Contains('^'))
                            {
                                StringBuilder _Temp = new StringBuilder();
                                string _Trigger;
                                string[] _Return;
                                bool _Contains = false;
                                ulong userID = 0;
                                TheMessageNormal = TheMessageNormal.Remove(0, 13);
                                while (TheMessageNormal[0] == ' ')
                                {
                                    TheMessageNormal = TheMessageNormal.Remove(0, 1);
                                }
                                while (TheMessageNormal[0] != '^')
                                {
                                    _Temp.Append(TheMessageNormal[0]);
                                    TheMessageNormal = TheMessageNormal.Remove(0, 1);
                                }
                                _Trigger = _Temp.ToString();
                                while (_Trigger[_Trigger.Count() - 1] == ' ')
                                {
                                    _Trigger = _Trigger.Remove(_Trigger.Count() - 1, 1);
                                }
                                TheMessageNormal = TheMessageNormal.Remove(0, 1);
                                while (TheMessageNormal[0] == ' ')
                                {
                                    TheMessageNormal = TheMessageNormal.Remove(0, 1);
                                }
                                _Return = TheMessageNormal.Split('^');
                                for (int i = 0; i < _Return.Count(); i++)
                                {
                                    while (_Return[i].EndsWith(" "))
                                    {
                                        _Return[i] = _Return[i].TrimEnd(' ');
                                    }
                                    while (_Return[i].StartsWith(" "))
                                    {
                                        _Return[i] = _Return[i].TrimStart(' ');
                                    }
                                }
                                if (CustomCommands.IsIn(_Trigger))
                                {
                                    CustomCommands.AppendCommand(_Trigger, _Return);
                                    await message.Channel.SendMessageAsync("Command successfully updated!\nShow all custom commands with ShowCustomCommands.");
                                    CustomCommands.SaveCommands();
                                }
                                else
                                {
                                    if (_Return[0].ToUpper() == "TRUE" || _Return[0].ToUpper() == "FALSE")
                                    {
                                        if (_Return[0].ToUpper() == "TRUE")
                                        {
                                            _Contains = true;
                                        }
                                        List<string> _TempList = _Return.ToList();
                                        _TempList.RemoveAt(0);
                                        _Return = _TempList.ToArray();
                                    }
                                    if (_Return[0].ToUpper() == "TRUE")
                                    {
                                        userID = message.Author.Id;
                                        List<string> _TempList = _Return.ToList();
                                        _TempList.RemoveAt(0);
                                        _Return = _TempList.ToArray();
                                    }
                                    if (!BannedCommands.Contains(_Trigger))
                                    {
                                        string _Value = CustomCommands.CheckExisting(_Trigger, "Add");
                                        if (_Value == "")
                                        {
                                            CustomCommands.AddCommand(_Trigger, _Return, _Contains, userID);
                                            await message.Channel.SendMessageAsync("Command successfully added!\nShow all custom commands with ShowCustomCommands.");
                                            CustomCommands.SaveCommands();
                                        }
                                        else
                                        {
                                            await message.Channel.SendMessageAsync($"The [{_Value}] command is entirely contained in your command. Please change the type of [{_Value}] or the spelling of your new command.");
                                        }
                                    }
                                    else
                                    {
                                        await message.Channel.SendMessageAsync("I am already using that trigger word, please try a different trigger word!");
                                    }
                                }
                            }
                            else if (TheMessage.StartsWith("REMOVEFROMCOMMAND") && TheMessage.Contains('^'))
                            {
                                StringBuilder _Temp = new StringBuilder();
                                string _Trigger;
                                string[] _Return;
                                TheMessageNormal = TheMessageNormal.Remove(0, "REMOVEFROMCOMMAND".Count());
                                while (TheMessageNormal[0] == ' ')
                                {
                                    TheMessageNormal = TheMessageNormal.Remove(0, 1);
                                }
                                while (TheMessageNormal[0] != '^')
                                {
                                    _Temp.Append(TheMessageNormal[0]);
                                    TheMessageNormal = TheMessageNormal.Remove(0, 1);
                                }
                                _Trigger = _Temp.ToString();
                                while (_Trigger[_Trigger.Count() - 1] == ' ')
                                {
                                    _Trigger = _Trigger.Remove(_Trigger.Count() - 1, 1);
                                }
                                TheMessageNormal = TheMessageNormal.Remove(0, 1);
                                while (TheMessageNormal[0] == ' ')
                                {
                                    TheMessageNormal = TheMessageNormal.Remove(0, 1);
                                }
                                _Return = TheMessageNormal.Split('^');
                                for (int i = 0; i < _Return.Count(); i++)
                                {
                                    while (_Return[i].EndsWith(" "))
                                    {
                                        _Return[i] = _Return[i].TrimEnd(' ');
                                    }
                                    while (_Return[i].StartsWith(" "))
                                    {
                                        _Return[i] = _Return[i].TrimStart(' ');
                                    }
                                }
                                if (CustomCommands.IsIn(_Trigger))
                                {
                                    CustomCommands.RemoveInCommand(_Trigger, _Return);
                                    await message.Channel.SendMessageAsync("Command successfully updated!\nShow all custom commands with ShowCustomCommands.");
                                    CustomCommands.SaveCommands();
                                }
                            }
                            else if ((TheMessage.StartsWith("ADDCOMMAND") || TheMessage.StartsWith("CREATECOMMAND")) && !TheMessage.Contains('^'))
                            {
                                await message.Channel.SendMessageAsync("Something weird happened, try typing CustomHelp to make sure you entered everything correctly!");
                            }
                            else if (TheMessage.StartsWith("SHOWCUSTOMCOMMANDS"))
                            {
                                StringBuilder sb = new StringBuilder();
                                string Temp1 = "";
                                string Temp2 = "";
                                sb.Append("List of all custom commands (caps do not matter)" +
                                    ":```");
                                foreach (var item in CustomCommands.commands)
                                {
                                    Temp2 = sb.ToString();
                                    StringBuilder sb3 = new StringBuilder();
                                    sb3.Append($"Trigger: {item.Trigger}\nResponse: ");
                                    int killmeimmediately = 1;
                                    foreach (var thing in item.Return)
                                    {
                                        if (killmeimmediately == 1)
                                        {
                                            killmeimmediately = 0;
                                            sb3.Append($"{thing.Replace("https://", "")}");
                                        }
                                        else
                                        {
                                            sb3.Append($"\nor {thing.Replace("https://", "")}");
                                        }
                                    }
                                    killmeimmediately = 1;
                                    if (sb3.ToString().Count() < 1990)
                                    {
                                        sb.Append($"Trigger: {item.Trigger}\nResponse: ");
                                        foreach (var thing in item.Return)
                                        {
                                            if (killmeimmediately == 1)
                                            {
                                                killmeimmediately = 0;
                                                sb.Append($"{thing.Replace("https://", "")}");
                                            }
                                            else
                                            {
                                                sb.Append($"\nor {thing.Replace("https://", "")}");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        sb.Append($"Trigger: {item.Trigger}\nResponse: (Too Long To fit in a message)");
                                    }
                                    sb.Append($"\n\n");
                                    Temp1 = sb.ToString();
                                    if (Temp1.Length + 171 >= 2000)
                                    {
                                        StringBuilder sb2 = new StringBuilder();
                                        sb2.Append($"{Temp2}```");
                                        await message.Channel.SendMessageAsync(sb2.ToString());
                                        sb.Clear();
                                        sb.Append($"```Trigger: {item.Trigger}\nResponse: ");
                                        foreach (var thing in item.Return)
                                        {
                                            if (thing == item.Return.First())
                                            {
                                                sb.Append($"{thing.Replace("https://", "")}");
                                            }
                                            else
                                            {
                                                sb.Append($"\nor {thing.Replace("https://", "")}");
                                            }
                                        }
                                        sb.Append($"\n\n");
                                    }
                                }
                                sb.Append("```\nAll commands associated with custom commands:```AddCommand, EraseCommand, AppendCommand, ShowCustomCommands, CustomHelp, ToggleCommandType, RemoveFromCommand, CheckCommand```");
                                await message.Channel.SendMessageAsync(sb.ToString());
                            }
                            else if (TheMessage.StartsWith("CUSTOMHELP"))
                            {
                                StringBuilder sb = new StringBuilder();
                                sb.Append("```In order to create a custom command, type 'AddCommand' followed by the trigger for the command, insert a '^'(hold shift and hit 6), then type whatever you want me to respond with when you use the trigger word.\n");
                                sb.Append("You can also include multiple returns for a command, sperate them with another '^'\n If you want to make a command work whenever the trigger word is used in a message, set your first response as 'True'\n\n");
                                sb.Append("Once a custom command is setup, simply call it on its own, for example, if I setup a command 'd' which responds with 'no d', I would type 'd' and hit enter; then the bot will respond with 'no d'\n\n");
                                sb.Append("If you want to change a command, just call AddCommand as usual, passing in the same trigger word (remember, caps do not matter)\n\n");
                                sb.Append("If you want to delete a command, call 'EraseCommand', followed by the trigger word you wish to erase.\n\n");
                                sb.Append("If you want to see all current custom commands, type ShowCustomCommands.\n\n");
                                sb.Append("Example of stuff: AddCommand TestCommand ^ True ^ Ligma Dude ^ Die ^ I'm gay```");
                                await message.Channel.SendMessageAsync(sb.ToString());
                            }
                            else if (TheMessage.StartsWith("ERASECOMMAND") || TheMessage.StartsWith("REMOVECOMMAND"))
                            {
                                if (TheMessage.StartsWith("ERASECOMMAND"))
                                {
                                    TheMessage = TheMessage.Remove(0, 12);
                                }
                                else
                                {
                                    TheMessage = TheMessage.Remove(0, 14);
                                }
                                while (TheMessage[0] == ' ')
                                {
                                    TheMessage = TheMessage.Remove(0, 1);
                                }
                                while (TheMessage[TheMessage.Count() - 1] == ' ' || TheMessage[TheMessage.Count() - 1] == '^')
                                {
                                    TheMessage = TheMessage.Remove(TheMessage.Count() - 1, 1);
                                }
                                if (CustomCommands.RemoveCommand(TheMessage))
                                {
                                    await message.Channel.SendMessageAsync("Command successfully removed!");
                                    CustomCommands.SaveCommands();
                                }
                                else
                                {
                                    await message.Channel.SendMessageAsync("I couldn't find the command you asked for.");
                                }
                            }
                            else if (TheMessage.StartsWith("TRASHRESET") && TheSenderNoString.Id == MyID)
                            {
                                trucktimer = 2;
                            }
                            else if (TheMessage.StartsWith("R/"))
                            {
                                RedditBrowse(message, TheMessageNormal);
                            }
                            else if (TheMessage.Contains("https://www.reddit.com/r/".ToUpper()))
                            {
                                Console.WriteLine("Reddit Time");
                                PostedRedditLink(message, TheMessageNormal);
                            }
                            else if (TheMessage.StartsWith("EGGRESTART") || TheMessage.StartsWith("EGG RESTART") || TheMessage.StartsWith("RESTART EGG") || TheMessage.StartsWith("RESTARTEGG"))
                            {
                                TopDown.SavePlayers();
                                await message.Channel.SendMessageAsync($"Restarting, this should only take a second or two.");
                                await musicManager.Leave();
                                Application.Restart();
                                Environment.Exit(0);
                            }
                            else if (TheMessage.Contains("GOOGLE IT") || TheMessage.Contains("GOOGLING IT"))
                            {
                                string PreviousMessageText = "";
                                foreach (var item in PreviousMessages)
                                {
                                    if (item.PrevChannel == message.Channel)
                                    {
                                        PreviousMessageText = item.PrevMessage;
                                    }
                                }
                                if (PreviousMessageText != "")
                                {
                                    string[] LMGTFY = PreviousMessageText.Split(' ');
                                    StringBuilder sb = new StringBuilder();
                                    sb.Append("http://lmgtfy.com/?q=");
                                    foreach (var item in LMGTFY)
                                    {
                                        sb.Append($"{item}+");
                                    }
                                    await message.Channel.SendMessageAsync(sb.ToString().Remove(sb.ToString().Count() - 1, 1));
                                }
                            }
                            else if (TheMessage.StartsWith("!VANISH"))
                            {
                                deadpeople.Add(new NonExistantUsers(message.Channel, user));
                                await message.Channel.SendMessageAsync("POOF");
                                message.DeleteAsync();
                            }
                            else if (TheMessage.StartsWith("OVERRIDE") || TheMessage.StartsWith("[OVERRIDE]"))
                            {
                                if (_tempcommand.Trigger != null && _tempcommand.Trigger != "")
                                {
                                    CustomCommands.AddCommand(_tempcommand.Trigger, _tempcommand.Return, _tempcommand.include, _tempcommand.asMeID);
                                    await message.Channel.SendMessageAsync("Command successfully overwritten!\nShow all custom commands with ShowCustomCommands.");
                                    CustomCommands.SaveCommands();
                                }
                                else if (_tempcommand.ID != 0)
                                {
                                    CustomCommands.AddMessages(_tempcommand.ID, _tempcommand.Return, message);
                                }
                            }
                            else if (TheMessage.StartsWith("APPEND") || TheMessage.StartsWith("[APPEND]"))
                            {
                                if (_tempcommand.Trigger != null && _tempcommand.Trigger != "")
                                {
                                    CustomCommands.AppendCommand(_tempcommand.Trigger, _tempcommand.Return);
                                    await message.Channel.SendMessageAsync("Command successfully updated!\nShow all custom commands with ShowCustomCommands.");
                                    CustomCommands.SaveCommands();
                                }
                                else if (_tempcommand.ID != 0)
                                {
                                    CustomCommands.AppendMessages(_tempcommand.ID, _tempcommand.Return, message);
                                }
                            }
                            else if (TheMessage.StartsWith("HOOK") && message.Author.Id == MyID && message.Channel.Id == 536673299304808478)
                            {
                                if (!File.Exists($@"..\..\{user.Id}image.png"))
                                {
                                    var yes = Client.GetUser(user.Id).GetAvatarUrl();
                                    System.Drawing.Image image = DownloadImageFromUrl(yes.Trim());
                                    image.Save($@"..\..\{user.Id}image.png");
                                }

                                var hook = (message.Channel as SocketTextChannel).CreateWebhookAsync("Fuck u");
                                Image im = new Image($@"..\..\{user.Id}image.png");
                                await hook.Result.ModifyAsync(x =>
                                {
                                    x.Name = (message.Author as IGuildUser).Nickname;
                                    x.Image = im;
                                });
                                DiscordWebhookClient d = new DiscordWebhookClient(hook.Result);
                                await d.SendMessageAsync(SeriousText(message.Content));
                                await d.DeleteWebhookAsync();
                            }
                            #region Minecraft?
                            else if (TheMessage.StartsWith("MHELP") || TheMessage.StartsWith("MINECRAFTHELP"))
                            {
                                StringBuilder sb = new StringBuilder();
                                sb.Append($"List of all commands:\n```");
                                sb.Append($"Up/Down/Left/Right/Stay: Move in the direction stated, you can also add a number after it to move more than one space. EX: 'Up 3'\n\n");
                                sb.Append($"PUp/PDown/PLeft/PRight/PStay: Place a block in the direction indicated. You can also use any emoji if I am in the server of said emoji. EX: 'PUp stone'\n\n");
                                sb.Append($"ShowDefaultBlocks: Show all blocks you can place in the world without needing the specific emoji. EX: 'ShowDefaultBlocks'\n\n");
                                sb.Append($"CreateSign: Create a sign at your feet which will display text whenever someone ends their move on top of it. EX: 'CreateSign What is ligma?'\n\n");
                                sb.Append($"ChangeIcon: Change the icon you are represented by in the world. EX: 'ChangeIcon :DHead:'\n\n");
                                sb.Append($"CreateArea: Create an area with a name and specified dimensions (X, then Y). EX: 'CreateArea LigmaBallsKiddoXDDD 5 10'\n\n");
                                sb.Append($"ShowAreas: Displays a list of all areas currently in the world. EX: 'ShowAreas'\n\n");
                                sb.Append($"ChangeArea: Teleports you to the middle of the specified area. EX: 'ChangeArea LigmaBallsKiddoXDDD'\n\n");
                                sb.Append($"ToggleStats: Toggles whether or not your X, Y and Area info will be displayed when you perform an action. EX: 'ToggleStats'\n\n");
                                sb.Append($"CreateDoor: Create two connected doors between any coordinates and any area. Call the command once to set the location of Door1, then call it again when you want to place Door2. EX: 'CreateDoor' ... 'ChangeArea LigmaBallsKiddoXDDD' ... 'CreateDoor'\n\n");
                                sb.Append($"DeleteDoor: Deletes a door in the specified direction along with its counterpart somehwere else in the world. EX: 'DeleteDoor down'\n\n");
                                sb.Append($"SaveWorld: The world auto-saves every hour, but if you want to manually save at any time, use this command. EX: 'SaveWorld'\n\n");
                                sb.Append($"```");
                                await message.Channel.SendMessageAsync(sb.ToString());
                                //sb.Append($"\n\n");
                            }
                            else if (TheMessage.StartsWith("SHOWDEFAULTBLOCKS"))
                            {
                                await TopDown.ShowParses(message);
                            }
                            else if (TheMessage.StartsWith("SHOWAREAS") || TheMessage.StartsWith("SHOWWORLDS"))
                            {
                                await TopDown.ShowAreas(message);
                            }
                            else if (TheMessage.StartsWith("CREATESIGN") || TheMessage.StartsWith("EDITSIGN"))
                            {
                                if (TheMessage.StartsWith("CREATESIGN"))
                                {
                                    TheMessageNormal = TheMessageNormal.Remove(0, "CREATESIGN".Length);
                                }
                                else
                                {
                                    TheMessageNormal = TheMessageNormal.Remove(0, "EDITSIGN".Length);
                                }
                                TopDown.CreateSign(message.Author.Id, TheMessageNormal, message);
                            }
                            else if (TheMessage.StartsWith("TOGGLESTATS") || TheMessage.StartsWith("F3"))
                            {
                                if (TopDown.ToggleStats(message.Author.Id))
                                {
                                    await message.Channel.SendMessageAsync($"Your stats will now be shown whenever you move.");
                                }
                                else
                                {
                                    await message.Channel.SendMessageAsync($"Your stats will now be hidden.");
                                }
                            }
                            else if (TheMessage.StartsWith("CREATEAREA") || TheMessage.StartsWith("CREATEWORLD"))
                            {
                                if (TheMessage.StartsWith("CREATEAREA"))
                                {
                                    TheMessageNormal = TheMessageNormal.Remove(0, "CREATEAREA".Length);
                                }
                                else
                                {
                                    TheMessageNormal = TheMessageNormal.Remove(0, "CREATEWORLD".Length);
                                }
                                while (TheMessageNormal[0] == ' ')
                                {
                                    TheMessageNormal = TheMessageNormal.Remove(0, 1);
                                }
                                string[] MessageArray = TheMessageNormal.Split(' ');
                                TopDown.CreateArea(MessageArray[0], int.Parse(MessageArray[1]), int.Parse(MessageArray[2]), message);
                            }
                            else if (TheMessage.StartsWith("SAVEWORLD"))
                            {
                                TopDown.SavePlayers();
                                await message.Channel.SendMessageAsync("Saved world successfully!");
                            }
                            else if (TheMessage.StartsWith("CREATEDOOR"))
                            {
                                TopDown.CreateDoor(TheSenderNoString.Id, message);
                            }
                            else if (TheMessage.StartsWith("CANCELDOOR"))
                            {
                                Console.WriteLine("ye");
                                for (int i = 0; i < TopDown.doors.Count; i++)
                                {
                                    if (TopDown.doors[i].ID == message.Author.Id)
                                    {
                                        TopDown.doors.RemoveAt(i);
                                        await message.Channel.SendMessageAsync("Door creation canceled");
                                        break;
                                    }
                                }
                            }
                            else if (TheMessage.StartsWith("DELETEDOOR") || TheMessage.StartsWith("REMOVEDOOR"))
                            {
                                if (TheMessage.StartsWith("DELETEDOOR"))
                                {
                                    TheMessageNormal = TheMessageNormal.Remove(0, "DELETEDOOR".Length);
                                }
                                else
                                {
                                    TheMessageNormal = TheMessageNormal.Remove(0, "REMOVEDOOR".Length);
                                }
                                while (TheMessageNormal[0] == ' ')
                                {
                                    TheMessageNormal = TheMessageNormal.Remove(0, 1);
                                }
                                while (TheMessageNormal.Contains(' '))
                                {
                                    TheMessageNormal = TheMessageNormal.Remove(TheMessageNormal.Length - 1, 1);
                                }
                                TopDown.DeleteDoor(TheSenderNoString.Id, TheMessageNormal, message);
                            }
                            else if (TheMessage.StartsWith("CHANGEAREA"))
                            {
                                TheMessageNormal = TheMessageNormal.Remove(0, "CHANGEAREA".Count());
                                while (TheMessageNormal[0] == ' ')
                                {
                                    TheMessageNormal = TheMessageNormal.Remove(0, 1);
                                }
                                while (TheMessageNormal.Contains(' '))
                                {
                                    TheMessageNormal = TheMessageNormal.Remove(TheMessageNormal.Count() - 1, 1);
                                }
                                TopDown.ChangeArea(message.Author.Id, TheMessageNormal, message);
                            }
                            else if (TheMessage.StartsWith("PUP") || TheMessage.StartsWith("PDOWN") || TheMessage.StartsWith("PLEFT") || TheMessage.StartsWith("PRIGHT") || TheMessage.StartsWith("PSTAY"))
                            {
                                string Direction = "";
                                if (TheMessage.StartsWith("PLEFT"))
                                {
                                    Direction = "LEFT";
                                    TheMessageNormal = TheMessageNormal.Remove(0, "PLEFT".Length);
                                }
                                else if (TheMessage.StartsWith("PUP"))
                                {
                                    Direction = "UP";
                                    TheMessageNormal = TheMessageNormal.Remove(0, "PUP".Length);
                                }
                                else if (TheMessage.StartsWith("PDOWN"))
                                {
                                    Direction = "DOWN";
                                    TheMessageNormal = TheMessageNormal.Remove(0, "PDOWN".Length);
                                }
                                else if (TheMessage.StartsWith("PSTAY"))
                                {
                                    Direction = "STAY";
                                    TheMessageNormal = TheMessageNormal.Remove(0, "PSTAY".Length);
                                }
                                else
                                {
                                    Direction = "RIGHT";
                                    TheMessageNormal = TheMessageNormal.Remove(0, "PRIGHT".Length);
                                }
                                while (TheMessageNormal[0] == ' ')
                                {
                                    TheMessageNormal = TheMessageNormal.Remove(0, 1);
                                }
                                while (TheMessageNormal.Contains(' '))
                                {
                                    TheMessageNormal = TheMessageNormal.Remove(TheMessageNormal.Length - 1, 1);
                                }
                                TopDown.SetBlock(Direction, message.Author.Id, TheMessageNormal, message);
                            }
                            else if ((TheMessage.StartsWith("UPP") || TheMessage.StartsWith("DOWNN") || TheMessage.StartsWith("LEFTT") || TheMessage.StartsWith("RIGHTT") || TheMessage.StartsWith("STAYY")))
                            {
                                if (!TopDown.IsIn(Egg))
                                {
                                    TopDown.SpawnPlayer(Egg, message);
                                }
                                else
                                {
                                    string Direction = "";
                                    if (TheMessage.StartsWith("UPP"))
                                    {
                                        Direction = "UP";
                                        TheMessage = TheMessage.Remove(0, "UPP".Length);
                                    }
                                    else if (TheMessage.StartsWith("DOWNN"))
                                    {
                                        Direction = "DOWN";
                                        TheMessage = TheMessage.Remove(0, "DOWNN".Length);
                                    }
                                    else if (TheMessage.StartsWith("LEFT"))
                                    {
                                        Direction = "LEFT";
                                        TheMessage = TheMessage.Remove(0, "LEFTT".Length);
                                    }
                                    else if (TheMessage.StartsWith("RIGHTT"))
                                    {
                                        Direction = "RIGHT";
                                        TheMessage = TheMessage.Remove(0, "RIGHTT".Length);
                                    }
                                    else
                                    {
                                        Direction = "STAY";
                                        TheMessage = TheMessage.Remove(0, "STAYY".Length);
                                    }
                                    try
                                    {
                                        TopDown.MovePlayer(Direction, int.Parse(TheMessage), Egg, message);
                                    }
                                    catch
                                    {
                                        TopDown.MovePlayer(Direction, 1, Egg, message);
                                    }
                                }
                            }
                            else if (TheMessage.StartsWith("CHANGEICON"))
                            {
                                TheMessageNormal = TheMessageNormal.Remove(0, "CHANGEICON".Count());
                                while (TheMessageNormal[0] == ' ')
                                {
                                    TheMessageNormal = TheMessageNormal.Remove(0, 1);
                                }
                                while (TheMessageNormal.Contains(' '))
                                {
                                    TheMessageNormal = TheMessageNormal.Remove(TheMessageNormal.Count() - 1, 1);
                                }
                                TopDown.ChangeIcon(message.Author.Id, TheMessageNormal, message);
                            }
                            else if ((TheMessage.StartsWith("UP") || TheMessage.StartsWith("DOWN") || TheMessage.StartsWith("LEFT") || TheMessage.StartsWith("RIGHT") || TheMessage.StartsWith("STAY")))
                            {
                                if (!TopDown.IsIn(TheSenderNoString.Id))
                                {
                                    TopDown.SpawnPlayer(TheSenderNoString.Id, message);
                                }
                                else
                                {
                                    string Direction = "";
                                    if (TheMessage.StartsWith("UP"))
                                    {
                                        Direction = "UP";
                                        TheMessage = TheMessage.Remove(0, "UP".Length);
                                    }
                                    else if (TheMessage.StartsWith("DOWN"))
                                    {
                                        Direction = "DOWN";
                                        TheMessage = TheMessage.Remove(0, "DOWN".Length);
                                    }
                                    else if (TheMessage.StartsWith("LEFT"))
                                    {
                                        Direction = "LEFT";
                                        TheMessage = TheMessage.Remove(0, "LEFT".Length);
                                    }
                                    else if (TheMessage.StartsWith("RIGHT"))
                                    {
                                        Direction = "RIGHT";
                                        TheMessage = TheMessage.Remove(0, "RIGHT".Length);
                                    }
                                    else
                                    {
                                        Direction = "STAY";
                                        TheMessage = TheMessage.Remove(0, "STAY".Length);
                                    }
                                    if (TheMessage.Length < 6)
                                    {
                                        try
                                        {
                                            TopDown.MovePlayer(Direction, int.Parse(TheMessage), TheSenderNoString.Id, message);
                                        }
                                        catch
                                        {
                                            TopDown.MovePlayer(Direction, 1, TheSenderNoString.Id, message);
                                        }
                                    }
                                }
                            }
                            #endregion
                            #region claiming commands
                            else if (TheMessage.StartsWith("CLAIMCOMMAND"))
                            {
                                TheMessage = TheMessage.Remove(0, "CLAIMCOMMAND".Count());
                                while (TheMessage[0] == ' ')
                                {
                                    TheMessage = TheMessage.Remove(0, 1);
                                }
                                while (TheMessage[TheMessage.Count() - 1] == ' ' || TheMessage[TheMessage.Count() - 1] == '^')
                                {
                                    TheMessage = TheMessage.Remove(TheMessage.Count() - 1, 1);
                                }
                                CustomCommands.ClaimCommand(TheMessage, message.Author.Id, message);
                            }
                            else if (TheMessage.StartsWith("UNCLAIMCOMMAND"))
                            {
                                TheMessage = TheMessage.Remove(0, "UNCLAIMCOMMAND".Count());
                                while (TheMessage[0] == ' ')
                                {
                                    TheMessage = TheMessage.Remove(0, 1);
                                }
                                while (TheMessage[TheMessage.Count() - 1] == ' ' || TheMessage[TheMessage.Count() - 1] == '^')
                                {
                                    TheMessage = TheMessage.Remove(TheMessage.Count() - 1, 1);
                                }
                                CustomCommands.UnClaimCommand(TheMessage, message.Author.Id, message);
                            }
                            else if (TheMessage.StartsWith("ADDMESSAGE") || TheMessage.StartsWith("CREATEMESSAGE"))
                            {
                                if (TheMessage.StartsWith("ADDMESSAGE"))
                                {
                                    TheMessageNormal = TheMessageNormal.Remove(0, "ADDMESSAGE".Length);
                                }
                                else
                                {
                                    TheMessageNormal = TheMessageNormal.Remove(0, "CREATEMESSAGE".Length);
                                }
                                StringBuilder _Temp = new StringBuilder();
                                string[] _Return;
                                while (TheMessageNormal[0] == ' ')
                                {
                                    TheMessageNormal = TheMessageNormal.Remove(0, 1);
                                }
                                _Return = TheMessageNormal.Split('^');
                                for (int i = 0; i < _Return.Count(); i++)
                                {
                                    while (_Return[i].EndsWith(" "))
                                    {
                                        _Return[i] = _Return[i].TrimEnd(' ');
                                    }
                                    while (_Return[i].StartsWith(" "))
                                    {
                                        _Return[i] = _Return[i].TrimStart(' ');
                                    }
                                }
                                if (!CustomCommands.AddMessageCheck(message, message.Author.Id, _Return))
                                {
                                    appendrules = true;
                                    _tempcommand.ID = message.Author.Id;
                                    _tempcommand.Return = _Return;
                                }
                            }
                            else if (TheMessage.StartsWith("APPENDMESSAGE"))
                            {
                                StringBuilder _Temp = new StringBuilder();
                                string[] _Return;
                                TheMessageNormal = TheMessageNormal.Remove(0, "APPENDMESSAGE".Length);
                                while (TheMessageNormal[0] == ' ')
                                {
                                    TheMessageNormal = TheMessageNormal.Remove(0, 1);
                                }
                                _Return = TheMessageNormal.Split('^');
                                for (int i = 0; i < _Return.Count(); i++)
                                {
                                    while (_Return[i].EndsWith(" "))
                                    {
                                        _Return[i] = _Return[i].TrimEnd(' ');
                                    }
                                    while (_Return[i].StartsWith(" "))
                                    {
                                        _Return[i] = _Return[i].TrimStart(' ');
                                    }
                                }
                                if (CustomCommands.IsOwnerIn(message.Author.Id))
                                {
                                    CustomCommands.AppendMessages(message.Author.Id, _Return, message);
                                }
                                else
                                {
                                    CustomCommands.AddMessages(message.Author.Id, _Return, message);
                                }
                            }
                            else if (TheMessage.StartsWith("REMOVEMESSAGE") || TheMessage.StartsWith("DELETEMESSAGE"))
                            {
                                if (TheMessage.StartsWith("REMOVEMESSAGE"))
                                {
                                    TheMessageNormal = TheMessageNormal.Remove(0, "REMOVEMESSAGE".Length);
                                }
                                else
                                {
                                    TheMessageNormal = TheMessageNormal.Remove(0, "DELETEMESSAGE".Length);
                                }
                                StringBuilder _Temp = new StringBuilder();
                                string[] _Return;
                                while (TheMessageNormal[0] == ' ')
                                {
                                    TheMessageNormal = TheMessageNormal.Remove(0, 1);
                                }
                                _Return = TheMessageNormal.Split('^');
                                for (int i = 0; i < _Return.Count(); i++)
                                {
                                    while (_Return[i].EndsWith(" "))
                                    {
                                        _Return[i] = _Return[i].TrimEnd(' ');
                                    }
                                    while (_Return[i].StartsWith(" "))
                                    {
                                        _Return[i] = _Return[i].TrimStart(' ');
                                    }
                                }
                                if (CustomCommands.IsOwnerIn(message.Author.Id))
                                {
                                    CustomCommands.RemoveMessage(message.Author.Id, _Return, message);
                                }
                            }
                            else if (TheMessage.StartsWith("GETMESSAGES") || TheMessage.StartsWith("SHOWMESSAGES"))
                            {
                                CustomCommands.GetMessage(message.Author.Id, message);
                            }
                            else if (TheMessage.StartsWith("MESSAGEHELP") || TheMessage.StartsWith("MESSAGE HELP"))
                            {
                                StringBuilder sb = new StringBuilder();
                                sb.Append($"All commands involved with claiming messages:\n\n``");
                                sb.Append($"Claim a command\nClaim a command which will prevent other users from changing how it works\nExample usage: Claimcommand best girl\n(Unclaim a command with UnClaimcommand instead)\n\n");
                                sb.Append($"Tampering message\nYou can set a custom message to play when people try to tamper with your claimed commands (insert more than one reply seperated by '^')\nExample usage: AddMessage no u ^ don't touch my waifu\n(use AppendMessage instead if you wish to add onto your list later)\n\n");
                                sb.Append($"Remove tampering message\nYou can similarly remove a message from your list of messages if you made a mistake.\nExample usage: Removemessage no u\n(Can also be seperated by '^' if you want to remove more than one message)\n\n");
                                sb.Append($"Get tempering messages\nSimply returns all of your current messages that can play\nExample usage: Getmessages\n\n");
                                //sb.Append($"\n\n");
                                sb.Append($"``");
                                await message.Channel.SendMessageAsync(sb.ToString());
                            }
                            #endregion
                            #endregion
                            //DMs
                            if (message.Channel is IDMChannel)
                            {
                                #region sendtorune
                                //if (TheMessage == "~KILLYOURSELF" && user.Id == Rune)
                                //{
                                //    await message.Channel.SendMessageAsync($"Understanda:b:le have a nice day");
                                //    KilledSelf = !KilledSelf;
                                //}
                                //else if (TheMessage.StartsWith("~SENDTORUNE") && !KilledSelf)
                                //{
                                //    sendtorune();
                                //}
                                //else if (TheMessage.StartsWith("~SENDTORUNE") && KilledSelf)
                                //{
                                //    await message.Channel.SendMessageAsync($"Rune has kiled himself so he won't receive the message!");
                                //}
                                //else
                                //{
                                //    await message.Channel.SendMessageAsync($"Try the command ~sendtorune followed by anything to make rune upset at me!");
                                //}
                                #endregion
                                if (TheMessage.StartsWith("CHANNELTEST") && message.Author.Id == MyID)
                                {
                                    foreach (var item in Client.Guilds)
                                    {
                                        if (item.Id == 269898750560829443)
                                        {
                                            StringBuilder sb = new StringBuilder();
                                            foreach (var channel in item.Channels)
                                            {
                                                sb.Append(channel.Name + '\n');
                                            }
                                            await message.Author.SendMessageAsync(sb.ToString());
                                        }
                                    }
                                }
                                if (TheMessage.StartsWith("ROLESTEST") && message.Author.Id == MyID)
                                {
                                    StringBuilder sb = new StringBuilder();
                                    using (var tempguild = Client.GetGuild(262637771368300544))
                                    {
                                        foreach (var role in tempguild.Roles)
                                        {
                                            sb.Append($"{role.Id.ToString()} {role.ToString()}\n");
                                        }
                                    }
                                    await message.Author.SendMessageAsync(sb.ToString());
                                }
                                if (TheMessage.StartsWith("I HAVE A SMALL PP"))
                                {
                                    foreach (var item in deadpeople)
                                    {
                                        if (item.user.Id == message.Author.Id)
                                        {
                                            item.timer = 1;
                                            await item.channel.SendMessageAsync($"{message.Author.Username} has a small pp");
                                        }
                                    }
                                }
                            }
                            //NotDMs
                            else if (message.Channel.Id != 538638653505667092)
                            {
                                if (TheMessage.StartsWith("TRASH") && message.MentionedUsers.Count > 0)
                                {
                                    StringBuilder sb = new StringBuilder();
                                    sb.Append($"TRASHTIME {message.Channel.Id}");
                                    foreach (var item in message.MentionedUsers)
                                    {
                                        sb.Append($" {item.Id}");
                                    }
                                    Console.WriteLine(sb.ToString());
                                    trashtimestring(sb.ToString(), "norm");
                                }
                                #region rockpaperscissors
                                else if (TheMessage.StartsWith("RPS") && message.MentionedUsers.Count == 1 && RPCcountdown == -1)
                                {
                                    EmbedBuilder eb = new EmbedBuilder();
                                    eb.Title = $"{message.MentionedUsers.ElementAt(0).Username}, you have been challenged to Rock-Paper-Scissors by {TheSenderNoString.Username}.";
                                    EmbedFooterBuilder ft = new EmbedFooterBuilder();
                                    ft.Text = "Do you accept? (y/n)";
                                    eb.Footer = ft;
                                    eb.ImageUrl = "https://i.imgur.com/DaO0yu8.png";
                                    await message.Channel.SendMessageAsync("", false, eb.Build());
                                    RPCcountdown = 10;
                                    RPCphasemom = 1;
                                    RPC1 = message.Author;
                                    RPC2 = message.MentionedUsers.ElementAt(0);
                                    RPCchannel = message.Channel;
                                }
                                else if ((TheMessage.StartsWith("Y") || TheMessage.StartsWith("N")) && RPCphasemom == 1 && TheMessage.Length == 1 && message.Author == RPC2 && RPCchannel == message.Channel)
                                {
                                    StringBuilder sb = new StringBuilder();
                                    if (TheMessage.StartsWith("N"))
                                    {
                                        sb.Append($"Too bad.\n");
                                    }
                                    RPCcountdown = 10;
                                    RPCphasemom = 2;
                                    sb.Append($"{RPC2.Username}, please choose either rock, paper, or scissors.");
                                    await RPCchannel.SendMessageAsync(sb.ToString());
                                }
                                else if ((TheMessage.Contains("ROCK") || TheMessage.Contains("PAPER") || TheMessage.Contains("SCISSORS") || (TheMessage.StartsWith("GUN") && TheMessage.Length == 3)) && TheMessage.Length < 9 && RPCphasemom == 2 && message.Author == RPC2 && RPCchannel == message.Channel)
                                {
                                    bool shot = false;
                                    EmbedBuilder eb = new EmbedBuilder();
                                    do
                                    {
                                        RPCcountdown = 10;
                                        RPCphasemom = 3;
                                        if (TheMessage.StartsWith("ROCK"))
                                        {
                                            RPCchoice1 = "ROCK";
                                            eb.ImageUrl = "https://i.imgur.com/3PJopPq.jpg";
                                            eb.Color = Color.DarkGreen;
                                        }
                                        else if (TheMessage.StartsWith("PAPER"))
                                        {
                                            RPCchoice1 = "PAPER";
                                            eb.ImageUrl = "https://i.imgur.com/H1An3MQ.png";
                                            eb.Color = Color.DarkBlue;
                                        }
                                        else if (TheMessage.StartsWith("SCISSORS"))
                                        {
                                            RPCchoice1 = "SCISSORS";
                                            eb.ImageUrl = "https://i.imgur.com/7zpEuOD.png";
                                            eb.Color = Color.DarkRed;
                                        }
                                        else
                                        {
                                            shot = true;
                                            if ((RPC1 as IGuildUser).VoiceChannel != null)
                                            {
                                                if ((message.Author as IGuildUser).VoiceChannel != null)
                                                {
                                                    PlaySong($@"play https://www.youtube.com/watch?v=E3TsZOV4Wvg", user, true);
                                                }
                                            }
                                            else if ((RPC2 as IGuildUser).VoiceChannel != null)
                                            {
                                                if ((message.Author as IGuildUser).VoiceChannel != null)
                                                {
                                                    PlaySong($@"play https://www.youtube.com/watch?v=E3TsZOV4Wvg", user, true);
                                                }
                                            }
                                            break;
                                        }
                                        eb.Title = $"{RPC1.Username}, {RPC2.Username} has picked {RPCchoice1}.";
                                        EmbedFooterBuilder ft = new EmbedFooterBuilder();
                                        ft.Text = "What will you use to counter it?";
                                        eb.Footer = ft;
                                        await RPCchannel.SendMessageAsync("", false, eb.Build());
                                    } while (false);
                                    if (shot == true)
                                    {
                                        RPCcountdown = -1;
                                        RPCphasemom = 0;
                                        eb.Title = $"WOAH! {RPC2.Username} JUST PULLED OUT A GUN!";
                                        EmbedFooterBuilder ft = new EmbedFooterBuilder();
                                        ft.Text = $"{RPC1.Username} is dead and can no longer continue.";
                                        eb.ImageUrl = "https://i.imgur.com/bb8oJps.png";
                                        eb.Footer = ft;
                                        eb.Color = Color.Red;
                                        await RPCchannel.SendMessageAsync("", false, eb.Build());
                                        trashtimestring($"TRASHTIME {RPCchannel.Id} {RPC1.Id}", "gears");
                                    }
                                }
                                else if ((TheMessage.Contains("ROCK") || TheMessage.Contains("PAPER") || TheMessage.Contains("SCISSORS") || (TheMessage.StartsWith("GUN") && TheMessage.Length == 3)) && TheMessage.Length < 9 && RPCphasemom == 3 && message.Author == RPC1 && RPCchannel == message.Channel)
                                {
                                    RPCcountdown = -1;
                                    RPCphasemom = 0;
                                    List<ulong> IDof2 = new List<ulong>();
                                    IDof2.Add(RPC2.Id);
                                    List<ulong> IDof1 = new List<ulong>();
                                    IDof1.Add(RPC1.Id);
                                    if (TheMessage.StartsWith("ROCK"))
                                    {
                                        if (RPCchoice1 == "PAPER")
                                        {
                                            await RPCchannel.SendMessageAsync($"Hah, you dipshit. {RPC2.Username} wins.");
                                            trashtimewithids(message.Channel.Id, IDof1, "RPC");
                                        }
                                        else if (RPCchoice1 == "ROCK")
                                        {
                                            await RPCchannel.SendMessageAsync($"It's a tie, guess I'll die.");
                                        }
                                        else
                                        {
                                            await RPCchannel.SendMessageAsync($"The winner is {RPC1.Username} by a landslide!");
                                            trashtimewithids(message.Channel.Id, IDof2, "RPC");
                                        }
                                    }
                                    else if (TheMessage.StartsWith("PAPER"))
                                    {
                                        if (RPCchoice1 == "SCISSORS")
                                        {
                                            await RPCchannel.SendMessageAsync($"Hah, you dipshit. {RPC2.Username} wins.");
                                            trashtimewithids(message.Channel.Id, IDof1, "RPC");
                                        }
                                        else if (RPCchoice1 == "PAPER")
                                        {
                                            await RPCchannel.SendMessageAsync($"It's a tie, guess I'll die.");
                                        }
                                        else
                                        {
                                            await RPCchannel.SendMessageAsync($"The winner is {RPC1.Username} by a landslide!");
                                            trashtimewithids(message.Channel.Id, IDof2, "RPC");
                                        }
                                    }
                                    else if (TheMessage.StartsWith("SCISSORS"))
                                    {
                                        if (RPCchoice1 == "ROCK")
                                        {
                                            await RPCchannel.SendMessageAsync($"Hah, you dipshit. {RPC2.Username} wins.");
                                            trashtimewithids(message.Channel.Id, IDof1, "RPC");
                                        }
                                        else if (RPCchoice1 == "SCISSORS")
                                        {
                                            await RPCchannel.SendMessageAsync($"It's a tie, guess I'll die.");
                                        }
                                        else
                                        {
                                            await RPCchannel.SendMessageAsync($"The winner is {RPC1.Username} by a landslide!");
                                            trashtimewithids(message.Channel.Id, IDof2, "RPC");
                                        }
                                    }
                                    else
                                    {
                                        EmbedBuilder eb = new EmbedBuilder();
                                        eb.Title = $"WOAH! {RPC1.Username} JUST PULLED OUT... a banana.";
                                        EmbedFooterBuilder ft = new EmbedFooterBuilder();
                                        ft.Text = $"{RPC2.Username} took this as an act of war and shot {RPC1.Username}.";
                                        eb.ImageUrl = "https://i.imgur.com/GbjBLxl.png";
                                        eb.Footer = ft;
                                        eb.Color = Color.Gold;
                                        await RPCchannel.SendMessageAsync("", false, eb.Build());
                                        if ((RPC1 as IGuildUser).VoiceChannel != null)
                                        {
                                            if ((message.Author as IGuildUser).VoiceChannel != null)
                                            {
                                                PlaySong($@"play https://www.youtube.com/watch?v=E3TsZOV4Wvg", user, true);
                                            }
                                        }
                                        else if ((RPC2 as IGuildUser).VoiceChannel != null)
                                        {
                                            if ((message.Author as IGuildUser).VoiceChannel != null)
                                            {
                                                PlaySong($@"play https://www.youtube.com/watch?v=E3TsZOV4Wvg", user, true);
                                            }
                                        }
                                        trashtimestring($"TRASHTIME {RPCchannel.Id} {RPC1.Id}", "gears");
                                    }
                                }
                                #endregion
                                else if (TheMessage.StartsWith("TESTSPOILER") && TheMessage.Length == "TESTSPOILER".Length)
                                {
                                    EmbedBuilder eb = new EmbedBuilder();
                                    eb.Title = "Ligma Dude";
                                    eb.ImageUrl = $"https://i.imgur.com/UlqP6Td.jpg";
                                    await message.Channel.SendMessageAsync("", false, eb.Build());
                                }
                                else if (TheMessage.Contains("I WANT TO DIE") || TheMessage.Contains("KILL ME") || TheMessage.Contains("KILL MYSELF") || TheMessage.Contains("SHOOT ME") || TheMessage.Contains("GUESS ILL DIE") || TheMessage.Contains("GUESS I'LL DIE") || TheMessage.Contains("END ME") || TheMessage.Contains("LIFE SUCKS") || TheMessage.Contains("I WOULD LIKE TO DIE") || TheMessage.Contains("KMS") || TheMessage.Contains("END MY LIFE") || TheMessage.Contains("DIE ME") || TheMessage.Contains("DIE ME") || TheMessage.Contains("TRASH ME") || TheMessage.Contains("I NO LONGER WISH TO BEE ALIVE") || TheMessage.Contains("I NO LONGER WISH TO BE ALIVE"))
                                {
                                    if (!trashing)
                                    {
                                        List<ulong> vbnfgf = new List<ulong>();
                                        vbnfgf.Add(message.Author.Id);
                                        trashtimewithids(message.Channel.Id, vbnfgf, "gears");
                                        EmbedBuilder eb = new EmbedBuilder();
                                        eb.Title = $"Don't worry {message.Author.Username}! I have just the tool.";
                                        eb.ImageUrl = "https://i.imgur.com/bb8oJps.png";
                                        eb.Color = Color.Red;
                                        await message.Channel.SendMessageAsync("", false, eb.Build());
                                        if ((message.Author as IGuildUser).VoiceChannel != null)
                                        {
                                            PlaySong($@"play https://www.youtube.com/watch?v=E3TsZOV4Wvg", user, true);
                                        }
                                    }
                                }
                                else if (TheMessage.StartsWith("EGGHELP") || TheMessage.StartsWith("HELPEGG") || TheMessage.StartsWith("EGG HELP") || TheMessage.StartsWith("HELP EGG"))
                                {
                                    StringBuilder sb = new StringBuilder();
                                    sb.Append($"All of my commands are:\n\n``");
                                    sb.Append($"Trash\nDump any amount of users in voice channels in the trash using this command.\nExample usage: trash @Egg#0204 @Metrosexual Fruitcake#6969\n\n");
                                    sb.Append($"Rock-Paper-Scissors\nChallange another player (or yourself if you are feeling adventurous) to a game of rock paper scissors!\nExample usage: rps @Egg#0204\n\n");
                                    sb.Append($"Kill Yourself\nIf you are ever feeling out of luck, I have just the thing to help you out!\nExample usage: I want to die\n\n");
                                    sb.Append($"Roll Dice\nRoll an amount of dice and see how unlucky you are!\nExample usage: roll 2d6\n\n");
                                    sb.Append($"2 Teams\nSplit a group of people into 2 different teams.\nExample usage: 2teams guy1 guy2 guy3 guy4 guy5 guy6\n\n");
                                    sb.Append($"Custom Commands\nYou can create your own (slightly limited) custom commands! Put a '^' (hold shift and hit 6) between the command and the action! (Type CustomHelp for more help)\nExample usage: CreateCommand d ^ no d\n\n");
                                    sb.Append($"Claim a command\nYou can claim commands to prevent someone from messing with your precious baby.\nExample usage: ClaimCommand best girl\n\n");
                                    sb.Append($"Tampering message\nYou can set a custom message to play when people try to tamper with your claimed commands (insert more than one reply seperated by '^')\nExample usage: AddMessage no u ^ don't touch my waifu\n\n");
                                    //sb.Append($"Play Youtube Song\nYou can play (most) youtube songs if you are in a voice channel! Simply type Play, followed by your url. You can make me leave a voice channel by typing Leave.\nExample usage: Play https://www.youtube.com/watch?v=3j6ZCVWNF4Q\nAll music commands: Play, Skip, Join, Leave\n\n");
                                    sb.Append($"LMGTFY\nIf someone asks a question, tell them to 'Google It' and I will give you a LetMeGoogleThatForYou link.\n\n");
                                    sb.Append($"(not)Minecraft\nYou can walk around the world and place blocks and stuff. To see more info, type MHelp.\n\n");
                                    sb.Append($"Vanish\n Who knows what this command does?\nExample usage: !vanish\n\n");
                                    sb.Append($"Create Reminders\nCreate reminders for yourself\nExample Usage: RemindMe 2 hours ^ yeet\nType RemindersHelp for more info and technicalities.\n\n");
                                    sb.Append($"Super Stroke!\nCombine the previous 3 messages in a random order.\nExample usage: superstroke\n\n");
                                    sb.Append($"Galaxy Stroke!\nCombine some amount of words said in the past 24 hours (default 30 words if not specified) in a random order. Type 'Galaxy Debug' for more info.\nExample usage: Galaxy Stroke 50\n\n");
                                    sb.Append($"Opt Out of Dad Commands.\nSelf explanitory.\nExample usage: 'NO DAD' or 'YES DAD'\n\n");
                                    //sb.Append($"\n\n");
                                    sb.Append($"``");
                                    await message.Channel.SendMessageAsync(sb.ToString());
                                }
                                else if (TheMessage.StartsWith("ROLL"))
                                {
                                    try
                                    {
                                        string tempmessage = TheMessageNormal.ToLower().Remove(0, 4);
                                        while (tempmessage[0] == ' ')
                                        {
                                            tempmessage = tempmessage.Remove(0, 1);
                                        }
                                        Console.WriteLine("temp message sorted out");
                                        StringBuilder dicetemp = new StringBuilder();
                                        while (tempmessage[0] != 'd')
                                        {
                                            Console.WriteLine("Starting while loop");
                                            dicetemp.Append(tempmessage[0]);
                                            tempmessage = tempmessage.Remove(0, 1);
                                        }
                                        Console.WriteLine("Dice amount succ");
                                        tempmessage = tempmessage.Remove(0, 1);
                                        int diceamount = int.Parse(dicetemp.ToString());
                                        dicetemp.Clear();
                                        dicetemp.Append($"You rolled {diceamount} d{tempmessage}'s; your numbers are:");
                                        Console.WriteLine("Dice you rolled message succ");
                                        Random dice2 = new Random();
                                        int total = 0;
                                        for (int i = 0; i < diceamount; i++)
                                        {
                                            int dice = int.Parse(tempmessage);
                                            dice = dice2.Next(1, dice + 1);
                                            dicetemp.Append($"\n``{dice}``");
                                            total += dice;
                                        }
                                        dicetemp.Append($"\nFor a total of ``{total}``");
                                        await message.Channel.SendMessageAsync(dicetemp.ToString());
                                    }
                                    catch
                                    {
                                        Console.WriteLine("Dice Failed");
                                    }
                                }
                                else if (TheMessage.StartsWith("2TEAMS"))
                                {
                                    try
                                    {
                                        StringBuilder sb = new StringBuilder();
                                        string tempmessage = message.Content.Remove(0, 6);
                                        while (tempmessage[0] == ' ')
                                        {
                                            tempmessage = tempmessage.Remove(0, 1);
                                        }
                                        string[] messagelist = tempmessage.Split(' ');
                                        List<string> thedudes = new List<string>();
                                        for (int i = 0; i < messagelist.Count(); i++)
                                        {
                                            thedudes.Add(messagelist[i]);
                                        }
                                        List<string> Team1 = new List<string>();
                                        List<string> Team2 = new List<string>();
                                        Random random = new Random();
                                        int half = thedudes.Count / 2;
                                        for (int i = 0; i < half; i++)
                                        {
                                            int tempnum = random.Next(0, thedudes.Count());
                                            Team1.Add(thedudes.ElementAt(tempnum));
                                            thedudes.RemoveAt(tempnum);
                                        }
                                        foreach (var item in thedudes)
                                        {
                                            Team2.Add(item);
                                        }
                                        sb.Append($"Red Team: ");
                                        foreach (var item in Team1)
                                        {
                                            sb.Append($"``{item}`` ");
                                        }
                                        sb.Append($"\nBlue Team: ");
                                        foreach (var item in Team2)
                                        {
                                            sb.Append($"``{item}`` ");
                                        }
                                        await message.Channel.SendMessageAsync(sb.ToString());
                                    }
                                    catch
                                    {
                                    }
                                }
                            }
                            //Bot Channel
                            else
                            {
                                if (TheMessage.StartsWith("TRASHTIME"))
                                {
                                    trashtimestring(TheMessage, "gears");
                                }
                            }
                            if (PreviousMessages.Count() == 0)
                            {
                                PreviousMessages.Add(new PreviousMessage(TheMessageNormal, message.Channel));
                            }
                            else
                            {
                                for (int i = 0; i < PreviousMessages.Count(); i++)
                                {
                                    if (PreviousMessages[i].PrevChannel == message.Channel)
                                    {
                                        PreviousMessages[i].PrevMessage = TheMessageNormal;
                                    }
                                    else if (i == PreviousMessages.Count() - 1)
                                    {
                                        PreviousMessages.Add(new PreviousMessage(TheMessageNormal, message.Channel));
                                        break;
                                    }
                                }
                            }
                            if (message.Channel is IDMChannel)
                            {
                                if (TheSenderNoString.Id != MyID)
                                {
                                    await Client.GetUser(MyID).SendMessageAsync($"{TheSender} sent: {TheMessageNormal}");
                                }
                            }
                            if (!appendrules)
                            {
                                _tempcommand.Trigger = "";
                                _tempcommand.ID = 0;
                            }
                        }
                        if (!strokeoutdab)
                        {
                            Last3Message.Add(message.Content);
                            if (Last3Message.Count == 4)
                            {
                                Last3Message.RemoveAt(0);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                if (message.Author.Id != Egg)
                {
                    try
                    {
                        if (!canudont)
                        {
                            tempalias = "";
                        }
                        if (!strokeoutdab)
                            galaxywords.AddWords(message.Content, message.Author.Id, message.Channel.Id);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            async Task Banned(SocketUser user, SocketGuild guild)
            {
                try
                {
                    if ((user.Id == MyID || user.Id == Gears) && guild.Id == 262637771368300544)
                    {
                        await guild.RemoveBanAsync(user);
                        await Client.GetUser(MyID).SendMessageAsync($"Unbanned {user.Username}");
                    }
                }
                catch
                {
                }

            }
            async Task Kicked(SocketGuildUser user)
            {
                try
                {
                    if (user.Id == MyID || user.Id == Gears)
                    {
                        await user.SendMessageAsync("https://discord.gg/esrusPr");
                        await Client.GetUser(MyID).SendMessageAsync($"Reinvited {user.Username}");
                    }
                }
                catch
                {
                }
            }
            async Task Joined(SocketGuildUser user)
            {
                try
                {
                    if (user.Id == MyID)
                    {
                        //await user.AddRoleAsync(Client.GetGuild(262637771368300544).GetRole(262639069975019520));
                        await user.AddRoleAsync(Client.GetGuild(262637771368300544).GetRole(458322716005498902));
                    }
                    else if (user.Id == Gears)
                    {
                        await user.AddRoleAsync(Client.GetGuild(262637771368300544).GetRole(458322716005498902));
                    }
                }
                catch
                {
                }
            }
            async Task Updated(SocketUser user, SocketUser user2)
            {
                try
                {
                    if (user.GetAvatarUrl() != user2.GetAvatarUrl())
                    {
                        string[] filenames = Directory.GetFiles(@"..\..\FinalImages");
                        foreach (var item in filenames)
                        {
                            if (item.Contains((user.Id / 10000000000).ToString()))
                            {
                                File.Delete(item);
                            }
                        }
                    }
                }
                catch
                {
                }
            }
            async Task TellMe(SocketChannel channel)
            {
                if (channel.ToString() != "@Metrosexual Fruitcake#6969")
                {
                    await Client.GetUser(MyID).SendMessageAsync($"{channel} was created");
                }
            }

            Client.MessageReceived += MessageReceived;
            Client.Ready += Connected;
            Client.UserBanned += Banned;
            Client.UserLeft += Kicked;
            Client.UserJoined += Joined;
            Client.UserUpdated += Updated;
            //Client.UserIsTyping += Typing;
            Client.ChannelCreated += TellMe;

            async Task Connected()
            {
                var thechannel = Client.GetChannel(538638653505667092);
                await (thechannel as ISocketMessageChannel).SendMessageAsync("Egg online am?");
                //audioClient = await (Client.GetChannel(348327013842419713) as ISocketAudioChannel).ConnectAsync();
                //foreach (var item in Client.Guilds)
                //{
                //    foreach (var voice in item.VoiceChannels)
                //    {
                //        await (Client.GetChannel(voice.Id) as ISocketAudioChannel).DisconnectAsync();
                //    }
                //}
                await musicManager.Leave();
            }
            async Task UpdateStuff()
            {
                if (DateTime.Now.Second == 0)
                {
                    Yes -= 60;
                }
                if (DateTime.Now.Second > Yes && Timer > 0)
                {
                    Yes = DateTime.Now.Second;
                    Timer--;
                }
                if (DateTime.Now.Second != truckseconds && trucktimer > 0)
                {
                    truckseconds = DateTime.Now.Second;
                    trucktimer--;
                }
                else if (trucktimer == 0)
                {
                    trucktimer = -1;
                    var yes = new EmbedBuilder();
                    yes.WithImageUrl("https://i.imgur.com/iIm2Nsk.png");
                    yes.Title = "The truck is back and ready to pick up the trash!";
                    yes.Color = Color.Green;
                    await (ogChannel as ISocketMessageChannel).SendMessageAsync("", false, yes.Build());
                }
            }
            async Task EverySecondUpdate()
            {
                if (EverySecond != DateTime.Now.Second)
                {
                    EverySecond = DateTime.Now.Second;
                    for (int i = 0; i < TopDown.doors.Count; i++)
                    {
                        TopDown.doors[i].Timer--;
                        if (TopDown.doors[i].Timer == 0)
                        {
                            await TopDown.doors[i].message.Channel.SendMessageAsync($"{Client.GetUser(TopDown.doors[i].ID).Username}, your door command has expired.");
                            TopDown.doors.RemoveAt(i);
                            i--;
                        }
                    }
                    if (RPCcountdown > 0)
                    {
                        RPCcountdown--;
                    }
                    UnSentHandler.Update();
                    for (int i = 0; i < deadpeople.Count; i++)
                    {
                        deadpeople[i].update();
                        if (deadpeople[i].timer <= 0)
                        {
                            deadpeople.RemoveAt(i);
                            i--;
                        }
                    }
                }
            }
            #region Unused stuff
            #region World Resume
            //async Task WorldResume()
            //{
            //    if (Timer == 0 && Messages.Count > 1)
            //    {
            //        string Reply = "so shi te toki wa u go ko da su!".ToUpper();
            //        for (int i = 0; i < Messages.Count; i++)
            //        {
            //            //URL.WithDescription(Messages[i].Author.GetAvatarUrl());
            //            string Name = Messages[i].Author.Username;
            //            string Message = Messages[i].Content;
            //            Reply = $"{Reply}\n{Name}: {Message}";
            //        }
            //        ogMessage.Channel.SendMessageAsync(Reply);
            //        Messages.Clear();
            //    }
            //}
            #endregion
            async void sendtorune()
            {
                string tempmessage = TheMessageNormal.Remove(0, 11);
                while (tempmessage[0] == ' ')
                {
                    tempmessage = tempmessage.Remove(0, 1);
                }
                await Client.GetUser(Rune).SendMessageAsync($"{TheSenderNoString.Username} wanted to tell you: {tempmessage}\n\nIf you want this to stop, type ~killyourself");
            }
            async void connectaudio(ISocketAudioChannel socketAudioChannel, string soundeffect)
            {
                try
                {
                    await musicManager.ConnectToChannel(socketAudioChannel);
                    playsound(audioClient, soundeffect);
                }
                catch
                {
                    Console.WriteLine("I dont know yet");
                }
            }
            async void trashtimestring(string TheMessage, string gears)
            {
                TheMessage = TheMessage.Remove(0, 10);
                string[] values = TheMessage.Split(' ');
                List<ulong> IDs = new List<ulong>();
                foreach (var item in values)
                {
                    IDs.Add(ulong.Parse(item));
                }
                List<ulong> UserIDs = new List<ulong>();
                for (int i = 1; i < IDs.Count; i++)
                {
                    UserIDs.Add(IDs[i]);
                }
                trashtimewithids(IDs.ElementAt(0), UserIDs, gears);
            }
            #region gay trashtime
            //async void trashtime(SocketMessage message, SocketUser user, bool gears)
            //{
            //    if (message.MentionedUsers.Count < 6 && message.MentionedUsers.Count > 0 && trucktimer == -1)
            //    {
            //        int atleast1 = 0;
            //        var vChan = await (user as IGuildUser).Guild.CreateVoiceChannelAsync("Trash Can");
            //        for (int i = 0; i < message.MentionedUsers.Count; i++)
            //        {
            //            var theuser = message.MentionedUsers.ElementAt(i);
            //            try
            //            {
            //                if ((theuser as IGuildUser).VoiceChannel != null)
            //                {
            //                    await (theuser as IGuildUser).ModifyAsync(x =>
            //                    {
            //                        x.ChannelId = vChan.Id;
            //                        temp.trashtaken++;
            //                        atleast1 = 1;
            //                    });
            //                }
            //            }
            //            catch
            //            {

            //            }
            //        }
            //        await vChan.DeleteAsync();
            //        if (atleast1 == 1)
            //        {
            //            var yes = new EmbedBuilder();
            //            yes.WithImageUrl("https://i.imgur.com/Jy9WCb9.png");
            //            if (gears == false)
            //            {
            //                yes.Title = "The trash has been taken out!";
            //            }
            //            else
            //            {
            //                yes.Title = $"Cleaning {message.MentionedUsers.ElementAt(0).Username} off the floor!";
            //            }
            //            var footer = new EmbedFooterBuilder();
            //            footer.Text = $"The trash has been taken out {temp.trashtaken} times.";
            //            yes.Footer = footer;
            //            yes.Color = Color.Blue;
            //            await message.Channel.SendMessageAsync("", false, yes.Build());
            //            trucktimer = 300;
            //            ogTrash = message;
            //            using (StreamWriter r = File.CreateText(@"..\..\trashdata.json"))
            //            {
            //                JsonSerializer s = new JsonSerializer();
            //                s.Serialize(r, temp);
            //            }
            //        }
            //    }
            //    else if (message.MentionedUsers.Count != 0 && trucktimer == -1)
            //    {
            //        await message.Channel.SendMessageAsync("Don't overload my truck.");
            //    }
            //    else if (trucktimer > -1)
            //    {
            //        var yes = new EmbedBuilder();
            //        yes.WithImageUrl("https://i.imgur.com/Iah9DlB.png");
            //        if (trucktimer % 60 > 9)
            //        {
            //            yes.Title = $"The truck is busy right now! It will be back in {trucktimer / 60}:{trucktimer % 60} minutes.";
            //        }
            //        else
            //        {
            //            yes.Title = $"The truck is busy right now! It will be back in {trucktimer / 60}:0{trucktimer % 60} minutes.";
            //        }
            //        yes.Color = Color.Red;
            //        await message.Channel.SendMessageAsync("", false, yes.Build());
            //    }
            //}
            #endregion
            #endregion
            async void trashtimewithids(ulong channelid, List<ulong> userid, string gears)
            {
                try
                {
                    if (!trashing)
                    {
                        trashing = true;
                        SocketChannel textchannel = Client.GetChannel(channelid);
                        bool breakit = false;
                        int atleast1 = 0;
                        if (trucktimer < 1 || gears == "gears" || gears == "RPC" || gears == "killself")
                        {
                            foreach (var guild in Client.Guilds)
                            {
                                foreach (var channel in guild.TextChannels)
                                {
                                    if (channel.Id == channelid)
                                    {
                                        IGuildUser runemastergaming580 = (Client.GetUser(Rune) as IGuildUser);
                                        SocketGuild no = guild;
                                        var thechannel = await no.CreateVoiceChannelAsync("TrashCan");
                                        foreach (var ID in userid)
                                        {
                                            var theuser = guild.GetUser(ID);
                                            Console.WriteLine(theuser);
                                            try
                                            {
                                                if ((theuser as IGuildUser).VoiceChannel != null)
                                                {
                                                    Console.WriteLine($"{theuser.Username} has been dumped in the trash.");
                                                    await (theuser as IGuildUser).ModifyAsync(x =>
                                                    {
                                                        x.ChannelId = thechannel.Id;
                                                        temp.trashtaken++;
                                                        atleast1 = 1;
                                                    });
                                                }
                                                else
                                                {
                                                    Console.WriteLine($"{theuser.Username} was not in a voice channel");
                                                }
                                            }
                                            catch
                                            {
                                                Console.WriteLine("Uh oh spaghetios");
                                            }
                                        }
                                        await thechannel.DeleteAsync();
                                        breakit = true;
                                        break;
                                    }
                                }
                                if (breakit)
                                {
                                    break;
                                }
                            }
                        }
                        else if (trucktimer > -1)
                        {
                            var yes = new EmbedBuilder();
                            yes.WithImageUrl("https://i.imgur.com/Iah9DlB.png");
                            if (trucktimer % 60 > 9)
                            {
                                yes.Title = $"The truck is busy right now! It will be back in {trucktimer / 60}:{trucktimer % 60} minutes.";
                            }
                            else
                            {
                                yes.Title = $"The truck is busy right now! It will be back in {trucktimer / 60}:0{trucktimer % 60} minutes.";
                            }
                            yes.Color = Color.Red;
                            await (textchannel as ISocketMessageChannel).SendMessageAsync("", false, yes.Build());
                        }
                        if (atleast1 == 1)
                        {
                            ogChannel = textchannel;
                            var maybe = new EmbedBuilder();
                            //maybe.WithImageUrl("https://i.imgur.com/Jy9WCb9.png");
                            if (gears == "norm")
                            {
                                maybe.Title = "The trash has been taken out!";
                                trucktimer = 300;
                            }
                            else if (gears == "gears")
                            {
                                StringBuilder sb = new StringBuilder();
                                foreach (var user in userid)
                                {
                                    if (userid.Count > 2)
                                    {
                                        if (user == userid.Last())
                                        {
                                            sb.Append($"{Client.GetUser(user).Username}");
                                        }
                                        else if (user == userid.ElementAt(userid.Count - 2))
                                        {
                                            sb.Append($"{Client.GetUser(user).Username}, and ");
                                        }
                                        else
                                        {
                                            sb.Append($"{Client.GetUser(user).Username}, ");
                                        }
                                    }
                                    else
                                    {
                                        if (user == userid.First())
                                        {
                                            sb.Append($"{Client.GetUser(user).Username}");
                                        }
                                        else
                                        {
                                            sb.Append($" and {Client.GetUser(user).Username}");
                                        }
                                    }
                                }
                                maybe.Title = $"Cleaning {sb.ToString()} off the floor!";
                            }
                            else if (gears == "RPC")
                            {
                                maybe.Title = $"{Client.GetUser(userid.ElementAt(0)).Username} lost. Get em outta here.";
                            }
                            StringBuilder notsb = new StringBuilder();
                            notsb.Append(@"..\..\FinalImages\final");
                            foreach (var id in userid)
                            {
                                notsb.Append(id / 10000000000);
                            }
                            notsb.Append(".png");
                            string fileName = notsb.ToString();
                            if (File.Exists(fileName))
                            {
                                Console.WriteLine("Existing image found!");
                                Console.WriteLine(Path.GetFileName(fileName));
                                maybe.ImageUrl = ($"attachment://{Path.GetFileName(fileName)}");
                            }
                            else
                            {
                                Console.WriteLine("Making new image...");
                                #region image factory
                                int inc = 1;
                                ImageFactory IMF = new ImageFactory();
                                IMF.Load(@"..\..\trash.png");
                                foreach (var user in userid)
                                {
                                    try
                                    {
                                        var yes = Client.GetUser(user).GetAvatarUrl();
                                        System.Drawing.Image image = DownloadImageFromUrl(yes.Trim());
                                        image.Save($@"..\..\{user}image.png");
                                        ImageLayer IMF2 = new ImageLayer();
                                        IMF2.Image = System.Drawing.Image.FromFile($@"..\..\{user}image.png");
                                        System.Drawing.Size size = new System.Drawing.Size();
                                        System.Drawing.Point point = new System.Drawing.Point();
                                        if (userid.Count < 7)
                                        {
                                            if (user == userid.ElementAt(0) || user == userid.ElementAt(1) || user == userid.ElementAt(2) || user == userid.ElementAt(3))
                                            {
                                                size.Height = 37;
                                                size.Width = 37;
                                                point.X = 10 + (40 * inc);
                                                point.Y = 140;
                                            }
                                            else if (user == userid.ElementAt(4))
                                            {
                                                size.Height = 37;
                                                size.Width = 37;
                                                point.X = 50;
                                                point.Y = 179;
                                            }
                                            else
                                            {
                                                size.Height = 37;
                                                size.Width = 37;
                                                point.X = 150;
                                                point.Y = 179;
                                            }
                                        }
                                        else
                                        {
                                            size.Height = 100 / userid.Count;
                                            size.Width = 100 / userid.Count;
                                            point.X = (50 + ((100 / userid.Count) * inc));
                                            point.Y = 140;
                                        }
                                        IMF2.Size = size;
                                        IMF2.Position = point;
                                        inc++;
                                        IMF.Overlay(IMF2);
                                    }
                                    catch
                                    {

                                    }
                                }
                                #endregion
                                IMF.Save(fileName);
                                maybe.ImageUrl = ($"attachment://{Path.GetFileName(fileName)}");
                            }
                            try
                            {
                                Console.WriteLine(maybe.ImageUrl);
                                var footer = new EmbedFooterBuilder();
                                footer.Text = $"The trash has been taken out {temp.trashtaken} times.";
                                maybe.Footer = footer;
                                maybe.Color = Color.Blue;
                                Embed possibly = maybe.Build();
                                await (textchannel as ISocketMessageChannel).SendFileAsync(fileName, embed: possibly);
                                using (StreamWriter r = File.CreateText(@"..\..\trashdata.json"))
                                {
                                    Newtonsoft.Json.JsonSerializer s = new Newtonsoft.Json.JsonSerializer();
                                    s.Serialize(r, temp);
                                }
                            }
                            catch
                            {
                            }
                        }
                        trashing = false;
                    }
                }
                catch
                {
                }


            }
            string downloadurl(string url)
            {
                int VideoNumber = 1;
                for (int i = 0; i < url.Count(); i++)
                {
                    VideoNumber += (url[i] * (i * 10001));
                }
                Process process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.FileName = @"..\ffmpeg\bin\youtube-dl.exe";
                startInfo.Arguments = $@"-f bestaudio -o {VideoNumber}.wav {url}";
                startInfo.WorkingDirectory = @"..\..\YoutubeDownload";
                process.StartInfo = startInfo;
                Console.WriteLine("Starting download...");
                process.Start();
                process.WaitForExit();
                process.Close();
                Console.WriteLine($"Probably completed download of {url}");
                startInfo.FileName = @"..\ffmpeg\bin\ffmpeg.exe";
                startInfo.Arguments = $@"-i {VideoNumber}.wav -qscale 0 {VideoNumber}.mp3";
                process.StartInfo = startInfo;
                Console.WriteLine($"Starting conversion...");
                process.Start();
                process.WaitForExit();
                process.Close();
                Console.WriteLine($"Probably completed conversion of {url}");
                startInfo.FileName = @"..\ffmpeg\bin\mp3gain.exe";
                startInfo.Arguments = $@"-r -c {VideoNumber}.mp3";
                process.StartInfo = startInfo;
                Console.WriteLine($"Starting normalization...");
                process.Start();
                Console.WriteLine($"Waiting for exit...");
                process.WaitForExit();
                Console.WriteLine($"Closing...");
                process.Close();
                Console.WriteLine($"Probably completed normilization of {url}");
                return url;
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
            System.Drawing.Image DownloadImageFromUrl(string imageUrl)
            {
                System.Drawing.Image image = null;

                try
                {
                    System.Net.HttpWebRequest webRequest = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(imageUrl);
                    webRequest.AllowWriteStreamBuffering = true;
                    webRequest.Timeout = 30000;

                    System.Net.WebResponse webResponse = webRequest.GetResponse();

                    System.IO.Stream stream = webResponse.GetResponseStream();

                    image = System.Drawing.Image.FromStream(stream);

                    webResponse.Close();
                }
                catch (Exception ex)
                {
                    return null;
                }

                return image;
            }
            async void playsound(IAudioClient audio, string thesound)
            {
                using (System.IO.Stream output = CreateStream(thesound).StandardOutput.BaseStream)
                using (AudioOutStream stream = audio.CreatePCMStream(AudioApplication.Music, 128 * 1024))
                {
                    try
                    {
                        await output.CopyToAsync(stream);
                        await stream.FlushAsync().ConfigureAwait(false);
                        Console.WriteLine("Finished Audio");
                        musicManager.Leave();
                    }
                    catch
                    {
                        Console.WriteLine("big gay");
                    }
                }
            }
            async void PlaySong(string URL, SocketUser sender, bool _Add = false)
            {
                try
                {
                    string song = "";
                    if (URL.ToUpper().Contains('/') && URL.ToUpper().Contains(".COM"))
                    {
                        while (URL[0] == ' ')
                        {
                            URL = URL.Remove(0, 1);
                        }
                        while (URL[0] != ' ')
                        {
                            URL = URL.Remove(0, 1);
                        }
                        while (URL[0] == ' ')
                        {
                            URL = URL.Remove(0, 1);
                        }


                        if (URL.ToUpper().Contains("YOUTUBE"))
                        {
                            while (URL.Contains('&'))
                            {
                                URL = URL.Remove(URL.Count() - 1, 1);
                            }
                        }
                        else if (URL.ToUpper().Contains("SOUNDCLOUD"))
                        {
                        }


                    }
                    else
                    {
                        VideoSearch Video = new VideoSearch();
                        URL = Video.SearchQuery(URL, 1)[0].Url;
                        while (URL.Contains('&'))
                        {
                            URL = URL.Remove(URL.Count() - 1, 1);
                        }
                    }
                    int VideoNumber = 1;
                    for (int i = 0; i < URL.Count(); i++)
                    {
                        VideoNumber += (URL[i] * (i * 10001));
                    }

                    song = $@"..\..\YoutubeDownload\{VideoNumber}.mp3";

                    Console.WriteLine(song);
                    if (File.Exists(song))
                    {
                        await musicManager.ConnectToChannel(((sender as IGuildUser).VoiceChannel as SocketVoiceChannel));
                        if (_Add)
                        {
                            musicManager.AddToStream(song);
                        }
                        else
                        {
                            musicManager.Enqueue(song);
                        }
                    }
                    else
                    {
                        downloadurl(URL);
                        if (File.Exists(song))
                        {
                            await musicManager.ConnectToChannel(((sender as IGuildUser).VoiceChannel as SocketVoiceChannel));
                        }
                        if (_Add)
                        {
                            musicManager.AddToStream(song);
                        }
                        else
                        {
                            musicManager.Enqueue(song);
                        }
                    }
                }
                catch
                {
                }
            }
            async void RedditBrowse(SocketMessage ChannelMessage, string MessageContent)
            {
                try
                {
                    MessageContent = MessageContent.Remove(0, 2);
                    while (MessageContent[0] == ' ')
                    {
                        MessageContent = MessageContent.Remove(0, 1);
                    }
                    Console.WriteLine($"Querying /r/{MessageContent}");
                    Reddit reddit = new Reddit();
                    var HotPage = reddit.GetSubreddit($"/r/{MessageContent}").Hot;
                    Random rand = new Random();
                    int RandInt = rand.Next(30);
                    EmbedBuilder eb = new EmbedBuilder();
                    EmbedFooterBuilder footer = new EmbedFooterBuilder();
                    bool UsedURL = false;
                    string TheUrl = "";
                    string TheSubreddit = "";
                    int FailSafeBecauseIWantToDie = 10;
                    foreach (var item in HotPage)
                    {
                        if (RandInt == 0)
                        {
                            foreach (var thing in UsedURLs)
                            {
                                if (thing.URL == item.Url.ToString())
                                {
                                    UsedURL = true;
                                    break;
                                }
                            }
                            Console.WriteLine($"Using: {item.Url.ToString()}");
                            FailSafeBecauseIWantToDie--;
                            if (FailSafeBecauseIWantToDie == 0)
                            {
                                break;
                            }
                            if (!UsedURL)
                            {
                                string TheComment = "";
                                foreach (var comment in item.Comments)
                                {
                                    if (TheComment.Count() < 2000 && !comment.IsStickied)
                                    {
                                        TheComment = comment.Body;
                                        footer.Text = TheComment;
                                        eb.Footer = footer;
                                        break;
                                    }
                                }
                                StringBuilder sb = new StringBuilder();
                                sb.Append("<a:blank:530985976542003200>\n");
                                if (item.Url.ToString().Contains("youtu.be") || item.Url.ToString().Contains("youtube") || item.Url.ToString().Contains("gfycat") || item.NSFW || item.Url.ToString().Contains(".gifv") || item.Url.ToString().Contains("redd.it"))
                                {
                                    if (item.NSFW)
                                    {
                                        sb.Append($"__**{item.Title.ToString()}**__ (MARKED AS NSFW)\n||{item.Url.ToString()}||");
                                    }
                                    else
                                    {
                                        sb.Append($"__**{item.Title.ToString()}**__\n{item.Url.ToString()}");
                                    }
                                    if (sb.ToString().Count() < 2000)
                                    {
                                        await ChannelMessage.Channel.SendMessageAsync(sb.ToString());
                                        if (TheComment.Count() != 0)
                                            await ChannelMessage.Channel.SendMessageAsync($"*{TheComment}*");
                                    }
                                    TheUrl = item.Url.ToString();
                                    TheSubreddit = item.SubredditName;
                                    break;
                                }
                                else if (item.Url.ToString().Contains("v.redd.it"))
                                {
                                    string _TempString = item.Url.ToString();
                                    while (_TempString.Contains('/'))
                                    {
                                        _TempString = _TempString.Remove(0, 1);
                                    }
                                    if (item.NSFW)
                                    {
                                        sb.Append($"__**{item.Title.ToString()}**__ (MARKED AS NSFW)\n||https://vredd.it/files/{_TempString}-{_TempString}.mp4||");
                                    }
                                    else
                                    {
                                        sb.Append($"__**{item.Title.ToString()}**__\nhttps://vredd.it/files/{_TempString}-{_TempString}.mp4");
                                    }
                                    if (sb.ToString().Count() < 2000)
                                    {
                                        await ChannelMessage.Channel.SendMessageAsync(sb.ToString());
                                        if (TheComment.Count() != 0)
                                            await ChannelMessage.Channel.SendMessageAsync($"*{TheComment}*");
                                    }
                                    TheUrl = item.Url.ToString();
                                    TheSubreddit = item.SubredditName;
                                    break;
                                }
                                else if (item.IsSelfPost)
                                {
                                    sb.Append($"__**{item.Title.ToString()}**__");
                                    if (item.NSFW)
                                    {
                                        sb.Append("(MARKED AS NSFW)\n||");
                                    }
                                    else
                                    {
                                        sb.Append('\n');
                                    }
                                    if (item.SelfText.Count() != 0)
                                    {
                                        sb.Append($"**{item.SelfText}**\n");
                                    }
                                    if (item.NSFW)
                                    {
                                        sb.Append("||");
                                    }
                                    if (sb.ToString().Count() < 2000)
                                    {
                                        await ChannelMessage.Channel.SendMessageAsync(sb.ToString());
                                        if (TheComment.Count() != 0)
                                            await ChannelMessage.Channel.SendMessageAsync($"*{TheComment}*");
                                    }
                                    TheUrl = item.Url.ToString();
                                    TheSubreddit = item.SubredditName;
                                    break;
                                }
                                else
                                {
                                    sb.Append($"__**{item.Title.ToString()}**__");
                                    if (item.NSFW)
                                    {
                                        sb.Append("(MARKED AS NSFW)\n||");
                                    }
                                    else
                                    {
                                        sb.Append('\n');
                                    }
                                    sb.Append($"**{item.Url}**\n");
                                    if (footer.Text.Count() != 0)
                                    {
                                        sb.Append($"*{footer.Text}*");
                                    }
                                    if (item.NSFW)
                                    {
                                        sb.Append("||");
                                    }
                                    if (sb.ToString().Count() < 2000)
                                    {
                                        await ChannelMessage.Channel.SendMessageAsync(sb.ToString());
                                    }
                                    TheUrl = item.Url.ToString();
                                    TheSubreddit = item.SubredditName;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            RandInt--;
                        }
                    }
                    if (FailSafeBecauseIWantToDie == 0)
                    {
                        Console.WriteLine("Ah fuck, I can't believe I've done this");
                        await ChannelMessage.Channel.SendMessageAsync($"ごめん なさい");
                    }
                    #region usedurl
                    RedditUrl _Temp = new RedditUrl();
                    _Temp.Timer = 60 * 12;
                    _Temp.URL = TheUrl;
                    _Temp.Subreddit = TheSubreddit;
                    UsedURLs.Add(_Temp);
                    File.WriteAllText(@"..\..\RedditURLS.txt", string.Empty);
                    using (StreamWriter w = new StreamWriter(@"..\..\RedditURLS.txt"))
                    {
                        foreach (var item in UsedURLs)
                        {
                            if (item == UsedURLs.Last())
                            {
                                w.Write($"{item.URL}\t{item.Timer}\t{item.Subreddit}");
                                break;
                            }
                            w.Write($"{item.URL}\t{item.Timer}\t{item.Subreddit}\n");
                        }
                    }
                    #endregion
                }
                catch
                {
                }
            }
            async void PostedRedditLink(SocketMessage ChannelMessage, string MessageContent)
            {
                try
                {
                    while (!MessageContent.StartsWith("https://www.reddit.com/r/"))
                    {
                        MessageContent = MessageContent.Remove(0, 1);
                    }
                    while (MessageContent.EndsWith(" "))
                    {
                        MessageContent = MessageContent.Remove(MessageContent.Count() - 1, 1);
                    }
                    if (!MessageContent.Contains(' '))
                    {
                        MessageContent = $"{MessageContent} ligma";
                    }
                    string URL = MessageContent.Split(' ')[0];
                    while (URL.Split('/').Length > "https://www.reddit.com/r/".Split('/').Length + 3)
                    {
                        URL = URL.Remove(URL.Count() - 1, 1);
                    }
                    Reddit reddit = new Reddit();
                    Uri uri = new Uri(URL);
                    var post = reddit.GetPost(uri);
                    string TheComment = "";
                    foreach (var comment in post.Comments)
                    {
                        if (!comment.IsStickied && TheComment.Count() < 2000)
                        {
                            TheComment = comment.Body;
                            break;
                        }
                    }
                    await ChannelMessage.Channel.SendMessageAsync(TheComment);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Reddit machine broke: {e.Message}");
                }
            }
            string SeriousText(string text)
            {
                bool caps = true;
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < text.Length; i++)
                {
                    if (text[i] != ' ')
                    {
                        if (caps)
                        {
                            sb.Append($"{text[i]}".ToUpper());
                        }
                        else
                        {
                            sb.Append($"{text[i]}".ToLower());
                        }
                        caps = !caps;
                    }
                    else
                    {
                        sb.Append(' ');
                    }
                }
                return sb.ToString();
            }
            async void CreateReminder(int years, int months, int days, int hours, int minutes, int seconds, ulong ID, ISocketMessageChannel c, string m, bool daily, bool weekly, bool monthly, bool yearly)
            {
                RemindMe reminder = new RemindMe(years, months, days, hours, minutes, seconds, ID, c.Id, m, daily, weekly, monthly, yearly);
                reminders.Add(reminder);
                SaveReminders();
            }
            async void UpdateReminders()
            {
                try
                {
                    int secondC = 0;
                    if (csecond != DateTime.Now.Second)
                    {
                        if (DateTime.Now.Second < csecond)
                        {
                            secondC = DateTime.Now.Second - csecond + 60;
                        }
                        else
                        {
                            secondC = DateTime.Now.Second - csecond;
                        }
                        csecond = DateTime.Now.Second;
                    }
                    for (int x = 0; x < reminders.Count; x++)
                    {
                        for (int i = 0; i < secondC; i++)
                        {
                            reminders[x].Update(1);
                        }
                        if (reminders[x].UseIt)
                        {
                            StringBuilder s = new StringBuilder();
                            s.Append($"<@{reminders[x].ID}>\n");
                            s.Append(reminders[x].message);
                            (Client.GetChannel(reminders[x].channel) as ISocketMessageChannel).SendMessageAsync(s.ToString());
                            reminders.RemoveAt(x);
                            x--;
                            SaveReminders();
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
            async void SaveReminders()
            {
                using (Stream r = new FileStream(@"..\..\reminders.sugondeez", FileMode.Create, FileAccess.Write))
                {
                    f.Serialize(r, reminders);
                }
            }
            async void SaveTheGalaxy()
            {
                galaxywords.UpdateWords();
                using (Stream r = new FileStream(@"..\..\galaxy.words", FileMode.Create, FileAccess.Write))
                {
                    f2.Serialize(r, galaxywords);
                }
            }
            async void SaveDaddy()
            {
                using (Stream r = new FileStream(@"..\..\daddydaddy.doo", FileMode.Create, FileAccess.Write))
                {
                    f2.Serialize(r, nodads);
                }
            }
            string GetIm(SocketMessage m)
            {
                List<string> s = m.Content.Split(' ').ToList<string>();
                while (s[0].ToUpper() != "IM" && s[0].ToUpper() != "I'M")
                {
                    s.RemoveAt(0);
                }
                s.RemoveAt(0);
                if (s.Count != 0)
                {
                    StringBuilder sb = new StringBuilder();
                    if (s.Count == 1 && s[0].ToUpper() == "DAD")
                    {
                        sb.Append("You're not dad, I'm dad!");
                        return sb.ToString();
                    }
                    sb.Append("Hi");
                    foreach (var item in s)
                    {
                        sb.Append($" {item}");
                    }
                    Random rand = new Random();
                    int num = rand.Next() % 100;
                    if (num == 0)
                    {
                        sb.Clear();
                        sb.Append("You're not");
                        foreach (var item in s)
                        {
                            sb.Append($" {item}");
                        }
                        sb.Append(", You're a dissapointment!");
                    }
                    else if (num == 1)
                    {
                        sb.Append(", I'm gay!");
                    }
                    else if (num == 2)
                    {
                        sb.Append(", I'm critically depressed!");
                    }
                    else
                    {
                        sb.Append(", I'm dad!");
                    }
                    return sb.ToString();
                }
                return "";
            }
            void RemoveWhiteSpace(ref string yeetem)
            {
                if (yeetem.Length == 0)
                {
                    return;
                }
                while (yeetem[0] == ' ')
                {
                    yeetem = yeetem.Remove(0, 1);
                    if (yeetem.Length == 0)
                    {
                        return;
                    }
                }
            }
            void RemoveFilledSpace(ref string yeetem)
            {
                if (yeetem.Length == 0)
                {
                    return;
                }
                while (yeetem[0] != ' ')
                {
                    yeetem = yeetem.Remove(0, 1);
                    if (yeetem.Length == 0)
                    {
                        return;
                    }
                }
            }
            bool NumberStuff(ref int tempnum, ref StringBuilder stronks, ref string yeetem, ref int mo, ref int y, ref int mi, ref int d, ref int h, ref int s, bool nospace, ref bool daily, ref bool weekly, ref bool monthly, ref bool yearly)
            {
                while (true)
                {
                    stronks.Clear();
                    while (yeetem[0] != ' ' && !nospace)
                    {
                        stronks.Append(yeetem[0]);
                        yeetem = yeetem.Remove(0, 1);
                    }
                    if (nospace)
                    {
                        stronks.Append(yeetem);
                    }
                    while (stronks.ToString().ToUpper() == "IN" || stronks.ToString().ToUpper() == "AND" || stronks.ToString().ToUpper() == "AFTER")
                    {
                        stronks.Clear();
                        RemoveWhiteSpace(ref yeetem);
                        while (yeetem[0] != ' ')
                        {
                            stronks.Append(yeetem[0]);
                            yeetem = yeetem.Remove(0, 1);
                        }
                    }
                    bool now = false;
                    Random r = new Random();
                    switch (stronks.ToString().ToUpper())
                    {
                        case "NEXT":
                            tempnum = 1;
                            break;
                        case "A":
                            if (yeetem.Length >= 4)
                            {
                                if (yeetem.ToUpper()[1] == 'C' && yeetem.ToUpper()[2] == 'O' && yeetem.ToUpper()[3] == 'U')
                                {
                                    tempnum = 2;
                                    RemoveWhiteSpace(ref yeetem);
                                    RemoveFilledSpace(ref yeetem);
                                    RemoveWhiteSpace(ref yeetem);
                                    if (yeetem.ToUpper()[0] == 'O' && yeetem.ToUpper()[1] == 'F' && yeetem.ToUpper()[2] == ' ')
                                    {
                                        RemoveFilledSpace(ref yeetem);
                                        RemoveWhiteSpace(ref yeetem);
                                    }
                                    break;
                                }
                                else if (yeetem.ToUpper()[1] == 'F' && yeetem.ToUpper()[2] == 'E' && yeetem.ToUpper()[3] == 'W')
                                {
                                    tempnum = 3;
                                    RemoveWhiteSpace(ref yeetem);
                                    RemoveFilledSpace(ref yeetem);
                                    break;
                                }
                            }
                            tempnum = 1;
                            break;
                        case "AN":
                            tempnum = 1;
                            break;
                        case "ONE":
                            tempnum = 1;
                            break;
                        case "TWO":
                            tempnum = 2;
                            break;
                        case "THREE":
                            tempnum = 3;
                            break;
                        case "FOUR":
                            tempnum = 4;
                            break;
                        case "FIVE":
                            tempnum = 5;
                            break;
                        case "SIX":
                            tempnum = 6;
                            break;
                        case "SEVEN":
                            tempnum = 7;
                            break;
                        case "EIGHT":
                            tempnum = 8;
                            break;
                        case "NINE":
                            tempnum = 9;
                            break;
                        case "TEN":
                            tempnum = 10;
                            break;
                        case "ELEVEN":
                            tempnum = 11;
                            break;
                        case "TWELVE":
                            tempnum = 12;
                            break;
                        case "THIRTEEN":
                            tempnum = 13;
                            break;
                        case "FOURTEEN":
                            tempnum = 14;
                            break;
                        case "FIFTEEN":
                            tempnum = 15;
                            break;
                        case "SIXTEEN":
                            tempnum = 16;
                            break;
                        case "SEVENTEEN":
                            tempnum = 17;
                            break;
                        case "EIGHTEEN":
                            tempnum = 18;
                            break;
                        case "NINETEEN":
                            tempnum = 19;
                            break;
                        case "TWENTY":
                            tempnum = 20;
                            break;
                        case "NOW":
                            now = true;
                            break;
                        case "YESTERDAY":
                            now = true;
                            break;
                        case "FEW":
                            tempnum = 3;
                            break;
                        case "COUPLE":
                            tempnum = 2;
                            break;
                        case "TOMMOROW":
                            now = true;
                            d = 1;
                            break;
                        case "LATER":
                            now = true;
                            d = r.Next() % 2;
                            h = r.Next() % 24;
                            mi = r.Next() % 60;
                            s = r.Next() % 60;
                            break;
                        case "EVENTUALLY":
                            now = true;
                            d = r.Next() % 14;
                            h = r.Next() % 24;
                            mi = r.Next() % 60;
                            s = r.Next() % 60;
                            break;
                        case "SOMETIME":
                            now = true;
                            h = r.Next() % 24;
                            mi = r.Next() % 60;
                            s = r.Next() % 60;
                            break;
                        case "SOON":
                            now = true;
                            h = r.Next() % 4;
                            mi = r.Next() % 60;
                            s = r.Next() % 60;
                            break;
                        case "TODAY":
                            now = true;
                            h = (r.Next() % (24 - DateTime.Now.Hour)) - 1;
                            mi = (r.Next() % (60 - DateTime.Now.Minute)) - 1;
                            s = (r.Next() % (60 - DateTime.Now.Minute)) - 1;
                            break;
                        case "DAILY":
                            now = true;
                            daily = true;
                            d = 1;
                            break;
                        case "WEEKLY":
                            now = true;
                            weekly = true;
                            d = 7;
                            break;
                        case "MONTHLY":
                            now = true;
                            monthly = true;
                            mo = 1;
                            break;
                        case "YEARLY":
                            now = true;
                            yearly = true;
                            y = 1;
                            break;
                        case "EVERY":
                            if (yeetem.Length >= 4)
                            {
                                bool one = false;
                                if (yeetem.ToUpper()[1] == 'D' && yeetem.ToUpper()[2] == 'A' && yeetem.ToUpper()[3] == 'Y')
                                {
                                    now = true;
                                    daily = true;
                                    d = 1;
                                    one = true;
                                }
                                else if (yeetem.ToUpper()[1] == 'W' && yeetem.ToUpper()[2] == 'E' && yeetem.ToUpper()[3] == 'E')
                                {
                                    now = true;
                                    weekly = true;
                                    d = 7;
                                    one = true;
                                }
                                else if (yeetem.ToUpper()[1] == 'M' && yeetem.ToUpper()[2] == 'O' && yeetem.ToUpper()[3] == 'N')
                                {
                                    now = true;
                                    monthly = true;
                                    mo = 1;
                                    one = true;
                                }
                                else if (yeetem.ToUpper()[1] == 'Y' && yeetem.ToUpper()[2] == 'E' && yeetem.ToUpper()[3] == 'A')
                                {
                                    now = true;
                                    yearly = true;
                                    y = 1;
                                    one = true;
                                }
                                if (one)
                                {
                                    RemoveWhiteSpace(ref yeetem);
                                    RemoveFilledSpace(ref yeetem);
                                    break;
                                }
                            }
                            break;
                        default:
                            int.TryParse(stronks.ToString(), out tempnum);
                            break;
                    }
                    stronks.Clear();
                    while (yeetem[0] == ' ')
                    {
                        yeetem = yeetem.Remove(0, 1);
                    }
                    if (!now)
                    {
                        switch (yeetem.ToUpper()[0])
                        {
                            case 'Y':
                                y = tempnum;
                                break;
                            case 'M':
                                if (yeetem.ToUpper()[1] == 'O')
                                    mo = tempnum;
                                else if (yeetem.ToUpper()[1] == 'I')
                                    mi = tempnum;
                                break;
                            case 'D':
                                if (yeetem.Length < 6)
                                {
                                    d = tempnum;
                                }
                                else if (yeetem.ToUpper().Remove(6, yeetem.Length - 6) == "DECADE" || yeetem.ToUpper().Remove(7, yeetem.Length - 7) == "DECADES")
                                {
                                    y = tempnum * 10;
                                }
                                else
                                {
                                    d = tempnum;
                                }
                                break;
                            case 'H':
                                h = tempnum;
                                break;
                            case 'S':
                                s = tempnum;
                                break;
                            case 'C':
                                y = tempnum * 100;
                                break;
                            case 'W':
                                d = tempnum * 7;
                                break;
                            default:
                                break;
                        }
                    }
                    if (yeetem.Length == 0)
                    {
                        return now;
                    }
                    while (yeetem[0] != ' ')
                    {
                        yeetem = yeetem.Remove(0, 1);
                        if (yeetem.Length == 0)
                        {
                            break;
                        }
                    }
                    if (yeetem.Length == 0)
                    {
                        return now;
                    }
                    while (yeetem[0] == ' ')
                    {
                        yeetem = yeetem.Remove(0, 1);
                    }
                    if (yeetem.Length == 0)
                    {
                        return now;
                    }
                    if ((yeetem.ToUpper()[0] == 'T' && yeetem.ToUpper()[1] == 'O') || yeetem[0] != '^')
                    {
                        return now;
                    }
                }
            }
            List<string> NumberStuffBetter(string[] numbers, ref int mo, ref int y, ref int mi, ref int d, ref int h, ref int s, ref bool daily, ref bool weekly, ref bool monthly, ref bool yearly)
            {
                List<string> badwords = new List<string>();
                bool expectnum = true;
                int num = 0;
                int num2 = 0;
                for (int i = 0; i < numbers.Length; i++)
                {
                    if (expectnum)
                    {
                        expectnum = !Int32.TryParse(numbers[i], out num);
                    }
                    else
                    {
                        switch (numbers[i].ToLower())
                        {
                            case "years":
                            case "year":
                                y += num;
                                expectnum = true;
                                break;
                            case "months":
                            case "month":
                                mo += num;
                                expectnum = true;
                                break;
                            case "days":
                            case "day":
                                d += num;
                                expectnum = true;
                                break;
                            case "hours":
                            case "hour":
                                h += num;
                                expectnum = true;
                                break;
                            case "minutes":
                            case "minute":
                                mi += num;
                                expectnum = true;
                                break;
                            case "seconds":
                            case "second":
                                s += num;
                                expectnum = true;
                                break;
                            case "n/a":
                                break;
                            //commented out until solution is found, no one used these anyway, like litterally no one except me one time
                            //case "daily":
                            //    daily = true;
                            //    break;
                            //case "weekly":
                            //    weekly = true;
                            //    break;
                            //case "monthly":
                            //    monthly = true;
                            //    break;
                            //case "yearly":
                            //    yearly = true;
                            //    break;
                            default:
                                if (Int32.TryParse(numbers[i], out num2))
                                {
                                    num *= num2;
                                }
                                else
                                {
                                    badwords.Add(numbers[i]);
                                }
                                break;
                        }

                    }
                }
                return badwords;
            }
            void SaveAlias()
            {
                using (Stream r = new FileStream(@"..\..\aliases.yourmother", FileMode.Create, FileAccess.Write))
                {
                    f2.Serialize(r, manager);
                }
            }
            do
            {
                UpdateReminders();
                if (DateTime.Now.Second == 0 && (every3seconds == 59 || every3seconds == 58 || every3seconds == 57 || every3seconds == 56))
                {
                    every3seconds -= 60;
                }
                else if (every3seconds < (DateTime.Now.Second - 4))
                {
                    every3seconds = DateTime.Now.Second;
                }
                if (RPCcountdown == 0)
                {
                    if (RPCphasemom == 1)
                    {
                        RPCcountdown = -1;
                        RPCphasemom = 0;
                        RPCchannel.SendMessageAsync("Guess not...");
                    }
                    else if (RPCphasemom == 2)
                    {
                        EmbedBuilder eb = new EmbedBuilder();
                        StringBuilder sb = new StringBuilder();
                        Random ran = new Random();
                        int choice = ran.Next(1, 4);
                        if (choice == 1)
                        {
                            sb.Append($"I'll take that as scissors.\n");
                            RPCchoice1 = "SCISSORS";
                            eb.ImageUrl = "https://i.imgur.com/7zpEuOD.png";
                            eb.Color = Color.DarkRed;
                        }
                        else if (choice == 2)
                        {
                            sb.Append($"I'll take that as rock.\n");
                            RPCchoice1 = "ROCK";
                            eb.ImageUrl = "https://i.imgur.com/3PJopPq.jpg";
                            eb.Color = Color.DarkGreen;
                        }
                        else
                        {
                            sb.Append($"I'll take that as paper.\n");
                            RPCchoice1 = "PAPER";
                            eb.ImageUrl = "https://i.imgur.com/H1An3MQ.png";
                            eb.Color = Color.DarkBlue;
                        }
                        eb.Title = $"{RPC1.Username}, {RPC2.Username} has picked {RPCchoice1}.";
                        EmbedFooterBuilder ft = new EmbedFooterBuilder();
                        ft.Text = "What will you use to counter it?";
                        eb.Footer = ft;
                        RPCchannel.SendMessageAsync(sb.ToString(), false, eb.Build());
                        RPCphasemom = 3;
                        RPCcountdown = 10;
                    }
                    else if (RPCphasemom == 3)
                    {
                        RPCchannel.SendMessageAsync($"You can't just not choose one. {RPC2.Username} wins!");
                        RPCcountdown = -1;
                        RPCphasemom = 0;
                        List<ulong> IDof1 = new List<ulong>();
                        IDof1.Add(RPC1.Id);
                        trashtimewithids(RPCchannel.Id, IDof1, "RPC");
                    }
                }
                EverySecondUpdate();
                UpdateStuff();
                if (DateTime.Now.Minute != CurrentMinute)
                {
                    CurrentMinute = DateTime.Now.Minute;
                    for (int i = 0; i < UsedURLs.Count; i++)
                    {
                        UsedURLs[i].Timer -= 1;
                        if (UsedURLs[i].Timer == 0)
                        {
                            UsedURLs.RemoveAt(i);
                            i--;
                        }
                    }
                }
                if (CurrentHour != DateTime.Now.Hour)
                {
                    SaveTheGalaxy();
                    TopDown.SavePlayers();
                    CurrentHour = DateTime.Now.Hour;
                }
                if (DateTime.Now.Month == 9 && DateTime.Now.Day == 11 && One911)
                {
                    One911 = false;
                    (Client.GetChannel(269898750560829443) as ISocketMessageChannel).SendMessageAsync("Happy 9/11!!! https://gfycat.com/ImpracticalScholarlyHalibut");
                }
            } while (true);
        }
    }
}