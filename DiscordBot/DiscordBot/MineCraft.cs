//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace DiscordBot
//{
//    public class Object
//    {
//        public Object(string _Stats)
//        {
//            string[] _TempString = _Stats.Split(' ');
//            Identifier = _TempString[0];
//            DayIcon = _TempString[1];
//            NightIcon = _TempString[2];
//            Health = int.Parse(_TempString[3]);
//            if (_TempString[4] == "null")
//            {
//                item = null;
//            }
//            else
//            {

//            }
//            BackGround = bool.Parse(_TempString[5]);
//        }
//        public string Identifier;
//        public string DayIcon;
//        public string NightIcon;
//        public int Health;
//        public DroppedItem item;
//        public bool BackGround;
//    }
//    public class MineCraft
//    {
//        public MineCraft()
//        {
//        }
//    }
//    public class World
//    {
//        public BaseObject[,] Blocks;
//        public List<InteractiveBlock> IBlocks;
//        public List<Entity> Entities;
//        public int Time;
//        public void Move(int _X, int _Y, string _Direction, string _Identifier)
//        {
//            int XChange = 0;
//            int YChange = 0;
//            switch (_Direction)
//            {
//                case "UP":
//                    YChange++;
//                    break;
//                case "DOWN":
//                    YChange--;
//                    break;
//                case "LEFT":
//                    XChange--;
//                    break;
//                case "RIGHT":
//                    XChange++;
//                    break;
//                default:
//                    break;
//            }
//            if (Blocks[_X+XChange, _Y+YChange].BackGround)
//            {
//                while (Blocks[_X + XChange, _Y + YChange - 1].BackGround)
//                {
//                    YChange--;
//                }
//            }
//            else
//            {
//                if (Blocks[_X + XChange, _Y + YChange + 1].BackGround)
//                {
//                    YChange++;
//                }
//            }
//            foreach (var item in Entities)
//            {
//                if (item.Identifier == _Identifier)
//                {
//                    item.X += XChange;
//                    item.Y += YChange;
//                }
//            }
//        }
//        public void Delete(int _X, int _Y)
//        {
//            Blocks[_X, _Y].ChangeType("Sky");
//        }
//    }
//    public class BaseObject
//    {
//        public List<Object> ObjectTypes = new List<Object>();
//        public BaseObject()
//        {
//            string[] Objects;
//            using (StreamReader r = new StreamReader(@"..\..\MineCraft\Objects.txt"))
//            {
//                Objects = r.ReadToEnd().Split('\n');
//            }
//            foreach (var item in Objects)
//            {
//                ObjectTypes.Add(new Object(item));
//            }
//        }
//        public string Identifier;
//        public int X, Y;
//        public string DayIcon;
//        public string NightIcon;
//        public int Health;
//        public DroppedItem Item;
//        public bool BackGround;

//        public void Setup(string _I, int _X, int _Y, string _Day, string _Night, int _Health, DroppedItem _Item, bool _BackGround)
//        {
//            X = _X;
//            Y = _Y;
//            DayIcon = _Day;
//            NightIcon = _Night;
//            Health = _Health;
//            Item = _Item;
//            BackGround = _BackGround;
//        }
//        public void ChangeType(string _New)
//        {
//            foreach (var item in ObjectTypes)
//            {
//                if (item.Identifier == _New)
//                {
//                    Setup(item.Identifier, X, Y, item.DayIcon, item.NightIcon, item.Health, item.item, item.BackGround);
//                }
//            }
//        }
//    }
//    public class Block : BaseObject
//    {
//    }
//    public class InteractiveBlock : Block
//    {

//    }
//    public class Entity : BaseObject
//    {

//    }
//    public class Mob : Entity
//    {

//    }
//    public class Player : Entity
//    {

//    }
//    public class DroppedItem : Entity
//    {

//    }
//    public class Item : BaseObject
//    {

//    }
//}
