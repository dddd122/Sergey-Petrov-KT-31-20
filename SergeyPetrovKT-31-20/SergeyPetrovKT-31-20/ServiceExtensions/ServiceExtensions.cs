using SergeyPetrovKT_31_20.interfaces.StudentInterfaces;
using SergeyPetrovKT_31_20.Models;

namespace SergeyPetrovKT_31_20.ServiceExtensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddService(this IServiceCollection services)
        {
            services.AddScoped<IStudentService, StudentFilterService>();
            return services;
        }
    }
}
