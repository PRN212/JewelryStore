using Repositories;
using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Services
{
    public class GoldPriceService
    {
        private GoldPriceRepository _goldPriceRepo;

        public GoldPriceService(GoldPriceRepository goldPriceRepo)
        {
            _goldPriceRepo = goldPriceRepo;
        }
    
        public void SaveNewGoldPrice(GoldPrice goldPrice)
        {
            _goldPriceRepo.AddGoldPrice(goldPrice);
        }

        public List<GoldPrice> GetLatestGoldPrices()
        {
            List<GoldPrice> goldPrices = new List<GoldPrice>();
            for (int i = 1; i < 9; i++)
            {
                GoldPrice goldPrice =  _goldPriceRepo.GetLatestGoldPriceByGoldId(i);
                goldPrices.Add(goldPrice);
            }
            return goldPrices;
        }
    }
}
