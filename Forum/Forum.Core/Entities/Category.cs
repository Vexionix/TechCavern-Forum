using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forum.Core.Entities
{
	public class Category
	{
		public Category(string name)
		{
			Name = name;
		}

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[Required]
		[MaxLength(50)]
		public string Name { get; set; } = "";
        [MaxLength(100)]
        public string GiIcon { get; set; }
        public List<Subcategory> Subcategories { get; set; } = [];
	}
}
