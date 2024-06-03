
using Microsoft.EntityFrameworkCore;
using Repositories.Entities;

namespace Repositories
{
    public class ProductRepository
    {
            private DataContext _context;
            public ProductRepository(DataContext context) 
            {
                _context = context;
            }

            public async Task<IEnumerable<Product>> GetProducts()
            {
                return await _context.Products
                .Include(p => p.Gold)
                .ToListAsync();
            }

            public Product GetProductById(int id)
            {
                return _context.Products
                .Include (p => p.Gold)
                .FirstOrDefault(p =>p.Id == id);
            }

        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            return await _context.Products
            .Include(p => p.Gold)
            .Where(p => p.Name.Contains(name))
            .ToListAsync();
        }
        public bool AddProduct(Product product)
            {
                _context.Add(product);
                return _context.SaveChanges() > 0;
            }

            public bool UpdateProduct(Product product)
            {
                _context.Update(product);
                return _context.SaveChanges() > 0;
            }

            public bool DeleteProduct(Product product)
            {
                _context.Remove(product);
                return _context.SaveChanges() > 0;
            }
        }
}
