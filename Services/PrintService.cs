using Microsoft.Extensions.DependencyInjection;
using Repositories.Entities.Orders;
using System.Text;

namespace Services
{
	public class PrintService
	{
		private IServiceProvider service;
		public PrintService(IServiceProvider service)
		{
			this.service = service;
		}
		public void ExportOrderToCsv(int orderId, string? filePath = null)
		{
			var orderService = service.GetRequiredService<SellOrderService>();
			Order order = orderService.Get(o => o.Id == orderId, includeProperties: "Customer");

			if (filePath == null)
			{
				filePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
			}
			var csv = new StringBuilder();

			// Order Header
			csv.AppendLine($"OrderCreatedDate,CustomerName,CustomerPhone,TotalPrice");
			csv.AppendLine($"{order.CreatedDate},{order.Customer.Name},{order.Customer.Phone},{order.TotalPrice}");


			// Order Details
			csv.AppendLine("Details:");
			csv.AppendLine("ProductName,TotalWeight,Quantity,Price");

			var detailService = service.GetRequiredService<OrderDetailService>();
			var details = detailService.GetAll(o => o.OrderId == orderId, includeProperties: "Product");
			foreach (var detail in details)
			{
				var product = detail.Product;

				csv.AppendLine($"{product.Name}{product.TotalWeight},{detail.Quantity},{detail.Price}");
			}

			using (StreamWriter outputFile = new StreamWriter(Path.Combine(filePath, $"Order{orderId}-JewelryStore.csv")))
			{
				outputFile.WriteLine(csv);
			}
		}
	}
}
