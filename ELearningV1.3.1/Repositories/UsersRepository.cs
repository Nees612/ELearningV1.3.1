using ELearningV1._3._1.Contexts;
using ELearningV1._3._1.Interfaces;
using ELearningV1._3._1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ELearningV1._3._1.Repositories
{
    public class UsersRepository : Repository<User>, IUsersRepository
    {
        public UsersRepository(ApiContext context) : base(context)
        {
        }

        public User GetUserByUserName(string userName)
        {
            return _dbSet.First(u => u.UserName.Equals(userName));
        }

        public string GetUserRoleByUserName(string userName)
        {
            try
            {
                var User = _dbSet.First(u => u.UserName.Equals(userName));
                return User.Role;

            }
            catch (System.InvalidOperationException)
            {
                return null;
            }
        }

        public async Task<IEnumerable<User>> GetUsersByRole(string role)
        {
            return await _dbSet.Where(u => u.Role.Equals(role)).Select(u => new User
            {
                UserName = u.UserName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                Role = u.Role
            }).ToListAsync();
        }
    }
}
