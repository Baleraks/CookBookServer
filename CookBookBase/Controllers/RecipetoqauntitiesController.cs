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
    public class RecipetoqauntitiesController : ControllerBase
    {
        private readonly CookBookDbContext _context;

        public RecipetoqauntitiesController(CookBookDbContext context)
        {
            _context = context;
        }

        // GET: api/Recipetoqauntities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Recipetoqauntity>>> GetRecipetoqauntities()
        {
            return await _context.Recipetoqauntities.ToListAsync();
        }

        // GET: api/Recipetoqauntities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Recipetoqauntity>> GetRecipetoqauntity(int id)
        {
            var recipetoqauntity = await _context.Recipetoqauntities.FindAsync(id);

            if (recipetoqauntity == null)
            {
                return NotFound();
            }

            return recipetoqauntity;
        }

        // PUT: api/Recipetoqauntities/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRecipetoqauntity(int id, Recipetoqauntity recipetoqauntity)
        {
            if (id != recipetoqauntity.Id)
            {
                return BadRequest();
            }

            _context.Entry(recipetoqauntity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecipetoqauntityExists(id))
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

        // POST: api/Recipetoqauntities
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Recipetoqauntity>> PostRecipetoqauntity(Recipetoqauntity recipetoqauntity)
        {
            _context.Recipetoqauntities.Add(recipetoqauntity);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRecipetoqauntity", new { id = recipetoqauntity.Id }, recipetoqauntity);
        }

        // DELETE: api/Recipetoqauntities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipetoqauntity(int id)
        {
            var recipetoqauntity = await _context.Recipetoqauntities.FindAsync(id);
            if (recipetoqauntity == null)
            {
                return NotFound();
            }

            _context.Recipetoqauntities.Remove(recipetoqauntity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RecipetoqauntityExists(int id)
        {
            return _context.Recipetoqauntities.Any(e => e.Id == id);
        }
    }
}
