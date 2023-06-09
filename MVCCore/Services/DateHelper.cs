namespace MVCCore.Services
{
    public class DateHelper
    {

        public static string GetTime(DateTime date)
        {
            var data = DateTime.UtcNow;
            if (date == DateTime.UtcNow) return " now ";
            var minutes = date.Minute;
            if (DateTime.UtcNow.Minute > minutes && DateTime.UtcNow.Hour == date.Hour && DateTime.UtcNow.Day == date.Day && DateTime.UtcNow.Month == date.Month && DateTime.UtcNow.Year == date.Year)
                return (DateTime.UtcNow.Minute - date.Minute).ToString() + " minutes ago";
            if (DateTime.UtcNow.Hour > date.Hour && DateTime.UtcNow.Minute >= date.Minute && DateTime.UtcNow.Day == date.Day && DateTime.UtcNow.Month == date.Month && DateTime.UtcNow.Year == date.Year)
            {
                return (DateTime.UtcNow.Hour - date.Hour).ToString() + " h and " + (DateTime.UtcNow.Minute - minutes).ToString() + " minutes ago";
            }

            if (DateTime.UtcNow.Hour > date.Hour && DateTime.UtcNow.Minute < date.Minute && DateTime.UtcNow.Day == date.Day && DateTime.UtcNow.Month == date.Month && DateTime.UtcNow.Year == date.Year)
            {
                var rest = (int)DateTime.UtcNow.Minute + (int)(60 - date.Minute);
                return rest.ToString() + " minutes ago";
            }
            if (DateTime.UtcNow.Day - 1 == date.Day) return " Yesterday ";

            return date.ToShortDateString();
        }
    }
}
