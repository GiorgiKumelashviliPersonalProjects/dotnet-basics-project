using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AdminController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;

        public AdminController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        //TODO xx
        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("users-with-admin-roles")]
        public async Task<ActionResult> AdminRole()
        {
            var users = await _userManager.Users
                .Include(user => user.UserRoles)
                .ThenInclude(role => role.Role)
                .OrderBy(user => user.UserName)
                .Select(user => new
                {
                    user.Id,
                    Username = user.UserName,
                    Roles = user.UserRoles.Select(r => r.Role.Name).ToList()
                })
                .ToListAsync();

            return Ok(users);
        }

        [Authorize(Policy = "RequireModeratorRole")]
        [HttpGet("users-with-moderator-roles")]
        public ActionResult ModeratorRole()
        {
            return Ok("For moderators");
        }

        [Authorize(Policy = "RequireMemberRole")]
        [HttpGet("users-with-member-roles")]
        public ActionResult MemberRole()
        {
            return Ok("For members");
        }
    }
}