
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;
using Repositories.Entities;

namespace Repositories
{
    public class GoldRepository
    {
        private readonly DataContext _context;
        public GoldRepository(DataContext context) 
        {
            _context = context;
        }
        public async Task<IEnumerable<Gold>> GetAllGoldAsync ()
        {
            return await _context.Golds.ToListAsync();
        }

        public Gold? GetById (int id)
        {
            return _context.Golds.SingleOrDefault(g => g.Id == id);
        }

        public void UpdateGold(Gold gold)
        {
            _context.Golds.Update(gold);
            _context.SaveChanges();
        }

        public List<Gold> GetAllGold()
        {
            return _context.Golds.ToList(); 
        }
    }
}
