using Catalog.Core.Entities;

namespace Catalog.Core.Interfaces
{
	public interface ITypesRepository
	{
		Task<IEnumerable<ProductType>> GetAllTypes();
	}
}