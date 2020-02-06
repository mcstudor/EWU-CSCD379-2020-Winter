namespace SecretSanta.Business.Dto
{
    public class Group : GroupInput, IEntity
    {
        public int Id { get; set; }
    }

    public class GroupInput
    {
        public string Title { get; set; }
    }
}
