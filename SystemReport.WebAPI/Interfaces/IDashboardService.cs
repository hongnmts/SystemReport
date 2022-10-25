using System.Threading.Tasks;
using SystemReport.WebAPI.ViewModels;

namespace SystemReport.WebAPI.Interfaces
{
    public interface IDashboardService
    {
        DashboardVM GetDashboard();
    }
}