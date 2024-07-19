
using AutoMapper;
using Repositories.Entities;
using Repositories;
using Services.Dto;
using Repositories.Entities.Orders;
using Repositories.IRepositories;
using Repositories.Specifications.Products;
using Repositories.Specifications.Orders;
using System.Linq.Expressions;

namespace Services
{
    public class OrderDetailService 
    {
        private readonly OrderDetailRepository _orderRepo;
        private readonly GoldPriceRepository _goldPriceRepository;
        private readonly SellProductRepository _sellProductRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public OrderDetailService(OrderDetailRepository orderRepo, IMapper mapper,
            GoldPriceRepository goldPriceRepository, SellProductRepository sellProductRepository, IUnitOfWork unitOfWork)
        {
            _orderRepo = orderRepo;
            _goldPriceRepository = goldPriceRepository;
            _unitOfWork = unitOfWork;
            _sellProductRepository = sellProductRepository;
            _mapper = mapper;
        }

		public List<OrderDetail> GetAll(Expression<Func<OrderDetail, bool>>? filter = null, string? includeProperties = null)
		{
			return _orderRepo.GetAll(filter, includeProperties).ToList();
		}

		public List<SellOrderDetailDto> GetDetailsFromOrder(int id)
        {
            if (id == 0) return null;
            var details = _orderRepo.GetDetailsFromOrder(id);
            List<SellOrderDetailDto> detailDtos = _mapper.Map<List<SellOrderDetailDto>>(details);
            foreach (var p in detailDtos)
            {
                p.GoldPrice = _goldPriceRepository.GetLatestGoldPrice(p.GoldId).BidPrice;
            }
            return detailDtos;
        }

        public async Task<bool> AddOrderDetail(ProductToAddDto productToAddDto, int orderId)
        {
            // add product
            var product = _mapper.Map<Product>(productToAddDto);
            _unitOfWork.Repository<Product>().Add(product);
            await _unitOfWork.Complete();
            product = await _unitOfWork.Repository<Product>().GetEntityWithSpec(new ProductSpecification(product.Id));

            // add order item
            var orderDetail = new OrderDetail
            {
                GoldPrice = product.Gold.AskPrice,
                ProductId = product.Id,
                OrderId = orderId,
                Quantity = product.Quantity,
            };
            orderDetail.Price = orderDetail.GoldPrice * product.GoldWeight*100 + product.GemPrice;
            _unitOfWork.Repository<OrderDetail>().Add(orderDetail);

            // update order's total price
            var order = await _unitOfWork.Repository<Order>().GetByIdAsync(orderId);
            order.TotalPrice = order.OrderDetails.Aggregate(0m, (acc, oi) => acc + oi.Price * oi.Quantity);
            _unitOfWork.Repository<Order>().Update(order);


            return await _unitOfWork.Complete() > 0;
        }

        public async Task<bool> UpdatePurchaseOrderDetail(ProductDto productDto, int orderId)
        {

            //update product
            Product? product = await _unitOfWork.Repository<Product>().GetByIdAsync(productDto.Id);
            if (product == null) { return false; }
            _mapper.Map(productDto, product);           
            _unitOfWork.Repository<Product>().Update(product);

            // get orderItem by (productId, orderId)
            var spec = new OrderDetailSpecification(new OrderDetailParam()
            {
                ProductId = productDto.Id,
                OrderId = orderId,
            });
            var orderDetail = await _unitOfWork.Repository<OrderDetail>().GetEntityWithSpec(spec);
            //update order item
            orderDetail.Quantity = productDto.Quantity;
            orderDetail.GoldPrice = product.Gold.AskPrice;
            orderDetail.Price = orderDetail.GoldPrice*100*productDto.GoldWeight + product.GemPrice;
            _unitOfWork.Repository<OrderDetail>().Update(orderDetail);

            //update order total price          
            var order = await _unitOfWork.Repository<Order>().GetEntityWithSpec(new OrdersSpecification(orderId));
            order.TotalPrice = order.OrderDetails.Aggregate(0m, (acc, oi) => acc + oi.Price * oi.Quantity);
            _unitOfWork.Repository<Order>().Update(order);

            // save to db
            return await _unitOfWork.Complete() > 0;
        }

        public async Task<bool> DetelePurchaseOrderDetail (ProductDto productDto, int orderId)
        {
            // delete order item
            var spec = new OrderDetailSpecification(new OrderDetailParam()
            {
                ProductId = productDto.Id,
                OrderId = orderId,
            });
            var orderDetail = await _unitOfWork.Repository<OrderDetail>().GetEntityWithSpec(spec);
            _unitOfWork.Repository<OrderDetail>().Delete(orderDetail);

            //delete product
            Product? product = await _unitOfWork.Repository<Product>().GetByIdAsync(productDto.Id);
            if (product == null) { return false; }
            _unitOfWork.Repository<Product>().Delete(product);

            //update order total price
            var order = await _unitOfWork.Repository<Order>().GetEntityWithSpec(new OrdersSpecification(orderId));
            order.TotalPrice -= orderDetail.Price * orderDetail.Quantity;
            _unitOfWork.Repository<Order>().Update(order);

            // save to db
            return await _unitOfWork.Complete() > 0;
        }

	}
}
