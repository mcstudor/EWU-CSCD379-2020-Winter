using System.ComponentModel.DataAnnotations;

namespace SecretSanta.Business.Dto
{
    public class Group : GroupInput, IEntity
    {
        public int Id { get; set; }
    }

    public class GroupInput
    {
        [Required]
        public string? Title { get; set; }
    }
}
