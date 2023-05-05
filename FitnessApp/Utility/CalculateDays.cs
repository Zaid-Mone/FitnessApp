namespace FitnessApp.Utility
{
    public class CalculateDays
    {
        public static int SetNumberOfDays(string BundleTitle)
        {
            int months;
            int years;
            if (int.TryParse(BundleTitle.Split()[0], out months) && BundleTitle.ToLower().Contains("month"))
            {
                return months * 30;
            }
            else if (int.TryParse(BundleTitle.Split()[0], out years) && BundleTitle.ToLower().Contains("year"))
            {
                return years * 360;
            }
            else
            {
                switch (BundleTitle.ToLower())
                {
                    case "1 month":
                        return 30;
                    case "3 month":
                        return 90;
                    case "6 month":
                        return 180;
                    case "1 year":
                        return 360;
                    default:
                        // Handle invalid bundle titles here
                        return 0;
                }
            }
        }

    }
}



