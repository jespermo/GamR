namespace GamR.Backend.Web
{
    static class Extensions
    {
        internal static int ValueOrZero(this string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return 0;

            return int.Parse(str);
        }
    }
}