using SimpleToDoApi.Models;
using SimpleToDoApi.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleToDoApi.Repository
{
   public  interface IUserRepository
    {
         Task<int> AddUser(UserDto model);
        Task<bool> UpdateUser(int id, UserDto model);
        Task<string> DeleteUser(int id);
        Task<User> GetUserById(int id);
        Task<User> GetUserByEmail(string email);
        Task<List<User>> GetAllUsers(PageQueryHelper pageHelper);

    }
}
