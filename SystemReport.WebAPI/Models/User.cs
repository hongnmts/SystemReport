using System.Dynamic;

namespace SystemReport.WebAPI.Models
{
    public class User : Audit
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Note { get; set; }
        public string AvatarName { get; set; }
        public string AvatarId { get; set; }
        public string DeparmentId { get; set; }
        public string DeparmentName { get; set; }
    }
}