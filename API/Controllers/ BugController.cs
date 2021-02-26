using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BugController : BasicController
    {
        private readonly DataContext _dataContext;
        public BugController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet("server-error")]
        public ActionResult<AppUser> GetServerError()
        {
            var thing = _dataContext.Users.Find(-1);
            string str = thing.ToString();
            return thing;
        }

        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest()
        {
            return BadRequest("not a request");
        }

        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetEasterEgg()
        {
            return "egg";
        }  

        [HttpGet("not-found")]
        public ActionResult<AppUser> GetNotFound()
        {
            var thing = _dataContext.Users.Find(-1);
            if(thing == null)
                return NotFound();
            return Ok(thing);        
        }              
    }
}