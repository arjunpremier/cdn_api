using System.ComponentModel.DataAnnotations;

namespace cdn_web.Models
{
	public class User
	{
        [Key]
        public int Id { get; set; }

        [Required, RegularExpression(@"^[a-zA-Z]*$", ErrorMessage = "User name must be in letters")]
        public required string UserName { get; set; }

        [Required, EmailAddress(ErrorMessage = "Invalid email address")]
        public string? Mail { get; set; }

        [Required, RegularExpression("^[0-9]{10}$", ErrorMessage = "Phone number must be 10 digits")]
        public string? PhoneNumber { get; set; }

        [MaxLength(500)]
        public string? SkillSets { get; set; }

        [MaxLength(500)]
        public string? Hobby { get; set; }

        public bool IsComplete { get; set; }
     }
}