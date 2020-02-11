using System.ComponentModel.DataAnnotations;

namespace SecretSanta.Business.Dto
{
    public class Gift : GiftInput, IEntity
    {
        public int Id { get; set; }
    }

    public class GiftInput
    {
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public string? Url { get; set; }
        [Required]
        public int? UserId { get; set; }
    }
}
