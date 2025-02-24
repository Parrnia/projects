using System.Globalization;

namespace Onyx.Application.Services;

public static class DateTimeExtensions
{
    public static string ToPersianDate(this DateTime dateTime1)
    {
        var persianCalendar1 = new PersianCalendar();
        var result = string.Format(@"{0}/{1}/{2} - {3}:{4}",
            persianCalendar1.GetYear(dateTime1),
            persianCalendar1.GetMonth(dateTime1),
            persianCalendar1.GetDayOfMonth(dateTime1),
            persianCalendar1.GetHour(dateTime1),
            persianCalendar1.GetMinute(dateTime1));
        return result;
    }

    public static string ToPersianDayOnly(this DateTime dateTime1)
    {
        var persianCalendar1 = new PersianCalendar();
        return string.Format(@"{0}{1}{2}{3}",
            persianCalendar1.GetMonth(dateTime1),
            persianCalendar1.GetDayOfMonth(dateTime1),
            persianCalendar1.GetHour(dateTime1),
            persianCalendar1.GetMinute(dateTime1));
    }
}