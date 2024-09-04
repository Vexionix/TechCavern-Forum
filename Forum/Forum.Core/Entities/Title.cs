using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Forum.Core.Entities
{
	public class Title
	{
		public Title(string titleName)
		{
			TitleName = titleName;
		}

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[Required]
		[MaxLength(20)]
		public string TitleName { get; set; }
		public List<User> Users { get; set; } = [];
	}
}
