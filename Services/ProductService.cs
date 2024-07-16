using AutoMapper;
using Repositories.Entities;
using Repositories.IRepositories;
using Repositories.Specifications.Products;
using Services.Dto;

namespace Services
{
    public class ProductService
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Product> _productRepository;   
        public ProductService (IMapper mapper, IGenericRepository<Product> productRepository)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            var products = await _productRepository.ListAllAsync();
            var productsDto = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(products); 

            return productsDto;
        }

        public async Task<ProductDto> GetProductById(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return null;
            }
            var productDto = _mapper.Map<ProductDto>(product);
            return productDto;
        }

        public async Task<IEnumerable<ProductDto>> GeProductByName(string name)
        {
            var param = new ProductParams()
            {
                Search = name
            };
            var spec = new ProductSpecification(param);
            var products = await _productRepository.ListAsync(spec);
            var productsDto = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(products);

            return productsDto;
        }

        public async Task<bool> AddProduct(ProductToAddDto productDto)
        {
            var product = _mapper.Map<Product>(productDto); 
            _productRepository.Add(product);
            return await _productRepository.SaveAllAsync();
        }

        public async Task<bool> UpdateProduct(ProductDto productDto)
        {
            Product? product = await _productRepository.GetByIdAsync(productDto.Id);
            if (product == null) { return false; }
            _mapper.Map(productDto, product);
            _productRepository.Update(product);
            return await _productRepository.SaveAllAsync();
        }
         
        public async Task<bool> DeleteProduct(ProductDto productDto)
        {
            Product? product = await _productRepository.GetByIdAsync(productDto.Id);
            if (product == null) { return false; }
            _mapper.Map(productDto, product);
            product.Status = false;
            _productRepository.Update(product);
            return await _productRepository.SaveAllAsync();
        }
    }
}
