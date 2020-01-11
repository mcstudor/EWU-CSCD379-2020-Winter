using System.Collections.Generic;

namespace SecretSanta.Business
{
    public class User
    {
        public int UserId { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public IEnumerable<Gift> Gifts { get; }

        public User(int userId, string firstName, string lastName, IEnumerable<Gift> gifts)
        {
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            Gifts = gifts;
        }
    }
}
