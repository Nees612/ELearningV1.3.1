using ELearningV1._3._1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELearningV1._3._1.Interfaces
{
    public interface IUsersRepository : IRepository<User>
    {
        Task<IEnumerable<User>> GetUsersByRole(string Role);
        User GetUserByUserName(string userName);
        string GetUserRoleByUserName(string userName);
    }
}
