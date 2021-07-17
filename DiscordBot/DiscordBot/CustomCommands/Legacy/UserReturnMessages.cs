using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Webhook;
using Discord.WebSocket;
using Image = Discord.Image;

namespace DiscordBot.CustomCommands.Legacy
{
    /// <summary>
    /// Custom Commands class
    /// </summary>
    public class UserReturnMessages
    {
        DiscordSocketClient Client;
        public List<ReturnCommands> commands = new List<ReturnCommands>();
        public List<Owners> owners = new List<Owners>();
        public UserReturnMessages(DiscordSocketClient _Client)
        {
            Client = _Client;
            LoadCommands();
            LoadOwners();
        }
        public bool AddCheck(SocketMessage message, string _Trigger, string[] _Return, bool _Contains, List<ulong> asUserID)
        {
            for (int i = 0; i <= commands.Count; i++)
            {
                if (i == commands.Count)
                {
                    AddCommand(_Trigger, _Return, _Contains, asUserID);
                    return true;
                }
                else if (commands[i].Trigger.ToUpper() == _Trigger.ToUpper())
                {
                    message.Channel.SendMessageAsync($"The trigger [{_Trigger}] already exists. \nIf you want to replace it with your new command type [OVERRIDE]\nIf you want to append the existing command type [APPEND]\nType anything else and this will cancel the command addition.");
                    return false;
                }
            }
            return false;
        }
        public void AddCommand(string _Trigger, string[] _Return, bool _Contains, List<ulong> _asPerson)
        {
            ReturnCommands temp = new ReturnCommands(_Trigger, _Return, _Contains, _asPerson);
            for (int i = 0; i <= commands.Count; i++)
            {
                if (i == commands.Count)
                {
                    commands.Add(temp);
                    break;
                }
                else if (commands[i].Trigger.ToUpper() == _Trigger.ToUpper())
                {
                    commands[i].Trigger = _Trigger;
                    commands[i].Return = _Return;
                    break;
                }
            }
        }
        public bool RemoveCommand(string _Trigger)
        {
            for (int i = 0; i < commands.Count; i++)
            {
                if (commands[i].Trigger.ToUpper() == _Trigger.ToUpper())
                {
                    commands.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }
        public void LoadCommands()
        {
            string line;
            string _Trigger = "";
            string[] _Return;
            ulong _Owner = 69;
            try
            {
                using (StreamReader r = new StreamReader(@"..\..\UserCommands.txt"))
                {
                    line = r.ReadToEnd();
                }
                string[] lines = line.Split('ª');
                for (int i = 0; i < lines.Count(); i++)
                {
                    if (i % 4 == 0)
                    {
                        _Trigger = lines[i];
                    }
                    else if (i % 4 == 1)
                    {
                        bool _Contains = false;
                        _Return = lines[i].Split('∰');
                        List<string> TempList = new List<string>();
                        foreach (var returnvalue in _Return)
                        {
                            if (returnvalue != "")
                            {
                                if (_Return.First() == returnvalue)
                                {
                                    if (returnvalue.ToUpper() != "TRUE" && returnvalue.ToUpper() != "FALSE")
                                    {
                                        TempList.Add(returnvalue);
                                    }
                                    else if (returnvalue.ToUpper() == "TRUE")
                                    {
                                        _Contains = true;
                                    }
                                }
                                else
                                {
                                    TempList.Add(returnvalue);
                                }
                            }
                        }
                        List<ulong> yeet = new List<ulong>();
                        yeet.Add(69);
                        AddCommand(_Trigger, TempList.ToArray(), _Contains, yeet);
                    }
                    else if (i % 4 == 2)
                    {
                        _Owner = ulong.Parse(lines[i]);
                        ClaimCommand(_Trigger, _Owner);
                    }
                    else if (i % 4 == 3)
                    {
                        string[] yeet = lines[i].Split('∰');
                        List<ulong> yeet2 = new List<ulong>();
                        foreach (var item in yeet)
                        {
                            if (item != "")
                            {
                                yeet2.Add(ulong.Parse(item));
                            }
                        }
                        ToggleAsMe(_Trigger, yeet2, false);
                    }
                }
            }
            catch
            {
            }
        }
        public void SaveCommands()
        {
            try
            {
                File.WriteAllText(@"..\..\UserCommands.txt", string.Empty);
                using (StreamWriter r = File.CreateText(@"..\..\UserCommands.txt"))
                {
                    foreach (var item in commands)
                    {
                        r.Write($"{item.Trigger}ª{item.Contains}∰");
                        foreach (var thing in item.Return)
                        {
                            r.Write($"{thing}∰");
                        }
                        r.Write("ª");
                        r.Write(item.Owner);
                        r.Write("ª");
                        foreach (var thing in item.asPerson)
                        {
                            r.Write($"{thing}∰");
                        }
                        r.Write("ª");
                    }
                }
            }
            catch
            {
            }
        }
        public void Transfer(ISocketMessageChannel channel)
        {
            for (int i = 0; i < commands.Count; i++)
            {
                if (commands[i].Return.Count() == 1 && commands[i].Return[0].ToUpper().Contains("PLAY "))
                {
                    commands.RemoveAt(i);
                    i--;
                }
            }
            SaveCommands();
        }
        public bool IsIn(string _Trigger)
        {
            while (true)
            {
                if (_Trigger.Contains("<") && _Trigger.Contains('>'))
                {
                    int position = 0;
                    while (_Trigger[position] != '<')
                    {
                        position++;
                    }
                    while (_Trigger[position] != '>')
                    {
                        _Trigger = _Trigger.Remove(position, 1);
                    }
                    _Trigger = _Trigger.Remove(position, 1);
                }
                else
                {
                    break;
                }
            }
            foreach (var item in commands)
            {
                if (item.Contains)
                {
                    if (_Trigger.ToUpper().Contains($"{item.Trigger.ToUpper()}"))
                    {
                        return true;
                    }
                }
                else
                {
                    if (_Trigger.ToUpper() == item.Trigger.ToUpper())
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public string ReturnIt(string _Trigger)
        {
            while (true)
            {
                if (_Trigger.Contains("<") && _Trigger.Contains('>'))
                {
                    int position = 0;
                    while (_Trigger[position] != '<')
                    {
                        position++;
                    }
                    while (_Trigger[position] != '>')
                    {
                        _Trigger = _Trigger.Remove(position, 1);
                    }
                    _Trigger = _Trigger.Remove(position, 1);
                }
                else
                {
                    break;
                }
            }
            foreach (var item in commands)
            {
                if (item.Contains)
                {
                    if (_Trigger.ToUpper().Contains($"{item.Trigger.ToUpper()}"))
                    {
                        Random random = new Random();
                        int RanReturn = random.Next(item.Return.Count());
                        return item.Return[RanReturn];
                    }
                }
                else
                {
                    if (_Trigger.ToUpper() == item.Trigger.ToUpper())
                    {
                        Random random = new Random();
                        int RanReturn = random.Next(item.Return.Count());
                        return item.Return[RanReturn];
                    }
                }
            }
            return "";
        }
        public ulong ReturnUser(string _Trigger)
        {
            while (true)
            {
                if (_Trigger.Contains("<") && _Trigger.Contains('>'))
                {
                    int position = 0;
                    while (_Trigger[position] != '<')
                    {
                        position++;
                    }
                    while (_Trigger[position] != '>')
                    {
                        _Trigger = _Trigger.Remove(position, 1);
                    }
                    _Trigger = _Trigger.Remove(position, 1);
                }
                else
                {
                    break;
                }
            }
            foreach (var item in commands)
            {
                if (item.Contains)
                {
                    if (_Trigger.ToUpper().Contains($"{item.Trigger.ToUpper()}"))
                    {
                        Random random = new Random();
                        int RanReturn = random.Next(item.asPerson.Count());
                        return item.asPerson[RanReturn];
                    }
                }
                else
                {
                    if (_Trigger.ToUpper() == item.Trigger.ToUpper())
                    {
                        Random random = new Random();
                        int RanReturn = random.Next(item.asPerson.Count());
                        return item.asPerson[RanReturn];
                    }
                }
            }
            return 0;
        }
        public void AppendCommand(string _Trigger, string[] _Return)
        {
            for (int i = 0; i <= commands.Count; i++)
            {
                if (commands[i].Trigger.ToUpper() == _Trigger.ToUpper())
                {
                    List<string> _Temp = commands[i].Return.ToList();
                    foreach (var item in _Return)
                    {
                        _Temp.Add(item);
                    }
                    commands[i].Return = _Temp.ToArray();
                    break;
                }
            }
        }
        public void RemoveInCommand(string _Trigger, string[] _Return)
        {
            for (int i = 0; i <= commands.Count; i++)
            {
                if (commands[i].Trigger.ToUpper() == _Trigger.ToUpper())
                {
                    List<string> _Temp = commands[i].Return.ToList();
                    foreach (var item in _Return)
                    {
                        _Temp.Remove(item);
                    }
                    commands[i].Return = _Temp.ToArray();
                    break;
                }
            }
        }
        public ReturnCommands ToggleType(string _Trigger)
        {
            foreach (var item in commands)
            {
                if (_Trigger.ToUpper() == item.Trigger.ToUpper())
                {
                    item.Contains = !item.Contains;
                    return item;
                }
            }
            return null;
        }
        public ReturnCommands ToggleAsMe(string _Trigger, List<ulong> ID, bool remove)
        {
            foreach (var item in commands)
            {
                if (_Trigger.ToUpper() == item.Trigger.ToUpper())
                {
                    foreach (var item2 in ID)
                    {
                        if (remove)
                        {
                            if (item.asPerson.Contains(item2))
                            {
                                item.asPerson.Remove(item2);
                            }
                        }
                        else
                        {
                            if (!item.asPerson.Contains(item2))
                            {
                                item.asPerson.Add(item2);
                            }
                        }

                    }
                    if (item.asPerson.Count > 0)
                    {
                        for (int i = 0; i < item.asPerson.Count; i++)
                        {
                            if (item.asPerson[i] < 100000)
                            {
                                item.asPerson.RemoveAt(i);
                                i--;
                            }
                        }
                    }
                    if (item.asPerson.Count == 0)
                    {
                        item.asPerson.Add(0);
                    }
                    SaveCommands();
                    return item;
                }
            }
            return null;
        }
        public string CheckExisting(string _Trigger, string _Type)
        {
            if (_Type == "Add")
            {
                foreach (var item in commands)
                {
                    if (item.Contains && _Trigger.ToUpper().Contains(item.Trigger.ToUpper()))
                    {
                        return item.Trigger;
                    }
                }
            }
            else if (_Type == "Change")
            {
                bool _PassedIt = false;
                foreach (var item in commands)
                {
                    if (_PassedIt && item.Trigger.ToUpper().Contains(_Trigger.ToUpper()))
                    {
                        return item.Trigger;
                    }
                    else if (item.Trigger.ToUpper() == _Trigger.ToUpper())
                    {
                        _PassedIt = true;
                    }
                }
            }
            return "";
        }
        async public void GetCommand(string _Trigger, SocketMessage message, DiscordSocketClient client)
        {
            int _Duplicates = 0;
            foreach (var command in commands)
            {
                if (command.Trigger.ToUpper() == _Trigger.ToUpper())
                {
                    StringBuilder sb = new StringBuilder();
                    int i = 0;
                    foreach (var item in command.Return)
                    {
                        int j = 0;
                        foreach (var dupe in command.Return)
                        {
                            if (item == dupe && i != j)
                            {
                                _Duplicates++;
                                break;
                            }
                            j++;
                        }
                        i++;
                    }
                    if (_Duplicates == 1)
                    {
                        sb.Append($"__**Total Count: {command.Return.Count()}\nWith {_Duplicates} duplicate.**__```");
                    }
                    else
                    {
                        sb.Append($"__**Total Count: {command.Return.Count()}\nWith {_Duplicates} duplicates.**__```");
                    }
                    int killmeimmediately = 1;
                    foreach (var item in command.Return)
                    {
                        if (sb.ToString().Count() + item.Count() + 10 < 2000)
                        {
                            if (killmeimmediately == 1)
                            {
                                killmeimmediately = 0;
                                sb.Append($"\n{item}");
                            }
                            else
                            {
                                sb.Append($"\nor {item}");
                            }
                        }
                        else
                        {
                            sb.Append("```");
                            await message.Channel.SendMessageAsync(sb.ToString());
                            sb.Clear();
                            sb.Append("```");
                        }
                    }
                    sb.Append("```");
                    Console.WriteLine(sb.ToString());
                    await message.Channel.SendMessageAsync(sb.ToString());
                    sb.Clear();
                    sb.Append($"__**Total Sender Count: {command.asPerson.Count()}**__```");
                    foreach (var item in command.asPerson)
                    {
                        foreach (var g in client.Guilds)
                        {
                            foreach (var c in g.TextChannels)
                            {
                                if (c == message.Channel)
                                {
                                    try
                                    {
                                        if (g.GetUser(item).Nickname != null)
                                        {
                                            sb.Append($"\n{g.GetUser(item).Nickname}");
                                        }
                                        else
                                        {
                                            sb.Append($"\n{client.GetUser(item).Username}");
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        try
                                        {
                                            sb.Append($"\n{client.GetUser(item).Username}");
                                        }
                                        catch (Exception)
                                        {
                                        }
                                    }
                                }
                            }
                        }
                    }
                    sb.Append("```");
                    await message.Channel.SendMessageAsync(sb.ToString());
                }
            }
        }
        async public void ReverseSearch(string _Return, SocketMessage message)
        {
            StringBuilder sb = new StringBuilder();
            int count = 0;
            foreach (var command in commands)
            {
                foreach (var Return in command.Return)
                {
                    if (_Return == Return)
                    {
                        count++;
                        break;
                    }
                }
            }
            if (count == 0)
            {
                await message.Channel.SendMessageAsync($"I couldn't find any commands which return [{_Return}]");
            }
            else
            {
                sb.Append($"__**Total Count: {count}**__\n");
                sb.Append("```");
                foreach (var command in commands)
                {
                    foreach (var Return in command.Return)
                    {
                        if (_Return == Return)
                        {
                            if (sb.ToString().Length + command.Trigger.Length + 3 >= 2000)
                            {
                                sb.Append("```");
                                await message.Channel.SendMessageAsync(sb.ToString());
                                sb.Clear();
                                sb.Append("```");
                            }
                            sb.Append($"{command.Trigger}\n");
                            break;
                        }
                    }
                }
                sb.Append("```");
                await message.Channel.SendMessageAsync(sb.ToString());
            }
        }
        async public Task DownloadCommand(string _Trigger)
        {
            foreach (var command in commands)
            {
                if (_Trigger.ToUpper() == command.Trigger.ToUpper())
                {
                    Console.WriteLine($"Downloading {command.Trigger}");
                    using (WebClient web = new WebClient())
                    {
                        int i = 0;
                        foreach (var url in command.Return)
                        {
                            await web.DownloadFileTaskAsync(new Uri(url), $@"..\..\SavedImages\{command.Trigger}{i}.png");
                            i++;
                        }
                    }
                }
            }
        }

        //Claiming
        async public void ClaimCommand(string _Trigger, ulong ID, SocketMessage message = null)
        {
            foreach (var item in commands)
            {
                if (_Trigger.ToUpper() == item.Trigger.ToUpper())
                {
                    if (item.Owner == 0)
                    {
                        item.Owner = ID;
                        if (message != null)
                        {
                            await message.Channel.SendMessageAsync("Command successfully claimed!");
                            SetupOwner(ID);
                            SaveCommands();
                            foreach (var userthing in owners)
                            {
                                if (userthing.ID == ID && userthing.Return.Length == 0)
                                {

                                    await message.Channel.SendMessageAsync("You are almost done! If you want you can leave this as is, but if you want, use AddMessage to leave a reply for those who wish to tamper with your commands. For more info, type MessageHelp");
                                }
                            }
                        }
                    }
                    else if (item.Owner != ID)
                    {
                        if (message != null)
                        {
                            TamperMessage(_Trigger, message);
                        }
                    }
                    else
                    {
                        if (message != null)
                        {
                            await message.Channel.SendMessageAsync("You've already claimed this command. Did you mean to use [UnClaimCommand]");
                        }
                    }
                    break;
                }
            }
        }
        async public void UnClaimCommand(string _Trigger, ulong ID, SocketMessage message)
        {
            foreach (var item in commands)
            {
                if (_Trigger.ToUpper() == item.Trigger.ToUpper())
                {
                    if (item.Owner == ID)
                    {
                        item.Owner = 0;
                        await message.Channel.SendMessageAsync("Command successfully unclaimed!");
                        SaveCommands();
                    }
                    else if (item.Owner != ID && item.Owner != 0)
                    {
                        TamperMessage(_Trigger, message);
                    }
                    else
                    {
                        await message.Channel.SendMessageAsync("This command is already unclaimed. Did you mean to use [ClaimCommand]");
                    }
                    break;
                }
            }
        }
        public bool TamperCheck(string _Trigger, ulong ID, SocketMessage message)
        {
            foreach (var item in commands)
            {
                if (item.Trigger.ToUpper() == _Trigger.ToUpper())
                {
                    if (item.Owner != 0 && item.Owner != ID)
                    {
                        TamperMessage(_Trigger, message);
                        return true;
                    }
                }
            }
            return false;
        }
        async public void TamperMessage(string _Trigger, SocketMessage message)
        {
            Random random = new Random();
            foreach (var user in owners)
            {
                foreach (var command in commands)
                {
                    if (_Trigger.ToUpper() == command.Trigger.ToUpper() && user.ID == command.Owner)
                    {
                        if (user.Return.Count() != 0)
                        {
                            int RanReturn = random.Next(user.Return.Count());
                            if (!File.Exists($@"..\..\{user.ID}image.png"))
                            {
                                var yes = Client.GetUser(user.ID).GetAvatarUrl();
                                var image = DownloadImageFromUrl(yes.Trim());
                                image.Save($@"..\..\{user.ID}image.png");
                            }

                            var hook = (message.Channel as SocketTextChannel).CreateWebhookAsync("Fuck u");
                            System.Drawing.Image im = System.Drawing.Image.FromFile($@"..\..\{user.ID}image.png");
                            await hook.Result.ModifyAsync(x =>
                            {
                                try
                                {
                                    x.Name = (Client.GetUser(user.ID) as IGuildUser).Nickname;
                                }
                                catch
                                {
                                    x.Name = Client.GetUser(user.ID).Username;
                                }
                                // TODO: fix image shit.
                                //x.Image = new Optional<Image?>(im);
                            });
                            DiscordWebhookClient d = new DiscordWebhookClient(hook.Result);
                            await d.SendMessageAsync(user.Return[RanReturn]);
                            await d.DeleteWebhookAsync();
                        }
                        else
                        {
                            await message.Channel.SendMessageAsync("No");
                        }
                        break;
                    }
                }
            }
        }
        public void SetupOwner(ulong ID)
        {
            for (int i = 0; i <= owners.Count; i++)
            {
                if (i == owners.Count)
                {
                    owners.Add(new Owners(ID, new string[0]));
                    SaveOwners();
                    break;
                }
                else if (owners[i].ID == ID)
                {
                    break;
                }
            }
        }
        public bool IsOwnerIn(ulong ID)
        {
            foreach (var item in owners)
            {
                if (item.ID == ID)
                {
                    return true;
                }
            }
            return false;
        }
        public bool AddMessageCheck(SocketMessage message, ulong ID, string[] _Return)
        {
            for (int i = 0; i <= owners.Count; i++)
            {
                if (i == owners.Count)
                {
                    AddMessages(ID, _Return, message);
                    return true;
                }
                else if (owners[i].ID == ID)
                {
                    message.Channel.SendMessageAsync($"You already have messages setup. \nIf you want to replace it with these new messages type [OVERRIDE]\nIf you want to append the existing messages type [APPEND]\nType anything else and this will cancel the message addition.");
                    return false;
                }
            }
            return false;
        }
        public void AddMessages(ulong ID, string[] _Return, SocketMessage message = null)
        {
            for (int i = 0; i <= owners.Count; i++)
            {
                if (i == owners.Count)
                {
                    owners.Add(new Owners(ID, _Return));
                    if (message != null)
                    {
                        message.Channel.SendMessageAsync("Tamper Messages successfully added!");
                    }
                    SaveOwners();
                    break;
                }
                else if (owners[i].ID == ID)
                {
                    if (message != null)
                    {
                        message.Channel.SendMessageAsync("Tamper Messages successfully added!");
                    }
                    owners[i].ID = ID;
                    owners[i].Return = _Return;
                    SaveOwners();
                    break;
                }
            }
        }
        public void AppendMessages(ulong ID, string[] _Return, SocketMessage message)
        {
            for (int i = 0; i <= owners.Count; i++)
            {
                if (owners[i].ID == ID)
                {
                    List<string> _Temp = owners[i].Return.ToList();
                    foreach (var item in _Return)
                    {
                        _Temp.Add(item);
                    }
                    message.Channel.SendMessageAsync("Tamper Messages successfully updated!");
                    owners[i].Return = _Temp.ToArray();
                    SaveOwners();
                    break;
                }
            }
        }
        public void RemoveMessage(ulong ID, string[] _Return, SocketMessage message)
        {
            for (int i = 0; i <= owners.Count; i++)
            {
                if (owners[i].ID == ID)
                {
                    List<string> _Temp = owners[i].Return.ToList();
                    foreach (var item in _Return)
                    {
                        _Temp.Remove(item);
                    }
                    message.Channel.SendMessageAsync("Tamper Messages successfully removed!");
                    owners[i].Return = _Temp.ToArray();
                    SaveOwners();
                    break;
                }
            }
        }
        async public void GetMessage(ulong ID, SocketMessage message)
        {
            int _Duplicates = 0;
            foreach (var command in owners)
            {
                if (command.ID == ID)
                {
                    StringBuilder sb = new StringBuilder();
                    int i = 0;
                    foreach (var item in command.Return)
                    {
                        int j = 0;
                        foreach (var dupe in command.Return)
                        {
                            if (item == dupe && i != j)
                            {
                                _Duplicates++;
                                break;
                            }
                            j++;
                        }
                        i++;
                    }
                    if (_Duplicates == 1)
                    {
                        sb.Append($"__**Total Count: {command.Return.Count()}\nWith {_Duplicates} duplicate.**__```");
                    }
                    else
                    {
                        sb.Append($"__**Total Count: {command.Return.Count()}\nWith {_Duplicates} duplicates.**__```");
                    }
                    int killmeimmediately = 1;
                    foreach (var item in command.Return)
                    {
                        if (sb.ToString().Count() + item.Count() + 10 < 2000)
                        {
                            if (killmeimmediately == 1)
                            {
                                killmeimmediately = 0;
                                sb.Append($"\n{item}");
                            }
                            else
                            {
                                sb.Append($"\nor {item}");
                            }
                        }
                        else
                        {
                            sb.Append("```");
                            await message.Channel.SendMessageAsync(sb.ToString());
                            sb.Clear();
                            sb.Append("```");
                        }
                    }
                    sb.Append("```");
                    Console.WriteLine(sb.ToString());
                    await message.Channel.SendMessageAsync(sb.ToString());
                }
            }
        }
        public void LoadOwners()
        {
            string line;
            string[] _Return;
            ulong _Owner = 0;
            try
            {
                using (StreamReader r = new StreamReader(@"..\..\Owners.txt"))
                {
                    line = r.ReadToEnd();
                }
                string[] lines = line.Split('ª');
                for (int i = 0; i < lines.Count(); i++)
                {
                    if (i % 2 == 0)
                    {
                        _Owner = ulong.Parse(lines[i]);
                    }
                    else
                    {
                        _Return = lines[i].Split('∰');
                        List<string> TempList = new List<string>();
                        foreach (var returnvalue in _Return)
                        {
                            if (returnvalue != "")
                            {
                                TempList.Add(returnvalue);
                            }
                        }
                        AddMessages(_Owner, TempList.ToArray());
                    }
                }
            }
            catch
            {
            }
        }
        public void SaveOwners()
        {
            try
            {
                File.WriteAllText(@"..\..\Owners.txt", string.Empty);
                using (StreamWriter r = File.CreateText(@"..\..\Owners.txt"))
                {
                    foreach (var item in owners)
                    {
                        r.Write($"{item.ID}ª");
                        foreach (var thing in item.Return)
                        {
                            r.Write($"{thing}∰");
                        }
                        r.Write("ª");
                    }
                }
            }
            catch
            {
            }
        }

        //thing
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
    }
    public class ReturnCommands
    {
        public ReturnCommands(string _Trigger, string[] _Return, bool _Contains, List<ulong> _asPerson)
        {
            Trigger = _Trigger;
            Return = _Return;
            Contains = _Contains;
            asPerson = _asPerson;
        }
        public string Trigger;
        public string[] Return;
        public bool Contains;
        public ulong Owner;
        public List<ulong> asPerson;
    }
    public class Owners
    {
        public Owners(ulong _ID, string[] _Return)
        {
            ID = _ID;
            Return = _Return;
        }
        public ulong ID;
        public string[] Return;
    }
}
