using MongoDB.Bson.Serialization.Attributes;

namespace SystemReport.WebAPI.Models
{
    public class Logging : Audit, TEntity<string>
    {

        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string CollectionName { get; set; }
        public string DatabaseName { get; set; }
        public string Action { get; set; } // EActionLog
        public string UserName { get; set; }
        public string ContentLog { get; set; }
        public string ActionResult { get; set; } // EActionResultLog


        public Logging() { }


    }
    public enum EActionLog
    {
        Unknown,
        Created,
        Updated,
        Deleted,
        View,
        CollectionData,
        Authentication,
        TokenAuthorization,
        ConvertUser,
        OtpToken,
        SendMail,
        SendSms,
        SendSmsAutoToAllUser,
        Payment,
        RegistryUser,
        Question,
        NewCitizen,
        VerifyCitizen
    }
    public enum EActionResultLog
    {
        Success,
        Fail
    }
}