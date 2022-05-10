using Microsoft.Extensions.DependencyInjection;
using WebApp.Core.Sqls;

namespace WebApp.Sql.Configurations
{
    public static class RepositoryDependency
    {
        public static void AddRepositoryDependency(this IServiceCollection services)
        {
            services.AddScoped(typeof(ISqlRepository<>), typeof(SqlRepository<>));
        }
    }
}
