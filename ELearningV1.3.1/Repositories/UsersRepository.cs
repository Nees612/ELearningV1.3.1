using ELearningV1._3._1.Contexts;
using ELearningV1._3._1.Interfaces;
using ELearningV1._3._1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using ELearningV1._3._1.ViewModels;

namespace ELearningV1._3._1.Repositories
{
    public class UsersRepository : Repository<User>, IUsersRepository
    {
        public UsersRepository(ApiContext context) : base(context)
        {
        }

        public async Task<User> GetUserByUserName(string userName)
        {
            try
            {
                return await _dbSet.FirstAsync(u => u.UserName.Equals(userName));
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public async Task<User> GetUserById(string Id)
        {
            try
            {
                return await _dbSet.FirstAsync(u => u.Id.Equals(Id));
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public async Task<string> GetUserRoleByUserName(string userName)
        {
            try
            {
                return (await _dbSet.FirstAsync(u => u.UserName.Equals(userName))).Role;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public async Task<IEnumerable<User>> GetUsersByRole(string role)
        {
            return await _dbSet.Where(u => u.Role.Equals(role)).Select(u => new User
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                Role = u.Role
            }).ToListAsync();
        }

        public async Task<User> GetUserByEmail(string Email)
        {
            try
            {
                return await _dbSet.FirstAsync(u => u.Email.Equals(Email));
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public async Task<IDictionary<string, string>> UpdateUser(UserUpdateViewModel UserInfo, string Id)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();

            var User = await GetUserById(Id);

            if (UserInfo.UserName != User.UserName)
            {
                if (await GetUserByUserName(UserInfo.UserName) == null)
                {
                    User.UserName = (UserInfo.UserName == User.UserName ? User.UserName : UserInfo.UserName);
                    User.NormalizedUserName = (UserInfo.UserName.ToUpper() == User.NormalizedUserName ? User.NormalizedUserName : UserInfo.UserName.ToUpper());
                }
                else
                {
                    errors.Add("UserName", "Username is already in use.");
                }
            }

            if (UserInfo.Email != User.Email)
            {
                if (await GetUserByEmail(UserInfo.Email) == null)
                {
                    User.Email = (UserInfo.Email == User.UserName ? User.UserName : UserInfo.Email);
                    User.NormalizedEmail = (UserInfo.Email.ToUpper() == User.NormalizedEmail ? User.NormalizedEmail : UserInfo.Email.ToUpper());
                }
                else
                {
                    errors.Add("Email", "Email is already in use.");
                }
            }

            if (UserInfo.PhoneNumber != User.PhoneNumber)
            {
                User.PhoneNumber = (UserInfo.PhoneNumber == User.PhoneNumber ? User.PhoneNumber : UserInfo.PhoneNumber);
            }

            if (errors.Count < 1)
            {
                _dbSet.Update(User);
                return null;
            }
            return errors;
        }
    }
}
