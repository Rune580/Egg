using System;
using DiscordBot.Reminders;

namespace DiscordBot.Helpers
{
    public static class TimeHelper
    {
        public static long MinutesToSeconds(this long value)
        {
            return value * 60;
        }

        public static long HoursToMinutes(this long value)
        {
            return value * 60;
        }

        public static long DaysToHours(this long value)
        {
            return value * 24;
        }

        public static long WeeksToDays(this long value)
        {
            return value * 7;
        }

        public static long MonthsToDaysRelative(this long value)
        {
            
        }

        public static long ToSecondsRelative(this long value, MessageToDateTime.TimeAlias timeScale)
        {
            var today = DateTime.Today;
            while (timeScale > MessageToDateTime.TimeAlias.Seconds)
            {
                switch (timeScale)
                {
                    case MessageToDateTime.TimeAlias.Minutes:
                        value = value.MinutesToSeconds();
                        timeScale--;
                        break;
                    case MessageToDateTime.TimeAlias.Hours:
                        value = value.HoursToMinutes();
                        timeScale--;
                        break;
                    case MessageToDateTime.TimeAlias.Days:
                        value = value.DaysToHours();
                        timeScale--;
                        break;
                    case MessageToDateTime.TimeAlias.Weeks:
                        value = value.WeeksToDays();
                        timeScale--;
                        break;
                    case MessageToDateTime.TimeAlias.Months:
                        int additionalYears = (int) Math.Floor((float) value / 12);
                        value = DateTime.DaysInMonth(today.Year + additionalYears, (int) value - additionalYears * 12);
                        break;
                    case MessageToDateTime.TimeAlias.Years:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(timeScale), timeScale, null);
                }
            }
        }
    }
}