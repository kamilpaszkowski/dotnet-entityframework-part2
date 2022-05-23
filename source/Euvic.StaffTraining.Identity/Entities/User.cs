namespace Euvic.StaffTraining.Identity.Entities
{
    public class User
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public UserSettings Settings { get; set; }
        public UserProfile Profile { get; set; }
    }

    public class UserProfile
    {
        public bool EnableNotifications { get; set; }
    }
}
