using MongoDB.Bson.Serialization.Attributes;

namespace SystemReport.WebAPI.Models
{
    public class StatusQuestion
    {
        public string StatusCode { get; set; }
        public string StatusName { get; set; }

        [BsonIgnore]
        public string QuestionId { get; set; }
    }

    public class HistoryAction
    {
        public string StatusCode { get; set; }
        public string StatusName { get; set; }
    }

    public class EnumModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
}