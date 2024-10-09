using System.ComponentModel.DataAnnotations;

namespace Forum.Models
{
	public class UserProfileEditDto
    {
        [Required]
        [MaxLength(250)]
        public string? Bio { get; set; }
        [Required]
		public string? SelectedTitle { get; set; }
	}
}
