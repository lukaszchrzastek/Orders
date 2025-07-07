namespace Orders.Domain.Services
{
	public interface IEmailUniquenessChecker
	{
		Task<List<uint>> GetDuplicateIdsAsync(List<uint> ids);
	}
}
