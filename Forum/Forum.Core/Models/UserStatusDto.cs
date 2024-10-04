using System.ComponentModel.DataAnnotations;

namespace Forum.Core.Models
{
    public class UserStatusDto
    {
        [Required]
        public bool IsActive { get; set; }
    }
}
