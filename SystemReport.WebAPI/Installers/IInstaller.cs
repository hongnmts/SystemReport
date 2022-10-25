using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SystemReport.WebAPI.Installers
{
    public interface IInstaller
    {
        void InstallServices(IServiceCollection services, IConfiguration configuration);
    }
}