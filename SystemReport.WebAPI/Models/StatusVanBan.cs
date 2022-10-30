using MongoDB.Bson.Serialization.Attributes;

namespace SystemReport.WebAPI.Models
{
    public class StatusVanBan
    {
        public string StatusCode { get; set; }
        public string StatusName { get; set; }

        [BsonIgnore]
        public string QuestionId { get; set; }
    }
}