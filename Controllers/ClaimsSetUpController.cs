using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimpleToDoApi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SimpleToDoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimsSetUpController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<ClaimsSetUpController> _logger;
        public ClaimsSetUpController(
             ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
             RoleManager<IdentityRole> roleManager,
             ILogger<ClaimsSetUpController> logger
            )
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        [Route("GetAllClaims")]
        public async Task<IActionResult> GetAllClaims(string email)
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
            // Get all claims
            var userClaims = await _userManager.GetClaimsAsync(user);
            return Ok(userClaims);
        }
        [HttpPost]
        [Route("AddClaimsToUser")]
        public async Task<IActionResult> AddClaimsToUser(string email, string claimName, string claimValue)
        {
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

                var userClaim = new Claim(claimName, claimValue);
                var result = await _userManager.AddClaimAsync(user, userClaim);
                if (result.Succeeded)
                {
                    return Ok(new
                    {
                        result = $"User {user.Email} has a claim {claimName} added to them"
                    });
                }
                return BadRequest(new
                {
                    error = $"Unable to add claim {claimName}to the user {user.Email}"

                });
            }
        }
    }
}
