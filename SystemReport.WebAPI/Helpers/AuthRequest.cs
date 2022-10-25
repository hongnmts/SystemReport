namespace SystemReport.WebAPI.Helpers
{
    public class AuthRequest
    {
        public string UserName { get; set; }

        public string Password { get; set; }
        
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}