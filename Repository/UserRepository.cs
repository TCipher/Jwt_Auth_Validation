using Microsoft.EntityFrameworkCore;
using SimpleToDoApi.Data;
using SimpleToDoApi.Models;
using SimpleToDoApi.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleToDoApi.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<int> AddUser(UserDto model)
        {
            var userEntity = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber
            };

            var user = await _context.Users.AddAsync(userEntity);
            await _context.SaveChangesAsync();
            return user.Entity.Id;
        }


        public async Task<bool> UpdateUser(int id, UserDto model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(usr => usr.Id == id);
            if (user == null)
                return false;

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<string> DeleteUser(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
                return "User not found!";

            _context.Remove(user);
            await _context.SaveChangesAsync();
            return "User deleted successfully";
        }

        public async Task<User> GetUserById(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
                return null;
            return user;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (user == null)
                return null;
            return user;
        }

        public async Task<List<User>> GetAllUsers(PageQueryHelper pageQuery)
        {
            var users =  _context.Users.AsQueryable();
            var count = users.Count();

            int pageSize = pageQuery.PageSize;
            int currentPage = pageQuery.PageNumber;

            int totalPages = (int)Math.Ceiling(count / (double)pageSize);

            var pagedUser = users.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            if (!pagedUser.Any())
                return null;
            return pagedUser;
        }
    }
}
