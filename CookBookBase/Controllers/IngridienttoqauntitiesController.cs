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
    public class IngridienttoqauntitiesController : ControllerBase
    {
        private readonly CookBookDbContext _context;

        public IngridienttoqauntitiesController(CookBookDbContext context)
        {
            _context = context;
        }

        // GET: api/Ingridienttoqauntities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ingridienttoqauntity>>> GetIngridienttoqauntities()
        {
            return await _context.Ingridienttoqauntities.ToListAsync();
        }

        // GET: api/Ingridienttoqauntities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ingridienttoqauntity>> GetIngridienttoqauntity(int id)
        {
            var ingridienttoqauntity = await _context.Ingridienttoqauntities.FindAsync(id);

            if (ingridienttoqauntity == null)
            {
                return NotFound();
            }

            return ingridienttoqauntity;
        }

        // PUT: api/Ingridienttoqauntities/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIngridienttoqauntity(int id, Ingridienttoqauntity ingridienttoqauntity)
        {
            if (id != ingridienttoqauntity.Id)
            {
                return BadRequest();
            }

            _context.Entry(ingridienttoqauntity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IngridienttoqauntityExists(id))
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

        // POST: api/Ingridienttoqauntities
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Ingridienttoqauntity>> PostIngridienttoqauntity(Ingridienttoqauntity ingridienttoqauntity)
        {
            _context.Ingridienttoqauntities.Add(ingridienttoqauntity);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIngridienttoqauntity", new { id = ingridienttoqauntity.Id }, ingridienttoqauntity);
        }

        // DELETE: api/Ingridienttoqauntities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIngridienttoqauntity(int id)
        {
            var ingridienttoqauntity = await _context.Ingridienttoqauntities.FindAsync(id);
            if (ingridienttoqauntity == null)
            {
                return NotFound();
            }

            _context.Ingridienttoqauntities.Remove(ingridienttoqauntity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool IngridienttoqauntityExists(int id)
        {
            return _context.Ingridienttoqauntities.Any(e => e.Id == id);
        }
    }
}
