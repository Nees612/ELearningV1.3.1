using ELearningV1._3._1.Models;
using ELearningV1._3._1.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELearningV1._3._1.Interfaces
{
    public interface IUsersRepository : IRepository<User>
    {
        Task<IEnumerable<User>> GetUsersByRole(string Role);
        Task<User> GetUserByUserName(string UserName);
        Task<string> GetUserRoleByUserName(string UserName);
        Task<User> GetUserByEmail(string Email);
        Task<ErrorContext> UpdateUser(UserUpdateViewModel UserInfo, User User);
    }
}
