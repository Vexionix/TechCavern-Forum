using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.Core.Exceptions
{
	public class BannedUserException : Exception
	{
		public BannedUserException(string message) : base(message) { } 
	}
}
