

using Repositories.Entities;

namespace Repositories.Specifications.Users
{
    public class UsersSpecification : BaseSpecification<User>
    {
        public UsersSpecification(UsersParam param) : 
            base (x => x.Username == param.Username && x.Password == param.Password) { }
    }
}
