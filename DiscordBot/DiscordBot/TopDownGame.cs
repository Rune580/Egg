using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class TopDownGame
    {
        #region find correct player
        /*  
            foreach (var player in players)
            {
                if (player.UserID == ID)
                {
                    foreach (var area in world.Areas)
                    {
                        if (area.Name == player.Area)
                        {
                        break;
                        }
                    }
                    break;
                }
            }
         */
        #endregion
        public TopDownGame()
        {
            CreateStringParseListThing();
            List<Area> a = new List<Area>();
            foreach (var file in Directory.GetFiles(@"..\..\TopDown\Areas\"))
            {
                using (StreamReader r = new StreamReader(file))
                {
                    string l = r.ReadToEnd();
                    string[] Height = l.Split('\n');
                    Tile[,] T = new Tile[Height[0].Split('ª').Length, Height.Length];
                    int i = 0;
                    string[] Temp = null;
                    foreach (var item in Height)
                    {
                        Temp = item.Split('ª');
                        for (int j = 0; j < Temp.Length; j++)
                        {
                            string[] Stats = Temp[j].Split('∰');
                            T[j, i] = new Tile(Stats[0], bool.Parse(Stats[1]), bool.Parse(Stats[2]), Stats[3], new Pos(int.Parse(Stats[4]), int.Parse(Stats[5])), Stats[6]);
                        }
                        i++;
                    }
                    Area area = new Area(file, T, new Pos((Temp.Length - 1) / 2, Height.Length / 2));
                    a.Add(area);
                }
            }
            world = new World(a);
            foreach (var file in Directory.GetFiles(@"..\..\TopDown\Players\"))
            {
                using (StreamReader r = new StreamReader(file))
                {
                    string[] l = r.ReadToEnd().Split('∰');
                    players.Add(new Player(int.Parse(l[0]), int.Parse(l[1]), ulong.Parse(l[2]), l[3], l[4], bool.Parse(l[5])));
                }
            }
        }
        public World world;
        public List<Player> players = new List<Player>();
        public List<DoorPlacement> doors = new List<DoorPlacement>();
        public List<StringParse> strings = new List<StringParse>();
        public async Task ShowParses(SocketMessage message)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("__**List of all default blocks:**__ (Caps do not matter)\n");
            foreach (var item in strings)
            {
                if (sb.ToString().Length + $"[{item.Input}]\n\n[{item.Output}]\n".Length > 2000)
                {
                    await message.Channel.SendMessageAsync(sb.ToString());
                    sb.Clear();
                }
                sb.Append($"[{item.Input}]\n[{item.Output}]\n\n");
            }
            await message.Channel.SendMessageAsync(sb.ToString());
        }
        public async Task ShowAreas(SocketMessage message)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("__**List of all areas:**__\n```");
            foreach (var area in world.Areas)
            {
                string Temp = area.Name;
                while (Temp.Contains('\\'))
                {
                    Temp = Temp.Remove(0, 1);
                }
                Temp = Temp.Remove(Temp.Count() - 4, 4);
                sb.Append($"{Temp}\n");
            }
            sb.Append("```");
            await message.Channel.SendMessageAsync(sb.ToString());
        }
        public void CreateSign(ulong ID, string Input, SocketMessage message)
        {
            foreach (var player in players)
            {
                if (player.UserID == ID)
                {
                    foreach (var area in world.Areas)
                    {
                        if (area.Name == player.Area)
                        {
                            if (area.Tiles[player.X, player.Y].Icon != "<:door2:587401079348527144>" && area.Tiles[player.X, player.Y].Icon != "<:door1:587401078946004994>")
                            {
                                area.Tiles[player.X, player.Y].Icon = "<:sign:588621183532924928>";
                                area.Tiles[player.X, player.Y].Text = Input;
                            }
                            break;
                        }
                    }
                    break;
                }
            }
        }
        public bool ToggleStats(ulong ID)
        {
            foreach (var player in players)
            {
                if (player.UserID == ID)
                {
                    player.HUD = !player.HUD;
                    return player.HUD;
                }
            }
            return false;
        }
        public void CreateArea(string Input, int X, int Y, SocketMessage message)
        {
            X += 2;
            Y += 2;
            string FileName = $@"..\..\TopDown\Areas\{Input}.txt";
            Tile[,] T = new Tile[X, Y];
            for (int x = 0; x < X; x++)
            {
                for (int y = 0; y < Y; y++)
                {
                    if (x == 0 || y == 0 || y == Y - 1 || x == X - 1)
                    {
                        T[x, y] = new Tile("<:bedrock:587401079092674639>", true, false, "None", new Pos(0, 0), "");
                    }
                    else
                    {
                        T[x, y] = new Tile("<a:blank:530985976542003200>", false, false, "None", new Pos(0, 0), "");
                    }
                }
            }
            Area area = new Area(FileName, T, new Pos(X / 2, Y / 2));
            world.Areas.Add(area);
            foreach (var player in players)
            {
                if (player.UserID == message.Author.Id)
                {
                    ChangeArea(player.UserID, Input, message);
                }
            }
        }
        public void ChangeArea(ulong ID, string Input, SocketMessage message)
        {
            foreach (var player in players)
            {
                if (player.UserID == ID)
                {
                    foreach (var area in world.Areas)
                    {
                        if (area.Name == $@"..\..\TopDown\Areas\{Input}.txt")
                        {
                            player.Area = area.Name;
                            player.X = area.SpawnPoint.X;
                            player.Y = area.SpawnPoint.Y;
                            Display(player.X, player.Y, area, message, ID);
                        }
                    }
                }
            }
        }
        public void SetBlock(string Direction, ulong ID, string Input, SocketMessage message)
        {
            Direction = Direction.ToUpper();
            foreach (var player in players)
            {
                if (player.UserID == ID)
                {
                    int _X = 0;
                    int _Y = 0;
                    if (Direction == "UP")
                    {
                        _Y = -1;
                    }
                    else if (Direction == "DOWN")
                    {
                        _Y = 1;
                    }
                    else if (Direction == "LEFT")
                    {
                        _X = -1;
                    }
                    else if (Direction == "RIGHT")
                    {
                        _X = 1;
                    }
                    foreach (var area in world.Areas)
                    {
                        if (area.Name == player.Area)
                        {
                            if (area.Tiles[player.X + _X, player.Y + _Y].Icon != "<:bedrock:587401079092674639>" && area.Tiles[player.X + _X, player.Y + _Y].Icon != "<:door2:587401079348527144>" && area.Tiles[player.X + _X, player.Y + _Y].Icon != "<:door1:587401078946004994>")
                            {
                                area.Tiles[player.X + _X, player.Y + _Y].Icon = ParseAString(Input);
                            }
                            Display(player.X, player.Y, area, message, ID);
                            break;
                        }
                    }
                    break;
                }
            }
        }
        public void CreateDoor(ulong ID, SocketMessage message)
        {
            foreach (var player in players)
            {
                if (player.UserID == ID)
                {
                    foreach (var area in world.Areas)
                    {
                        if (area.Name == player.Area)
                        {
                            if (!area.Tiles[player.X, player.Y].TransitionTile && !area.Tiles[player.X, player.Y - 1].TransitionTile && area.Tiles[player.X, player.Y - 1].Icon != "<:bedrock:587401079092674639>" && area.Tiles[player.X, player.Y - 1].Icon != "<:door2:587401079348527144>" && area.Tiles[player.X, player.Y - 1].Icon != "<:door1:587401078946004994>" && area.Tiles[player.X, player.Y].Icon != "<:door2:587401079348527144>" && area.Tiles[player.X, player.Y].Icon != "<:door1:587401078946004994>")
                            {
                                bool ScuffedUser = false;
                                bool Second = false;
                                foreach (var door in doors)
                                {
                                    if (door.X == player.X && door.area == player.Area && (door.Y == player.Y || door.Y == player.Y - 1 || door.Y == player.Y + 1))
                                    {
                                        message.Channel.SendMessageAsync("I bet you think you're really funny.");
                                        ScuffedUser = true;
                                        break;
                                    }
                                }
                                if (!ScuffedUser)
                                {
                                    foreach (var door in doors)
                                    {
                                        if (door.ID == ID)
                                        {
                                            Second = true;
                                            area.Tiles[player.X, player.Y].TransitionCoords.X = door.X;
                                            area.Tiles[player.X, player.Y - 1].TransitionCoords.X = door.X;
                                            area.Tiles[player.X, player.Y].TransitionCoords.Y = door.Y;
                                            area.Tiles[player.X, player.Y - 1].TransitionCoords.Y = door.Y - 1;
                                            area.Tiles[player.X, player.Y].TransitionTile = true;
                                            area.Tiles[player.X, player.Y - 1].TransitionTile = true;
                                            area.Tiles[player.X, player.Y].Icon = "<:door2:587401079348527144>";
                                            area.Tiles[player.X, player.Y - 1].Icon = "<:door1:587401078946004994>";
                                            area.Tiles[player.X, player.Y].TransitionArea = door.area;
                                            area.Tiles[player.X, player.Y - 1].TransitionArea = door.area;

                                            foreach (var area2 in world.Areas)
                                            {
                                                if (area2.Name == door.area)
                                                {
                                                    area2.Tiles[door.X, door.Y].TransitionCoords.X = player.X;
                                                    area2.Tiles[door.X, door.Y - 1].TransitionCoords.X = player.X;
                                                    area2.Tiles[door.X, door.Y].TransitionCoords.Y = player.Y;
                                                    area2.Tiles[door.X, door.Y - 1].TransitionCoords.Y = player.Y - 1;
                                                    area2.Tiles[door.X, door.Y].TransitionTile = true;
                                                    area2.Tiles[door.X, door.Y - 1].TransitionTile = true;
                                                    area2.Tiles[door.X, door.Y].Icon = "<:door2:587401079348527144>";
                                                    area2.Tiles[door.X, door.Y - 1].Icon = "<:door1:587401078946004994>";
                                                    area2.Tiles[door.X, door.Y].TransitionArea = area.Name;
                                                    area2.Tiles[door.X, door.Y - 1].TransitionArea = area.Name;
                                                }
                                            }
                                        }
                                    }
                                    if (!Second)
                                    {
                                        message.Channel.SendMessageAsync("First door position set! Call the command again when you are where you want the other door.");
                                        doors.Add(new DoorPlacement(ID, message, player.X, player.Y, area.Name));
                                    }
                                    else
                                    {
                                        for (int i = 0; i < doors.Count; i++)
                                        {
                                            if (doors[i].ID == ID)
                                            {
                                                doors.RemoveAt(i);
                                                break;
                                            }
                                        }
                                        Display(player.X, player.Y, area, message, ID);
                                    }
                                }
                            }
                            break;
                        }
                    }
                    break;
                }
            }

        }
        public void DeleteDoor(ulong ID, string Direction, SocketMessage message)
        {
            Direction = Direction.ToUpper();
            foreach (var player in players)
            {
                if (player.UserID == ID)
                {
                    int _X = 0;
                    int _Y = 0;
                    if (Direction == "UP")
                    {
                        _Y = -1;
                    }
                    else if (Direction == "DOWN")
                    {
                        _Y = 1;
                    }
                    else if (Direction == "LEFT")
                    {
                        _X = -1;
                    }
                    else if (Direction == "RIGHT")
                    {
                        _X = 1;
                    }
                    foreach (var area in world.Areas)
                    {
                        if (area.Name == player.Area)
                        {
                            if (area.Tiles[player.X + _X, player.Y + _Y].Icon == "<:door2:587401079348527144>")
                            {
                                area.Tiles[player.X + _X, player.Y + _Y].Icon = "<a:blank:530985976542003200>";
                                area.Tiles[player.X + _X, player.Y + _Y - 1].Icon = "<a:blank:530985976542003200>";
                                area.Tiles[player.X + _X, player.Y + _Y].TransitionTile = false;
                                area.Tiles[player.X + _X, player.Y + _Y - 1].TransitionTile = false;
                                foreach (var area2 in world.Areas)
                                {
                                    if (area2.Name == area.Tiles[player.X + _X, player.Y + _Y].TransitionArea)
                                    {
                                        area2.Tiles[area.Tiles[player.X + _X, player.Y + _Y].TransitionCoords.X, area.Tiles[player.X + _X, player.Y + _Y].TransitionCoords.Y].Icon = "<a:blank:530985976542003200>";
                                        area2.Tiles[area.Tiles[player.X + _X, player.Y + _Y - 1].TransitionCoords.X, area.Tiles[player.X + _X, player.Y + _Y - 1].TransitionCoords.Y].Icon = "<a:blank:530985976542003200>";
                                        area2.Tiles[area.Tiles[player.X + _X, player.Y + _Y].TransitionCoords.X, area.Tiles[player.X + _X, player.Y + _Y].TransitionCoords.Y].TransitionTile = false;
                                        area2.Tiles[area.Tiles[player.X + _X, player.Y + _Y - 1].TransitionCoords.X, area.Tiles[player.X + _X, player.Y + _Y - 1].TransitionCoords.Y].TransitionTile = false;
                                    }
                                }
                            }
                            else if (area.Tiles[player.X + _X, player.Y + _Y].Icon == "<:door1:587401078946004994>")
                            {
                                area.Tiles[player.X + _X, player.Y + _Y].Icon = "<a:blank:530985976542003200>";
                                area.Tiles[player.X + _X, player.Y + _Y + 1].Icon = "<a:blank:530985976542003200>";
                                area.Tiles[player.X + _X, player.Y + _Y].TransitionTile = false;
                                area.Tiles[player.X + _X, player.Y + _Y + 1].TransitionTile = false;
                                foreach (var area2 in world.Areas)
                                {
                                    if (area2.Name == area.Tiles[player.X + _X, player.Y + _Y].TransitionArea)
                                    {
                                        area2.Tiles[area.Tiles[player.X + _X, player.Y + _Y].TransitionCoords.X, area.Tiles[player.X + _X, player.Y + _Y].TransitionCoords.Y].Icon = "<a:blank:530985976542003200>";
                                        area2.Tiles[area.Tiles[player.X + _X, player.Y + _Y + 1].TransitionCoords.X, area.Tiles[player.X + _X, player.Y + _Y + 1].TransitionCoords.Y].Icon = "<a:blank:530985976542003200>";
                                        area2.Tiles[area.Tiles[player.X + _X, player.Y + _Y].TransitionCoords.X, area.Tiles[player.X + _X, player.Y + _Y].TransitionCoords.Y].TransitionTile = false;
                                        area2.Tiles[area.Tiles[player.X + _X, player.Y + _Y + 1].TransitionCoords.X, area.Tiles[player.X + _X, player.Y + _Y + 1].TransitionCoords.Y].TransitionTile = false;
                                    }
                                }
                            }
                            Display(player.X, player.Y, area, message, ID);
                            break;
                        }
                    }
                    break;
                }
            }
        }
        public string ParseAString(string Input)
        {
            string Temp = Input.ToUpper();
            foreach (var item in strings)
            {
                if (item.Input == Temp)
                {
                    return item.Output;
                }
            }
            return Input;
        }
        public void CreateStringParseListThing()
        {
            strings.Add(new StringParse("GRASS", "<:grass:587340671334219800>"));
            strings.Add(new StringParse("DIRT", "<:dirt:587160299023499282>"));
            strings.Add(new StringParse("STONE", "<:stone:587160342027698206>"));
            strings.Add(new StringParse("WOOD", "<:wood:587401082401980449>"));
            strings.Add(new StringParse("LOG", "<:log:587401079759568907>"));
            strings.Add(new StringParse("LOGUP", "<:logup:587401081563250699>"));
            strings.Add(new StringParse("TNT", "<:tnt:587401081852657710>"));
            strings.Add(new StringParse("STONESLAB", "<:slab:587401082012041230>"));
            strings.Add(new StringParse("DOUBLESLAB", "<:dslab:587401079239737355>"));
            strings.Add(new StringParse("COBBLESTONE", "<:cobble:587401079315103744>"));
            strings.Add(new StringParse("SANDSTONE", "<:sandst:587401081793937408>"));
            strings.Add(new StringParse("OBSIDIAN", "<:obby:587401081659719685>"));
            strings.Add(new StringParse("MOSSYCOBBLESTONE", "<:moss:587401081336627277>"));
            strings.Add(new StringParse("LAMPON", "<:lamp2:587401079524687883>"));
            strings.Add(new StringParse("LAMPOFF", "<:lamp1:587401079088480313>"));
            strings.Add(new StringParse("IRON", "<:iron:587401079092674569>"));
            strings.Add(new StringParse("GOLD", "<:gold:587401079512104960>"));
            strings.Add(new StringParse("DIAMOND", "<:diamond:587401079227023514>"));
            strings.Add(new StringParse("GLOWSTONE", "<:glow:587401079461773313>"));
            strings.Add(new StringParse("SOULSAND", "<:soul:587401081529565187>"));
            strings.Add(new StringParse("NETHERRACK", "<:nether:587401081689210900>"));
            strings.Add(new StringParse("CAKE", "<:cake:587401079239737344>"));
            strings.Add(new StringParse("BOOKSHELF", "<:book:587401079227023441>"));
            strings.Add(new StringParse("SNOW", "<:snow:587401081919897650>"));
            strings.Add(new StringParse("SAND", "<:sand:587401081760251924>"));
            strings.Add(new StringParse("PUMPKIN1", "<:pump1:587401081798131732>"));
            strings.Add(new StringParse("PUMPKIN2", "<:pump2:587401081538084881>"));
            strings.Add(new StringParse("PUMPKIN", "<:pump1:587401081798131732>"));
            strings.Add(new StringParse("LAVA", "<:lava:587401079390601238>"));
            strings.Add(new StringParse("WATER", "<:water:587401082422952009>"));
            strings.Add(new StringParse("SPONGE", "<:spong:587401081542279203>"));
            strings.Add(new StringParse("GRAVEL", "<:gravel:587401079143268364>"));
            strings.Add(new StringParse("FURNACEON", "<:furnace2:587401079092674566>"));
            strings.Add(new StringParse("FURNACEOFF", "<:furnace:587401079092674719>"));
            strings.Add(new StringParse("CRAFTINGTABLE1", "<:craft:587401078908387329>"));
            strings.Add(new StringParse("CRAFTINGTABLE2", "<:craft2:587401079294132224>"));
            strings.Add(new StringParse("CRAFTINGTABLE", "<:craft:587401078908387329>"));
            strings.Add(new StringParse("CHEST", "<:chest:587401079231217857>"));
            strings.Add(new StringParse("BRICK", "<:brick:587401079004725266>"));
            strings.Add(new StringParse("BED1", "<:bed1:587401078052487358>"));
            strings.Add(new StringParse("BED2", "<:bed2:587401079277486146>"));
            strings.Add(new StringParse("BED", "<:bed1:587401078052487358>"));
            strings.Add(new StringParse("DELETE", "<a:blank:530985976542003200>"));
            strings.Add(new StringParse("BLANK", "<a:blank:530985976542003200>"));
            strings.Add(new StringParse("NOTHING", "<a:blank:530985976542003200>"));
            //strings.Add(new StringParse("", ""));

        }
        public void SavePlayers()
        {
            foreach (var item in players)
            {
                File.WriteAllText($@"..\..\TopDown\Players\{item.UserID}.txt", "");
                using (StreamWriter w = new StreamWriter($@"..\..\TopDown\Players\{item.UserID}.txt"))
                {
                    w.Write($"{item.X}∰{item.Y}∰{item.UserID}∰{item.Icon}∰{item.Area}∰{item.HUD}");
                }
            }
            foreach (var item in world.Areas)
            {
                File.WriteAllText(item.Name, "");
                using (StreamWriter w = new StreamWriter(item.Name))
                {
                    StringBuilder sb = new StringBuilder();
                    for (int y = 0; y < item.Tiles.GetLength(1); y++)
                    {
                        for (int x = 0; x < item.Tiles.GetLength(0); x++)
                        {
                            sb.Append($"{item.Tiles[x, y].Icon}∰{item.Tiles[x, y].Solid}∰{item.Tiles[x, y].TransitionTile}∰{item.Tiles[x, y].TransitionArea}∰{item.Tiles[x, y].TransitionCoords.X}∰{item.Tiles[x, y].TransitionCoords.Y}∰{item.Tiles[x, y].Text}ª");
                        }
                        sb.Remove(sb.ToString().Count() - 1, 1);
                        if (y != item.Tiles.GetLength(1) - 1)
                        {
                            sb.Append($"\n");
                        }
                    }
                    w.Write(sb.ToString());
                }
            }
        }
        public bool IsIn(ulong ID)
        {
            foreach (var item in players)
            {
                if (item.UserID == ID)
                {
                    return true;
                }
            }
            return false;
        }
        public void SpawnPlayer(ulong ID, SocketMessage message)
        {
            foreach (var item in world.Areas)
            {
                if (item.Name == $@"..\..\TopDown\Areas\Spawn.txt")
                {
                    players.Add(new Player(item.SpawnPoint.X, item.SpawnPoint.Y, ID, "<:steve:587160369898717196>", $@"..\..\TopDown\Areas\Spawn.txt", false));
                    MovePlayer("Stay", 1, ID, message);
                    break;
                }
            }
        }
        public void ChangeIcon(ulong ID, string Icon, SocketMessage message)
        {
            string PrevIcon = "";
            foreach (var item in players)
            {
                if (item.UserID == ID)
                {

                    PrevIcon = item.Icon;
                    item.Icon = Icon;
                    message.Channel.SendMessageAsync($"Changed your icon from {PrevIcon} to {item.Icon}");
                    break;
                }
            }
        }
        public void MovePlayer(string Direction, int Count, ulong ID, SocketMessage message = null)
        {
            Direction = Direction.ToUpper();
            foreach (var player in players)
            {
                if (player.UserID == ID)
                {
                    int _X = 0;
                    int _Y = 0;
                    if (Direction == "UP")
                    {
                        _Y = -1;
                    }
                    else if (Direction == "DOWN")
                    {
                        _Y = 1;
                    }
                    else if (Direction == "LEFT")
                    {
                        _X = -1;
                    }
                    else if (Direction == "RIGHT")
                    {
                        _X = 1;
                    }
                    foreach (var item in world.Areas)
                    {
                        var Area = item;
                        if (item.Name == player.Area)
                        {
                            string Sign = "";
                            List<Pos> pos = new List<Pos>();
                            foreach (var s in players)
                            {
                                if (s.Area == item.Name && s != player)
                                {
                                    pos.Add(new Pos(s.X, s.Y));
                                }
                            }
                            while (Count > 0)
                            {
                                Count--;
                                if (!item.Tiles[player.X + _X, player.Y + _Y].Solid)
                                {
                                    player.X += _X;
                                    player.Y += _Y;
                                }
                                #region unsued debug
                                //else
                                //{
                                //    StringBuilder sb = new StringBuilder();
                                //    int InitialI = 0;
                                //    for (int o = 0; o < item.Tiles.GetLength(1); o++)
                                //    {
                                //        for (int p = 0; p < item.Tiles.GetLength(0); p++)
                                //        {
                                //            Console.WriteLine($"{p} compared to {player.X} | {o} compared to {player.Y}");
                                //            if (InitialI != o)
                                //            {
                                //                sb.Append('\n');
                                //            }
                                //            if (p == player.X && o == player.Y)
                                //            {
                                //                sb.Append('P');
                                //            }
                                //            else if (item.Tiles[p,o].Solid)
                                //            {
                                //                sb.Append('X');
                                //            }
                                //            else
                                //            {
                                //                sb.Append('O');
                                //            }
                                //        }
                                //    }
                                //    Console.WriteLine(sb.ToString());
                                //}
                                #endregion
                                if (Count == 0)
                                {
                                    if (item.Tiles[player.X, player.Y].TransitionTile)
                                    {
                                        player.Area = item.Tiles[player.X, player.Y].TransitionArea;
                                        int TempX = item.Tiles[player.X, player.Y].TransitionCoords.X;
                                        int TempY = item.Tiles[player.X, player.Y].TransitionCoords.Y;
                                        player.X = TempX;
                                        player.Y = TempY;
                                        foreach (var area in world.Areas)
                                        {
                                            if (area.Name == player.Area)
                                            {
                                                Area = area;
                                                break;
                                            }
                                        }
                                        break;
                                    }
                                    else if (item.Tiles[player.X, player.Y].Icon == "<:sign:588621183532924928>")
                                    {
                                        Sign = item.Tiles[player.X, player.Y].Text;
                                    }
                                    while (true)
                                    {
                                        foreach (var place in pos)
                                        {
                                            if (place.X == player.X && place.Y == player.Y)
                                            {
                                                player.X -= _X;
                                                player.Y -= _Y;
                                                continue;
                                            }
                                        }
                                        break;
                                    }
                                }
                            }
                            if (message != null)
                            {
                                Display(player.X, player.Y, Area, message, ID, Sign);
                            }
                            break;
                        }
                    }
                }
            }
        }
        public Player SwitchArea(Player player, string Area, int X, int Y)
        {
            player.Area = Area;
            player.X = X;
            player.Y = Y;
            return player;
        }
        public async Task Display(int _X, int _Y, Area _Area, SocketMessage message, ulong ID, string Sign = "")
        {
            int PrevY = 0;
            StringBuilder sb = new StringBuilder();
            #region camera lock
            while (_Y - 3 < 0 && _Y + 1 < _Area.Tiles.GetLength(1))
            {
                _Y++;
            }
            while (_Y + 4 > _Area.Tiles.GetLength(1) && _Y != 0)
            {
                _Y--;
            }
            while (_X - 4 < 0 && _X + 1 < _Area.Tiles.GetLength(0))
            {
                _X++;
            }
            while (_X + 5 > _Area.Tiles.GetLength(0) && _X != 0)
            {
                _X--;
            }
            #endregion
            for (int y = _Y - 4; y < _Area.Tiles.GetLength(1); y++)
            {
                if (y < 0)
                {
                    y = 0;
                }
                for (int x = _X - 4; x < _Area.Tiles.GetLength(0); x++)
                {
                    if (x < 0)
                    {
                        x = 0;
                    }
                    if (y != PrevY)
                    {
                        PrevY = y;
                        sb.Append('\n');
                    }
                    if (y <= _Y + 3 && y >= _Y - 3 && x <= _X + 4 && x >= _X - 4)
                    {
                        bool FoundPlayer = false;
                        foreach (var player in players)
                        {
                            if (player.Area == _Area.Name && player.X == x && player.Y == y)
                            {
                                FoundPlayer = true;
                                sb.Append(player.Icon);
                                break;
                            }
                        }
                        if (!FoundPlayer)
                        {
                            sb.Append(_Area.Tiles[x, y].Icon);
                        }
                    }
                }
            }
            foreach (var player in players)
            {
                if (player.HUD && player.Area == _Area.Name && player.UserID == ID)
                {
                    Console.WriteLine("Found player");
                    string Temp = player.Area;
                    while (Temp.Contains('\\'))
                    {
                        Temp = Temp.Remove(0, 1);
                    }
                    Temp = Temp.Remove(Temp.Count() - 4, 4);
                    sb.Append($"\nPosition: [{player.X},{player.Y}]\nArea: [{Temp}]");
                    break;
                }
            }
            Console.WriteLine($"Message Length: {sb.ToString().Length}");
            await message.Channel.SendMessageAsync(sb.ToString());
            if (Sign != "")
            {
                await message.Channel.SendMessageAsync(Sign);
            }
        }
    }
    public class World
    {
        public World(List<Area> _Areas)
        {
            Areas = _Areas;
        }
        public List<Area> Areas = new List<Area>();
    }
    public class Area
    {
        public Area(string _Name, Tile[,] _Tiles, Pos _SpawnPoint)
        {
            Name = _Name;
            Tiles = _Tiles;
            SpawnPoint = _SpawnPoint;
        }
        public string Name;
        public Tile[,] Tiles;
        public Pos SpawnPoint;
    }
    public class Tile
    {
        public Tile(string _Icon, bool _Solid, bool _TransitionTile, string _TransitionArea, Pos _TransitionCoords, string _Text)
        {
            Icon = _Icon;
            Solid = _Solid;
            TransitionTile = _TransitionTile;
            TransitionArea = _TransitionArea;
            TransitionCoords = _TransitionCoords;
            Text = _Text;
        }
        public string Text;
        public string Icon;
        public bool Solid;
        public bool TransitionTile;
        public string TransitionArea;
        public Pos TransitionCoords;
    }
    public class Player
    {
        public Player(int _X, int _Y, ulong _UserID, string _Icon, string _Area, bool _HUD)
        {
            X = _X;
            Y = _Y;
            UserID = _UserID;
            Icon = _Icon;
            Area = _Area;
            HUD = _HUD;
        }
        public int X, Y;
        public ulong UserID;
        public string Icon;
        public string Area;
        public bool HUD;
    }
    public class Pos
    {
        public Pos(int x, int y)
        {
            X = x;
            Y = y;
        }
        public int X, Y;
    }
    public class DoorPlacement
    {
        public int Timer = 300;
        public ulong ID;
        public SocketMessage message;
        public int X, Y;
        public string area;
        public DoorPlacement(ulong _ID, SocketMessage _message, int _X, int _Y, string _area)
        {
            message = _message;
            ID = _ID;
            X = _X;
            Y = _Y;
            area = _area;
        }
    }
    public class StringParse
    {
        public string Input;
        public string Output;
        public StringParse(string In, string Out)
        {
            Input = In;
            Output = Out;
        }
    }
}
