using Orders.Domain.Enums;
using Orders.Domain.Models;

namespace Orders.Domain.Services
{
	public interface IEmailProcessingRules
	{
		EmailStatus DetermineStatus(Email email, string? html);
	}
}