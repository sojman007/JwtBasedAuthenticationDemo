using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JwtAuthenticationAndIdentityDemo;
using JwtAuthenticationAndIdentityDemo.DatabaseEntities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace JwtAuthenticationAndIdentityDemo.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    
    public class UserTasksController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly SignInManager<CustomUser> _signInManager;

        public UserTasksController(DatabaseContext context , SignInManager<CustomUser> man)
        {
            _context = context;
            _signInManager = man;
        }

        // GET: api/UserTasks
        [HttpGet]
        
        public async Task<ActionResult<IEnumerable<UserTask>>> GetUserTasks()
        {
            return await _context.UserTasks.ToListAsync();
        }

        // GET: api/UserTasks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserTask>> GetUserTask(string id)
        {
            var userTask = await _context.UserTasks.FindAsync(id);

            if (userTask == null)
            {
                return NotFound();
            }

            return userTask;
        }

        // PUT: api/UserTasks/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserTask(string id, UserTask userTask)
        {
            if (id != userTask.Id)
            {
                return BadRequest();
            }

            _context.Entry(userTask).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserTaskExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/UserTasks
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<UserTask>> PostUserTask(UserTask userTask)
        {
            //TO-DO => insert the User Id for the current Task by accessing the currently 
            //logged in user's details
            _context.UserTasks.Add(userTask);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserTaskExists(userTask.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUserTask", new { id = userTask.Id }, userTask);
        }

        // DELETE: api/UserTasks/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserTask>> DeleteUserTask(string id)
        {
            var userTask = await _context.UserTasks.FindAsync(id);
            if (userTask == null)
            {
                return NotFound();
            }

            _context.UserTasks.Remove(userTask);
            await _context.SaveChangesAsync();

            return userTask;
        }

        private bool UserTaskExists(string id)
        {
            return _context.UserTasks.Any(e => e.Id == id);
        }
    }
}
