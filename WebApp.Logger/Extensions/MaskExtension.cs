namespace WebApp.Logger.Extensions
{
    public static class MaskExtension
    {
        public static string MaskMe(this string value, int masklength = 6, char maskChar = '*')
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            int midlength = 0;
            int firstlength = 0;
            int lastlength = 0;
            int length = value.Length;

            if (length < 3)
            {
                midlength = length;
            }
            else if (length < 8)
            {
                midlength = length / 3;
            }
            else
            {
                midlength = length - masklength;
            }

            int remaingPart = length - midlength;
            firstlength = remaingPart / 2;
            lastlength = length - firstlength - midlength;

            string firstParts = value.Substring(0, firstlength);
            string middleParts = new(maskChar, masklength);
            string lastParts = value.Substring(firstlength + midlength, lastlength);

            return firstParts + middleParts + lastParts;
        }
    }
}
