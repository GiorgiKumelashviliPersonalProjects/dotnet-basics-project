using API.Data;
using API.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BuggyController : BaseApiController
    {
        private readonly DataContext _context;

        public BuggyController(DataContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetSecret(RegisterDto registerDto)
        {
            return "s";
        }

        [HttpGet("not-found")]
        public dynamic GetNotFound()
        {
            var thing = _context.Users.Find(-1);
            
            if (thing is null) return NotFound();

            return Ok(thing);
        }
        
        [HttpGet("bad-request")]
        public dynamic GetBadRequest()
        {
            return BadRequest("this is bad request");
        }
        
        [HttpGet("server-error")]
        public dynamic GetServerError()
        {
            var thing = _context.Users.Find(-1);

            // will cause server error
            var thingReturned = thing.ToString();

            return thingReturned;
        }
    }
}