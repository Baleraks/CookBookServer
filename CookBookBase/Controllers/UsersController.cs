using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CookBookBase;
using CookBookBase.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis.Scripting;
using System.Security.Policy;



namespace CookBookBase.Controllers
{
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly CookBookDbContext _context;
        PasswordHasher passwordHasher = new PasswordHasher();

        public UsersController(CookBookDbContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [Authorize]
        [HttpGet]
        [Route("api/GetUsers")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        //[Authorize]
        [HttpGet("api/GetUser/{id}")]
        //[Route("api/GetUser")]
        public async Task<ActionResult<RedactedUser>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var Likes = _context.Likes.Where(e => e.UseId == user.Id).ToList();
            var Recipes = _context.Recipes.Where(e => e.UseId == user.Id).ToList();
            var Comments = _context.Comments.Where(e => e.UseId == user.Id).ToList();

            for(int i = 0; i< Comments.Count(); i++)
            {
                if (Comments[i].Id == Comments[i].Firstcommentid)
                {
                    Comments[i].Firstcomment = null;
                    Comments[i].InverseFirstcomment = null;
                }
                Comments[i].Use = null;
            }

            for (int i = 0; i < Likes.Count(); i++)
            {
                Likes[i].Use = null;
                var likeRecipes = _context.Recipes.Where(e => e.Id == Likes[i].RecId).ToList();
                for (int j = 0; j < likeRecipes.Count(); j++)
                {
                    likeRecipes[j].LikesNavigation = null;
                }
            }

            for (int i = 0; i < Recipes.Count; i++)
            {
                Recipes[i].Use = null;
            }

            var NewUser = new RedactedUser()
            {
                Id = user.Id,
                Nick = user.Nick,
                Comments= Comments,
                Likes = Likes,
                Recipes = Recipes,
                Reports = user.Reports

            };
            return NewUser ;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("api/ChangeUser/{id}")]
        //[Route("api/ChangeUser")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Route("api/Register")]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            user.Hashpassword = passwordHasher.HashPassword(user.Hashpassword);
            var resultLoginCheck = _context.Users
                  .Where(e => e.Nick == user.Nick).FirstOrDefault();
            if (resultLoginCheck != null)
            {
                return BadRequest("Invalid Credentials");
            }
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("api/DeleteUser/{id}")]
        //[Route("api/DeleteUser")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
