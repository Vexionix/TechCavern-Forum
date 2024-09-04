using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Forum.Core.Entities
{
	public class Subcategory
	{
		public Subcategory(string name, int categoryId)
		{
			Name = name;
			CategoryId = categoryId;
		}

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[Required]
		[MaxLength(50)]
		public string Name { get; set; }
		public int CategoryId { get; set; }
		[ForeignKey("CategoryId")]
		public Category Category { get; set; }
		public List<Post> Posts { get; set; } = [];
	}
}
