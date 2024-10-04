using Forum.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace Forum.Models
{
    public class CategoryGetDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? GiIcon { get; set; }
        [Required]
        public List<SubcategoryGetDto>? Subcategories { get; set; }
    }
}
