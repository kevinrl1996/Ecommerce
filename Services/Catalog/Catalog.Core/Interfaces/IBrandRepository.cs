using Catalog.Core.Entities;

namespace Catalog.Core.Interfaces
{
	public interface IBrandRepository
	{
		Task<IEnumerable<ProductBrand>> GetAllBrands();
	}
}