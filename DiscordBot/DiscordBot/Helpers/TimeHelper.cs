using System;
using DiscordBot.Reminders;

namespace DiscordBot.Helpers
{
    public static class TimeHelper
    {
        private static long MinutesToSeconds(this long value)
        {
            return value * 60;
        }

        private static long HoursToMinutes(this long value)
        {
            return value * 60;
        }

        private static long DaysToHours(this long value)
        {
            return value * 24;
        }

        private static long WeeksToDays(this long value)
        {
            return value * 7;
        }

        private static long MonthsToDaysRelative(this long value)
        {
            int additionalYears = 0;

            long days = DateTime.Today.Day;

            int currentMonth = DateTime.Today.Month + 1;

            if (currentMonth > 12)
            {
                currentMonth = 0;
            }

            while (currentMonth + additionalYears * 12 < value)
            {
                days += DateTime.DaysInMonth(currentMonth, additionalYears);
                
                currentMonth++;
                
                if (currentMonth > 12)
                {
                    currentMonth = 0;
                    additionalYears++;
                }
            }

            return days;
        }

        private static long YearsToDaysRelative(this long value)
        {
            long days = DateTime.Today.Day;
            
            int currentMonth = DateTime.Today.Month - 1;
            
            for (int i = 0; i < value; i++)
            {
                for (int m = currentMonth; m > 0; m--)
                {
                    days += DateTime.DaysInMonth(DateTime.Today.Year + i, m);
                }

                currentMonth = 12;
            }

            return days;
        }

        public static long ToSecondsRelative(this long value, MessageToDateTime.TimeAlias timeScale)
        {
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
                        value = value.MonthsToDaysRelative();
                        timeScale = MessageToDateTime.TimeAlias.Days;
                        break;
                    case MessageToDateTime.TimeAlias.Years:
                        value = value.YearsToDaysRelative();
                        timeScale = MessageToDateTime.TimeAlias.Days;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(timeScale), timeScale, null);
                }
            }

            return value;
        }
    }
}