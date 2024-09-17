namespace Forum.Core.Interfaces.Services
{
	public interface IPasswordService
	{
		string Encrypt(string password);
		bool IsCompliantWithValidityCriteria(string password);
		bool IsMatchingHash(string password, string hash);
	}
}
