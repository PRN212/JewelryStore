
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
    }
}
