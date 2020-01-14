using System.Collections.Generic;

namespace SecretSanta.Business
{
    public class User
    {
        public int Id { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public IEnumerable<Gift> Gifts { get; }

        public User(int id, string firstName, string lastName, IEnumerable<Gift> gifts)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Gifts = gifts;
        }
    }
}
