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
        }
    }
}
