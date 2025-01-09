namespace Models
{
    public class UserPasswords
    {
        public int PasswordID { get; set; }
        public int UserID { get; set; }
        public string PasswordHash { get; set; }
    }
}
