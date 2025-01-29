using AutoMapper;
using Catalog.Core.DTOS;
using Catalog.Core.Entities;
using Catalog.Core.Interfaces;
using Catalog.Core.Specs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Net;

namespace Catalog.API.Controllers
{
	public class CatalogController : ApiController
	{
		private readonly IProductRepository _productRepository;
		private readonly IBrandRepository _brandRepository;
		private readonly ITypesRepository _typesRepository;
		private readonly IMapper _mapper;

		public CatalogController(IMapper mapper, IProductRepository productRepository, IBrandRepository brandRepository, ITypesRepository typesRepository)
		{
			_mapper = mapper;
			_productRepository = productRepository;
			_brandRepository = brandRepository;
			_typesRepository = typesRepository;
		}

		[HttpGet]
		[Route("[action]/{id}", Name = "GetProductById")]
		[ProducesResponseType(typeof(ProductDto), (int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		public async Task<ActionResult<ProductDto>> GetProductById(string id)
		{
			var product = await _productRepository.GetProduct(id);
			var productDto = _mapper.Map<ProductDto>(product);

			return Ok(productDto);
		}

		[HttpGet]
		[Route("[action]/{productName}", Name = "GetProductByProductName")]
		[ProducesResponseType(typeof(IEnumerable<ProductDto>), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductByProductName(string productName)
		{
			var product = await _productRepository.GetProductsByName(productName);
			var productDto = _mapper.Map<IEnumerable<ProductDto>>(product);

			return Ok(productDto);
		}

		[HttpGet]
		[Route("GetAllProducts")]
		[ProducesResponseType(typeof(Pagination<ProductDto>), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProducts([FromQuery] CatalogSpecParams catalogSpecParams)
		{
			var products = await _productRepository.GetProducts(catalogSpecParams);
			var productsDto = _mapper.Map<Pagination<ProductDto>>(products);

			return Ok(productsDto);
		}

		[HttpGet]
		[Route("GetAllBrands")]
		[ProducesResponseType(typeof(IEnumerable<BrandDto>), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<IEnumerable<BrandDto>>> GetAllBrands()
		{
			var brands = await _brandRepository.GetAllBrands();
			var brandsDto = _mapper.Map<IEnumerable<BrandDto>>(brands);

			return Ok(brandsDto);
		}

		[HttpGet]
		[Route("GetAllTypes")]
		[ProducesResponseType(typeof(IEnumerable<TypesDto>), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<IEnumerable<TypesDto>>> GetAllTypes()
		{
			var types = await _typesRepository.GetAllTypes();
			var typesDto = _mapper.Map<IEnumerable<TypesDto>>(types);

			return Ok(typesDto);
		}

		[HttpGet]
		[Route("[action]/{brand}", Name = "GetProductsByBrandName")]
		[ProducesResponseType(typeof(IEnumerable<ProductDto>), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsByBrandName(string brand)
		{
			var products = await _productRepository.GetProductsByName(brand);
			var productsDto = _mapper.Map<IEnumerable<ProductDto>>(products);

			return Ok(productsDto);
		}

		[HttpPost]
		[Route("CreateProduct")]
		[ProducesResponseType(typeof(ProductDto), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] ProductDto productDto)
		{
			var product = _mapper.Map<Product>(productDto);
			await _productRepository.CreateProduct(product);
			productDto = _mapper.Map<ProductDto>(product);

			return Ok(productDto);
		}

		[HttpPut]
		[Route("UpdateProduct")]
		[ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<ProductDto>> UpdateProduct([FromBody] ProductDto productDto)
		{
			var product = _mapper.Map<Product>(productDto);
			var result = await _productRepository.UpdateProduct(product);

			return Ok(result);
		}

		[HttpDelete]
		[Route("{id}", Name = "DeleteProduct")]
		[ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<ProductDto>> DeleteProduct(string id)
		{
			var result = await _productRepository.DeleteProduct(id);
			return Ok(result);
		}
	}
}
