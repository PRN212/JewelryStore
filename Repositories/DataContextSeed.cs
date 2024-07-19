
using Repositories.Entities;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Repositories
{
	public class DataContextSeed
	{
		public static void SeedData(DataContext context)
		{
			var path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName;
			//var path = Path.GetDirectoryName(Directory.GetCurrentDirectory());

			var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
			options.Converters.Add(new JsonStringEnumConverter());
			// Seed Gold
			if (!context.Golds.Any())
			{
				var data = File.ReadAllText(path + @"/Repositories/SeedData/Gold.json");

				var list = JsonSerializer.Deserialize<List<Gold>>(data, options);

				foreach (var item in list)
				{
					context.Golds.Add(item);
				}

				context.SaveChanges();
			}
			//Seed Product Data
			if (!context.Products.Any())
			{
				var data = File.ReadAllText(path + @"/Repositories/SeedData/Product.json");

				var list = JsonSerializer.Deserialize<List<Product>>(data, options);

				foreach (var item in list)
				{
					context.Products.Add(item);
				}

				context.SaveChanges();
			}
			//Seed User
			if (!context.Users.Any())
			{
				var data = File.ReadAllText(path + @"/Repositories/SeedData/User.json");

				var list = JsonSerializer.Deserialize<List<User>>(data, options);

				foreach (var item in list)
				{
					context.Users.Add(item);
				}

				context.SaveChanges();
			}
		}
	}
}
