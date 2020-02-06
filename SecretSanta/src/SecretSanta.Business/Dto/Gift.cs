namespace SecretSanta.Business.Dto
{
    public class Gift : GiftInput, IEntity
    {
        public int Id { get; set; }
    }

    public class GiftInput
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Url { get; set; }
        public UserInput? User { get; set; }
    }
}
