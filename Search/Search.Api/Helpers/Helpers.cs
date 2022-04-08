using System.Globalization;

namespace Search.Elastic.Service.Helpers
{
    public class Helpers
    {
        public static string ToTitleCase(string title)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(title.ToLower()); 
        }
    }
}