
using Repositories;
using Repositories.Entities;

namespace Services
{
	public class GoldService
	{
		private readonly GoldRepository _goldRepo;
		public GoldService(GoldRepository goldRepo)
		{
			_goldRepo = goldRepo;
		}
		public async Task<IEnumerable<Gold>> GetAllGoldTypeAsync()
		{
			return await _goldRepo.GetAllGoldAsync();
		}

		public Gold? GetGoldById(int id)
		{
			return _goldRepo.GetById(id);
		}

		public void UpdateGold(Gold gold)
		{
			_goldRepo.UpdateGold(gold);
		}
	}
}
