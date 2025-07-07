using Orders.Domain.Repositories;

namespace Orders.Domain.Services
{
	public class EmailUniquenessChecker(IEmailRepository repository) : IEmailUniquenessChecker
	{
		public async Task<List<uint>> GetDuplicateIdsAsync(List<uint> ids)
		{
			var existing = (await repository.GetByIdsAsync(ids)).Select(s=>s.EmailId);
			return [.. ids.Intersect(existing)];
		}
	}
}
