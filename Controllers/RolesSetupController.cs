using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SimpleToDoApi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleToDoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesSetupController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<RolesSetupController> _logger;
        public RolesSetupController(
             ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
             RoleManager<IdentityRole> roleManager,
             ILogger<RolesSetupController> logger
            )
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        [Route("GetAllRoles")]

        public IActionResult GetAllRoles()
        {
            var roles = _roleManager.Roles.ToList();
            return Ok(roles);
        }

        [HttpPost]
        [Route("CreateRoles")]
        public async Task<IActionResult> CreateRoles(string name)
        {
            //Check if the role exist
            var roleExist = await _roleManager.RoleExistsAsync(name);
            if (!roleExist)// checks on the role exist status
            {
                //we need to check if the role has been added successfully
                var roleResult = await _roleManager.CreateAsync(new IdentityRole(name));
                //we need to check if the role has been added successfully

                if (roleResult.Succeeded)
                {
                    _logger.LogInformation($"The Role {name} has been added successfully");
                    return Ok(new
                    {
                        result = $"The role {name} has been added successfully"
                    });
                }
                else
                {
                    _logger.LogInformation($"The Role {name} has not been added successfully");
                    return BadRequest(new
                    {
                        result = $"The role {name} has not  been added successfully"
                    });

                }

            }
            return BadRequest(new { erro = "Role already exist" });
        }

        [HttpGet]
        [Route("GetAllUsers")]

        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            return Ok(users);
        }

        [HttpPost]
        [Route("AddUserToRole")]
        public async Task<IActionResult> AddUserToRoles(string email, string roleName)
        {
            //Check if the user exist
            var user = await _userManager.FindByEmailAsync(email);
            if(user == null) //User does not exist
            {
                _logger.LogInformation($"The user with the {email} does not exist");
                return BadRequest(new
                    {
                        error = "User does not exist"
                   
                });
            }
            //Check if the role exist

            var roleExist = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExist)// checks on the role exist status
            {
                _logger.LogInformation($"The role with the {email} does not exist");
                return BadRequest(new
                {
                    error = "Role does not exist"

                });
            }
            //if the user exist, then add the role to the user
            var result = await _userManager.AddToRoleAsync(user, roleName);
            //Check if the user is assigned to the role successfully
            if (result.Succeeded)
            {
                return Ok(new
                {
                    result = "Success ,user has been addes to the role"
                });
               
            }
            else
            {
                _logger.LogInformation($"The user was not able to be added to the role");
                return BadRequest(new
                {
                    error = "The user was not able to be added to the role"

                });
            }
        }
        [HttpGet]
        [Route("GetUserRoles")]
        public async Task<IActionResult> GetUserRole(string email)
        {
            //check if the email is valid
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) //User does not exist
            {
                _logger.LogInformation($"The user with the {email} does not exist");
                return BadRequest(new
                {
                    error = "User does not exist"

                });
            }
            // return the roles
            var roles = await _userManager.GetRolesAsync(user);
            return Ok(roles);
        }

        [HttpPost]
        [Route("RemoveUserFromRole")]
        public async Task<IActionResult> RemoveUserFromRole(string email, string roleName)
        {
            //Check if the user exist
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) //User does not exist
            {
                _logger.LogInformation($"The user with the {email} does not exist");
                return BadRequest(new
                {
                    error = "User does not exist"

                });
            }
            //Check if the role exist

            var roleExist = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExist)// checks on the role exist status
            {
                _logger.LogInformation($"The role with the {email} does not exist");
                return BadRequest(new
                {
                    error = "Role does not exist"

                });
            }

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                return Ok(new
                {
                    result = $"User {email} has been removed from the role {roleName}"
                });
            }
            return BadRequest(new
            {
                error = $"Unable to remove User {email} from rolw{roleName}"

            });
        }

    }
}
