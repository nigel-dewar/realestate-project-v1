namespace Manage.Repository.Entities
{
    public class UserPermission
    {
        public string UserId { get; set; }

        public UserProfile UserProfile { get; set; }

        public string PropertyId { get; set; }

        public Property Property { get; set; }

        public string Relation { get; set; } // Relation = Owner, Agent, AgencyStaff

        public string Value { get; set; }

    }
}
