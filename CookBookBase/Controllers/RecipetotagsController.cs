using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CookBookBase;
using CookBookBase.Models;

namespace CookBookBase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipetotagsController : ControllerBase
    {
        private readonly CookBookDbContext _context;

        public RecipetotagsController(CookBookDbContext context)
        {
            _context = context;
        }

        // GET: api/Recipetotags
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Recipetotag>>> GetRecipetotags()
        {
            return await _context.Recipetotags.ToListAsync();
        }

        // GET: api/Recipetotags/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Recipetotag>> GetRecipetotag(int id)
        {
            var recipetotag = await _context.Recipetotags.FindAsync(id);

            if (recipetotag == null)
            {
                return NotFound();
            }

            return recipetotag;
        }

        // PUT: api/Recipetotags/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRecipetotag(int id, Recipetotag recipetotag)
        {
            if (id != recipetotag.Id)
            {
                return BadRequest();
            }

            _context.Entry(recipetotag).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecipetotagExists(id))
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

        // POST: api/Recipetotags
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Recipetotag>> PostRecipetotag(Recipetotag recipetotag)
        {
            _context.Recipetotags.Add(recipetotag);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRecipetotag", new { id = recipetotag.Id }, recipetotag);
        }

        // DELETE: api/Recipetotags/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipetotag(int id)
        {
            var recipetotag = await _context.Recipetotags.FindAsync(id);
            if (recipetotag == null)
            {
                return NotFound();
            }

            _context.Recipetotags.Remove(recipetotag);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RecipetotagExists(int id)
        {
            return _context.Recipetotags.Any(e => e.Id == id);
        }
    }
}
