namespace SystemReport.WebAPI.Models
{
    public class KySoPhapLyModel
    {
        public string Id { get; set; }
        public FileShort File { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Px { get; set; }
        public string Py { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public int Page { get; set; }
        public string Image { get; set; }
    }
}