using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Core.DataType
{
    public static class NumberExtention
    {
        public static bool IsNumeric(this object o)
        {
            switch (Type.GetTypeCode(o.GetType()))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }

        public static string ToWords(this int amount)
        {
            return ((long)amount).ToWords();
        }

        public static string ToWords(this float amount)
        {
            return ((long)amount).ToWords();
        }

        public static string ToWords(this decimal amount)
        {
            return ((long)amount).ToWords();
        }

        public static string ToWords(this double amount)
        {
            return ((long)amount).ToWords();
        }

        public static string ToWords(this long amount)
        {
            var n = Convert.ToInt64(amount);

            if (n == 0)
            {
                return string.Empty;
            }
            else if (n > 0 && n <= 99)
            {
                var arr = new[]
                {
                    "এক", "দুই", "তিন", "চার", "পাঁচ", "ছয়", "সাত", "আট", "নয়", "দশ", "এগার", "বারো", "তের", "চৌদ্দ",
                    "পনের", "ষোল", "সতের", "আঠার", "ঊনিশ", "বিশ", "একুশ", "বাইস", "তেইশ", "চব্বিশ", "পঁচিশ", "ছাব্বিশ",
                    "সাতাশ", "আঠাশ", "ঊনত্রিশ", "ত্রিশ", "একত্রিস", "বত্রিশ", "তেত্রিশ", "চৌত্রিশ", "পঁয়ত্রিশ",
                    "ছত্রিশ", "সাঁইত্রিশ", "আটত্রিশ", "ঊনচল্লিশ", "চল্লিশ", "একচল্লিশ", "বিয়াল্লিশ", "তেতাল্লিশ",
                    "চুয়াল্লিশ", "পয়তাল্লিশ", "ছিচল্লিশ", "সাতচল্লিশ", "আতচল্লিশ", "উনপঞ্চাশ", "পঞ্চাশ", "একান্ন",
                    "বায়ান্ন", "তিপ্পান্ন", "চুয়ান্ন", "পঞ্চান্ন", "ছাপ্পান্ন", "সাতান্ন", "আটান্ন", "উনষাট", "ষাট",
                    "একষট্টি", "বাষট্টি", "তেষট্টি", "চৌষট্টি", "পয়ষট্টি", "ছিষট্টি", " সাতষট্টি", "আটষট্টি",
                    "ঊনসত্তর ", "সত্তর", "একাত্তর ", "বাহাত্তর", "তেহাত্তর", "চুয়াত্তর", "পঁচাত্তর", "ছিয়াত্তর",
                    "সাতাত্তর", "আটাত্তর", "ঊনাশি", "আশি", "একাশি", "বিরাশি", "তিরাশি", "চুরাশি", "পঁচাশি", "ছিয়াশি",
                    "সাতাশি", "আটাশি", "উননব্বই", "নব্বই", "একানব্বই", "বিরানব্বই", "তিরানব্বই", "চুরানব্বই",
                    "পঁচানব্বই ", "ছিয়ানব্বই ", "সাতানব্বই", "আটানব্বই", "নিরানব্বই"
                };
                return arr[n - 1] + " ";
            }
            else if (n >= 100 && n <= 199)
            {
                return (n / 100).ToWords() + "এক শত " + (n % 100).ToWords();
            }

            else if (n >= 100 && n <= 999)
            {
                return (n / 100).ToWords() + "শত " + (n % 100).ToWords();
            }
            else if (n >= 1000 && n <= 1999)
            {
                return "এক হাজার " + (n % 1000).ToWords();
            }
            else if (n >= 1000 && n <= 99999)
            {
                return (n / 1000).ToWords() + "হাজার " + (n % 1000).ToWords();
            }
            else if (n >= 100000 && n <= 199999)
            {
                return "এক লাখ " + (n % 100000).ToWords();
            }
            else if (n >= 100000 && n <= 9999999)
            {
                return (n / 100000).ToWords() + "লাখ " + (n % 100000).ToWords();
            }
            else if (n >= 10000000 && n <= 19999999)
            {
                return "এক কোটি " + (n % 10000000).ToWords();
            }
            else
            {
                return (n / 10000000).ToWords() + "কোটি " + (n % 10000000).ToWords();
            }
        }

    }
}