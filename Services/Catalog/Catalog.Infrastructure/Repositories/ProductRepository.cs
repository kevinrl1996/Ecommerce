﻿using Catalog.Core.Entities;
using Catalog.Core.Interfaces;
using Catalog.Core.Specs;
using Catalog.Infrastructure.Data;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories
{
	public class ProductRepository : IProductRepository
	{
		public readonly ICatalogContext _context;

		public ProductRepository(ICatalogContext context)
		{
			_context = context;
		}

		async Task<Product> IProductRepository.GetProduct(string id)
		{
			return await _context
				.Products
				.Find(p => p.Id == id)
				.FirstOrDefaultAsync();
		}

		public async Task<Pagination<Product>> GetProducts(CatalogSpecParams catalogSpecParams)
		{
			var builder = Builders<Product>.Filter;
			var filter = builder.Empty;

			if (!string.IsNullOrEmpty(catalogSpecParams.Search))
			{
				filter = filter & builder.Where(p => p.Name.ToLower().Contains(catalogSpecParams.Search.ToLower()));
			}
			if (!string.IsNullOrEmpty(catalogSpecParams.BrandId))
			{
				var brandFilter = builder.Eq(p => p.Brands.Id, catalogSpecParams.BrandId);
				filter &= brandFilter;
			}
			if (!string.IsNullOrEmpty(catalogSpecParams.TypeId))
			{
				var TypeFilter = builder.Eq(p => p.Types.Id, catalogSpecParams.TypeId);
				filter &= TypeFilter;
			}

			var totalItems = await _context.Products.CountDocumentsAsync(filter);
			var data = await DataFilter(catalogSpecParams, filter);

			return new Pagination<Product>(
				catalogSpecParams.PageIndex,
				catalogSpecParams.PageSize,
				(int)totalItems,
				data
			);
		}

		public async Task<IEnumerable<Product>> GetProductsByBrand(string brandName)
		{
			return await _context
				.Products
				.Find(p => p.Brands.Name.ToLower() == brandName.ToLower())
				.ToListAsync();
		}

		async Task<IEnumerable<Product>> IProductRepository.GetProductsByName(string name)
		{
			return await _context
				.Products
				.Find(p => p.Name.ToLower() == name.ToLower())
				.ToListAsync();
		}

		public async Task<Product> CreateProduct(Product product)
		{
			await _context.Products.InsertOneAsync(product);
			return product;
		}

		public async Task<bool> DeleteProduct(string id)
		{
			var deletedProduct = await _context
				.Products
				.DeleteOneAsync(p => p.Id == id);
			return deletedProduct.IsAcknowledged && deletedProduct.DeletedCount > 0;
		}

		public async Task<bool> UpdateProduct(Product product)
		{
			Product newProduct = new()
			{
				Id = product.Id,
				Description = product.Description,
				ImageFile = product.ImageFile,
				Name = product.Name,
				Price = product.Price,
				Summary = product.Summary,
				Brands = product.Brands,
				Types = product.Types
			};

			var updatedProduct = await _context
				.Products
				.ReplaceOneAsync(p => p.Id == product.Id, newProduct);
			return updatedProduct.IsAcknowledged && updatedProduct.ModifiedCount > 0;
		}

		private async Task<IReadOnlyList<Product>> DataFilter(CatalogSpecParams catalogSpecParams, FilterDefinition<Product> filter)
		{
			var sortDefn = Builders<Product>.Sort.Ascending("Name"); // Default
			if (!string.IsNullOrEmpty(catalogSpecParams.Sort))
			{
				switch (catalogSpecParams.Sort)
				{
					case "priceAsc":
						sortDefn = Builders<Product>.Sort.Ascending(p => p.Price);
						break;
					case "priceDesc":
						sortDefn = Builders<Product>.Sort.Descending(p => p.Price);
						break;
					default:
						sortDefn = Builders<Product>.Sort.Ascending(p => p.Name);
						break;

				}
			}
			return await _context
			.Products
			.Find(filter)
			.Sort(sortDefn)
			.Skip(catalogSpecParams.PageSize * (catalogSpecParams.PageIndex - 1))
			.Limit(catalogSpecParams.PageSize)
			.ToListAsync();
		}
	}
}