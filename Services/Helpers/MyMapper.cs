using AutoMapper;
using Repositories.Entities;
using Services.Dto;

namespace Services.Helpers
{
    public class MyMapper : Profile
    {
        public MyMapper()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(d => d.GoldType, o => o.MapFrom(s => s.Gold.Name));

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
                .ForMember(d => d.CustomerName, o => o.MapFrom(src => src.Customer.Name));

            CreateMap<OrderDetailDto, OrderDetail>();
        }
    }
}
