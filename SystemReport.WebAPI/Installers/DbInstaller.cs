using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SystemReport.WebAPI.Extensions;

namespace SystemReport.WebAPI.Installers
{
    public class DbInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            // services.Configure<DbSettings>(configuration.GetSection(nameof(DbSettings)));
            //services.AddSingleton<DbSettings>(sp => sp.GetRequiredService<IOptions<DbSettings>>().Value);
            // services.AddSingleton<DataContext>();
            //services.AddSingleton<MongoContext>();
        }
    }
}