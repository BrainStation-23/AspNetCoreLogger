namespace WebApp.Core.Messaging
{
    public class CommonMessages
    {
    }

    public static class EntityMessages
    {
        public const string EntityNotExist = "{0} '{1}' does not exist.";
        public const string EntityExist = "{0} '{1}' already exist.";
        public const string EntityDuplicateFound = "{0} '{1}' duplicate data exist.";
        public const string EntityForienKeyError = "Foreign key violation";
    }
}
