using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

namespace WebApp.Core.Contexts
{
    public static class DbContextExtensions
    {
        public static DataTable GetDataTable(this DbContext context, string sqlQuery,
            List<SqlParameter> parameters = null,
            CommandType commandType = CommandType.Text
            )
        {
            DbProviderFactory dbFactory = DbProviderFactories.GetFactory(context.Database.GetDbConnection());

            using (var cmd = dbFactory.CreateCommand())
            {
                cmd.Connection = context.Database.GetDbConnection();
                cmd.CommandType = commandType;
                cmd.CommandText = sqlQuery;
                if (parameters != null && parameters.Any())
                {
                    cmd.Parameters.AddRange(parameters.ToArray());
                }
                using (DbDataAdapter adapter = dbFactory.CreateDataAdapter())
                {
                    adapter.SelectCommand = cmd;
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    return dt;
                }
            }
        }

        public static DataSet GetDataSet(this DbContext context, string sqlQuery, List<SqlParameter> parameters = null)
        {
            DbProviderFactory dbFactory = DbProviderFactories.GetFactory(context.Database.GetDbConnection());

            using (var cmd = dbFactory.CreateCommand())
            {
                cmd.Connection = context.Database.GetDbConnection();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sqlQuery;
                if (parameters != null && parameters.Any())
                {
                    cmd.Parameters.AddRange(parameters.ToArray());
                }
                using (DbDataAdapter adapter = dbFactory.CreateDataAdapter())
                {
                    adapter.SelectCommand = cmd;

                    DataSet ds = new DataSet();
                    adapter.Fill(ds);

                    return ds;
                }
            }
        }
    }
}
