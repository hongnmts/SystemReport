using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SystemReport.WebAPI.Data;
using SystemReport.WebAPI.Interfaces;
using SystemReport.WebAPI.Interfaces.Identity;
using SystemReport.WebAPI.Services;

namespace SystemReport.WebAPI.Installers
{
    public class DbInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DbSettings>(configuration.GetSection(nameof(DbSettings)));
            services.AddSingleton<IDbSettings>(sp => sp.GetRequiredService<IOptions<DbSettings>>().Value);
            services.Configure<SignatureDigitalApiKey>(configuration.GetSection(nameof(SignatureDigitalApiKey)));
            services.AddSingleton<ISignatureDigitalApiKey>(sp => sp.GetRequiredService<IOptions<SignatureDigitalApiKey>>().Value);
            services.AddSingleton<DataContext>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<IDefineStatusService, DefineStatusService>();
            services.AddScoped<IModuleService, ModuleService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<ILoggingService, LoggingService>();
            // services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IDonViService, DonViService>();
            services.AddScoped<IChucVuService, ChucVuService>();
            services.AddScoped<ILinhVucService, LinhVucService>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            services.AddScoped<ITrangThaiService, TrangThaiService>();
            services.AddScoped<IModuleTrangThaiService, ModuleTrangThaiService>();
            services.AddScoped<IKhoiCoQuanService, KhoiCoQuanService>();
            services.AddScoped<ICoQuanService, CoQuanService>();
            services.AddScoped<INotifyService, NotifyService>();
            services.AddScoped<IEnumService, EnumService>();
            services.AddScoped<ILoaiTrangThaiService, LoaiTrangThaiService>();
            services.AddScoped<IDashboardService, DashboardService>();

            services.AddScoped<IKyBaoCaoService, KyBaoCaoService>();
            services.AddScoped<IDonViTinhService, DonViTinhService>();
            services.AddScoped<IMauBieuService, MauBieuService>();
            services.AddScoped<IBangBieuService, BangBieuService>();
            services.AddScoped<IThuocTinhService, ThuocTinhService>();
            services.AddScoped<IRowValueService, RowValueService>();
            services.AddScoped<ILoaiMauBieuService, LoaiMauBieuService>();
            services.AddScoped<IExportExcelService, ExportExcelService>();
            services.AddScoped<IHomeService, HomeService>();
            services.AddScoped<IHistoryMauBieuService, HistoryMauBieuService>();
            services.AddScoped<ICommonItemService, CommonItemService>();
            services.AddScoped<IHoTroDoanhNghiepService, HoTroDoanhNghiepService>();
        }
    }
}