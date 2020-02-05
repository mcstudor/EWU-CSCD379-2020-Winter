namespace SecretSanta.Business.Dto
{
    public class User : UserInput
    {
        public int Id { get; set; }
    }

    public class UserInput
    {
        public string? FirstName { get; set; }
        public string?  LastName { get; set; }
    }
}
