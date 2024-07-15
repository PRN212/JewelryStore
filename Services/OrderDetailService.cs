﻿
using AutoMapper;
using Repositories.Entities;
using Repositories;
using Services.Dto;
using Repositories.Entities.Orders;
using Repositories.IRepositories;
using Repositories.Specifications.Products;

namespace Services
{
    public class OrderDetailService
    {
        private readonly OrderDetailRepository _orderRepo;
        private readonly GoldPriceRepository _goldPriceRepository;
        private readonly OrderDetailRepository _orderDetailRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public OrderDetailService(OrderDetailRepository orderRepo, IMapper mapper,
            GoldPriceRepository goldPriceRepository,
            OrderDetailRepository orderDetailRepository, IUnitOfWork unitOfWork)
        {
            _orderRepo = orderRepo;
            _goldPriceRepository = goldPriceRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _orderDetailRepository = orderDetailRepository;
        }

        public List<SellOrderDetailDto> GetDetailsFromOrder(int id)
        {
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
            var goldPrice = _goldPriceRepository.GetLatestGoldPrice(product.GoldId).AskPrice;
            var orderDetail = new OrderDetail
            {
                ProductId = product.Id,
                OrderId = orderId,
                Quantity = product.Quantity,
                Price = product.GoldWeight * goldPrice*100 + product.GemPrice
            };

            // update order's total price
            var order = await _unitOfWork.Repository<Order>().GetByIdAsync(orderId);
            order.TotalPrice += orderDetail.Price * orderDetail.Quantity;

            return await _orderDetailRepository.AddOrderDetail(orderDetail);
        }
    }
}
