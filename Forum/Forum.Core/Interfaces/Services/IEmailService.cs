using Forum.Models;

namespace Forum.Core.Interfaces.Services
{
	public interface IEmailService
	{
		Task SendContactFormData(ContactFormModel contactFormBody, string secretPass);
	}
}
