using MongoDB.Bson.Serialization.Attributes;

namespace SystemReport.WebAPI.Models
{
    public class ChucVu : Audit, TEntity<string>
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string Ten { get; set; }
        public string MoTa { get; set; }
        public int ThuTu { get; set; }
        
        [BsonIgnore]
        public ChucVu Value { get; set; }
        public string Label { get; set; }

        public ChucVu()
        {
            
        }

        public ChucVu(ChucVu model)
        {
            this.Id = model.Id;

            this.Ten = model.Ten;
            this.ThuTu = model.ThuTu;
            this.Value = model;
            this.Label = Ten;
        }
    }
    public class ChucVuShort
    {
        public string Id { get; set; }
        public string Ten { get; set; }
        // public string MoTa { get; set; }
        // public string ThuTu { get; set; }
    }
}