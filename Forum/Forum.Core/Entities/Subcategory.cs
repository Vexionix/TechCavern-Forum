using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.Core.Entities
{
	internal class Subcategory
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public Category Category { get; set; }
	}
}
