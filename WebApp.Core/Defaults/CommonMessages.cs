using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Core.Messaging
{
    public class CommonMessages
    {
    }

    public static class EntityMessages
    {
        public static string EntityNotExist = "{0} '{1}' does not exist.";
        public static string EntityExist = "{0} '{1}' already exist.";
        public static string EntityDuplicateFound = "{0} '{1}' duplicate data exist.";
        public static string EntityForienKeyError = "Foreign key violation";
    }
}
