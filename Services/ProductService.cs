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
        private readonly GoldPriceRepository _goldPriceRepository;
        public ProductService (IMapper mapper, ProductRepository productRepository, 
            GoldPriceRepository goldPriceRepository)
        {
            _productRepository = productRepository;
            _goldPriceRepository = goldPriceRepository;
            _mapper = mapper;
        }
         public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            var products = await _productRepository.GetProducts();
            var productsDto = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(products); 

            foreach (var p in productsDto)
            {
                // calculate total gold price
                p.GoldPrice = _goldPriceRepository.GetLatestGoldPrice(p.Id).BidPrice * p.GoldWeight;
            }

            return productsDto;
        }

        public ProductDto GeProductById(int id)
        {
            var product = _productRepository.GetProductById(id);
            var productDto = _mapper.Map<ProductDto>(product);
            // calculate total gold price
            productDto.GoldPrice = _goldPriceRepository.GetLatestGoldPrice(productDto.Id).BidPrice * productDto.GoldWeight;
            return productDto;
        }

        public async Task<IEnumerable<ProductDto>> GeProductByName(string name)
        {
            var products = await _productRepository.GetProductByName(name);
            var productsDto = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(products);

            foreach (var p in productsDto)
            {
                // calculate total gold price
                p.GoldPrice = _goldPriceRepository.GetLatestGoldPrice(p.Id).BidPrice * p.GoldWeight;
            }

            return productsDto;
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
