using System.ComponentModel.DataAnnotations;

namespace SecretSanta.Business.Dto
{
    public class User : UserInput, IEntity
    {
        public int Id { get; set; }
    }

    public class UserInput
    {
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string?  LastName { get; set; }
    }
}
