namespace Domain.Entities
{
    public class User : Entity<int>
    {
        public User()
        {

        }

        public User(int createdBy) : base(createdBy)
        {

        }


        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public UserType UserType { get; set; }

    }

    public enum UserType
    {
        Admin,
        Staff
    }
}
