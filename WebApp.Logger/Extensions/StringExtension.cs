namespace WebApp.Logger.Test.Extensions
{
    public static class StringExtension
    {
        public  static bool IsNotNullOrEmpty(this string str) {
            return !string.IsNullOrEmpty(str);
        }

        public static bool IsNotNullOrWhiteSpace(this string str)
        {
            return !string.IsNullOrWhiteSpace(str);
        }
    }
}
