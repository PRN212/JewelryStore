using AutoMapper;
using Repositories.Entities;
using Repositories.Entities.Orders;
using Services.Dto;

namespace Services.Helpers
{
    public class MyMapper : Profile
    {
        public MyMapper()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(d => d.GoldType, o => o.MapFrom(s => s.Gold.Name))
                .ForMember(d => d.GoldPrice, o => o.MapFrom(s => s.Gold.BidPrice));

            CreateMap<ProductToAddDto, Product>();
            CreateMap<ProductDto, Product>();

            CreateMap<Order, SellOrderDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Name))
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.Name));

            CreateMap<OrderDetail, SellOrderDetailDto>()
                //.ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.OrderId))
                //.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                //.ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Product.Description))
                .ForMember(dest => dest.GoldId, opt => opt.MapFrom(src => src.Product.Gold.Id))
                .ForMember(dest => dest.GoldType, opt => opt.MapFrom(src => src.Product.Gold.Name))
                .ForMember(dest => dest.GoldWeight, opt => opt.MapFrom(src => src.Product.GoldWeight))
                //.ForMember(dest => dest.GoldPrice, opt => opt.MapFrom(src => src.Product.Gold.Price))
                .ForMember(dest => dest.GemName, opt => opt.MapFrom(src => src.Product.GemName))
                .ForMember(dest => dest.GemWeight, opt => opt.MapFrom(src => src.Product.GemWeight))
                .ForMember(dest => dest.GemPrice, opt => opt.MapFrom(src => src.Product.GemPrice))
                .ForMember(dest => dest.Labour, opt => opt.MapFrom(src => src.Product.Labour))
                //.ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.TotalPrice))
                .ForMember(dest => dest.ImgUrl, opt => opt.MapFrom(src => src.Product.ImgUrl));


            CreateMap<PurchaseOrderDto, Order>();

            CreateMap<Order, PurchaseOrderDto>()
                .ForMember(d => d.UserName, o => o.MapFrom(src => src.User.Name))
                .ForMember(d => d.CustomerName, o => o.MapFrom(src => src.Customer.Name))
                .ForMember(d => d.CustomerPhone, o => o.MapFrom(src => src.Customer.Phone))
                .ForMember(d => d.CustomerAddress, o => o.MapFrom(src => src.Customer.Address))
                .ForMember(d => d.OrderDetails, o => o.MapFrom(src => src.OrderDetails));

            CreateMap<OrderDetail, PurchaseOrderDetailDto>()
                .ForMember(d => d.ProductId, o => o.MapFrom(src => src.Product.Id))
                .ForMember(d => d.ProductName, o => o.MapFrom(src => src.Product.Name))
                .ForMember(d => d.Description, o => o.MapFrom(src => src.Product.Description))
                .ForMember(d => d.GoldId, o => o.MapFrom(src => src.Product.GoldId))
                .ForMember(d => d.GoldType, o => o.MapFrom(src => src.Product.Gold.Name))
                .ForMember(d => d.GoldWeight, o => o.MapFrom(src => src.Product.GoldWeight))
                .ForMember(d => d.ProductWeight, o => o.MapFrom(src => src.Product.TotalWeight))
                .ForMember(d => d.GemName, o => o.MapFrom(src => src.Product.GemName))
                .ForMember(d => d.GemWeight, o => o.MapFrom(src => src.Product.GemWeight))
                .ForMember(d => d.GemPrice, o => o.MapFrom(src => src.Product.GemPrice));
            CreateMap<PurchaseOrderDetailDto, OrderDetail>();
        }
    }
}
