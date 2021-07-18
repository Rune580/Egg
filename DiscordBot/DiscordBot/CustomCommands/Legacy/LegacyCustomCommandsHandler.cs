using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DiscordBot.CustomCommands.Storage;

namespace DiscordBot.CustomCommands.Legacy
{
    public static class LegacyCustomCommandsHandler
    {
        private static CustomCommand _currentCommand;

        private static List<CustomCommand> _commands;
        
        public static CustomCommand[] LoadCommands()
        {
            _commands = new List<CustomCommand>();
            
            string line;
            string _Trigger = "";
            string[] _Return;
            ulong _Owner = 69;
            try
            {
                using (StreamReader r = new StreamReader(@"..\..\..\UserCommands.txt"))
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
                        
                        EndCurrentCommand();
                        
                        var command = GetCurrentCommand();
                        command.trigger = _Trigger;
                        command.content = TempList;
                        command.fuzzy = _Contains;
                        //TODO implement disguising
                        SetCurrentCommand(command);
                        
                        //AddCommand(_Trigger, TempList.ToArray(), _Contains, yeet);

                    }
                    else if (i % 4 == 2)
                    {
                        _Owner = ulong.Parse(lines[i]);
                        
                        var command = GetCurrentCommand();
                        command.owner = _Owner;
                        SetCurrentCommand(command);
                        //ClaimCommand(_Trigger, _Owner);
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

                        var command = GetCurrentCommand();
                        command.disguises = yeet2;
                        SetCurrentCommand(command);
                        //ToggleAsMe(_Trigger, yeet2, false);
                    }
                }
            }
            catch
            {
            }

            var commandArray = _commands.ToArray();
            
            _commands.Clear();

            return commandArray;
        }

        private static CustomCommand GetCurrentCommand()
        {
            return _currentCommand ?? new CustomCommand();
        }

        private static void SetCurrentCommand(CustomCommand command)
        {
            _currentCommand = command;
        }

        private static void EndCurrentCommand()
        {
            if (_currentCommand == null)
                return;
            
            _commands.Add(_currentCommand);
            SetCurrentCommand(null);
        }
    }
}