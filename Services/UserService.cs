using Repositories.Entities;
using Repositories;

namespace Services
{
    public class UserService
    {
        private readonly UserRepository _repository;
        public UserService (UserRepository repository)
        {
            _repository = repository;
        }
        public User? CheckLogin(string username, string password)
        {
            return _repository.CheckLogin(username, password);
        }
    }
}
