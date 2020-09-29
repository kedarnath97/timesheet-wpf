using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace timesheet.wpf.DateTimeHelpers
{
    public static class DateExtensions
    {
        public static DateTime GetStartOfWeek(this DateTime forDay, DayOfWeek startDayOfWeek)
        {
            int difference = (7 + (forDay.DayOfWeek - startDayOfWeek)) % 7;
            return forDay.AddDays(-1 * difference).Date;
        }

        public static Tuple<DateTime, DateTime> GetWeekDates(DateTime day)
        {
            var startOfWeekDate = day.GetStartOfWeek(DayOfWeek.Sunday);
            return new Tuple<DateTime, DateTime>(startOfWeekDate, startOfWeekDate.AddDays(6));
        }

    }
}
