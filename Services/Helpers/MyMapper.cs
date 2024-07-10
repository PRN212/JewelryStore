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
        }
    }
}
