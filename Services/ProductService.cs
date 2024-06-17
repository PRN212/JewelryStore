using AutoMapper;
using Repositories;
using Repositories.Entities;
using Services.Dto;

namespace Services
{
    public class ProductService
    {
        private readonly IMapper _mapper;
        private readonly ProductRepository _productRepository;
        public ProductService (IMapper mapper, ProductRepository productRepository)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }
         public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            var products = await _productRepository.GetProducts();
            return _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(products);
        }

        public ProductDto GeProductById(int id)
        {
            var product = _productRepository.GetProductById(id);
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<IEnumerable<ProductDto>> GeProductByName(string name)
        {
            var products = await _productRepository.GetProductByName(name);
            return _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(products);
        }

        public bool AddProduct(Product product)
        {
            return _productRepository.AddProduct(product);
        }

        public bool UpdateProduct(ProductDto productDto)
        {
            Product product = _productRepository.GetProductById(productDto.Id);
            if (product == null) { return false; }
            _mapper.Map(productDto, product);
            return _productRepository.UpdateProduct(product);
        }
         
        public bool DeleteProduct(ProductDto productDto)
        {
            Product product = _productRepository.GetProductById(productDto.Id);
            if (product == null) { return false; }
            _mapper.Map(productDto, product);
            product.Status = false;
            return _productRepository.UpdateProduct(product);
        }

        public async Task<IEnumerable<Product>> SearchProducts (string searchValue)
        {
            return await _productRepository.SearchProducts(searchValue);
        }

    }
}
