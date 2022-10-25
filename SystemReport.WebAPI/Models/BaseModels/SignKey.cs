namespace SystemReport.WebAPI.Models
{
    public class SignKey
    {
        public byte[] PrivateKey { get; set; }
        public byte[] PublicKey { get; set; }
        public string PrivateKey_string { get; set; }
        public string PublicKey_string { get; set; }
    }
}