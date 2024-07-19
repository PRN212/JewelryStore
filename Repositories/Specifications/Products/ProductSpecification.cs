using Repositories.Entities;

namespace Repositories.Specifications.Products
{
	public class ProductSpecification : BaseSpecification<Product>
	{
		public ProductSpecification(ProductParams productParams)
		: base(x =>
		(string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower().Contains(productParams.Search)) &&
		(!productParams.GoldTypeId.HasValue || x.GoldId == productParams.GoldTypeId))
		{
			AddInclude(x => x.Gold);
		}

		public ProductSpecification(int id)
		: base(x => x.Id == id)
		{
			AddInclude(x => x.Gold);
		}
	}
}
