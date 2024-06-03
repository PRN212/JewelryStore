
using Repositories.Entities;

namespace Repositories
{
    public class UserRepository
    {
        DataContext _context;
        public UserRepository(DataContext dataContext) 
        {
            _context = dataContext;
        }
        
        public User? CheckLogin(string username, string password)
        {
            return _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
        }
    }
}
