namespace WebApp.Logger.Providers.Sqls
{
    internal class SqlVariable
    {
        public const string SchemaName = "[log]";
        public const string AuditTableName = $"{SchemaName}.[AuditLogs]";
        public const string SqlTableName = $"{SchemaName}.[SqlLogs]";
        public const string ErrorTableName = $"{SchemaName}.[ErrorLogs]";
        public const string RequestTableName = $"{SchemaName}.[RequestLogs]";
    }
}
