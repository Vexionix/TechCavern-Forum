namespace Forum.Core.Interfaces.Services
{
	public interface IPasswordService
	{
		string Encrypt(string password);
		bool CheckCriteria(string password);
		bool CheckMatchingHash(string password, string hash);
	}
}
