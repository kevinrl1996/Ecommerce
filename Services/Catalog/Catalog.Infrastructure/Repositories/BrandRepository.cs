using Catalog.Core.Entities;
using Catalog.Core.Interfaces;
using Catalog.Infrastructure.Data;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories
{
	public class BrandRepository : IBrandRepository
	{
		public readonly ICatalogContext _context;

		public BrandRepository(ICatalogContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<ProductBrand>> GetAllBrands()
		{
			return await _context
				.Brands
				.Find(brand => true)
				.ToListAsync();
		}
	}
}
