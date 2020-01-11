namespace SecretSanta.Business
{
    public class Gift
    {
        public int Id { get; }
        public string Title { get; }
        public string Description { get; }
        public string Url { get; }
        public User User { get; }

        public Gift(in int id, string title, string description, string url, User user)
        {
            Id = id;
            Title = title;
            Description = description;
            Url = url;
            User = user;
        }
    }
}
