using Forum.Core.Interfaces.Services;

namespace Forum.Application.Services
{
	public class PasswordService : IPasswordService
	{
		int workFactor = 13;

		public string Encrypt(string password)
		{
			string passwordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(password, workFactor);
			return passwordHash;
		}

		public bool IsCompliantWithValidityCriteria(string password)
		{
			char[] specialCh = (@"%!@#$%^&*()?/>.<,:;'\|}]{[_~`+=-" + "\"").ToCharArray();

			if (password == null
				|| password.Length < 8
				|| password.Length > 32
				|| !password.Any(char.IsUpper)
				|| !password.Any(char.IsLower)
				|| !password.Any(ch => specialCh.Contains(ch))
				|| password.Contains(" ")) { return false; }

			return true;
		}

		public bool IsMatchingHash(string password, string passwordHash)
		{
			return BCrypt.Net.BCrypt.EnhancedVerify(password, passwordHash);
		}
	}
}
