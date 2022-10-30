using System.Collections.Generic;

namespace SystemReport.WebAPI.Helpers
{
    public class DefaultRoleCode
    {
        public const string LANH_DAO_SO = "9999";
        public const string LANH_DAO_DON_VI = "8888";
        public const string CAN_BO_TONG_HOP = "2222";
        public const string BAO_CAO_VIEN = "1111";
        public const string QUAN_TRI_VIEN = "0000";

        public const string HIEU_TRUONG = "9999";
        public const string THU_KY_HIEU_TRUONG = "9998";
        public const string VAN_THU_TRUONG = "9997";

        public const string TRINH_LANH_DAO_TRUONG = "TLDT";
        public const string TRINH_THU_KY_HIEU_TRUONG = "TKHTXN";
        public const string TRINH_LANH_DAO_DON_VI = "TLDDV";
        public const string LANH_DAO_DON_VI_DUYET = "LDDVD";
        public const string LANH_DAO_DON_VI_TU_CHOI = "LDDVTC";
        public const string KY_SO_PHAP_LY = "HTKS";
        public const string TU_CHOI = "TC";
        public const string BAN_HANH = "BH";
        public const string DA_DUYET = "DD";
        public const string HIEU_TRUONG_DA_DUYET = "HTD";
        public const string HIEU_TRUONG_DA_KY = "HTK";
        public const string HIEU_TRUONG_TU_CHOI_DUYET = "HTTCD";
        public const string HIEU_TRUONG_TU_CHOI_KY = "HTTTK";
        public const string DUYET_VAN_BAN_PHAP_LY = "DVBPL";
        public const string VAN_THU_TRUONG_TU_CHOI_DUYET = "VTTTC";
        public const string THU_KY_HIEU_TRUONG_TU_CHOI_DUYET = "TKHTTC";
        public const string KHOI_TAO_VAN_BAN = "KTVB";
        public const string DA_THIET_LAP_KY_SO = "DTLKS";
        public const string DA_KY_SO_DUYET = "dksxd";
        public const string HOAN_THANH_KY_SO = "htks";
        public const string KY_SO_PHAP_LY_THIETLAP = "kpl";

        public const string KHOI_TAO_VAN_BAN_DEN = "KTVBD";
        public const string BAN_HANH_VAN_BAN_DEN = "BHVBD";
        public const string TRINH_LANH_DAO_VAN_BAN_DEN = "TLDVBD";
        public const string TU_CHOI_VAN_BAN_DEN = "TCVBD";
        public const string PHAN_CONG_VAN_BAN_DEN = "PCVBD";
        public const string HOAN_THANH_VAN_BAN_DEN = "HTVBD";
        public const string THU_KY_DUYET_VAN_BAN_DEN = "TKDVBD";
        public const string KHONG_HOAN_THANH_VAN_BAN_DEN = "KHTVBD";
        public const string DUYET_VAN_BAN_DEN = "DVBD";
        public const string TRINH_THU_THU_KY_HIEU_TRUONG_VAN_BAN_DEN = "TTKHTVBD";
        public const string HIEU_TRUONG_DUYET_VAN_BAN_DEN = "HTDVD";
        public const string DA_DUYET_VAN_BAN_DEN = "DVBD";
        public const string HTXL_VAN_BAN_DEN = "HTXLVBD";
        public const string KHT_VAN_BAN_DEN = "KHTXLVBD";
        public const string TU_CHOI_KSNB = "TCKSNB";


        public static List<string> TrangThaiCapTruong = new List<string>() { "KPL", "DVBPL" };
        public static List<string> TrangThaiGhiNhanThongTin = new List<string>()
        {
            THU_KY_HIEU_TRUONG_TU_CHOI_DUYET,
            VAN_THU_TRUONG_TU_CHOI_DUYET,
            DUYET_VAN_BAN_PHAP_LY,
            TRINH_THU_KY_HIEU_TRUONG,
            KY_SO_PHAP_LY ,
            TU_CHOI, DA_DUYET,
            HIEU_TRUONG_DA_DUYET,
            HIEU_TRUONG_DA_KY,
            HIEU_TRUONG_TU_CHOI_DUYET,
            HIEU_TRUONG_TU_CHOI_KY
        };

        public static List<string> TrangThaiGhiNhanThongTinVBD = new List<string>()
        {
            TRINH_LANH_DAO_VAN_BAN_DEN,
            PHAN_CONG_VAN_BAN_DEN,
            HOAN_THANH_VAN_BAN_DEN,
            THU_KY_DUYET_VAN_BAN_DEN,
            KHONG_HOAN_THANH_VAN_BAN_DEN,
            DUYET_VAN_BAN_DEN,
            HIEU_TRUONG_DUYET_VAN_BAN_DEN,
            TRINH_THU_THU_KY_HIEU_TRUONG_VAN_BAN_DEN,
            TU_CHOI_VAN_BAN_DEN,
        };

    }
}