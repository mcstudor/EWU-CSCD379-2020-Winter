using System;

namespace SecretSanta.Business
{
    public class Gift
    {
        public int Id { get; }
        public string Title { get; set; }
        public string Description { get; set; }

        public Uri Url { get; set; }

        public User User { get; set;  }

        public Gift(in int id, string title, string description, Uri url, User user)
        {
            Id = id;
            Title = title;
            Description = description;
            Url = url;
            User = user;
        }
    }
}
