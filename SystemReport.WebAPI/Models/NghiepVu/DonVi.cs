using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace SystemReport.WebAPI.Models
{
    public class DonVi : Audit, TEntity<string>
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string MaDonVi { get; set; }
        public string Ten { get; set; }
        public string DonViCha { get; set; }
        public string TenDonViCha { get; set; }
        public int CapDV { get; set; }
        public string MaLoaiHinhDonVi { get; set; }
        public string TenLoaiHinhDonVi { get; set; }
        public string MaCapDonVi { get; set; }
        public string TenCapDonVi { get; set; }
    }

    public class DonViShort
    {
        public string Id { get; set; }
        public string Ten { get; set; }
        public string TenKhongDau { get; set; }

        [BsonIgnore]
        public string linhVucId { get; set; }

    }




    public class DonViQuanLy : DonViShort
    {
        public bool IsParent { get; set; }

        //    public List<CoQuanShort> list { get; set; } = new List<CoQuanShort>();
        public string DuAnId { get; set; }
    }


    public class DonViTree : DonViShort
    {
        public string id { get; set; }
        public string label
        {
            get;
            set;
        }
        public List<DonViTree> children { get; set; }
    }

    public class DonViParams
    {
        public string DuAnId { get; set; }
        public List<DonViTree> DonViTree { get; set; }
    }
}