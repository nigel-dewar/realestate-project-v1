namespace Manage.Repository.Entities
{
    public class UserProfileType
    {
        public string UserProfileId { get; set; }
        public int UserTypeId { get; set; }

        public UserProfile UserProfile { get; set; }
        public UserType UserType { get; set; }
    }
}