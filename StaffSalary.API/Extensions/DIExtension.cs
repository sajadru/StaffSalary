using StaffSalary.Infrastructure.UnitOfWork;

namespace StaffSalary.API.Extensions
{
    internal static class DIExtension
    {
        public static void DependencyInjection(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork,UnitOfWork>();
        }
    }
}
