using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POSUNO.Api.Data.Entities;
using POSUNO.Api.Models;

namespace POSUNO.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly DataContext context;

        public AccountController(DataContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await this.context.Users.ToListAsync();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            User user = await this.context.Users.FindAsync(id);

            if (user == null)
            {
                return this.NotFound();
            }

            return user;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return this.BadRequest();
            }

            this.context.Entry(user).State = EntityState.Modified;

            try
            {
                await this.context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!this.UserExists(id))
                {
                    return this.NotFound();
                }
                else
                {
                    throw;
                }
            }

            return this.NoContent();
        }


        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<User>> Login(LoginRequest loginRequest)
        {
            return await this.context.Users.FirstOrDefaultAsync(userLogin => userLogin.Email == loginRequest.Email && userLogin.Password == loginRequest.Password);
        }
        
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            this.context.Users.Add(user);
            await this.context.SaveChangesAsync();

            return this.CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await this.context.Users.FindAsync(id);
            if (user == null)
            {
                return this.NotFound();
            }

            this.context.Users.Remove(user);
            await this.context.SaveChangesAsync();

            return this.NoContent();
        }

        private bool UserExists(int id)
        {
            return this.context.Users.Any(e => e.Id == id);
        }
    }
}
