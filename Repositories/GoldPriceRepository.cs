﻿
using Repositories.Entities;

namespace Repositories
{
    public class GoldPriceRepository
    {
        private readonly DataContext _context;

        public GoldPriceRepository (DataContext context)
        {
            _context = context;
        }
        public GoldPrice GetLatestGoldPrice(int id)
        {
            return _context.GoldPrices.Where(g => g.Id == id)
                .OrderByDescending(g => g.DateTime)
                .Take(1).FirstOrDefault();
        }
    }
}
