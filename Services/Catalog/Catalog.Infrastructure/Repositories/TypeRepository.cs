using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Data;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories
{
	public class TypeRepository : ITypesRepository
	{
		public readonly ICatalogContext _context;

		public TypeRepository(ICatalogContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<ProductType>> GetAllTypes()
		{
			return await _context
				.Types
				.Find(type => true)
				.ToListAsync();
		}
	}
}
