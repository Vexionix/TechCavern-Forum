using Forum.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Forum.Models
{
	public class ContactFormModel
	{
		[Required]
		public int UserId { get; set; }
        [Required]
        [MaxLength(256)]
        public string Subject { get; set; }
        [Required]
        [MaxLength(254)]
        public string Email { get; set; }
        [Required]
        [MaxLength(4096)]
        public string Message { get; set; }
	}
}
