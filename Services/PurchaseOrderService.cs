using AutoMapper;
using Repositories.Entities;
using Repositories.Entities.Orders;
using Repositories.IRepositories;
using Repositories.Specifications.Orders;
using Services.Dto;

namespace Services
{
	public class PurchaseOrderService
	{
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;
		public PurchaseOrderService(IMapper mapper, IUnitOfWork unitOfWork)
		{
			_mapper = mapper;
			_unitOfWork = unitOfWork;
		}
		public async Task<IReadOnlyList<PurchaseOrderDto>> GetOrdersWithSpec(string search, string orderType, string? orderStatus)
		{
			OrderSpecParams param = new OrderSpecParams
			{
				Search = search,
				OrderType = orderType,
				OrderStatus = orderStatus
			};
			OrdersSpecification spec = new OrdersSpecification(param);
			var orders = await _unitOfWork.Repository<Order>().ListAsync(spec);

			var purchaseOrdersDto = _mapper.Map<IReadOnlyList<Order>, IReadOnlyList<PurchaseOrderDto>>(orders);

			return purchaseOrdersDto;
		}

		public async Task<PurchaseOrderDto?> GetOrderById(int orderId)
		{
			var order = await _unitOfWork.Repository<Order>().GetEntityWithSpec(new OrdersSpecification(orderId));
			var orderDto = _mapper.Map<Order, PurchaseOrderDto>(order);
			return orderDto;
		}

		public async Task<PurchaseOrderDto?> AddOrder(Customer customer, int userId, string orderType)
		{
			// add customer
			if (customer.Id == 0)
			{
				_unitOfWork.Repository<Customer>().Add(customer);
			}
			// update customer
			else
			{
				_unitOfWork.Repository<Customer>().Update(customer);
			}
			await _unitOfWork.Repository<Customer>().SaveAllAsync();

			// create order
			var order = new Order()
			{
				CustomerId = customer.Id,
				UserId = userId,
				CreatedDate = DateTime.UtcNow,
				Status = OrderStatus.Pending.GetEnumMemberValue(),
				Type = orderType,
				PaymentMethod = PaymentMethod.Cash.GetEnumMemberValue(),
			};
			_unitOfWork.Repository<Order>().Add(order);

			// save to db
			if (await _unitOfWork.Complete() > 0)
			{
				order = await _unitOfWork.Repository<Order>().GetEntityWithSpec(new OrdersSpecification(order.Id));
				return _mapper.Map<PurchaseOrderDto>(order);
			}
			return null;
		}

		public async Task<bool> UpdateOrder(PurchaseOrderDto purchaseOrderDto)
		{
			var existingOrder = await _unitOfWork.Repository<Order>().GetEntityWithSpec(
				new OrdersSpecification(purchaseOrderDto.Id));
			if (existingOrder == null) return false;

			//update customer infor
			var customer = await _unitOfWork.Repository<Customer>().GetByIdAsync(existingOrder.CustomerId);
			customer.Name = purchaseOrderDto.CustomerName;
			customer.Address = purchaseOrderDto.CustomerAddress;
			customer.Phone = purchaseOrderDto.CustomerPhone;
			_unitOfWork.Repository<Customer>().Update(customer);

			return await _unitOfWork.Complete() > 0;
		}

		public async Task<bool> DeleteOrder(PurchaseOrderDto purchaseOrderDto)
		{
			var existingOrder = await _unitOfWork.Repository<Order>().GetEntityWithSpec(
				new OrdersSpecification(purchaseOrderDto.Id));

			if (existingOrder == null) return false;

			// update order
			existingOrder.Status = OrderStatus.Cancel.GetEnumMemberValue();
			_unitOfWork.Repository<Order>().Update(existingOrder);

			return await _unitOfWork.Complete() > 0;
		}

		public async Task<bool> IsPaid(PurchaseOrderDto purchaseOrderDto)
		{
			var existingOrder = await _unitOfWork.Repository<Order>().GetEntityWithSpec(
				new OrdersSpecification(purchaseOrderDto.Id));

			if (existingOrder == null) return false;

			// update order
			existingOrder.Status = OrderStatus.PaymentReceived.GetEnumMemberValue();
			_unitOfWork.Repository<Order>().Update(existingOrder);

			return await _unitOfWork.Complete() > 0;
		}
	}
}
