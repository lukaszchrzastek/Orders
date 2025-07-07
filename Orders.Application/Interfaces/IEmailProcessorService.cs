namespace Orders.Application.Interfaces
{
	public interface IEmailProcessorService
	{
		Task ProcessNewEmailsAsync(CancellationToken cancellationToken);
	}
}
