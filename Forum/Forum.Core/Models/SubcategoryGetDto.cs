using System.ComponentModel.DataAnnotations;

namespace Forum.Core.Models
{
    public class SubcategoryGetDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        [MaxLength(256)]
        public string? Description { get; set; }
        [Required]
        public string? GiIcon { get; set; }
        [Required]
        public int NumberOfPosts { get; set; }
        [Required]
        public int NumberOfComments { get; set; }
        [Required]
        public GetPostDisplayDataDto? PostWithMostRecentActivity { get; set; }
    }
}
