using System;
using System.Globalization;

namespace FitnessApp.Utility
{
    public partial class ChangeDateFormat
    {
        public static string ConvertTimeLocal(TimeSpan span)
        {
            var res = span.ToString("h\\:mm");
            string formattedTime = (DateTime.ParseExact(res, "H:mm", CultureInfo.InvariantCulture)
                .ToString("h:mm", CultureInfo.InvariantCulture).ToLowerInvariant());
            return formattedTime;

            // 15:55 -16:42 => 3:55-4:43
        }
    }
}
