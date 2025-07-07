namespace Orders.Application.Interfaces
{
	public interface IEmailFetcherService
	{
		Task FetchUnreadEmailsAsync(CancellationToken cancellationToken);
	}
}