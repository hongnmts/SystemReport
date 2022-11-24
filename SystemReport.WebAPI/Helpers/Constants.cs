namespace SystemReport.WebAPI.Helpers
{

    public class Constants
    {
        public const string DEFAULT_LOGO = "assets/DTHU.aead3196.png";
    }
    public class RoleConstants
    {
        public const string VAN_THU_TRUONG = "9997";
        public const string THU_KY_HIEU_TRUONG = "9998";
        public const string HIEU_TRUONG = "9999";
    }

    public class StatusConstants
    {
        public const string TRINH_LANH_DAO_TRUONG = "TLDT";
        public const string THU_KY_HIEU_TRUONG = "9998";
    }

    public class LoaiLichCongTac
    {
        public const string LICH_CONG_TAC_TRUONG = "LCTT";
        public const string LICH_CONG_TAC_DON_VI = "LCTDV";
    }

    public class TypeSignature
    {
        public const string SIGN = "SIGN";
        public const string COMMENT = "COMMENT";
    }

    public class CodeKyBaoCao
    {
        public const string NAM = "nam";
        public const string _6THANGDAUNAM = "6thangdaunam";
        public const string _6THANGCUOINAM = "6thangcuoinam";
        public const string QUYI = "quyi";
        public const string QUYII = "quyii";
        public const string QUYIII = "quyiii";
        public const string QUYIV = "quyiv";
        public const string THANG = "thang";
        public const string GIAIDOAN = "giaidoan";
    }

    public class StatusForm
    {
        public const string NHAP_LIEU = "NL";
        public const string KIEM_TRA = "KT";
        public const string TONG_HOP = "TH";
        public const string BAN_HANH = "BH";
        public const string LANH_DAO_SO_TU_CHOI = "LDSTC";
        public const string TRINH_LANH_DAO_SO = "TLDS";
        public const string CAN_BO_KIEM_TRA_TU_CHOI = "CBKTTC";
        public const string CHAN_BO_TONG_HOP_TU_CHOI = "CBTHTC";
        public const string LUU_HANH_NOI_BO = "LHNB";
        public const string THU_HOI = "THO";
    }

    public class HoTroDoanhNghiep
    {
        public const string LOAIHINH = "LOAIHINH";
        public const string TOCHUC = "TOCHUC";
        public const string HUYEN = "HUYEN";
        public const string QUYETDINH = "QUYETDINH";
        public const string NOIDUNGHOTRO = "NOIDUNGHOTRO";
    }    
    public class QuanLyDeTai
    {
        public const string DETAI = "DETAI";
        public const string CHUTRI = "CHUTRI";
        public const string CHUNHIEM = "CHUNHIEM";
        public const string LINHVUC = "LINHVUC";
        public const string QUYETDINHPHEQUYET = "QUYETDINHPHEQUYET";
        public const string DANGTHUCHIEN = "DANGTHUCHIEN";
        public const string XEPLOAI = "XEPLOAI";
        public const string QUYETDINHCHUYENGIAO = "QUYETDINHCHUYENGIAO";
        public const string DONVITIEPNHAN = "QUYETDINHCHUYENGIAO";
    }

    public class ECommonItem
    {
        public ECommonItem(string id, string name, string colName)
        {
            Id = id;
            Name = name;
            ColName = colName;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string ColName { get; set; }
    }
}