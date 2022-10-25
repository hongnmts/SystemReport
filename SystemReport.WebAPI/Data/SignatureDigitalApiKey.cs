namespace SystemReport.WebAPI.Data
{
    public class SignatureDigitalApiKey : ISignatureDigitalApiKey
    {
        public string ServiceUri { get; set; }
       public string ClientId { get; set; }
       public string ClientSecret { get; set; }
    }

    public interface ISignatureDigitalApiKey
    {
        string ServiceUri { get; set; }
        string ClientId { get; set; }
        string ClientSecret { get; set; }
    }
}