using Microsoft.AspNetCore.Mvc;
using SimpleToDoApi.Models.Dto;
using SimpleToDoApi.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleToDoApi.Controllers
{
     [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        [HttpPost]
        [Route("AddUser")]
        public async Task<IActionResult> AddUser(UserDto model)
        {
            var result = await _userRepository.AddUser(model);
            if (result <= 0)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPatch("{id}")]
       // [Route("UpdateUser")]
        public async Task<IActionResult> UpdateUser(int id, UserDto model)
        {
            var user = await _userRepository.UpdateUser(id, model);
            if (user == false)
                return BadRequest();
            return Ok(model);
        }   
        [HttpDelete]
       // [Route("DeleteUser")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userRepository.DeleteUser(id);
            if(result == "User not found!")
                return BadRequest(result);
            return Ok(result);

        }

        [HttpGet("{id:int}")]
      //  [Route("GetUserById")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userRepository.GetUserById(id);
            if(user == null)
                return NotFound();
            return Ok(user);
        }

        [HttpGet("{email}")]
       // [Route("GetUserByEmail")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var user = await _userRepository.GetUserByEmail(email);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        [HttpGet]
       // [Route("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers([FromQuery] int pageNum, [FromQuery]int pageSize)
        {
            var users = await _userRepository.GetAllUsers(new PageQueryHelper
            {
                PageNumber = pageNum,
                PageSize = pageSize
            });
            if (users == null)
                return NotFound();
            return Ok(users);
        }
    }
}
