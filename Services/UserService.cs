using Repositories.Entities;
using Repositories;
using Repositories.IRepositories;
using Repositories.Specifications.Users;

namespace Services
{
    public class UserService
    {
        private readonly IGenericRepository<User> _repository;
        public UserService (IGenericRepository<User> repository)
        {
            _repository = repository;
        }
        public async Task<User?> CheckLogin(string username, string password)
        {
            var spec = new UsersSpecification(new UsersParam()
            {
                Username = username,
                Password = password
            });
            return await _repository.GetEntityWithSpec(spec);
        }
    }
}
