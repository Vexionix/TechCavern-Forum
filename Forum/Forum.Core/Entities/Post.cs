using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.Core.Entities
{
	internal class Post
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		public int Views { get; set; }
		public int Likes { get; set; }
		public DateTime CreatedAt { get; set; }
		public bool IsEdited { get; set; }
		public DateTime LastEditedAt { get; set; }
		public bool IsDeleted {  get; set; }
		public bool IsRemoved {  get; set; }
		public User User {  get; set; }
		public Subcategory Subcategory { get; set; }
	}
}
