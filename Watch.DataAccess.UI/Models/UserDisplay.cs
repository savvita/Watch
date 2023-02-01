namespace Watch.DataAccess.UI.Models
{
    public class UserDisplay
    {
        public string Id { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool IsManager { get; set; }
        public bool IsAdmin { get; set; }
    }
}
