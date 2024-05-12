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

namespace CookBookBase.Controllers
{
   
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly CookBookDbContext _context;

        public ReportsController(CookBookDbContext context)
        {
            _context = context;
        }

        // GET: api/Reports
        [HttpGet]
        [Route("api/GetReports")]
        public async Task<ActionResult<IEnumerable<Report>>> GetReports()
        {
            return await _context.Reports.ToListAsync();
        }

        // GET: api/Reports/5
        [HttpGet("api/GetReport/{id}")]
        public async Task<ActionResult<Report>> GetReport(int id)
        {
            var report = await _context.Reports.FindAsync(id);
            if (report == null)
            {
                return NotFound();
            }

            var User = _context.Users.Where(e => e.Id == report.UseId).ToList();
            var Recipe = _context.Recipes.Where(e => e.Id == report.RecId).ToList();

            for (int i = 0; i < User.Count(); i++)
            {
                User[i].Reports = null;
                Recipe[i].Reports = null;
            }
            return report;
        }

        // PUT: api/Reports/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("api/EditReport/{id}\"")]
        public async Task<IActionResult> PutReport(int id, Report report)
        {
            if (id != report.Id)
            {
                return BadRequest();
            }

            _context.Entry(report).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReportExists(id))
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

        // POST: api/Reports
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        [Route("api/AddReport")]
        public async Task<ActionResult<Report>> PostLike(RedactedReport RedactedReport)
        {
            var UserReports = _context.Reports.Where(e => e.UseId == RedactedReport.UseId && e.RecId == RedactedReport.RecId).FirstOrDefault();
            var ReportedRecipe = _context.Recipes.Where(e => e.UseId == RedactedReport.UseId).FirstOrDefault();
            var report = new Report()
            {
                RecId = RedactedReport.RecId,
                UseId = RedactedReport.UseId
            };
            var Recipe = _context.Recipes.Where(e => e.Id == report.RecId).FirstOrDefault();
            if (ReportedRecipe != null)
            {
                return NoContent();
            }
            else if (UserReports != null)
            {
                _context.Reports.Remove(UserReports);
                Recipe.Reportsnum --;
                _context.Entry(Recipe).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return NoContent();
            }
            else
            {
                _context.Reports.Add(report);
                Recipe.Reportsnum++;
                _context.Entry(Recipe).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return NoContent();
            }
        }

        // DELETE: api/Reports/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReport(int id)
        {
            var report = await _context.Reports.FindAsync(id);
            if (report == null)
            {
                return NotFound();
            }

            _context.Reports.Remove(report);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReportExists(int id)
        {
            return _context.Reports.Any(e => e.Id == id);
        }
    }
}
