using System;

namespace WebApp.Logger.Extensions
{
    internal static class DateTimeExtension
    {
        public static DateTime GetCurrentDateUtc() {  return DateTime.UtcNow; }
        public static DateTime GetCurrentDate() { return DateTime.Now; }
        public static DateTime GetLocalDate() { return DateTime.Now; }
    }
}
