using FinalBlog.Data.Repositories;
using FinalBlog.Data.UoW;

namespace FinalBlog.WebApi
{
    public static class ServiceExtentions
    {
        public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        public static IServiceCollection AddCustomRepository<TEntity, IRepository>(this IServiceCollection services)
                 where TEntity : class
                 where IRepository : class, IRepository<TEntity>
        {
            services.AddTransient<IRepository<TEntity>, IRepository>();

            return services;
        }
    }
}
