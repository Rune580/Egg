using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot
{
    [Serializable]
    class RemindMe
    {
        public int years, months, days, hours, minutes, seconds;
        public int setyear, setmonth, setday, sethour, setminute, setsecond;
        public string message;
        public ulong channel;
        public ulong ID;
        public bool UseIt = false;
        public bool weekly = false, daily = false, monthly = false, yearly = false;
        public void FixTimes()
        {
            years = setyear - DateTime.Now.Year;
            months = setmonth - DateTime.Now.Month;
            if (months < 0 && years >= 0)
            {
                months += 12;
                years--;
            }
            days = setday - DateTime.Now.AddYears(years).AddMonths(months).Day;
            if (days < 0 && months >= 0)
            {
                if (months % 2 == 0)
                {
                    if (months == 2)
                    {
                        if (DateTime.IsLeapYear(setyear))
                        {
                            days += 29;
                        }
                        else
                        {
                            days += 28;
                        }
                    }
                    else
                    {
                        days += 30;
                    }
                }
                else
                {
                    days += 31;
                }
                months--;
            }
            hours = sethour - DateTime.Now.Hour;
            if (hours < 0 && days >= 0)
            {
                hours += 24;
                days--;
            }
            minutes = setminute - DateTime.Now.Minute;
            if (minutes < 0 && hours >= 0)
            {
                minutes += 60;
                hours--;
            }
            seconds = setsecond - DateTime.Now.Second;
            if (seconds < 0 && minutes >= 0)
            {
                seconds += 60;
                minutes--;
            }
        }
        public void Update(int secondChange)
        {
            seconds -= secondChange;
            if (setyear < DateTime.Now.Year || 
                (setyear == DateTime.Now.Year && setmonth < DateTime.Now.Month) ||
                (setyear == DateTime.Now.Year && setmonth == DateTime.Now.Month && setday < DateTime.Now.Day) ||
                (setyear == DateTime.Now.Year && setmonth == DateTime.Now.Month && setday == DateTime.Now.Day && sethour < DateTime.Now.Hour) ||
                (setyear == DateTime.Now.Year && setmonth == DateTime.Now.Month && setday == DateTime.Now.Day && sethour == DateTime.Now.Hour && setminute < DateTime.Now.Minute) ||
                (setyear == DateTime.Now.Year && setmonth == DateTime.Now.Month && setday == DateTime.Now.Day && sethour == DateTime.Now.Hour && setminute == DateTime.Now.Minute && setsecond <= DateTime.Now.Second))
            {
                if (weekly)
                {
                    Setup(0, 0, 7, 0, 0, 0, ID, channel, message, daily, weekly, monthly, yearly);
                }
                else if (daily)
                {
                    Setup(0, 0, 1, 0, 0, 0, ID, channel, message, daily, weekly, monthly, yearly);
                }
                else if (monthly)
                {
                    Setup(0, 1, 0, 0, 0, 0, ID, channel, message, daily, weekly, monthly, yearly);
                }
                else if (yearly)
                {
                    Setup(1, 0, 0, 0, 0, 0, ID, channel, message, daily, weekly, monthly, yearly);
                }
                else
                {
                    UseIt = true;
                    return;
                }
            }
            if (seconds == -1)
            {
                seconds = 59;
                minutes--;
            }
            if (minutes == -1)
            {
                minutes = 59;
                hours--;
            }
            if (hours == -1)
            {
                hours = 23;
                days--;
            }
            if (days == -1)
            {
                months--;
                if (months == 1)
                {
                    if (DateTime.IsLeapYear(DateTime.Now.Year))
                    {
                        days = 29;
                    }
                    else
                    {
                        days = 28;
                    }
                }
                else
                {
                    if (months % 2 == 0)
                    {
                        days = 30;
                    }
                    else
                    {
                        days = 31;
                    }
                }
            }
            if (months == -1)
            {
                months = 11;
                years--;
            }
        }
        public void Setup(int _years, int _months, int _days, int _hours, int _minutes, int _seconds, ulong _ID, ulong _c, string m, bool _daily, bool _weekly, bool _monthly, bool _yearly)
        {
            try
            {
                int amin = 0, ahour = 0, aday = 0, amonth = 0, ayear = 0;
                years = _years;
                months = _months;
                days = _days;
                hours = _hours;
                minutes = _minutes;
                seconds = _seconds;
                ID = _ID;
                channel = _c;
                message = m;
                int temp;
                int prev;
                daily = _daily;
                weekly = _weekly;
                monthly = _monthly;
                yearly = _yearly;

                temp = seconds;
                prev = DateTime.Now.Minute;
                setsecond = DateTime.Now.AddSeconds(seconds).Second;
                for (int i = 0; i < temp; i++)
                {
                    int temp2 = DateTime.Now.AddSeconds(i).Minute;
                    if (prev != temp2)
                    {
                        prev = temp2;
                        amin++;
                    }
                }

                temp = minutes + amin;
                prev = DateTime.Now.Hour;
                setminute = DateTime.Now.AddMinutes(minutes + amin).Minute;
                for (int i = 0; i < temp; i++)
                {
                    int temp2 = DateTime.Now.AddMinutes(i).Hour;
                    if (prev != temp2)
                    {
                        prev = temp2;
                        ahour++;
                    }
                }

                temp = hours + ahour;
                prev = DateTime.Now.Day;
                sethour = DateTime.Now.AddHours(hours + ahour).Hour;
                for (int i = 0; i < temp; i++)
                {
                    int temp2 = DateTime.Now.AddHours(i).Day;
                    if (prev != temp2)
                    {
                        prev = temp2;
                        aday++;
                    }
                }

                temp = days + aday;
                prev = DateTime.Now.AddYears(years).AddMonths(months).Month;
                setday = DateTime.Now.AddYears(years).AddMonths(months).AddDays(temp).Day;
                for (int i = 0; i < temp; i++)
                {
                    int temp2 = DateTime.Now.AddYears(years).AddMonths(months).AddDays(i).Month;
                    if (prev != temp2)
                    {
                        prev = temp2;
                        amonth++;
                    }
                }

                temp = months + amonth;
                prev = DateTime.Now.Year;
                setmonth = DateTime.Now.AddMonths(months + amonth).Month;
                for (int i = 0; i < temp; i++)
                {
                    int temp2 = DateTime.Now.AddMonths(i).Year;
                    if (prev != temp2)
                    {
                        prev = temp2;
                        ayear++;
                    }
                }

                setyear = DateTime.Now.AddYears(years + ayear).Year;
                FixTimes();
            }
            catch (Exception)
            {
            }
        }
        public RemindMe(int _years, int _months, int _days, int _hours, int _minutes, int _seconds, ulong _ID, ulong _c, string m, bool _daily, bool _weekly, bool _monthly, bool _yearly)
        {
            Setup(_years, _months, _days, _hours, _minutes, _seconds, _ID, _c, m, _daily, _weekly, _monthly, _yearly);
        }
    }
}
