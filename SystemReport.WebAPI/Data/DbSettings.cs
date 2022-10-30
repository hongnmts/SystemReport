namespace SystemReport.WebAPI.Data
{
    public class DbSettings : IDbSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string LoggingCollectionName { get; set; }
        public string UserCollectionName { get; set; }
        public string RoleCollectionName { get; set; }
        public string ModuleCollectionName { get; set; }
        public string MenuCollectionName { get; set; }
        public string FileCollectionName { get; set; }
        public string RefreshTokenCollectionName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string DonViCollectionName { get; set; }

        public string LinhVucCollectionName { get; set; }
        public string ChucVuCollectionName { get; set; }
        public string XaCollectionName { get; set; }
        public string HuyenCollectionName { get; set; }
        public string QuestionCollectionName { get; set; }
        public string AnswerCollectionName { get; set; }
        public string HistoryVanBanDiCollectionName { get; set; }
        public string HistoryVanBanDenCollectionName { get; set; }
        public string WarningCollectionName { get; set; }
        public string TrangThaiCollectionName { get; set; }
        public string ModuleTrangThaiCollectionName { get; set; }
        public string NotifyCollectionName { get; set; }
        public string VanBanDiCollectionName { get; set; }
        public string VanBanDenCollectionName { get; set; }
        public string LichCongTacCollectionName { get; set; }
        public string LoaiVanBanCollectionName { get; set; }
        public string HoSoDonViCollectionName { get; set; }
        public string HinhThucGuiCollectionName { get; set; }
        public string CoQuanCollectionName { get; set; }
        public string KhoiCoQuanCollectionName { get; set; }
        public string LoaiTrangThaiCollectionName { get; set; }
        public string HopThuCollectionName { get; set; }
        public string CongVanCollectionName { get; set; }
        public string LuuCVDenCollectionName { get; set; }
        public string LuuCVDiCollectionName { get; set; }
        public string Environment { get; set; }


        public string KyBaoCaoCollectionName { get; set; }
        public string DonViTinhCollectionName { get; set; }
        public string MauBieuCollectionName { get; set; }
        public string BangBieuCollectionName { get; set; }
        public string ThuocTinhCollectionName { get; set; }
        public string RowValueCollectionName { get; set; }
        public string LoaiMauBieuCollectionName { get; set; }
        public string HistoryMauBieuCollectionName { get; set; }
    }

    public interface IDbSettings
    {
        string Environment { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
        string LoggingCollectionName { get; set; }
        string UserCollectionName { get; set; }
        string RoleCollectionName { get; set; }
        string ModuleCollectionName { get; set; }
        string MenuCollectionName { get; set; }
        string FileCollectionName { get; set; }
        string RefreshTokenCollectionName { get; set; }


        string DonViCollectionName { get; set; }
        string LinhVucCollectionName { get; set; }
        string ChucVuCollectionName { get; set; }
        string XaCollectionName { get; set; }
        string HuyenCollectionName { get; set; }
        string QuestionCollectionName { get; set; }
        string AnswerCollectionName { get; set; }
        string HistoryVanBanDiCollectionName { get; set; }
        string HistoryVanBanDenCollectionName { get; set; }
        string WarningCollectionName { get; set; }
        string TrangThaiCollectionName { get; set; }
        string ModuleTrangThaiCollectionName { get; set; }
        string NotifyCollectionName { get; set; }
        string VanBanDiCollectionName { get; set; }
        string VanBanDenCollectionName { get; set; }
        string LichCongTacCollectionName { get; set; }
        string LoaiVanBanCollectionName { get; set; }
        string HoSoDonViCollectionName { get; set; }
        string HinhThucGuiCollectionName { get; set; }
        string CoQuanCollectionName { get; set; }
        string KhoiCoQuanCollectionName { get; set; }
        string LoaiTrangThaiCollectionName { get; set; }
        string HopThuCollectionName { get; set; }
        string CongVanCollectionName { get; set; }
        string LuuCVDenCollectionName { get; set; }
        string LuuCVDiCollectionName { get; set; }

        // System Report

        string KyBaoCaoCollectionName { get; set; }
        string DonViTinhCollectionName { get; set; }
        string MauBieuCollectionName { get; set; }
        string BangBieuCollectionName { get; set; }
        string ThuocTinhCollectionName { get; set; }
        string RowValueCollectionName { get; set; }
        string LoaiMauBieuCollectionName { get; set; }

        // History
        string HistoryMauBieuCollectionName { get; set; }
    }
}