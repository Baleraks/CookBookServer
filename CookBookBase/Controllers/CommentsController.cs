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
    public class CommentsController : ControllerBase
    {
        private readonly CookBookDbContext _context;

        public CommentsController(CookBookDbContext context)
        {
            _context = context;
        }

        // GET: api/Comments
        [HttpGet]
        [Route("api/GetComments")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetComments()
        {
            return await _context.Comments.ToListAsync();
        }

        // GET: api/Comments/5
        [HttpGet("api/GetComment/{id}")]
        public async Task<ActionResult<IEnumerable<RedactedComment>>> GetComment(int id)
        {
            var comment = _context.Comments.Where(e => e.RecId == id).ToList();
            if (comment == null)
            {
                return NotFound();
            }


            for (int i = 0; i < comment.Count; i++)
            {
                if (comment[i].Id == comment[i].Firstcommentid)
                {
                    comment[i].Firstcomment = null;
                    comment[i].InverseFirstcomment = null;
                }

                _context.Users.Where(e => e.Id == comment[i].UseId).First();
            }

            List<RedactedComment> result = new();

            foreach (var item in comment)
            {
                var a = new RedactedComment();
                a.Commenttext = item.Commenttext;
                a.RecId = item.RecId;
                a.UseId = item.UseId;
                a.Firstcommentid = item.Firstcommentid;
                a.UserNick = item.Use.Nick;
                a.Id = item.Id;
                result.Add(a);
            }
            return result;
        }

        // PUT: api/Comments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("api/EditComment/{id}")]
        public async Task<IActionResult> PutComment(int id, Comment comment)
        {
            if (id != comment.Id)
            {
                return BadRequest();
            }

            _context.Entry(comment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentExists(id))
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

        // POST: api/Comments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        [Route("api/AddComment")]
        public async Task<ActionResult<Comment>> PostComment(RedactedComment RedactedComment)
        {
            var comment = new Comment()
            {
                Commenttext = RedactedComment.Commenttext,
                Firstcommentid = RedactedComment.Firstcommentid,
                RecId = RedactedComment.RecId,
                UseId = RedactedComment.UseId
            };
            _context.Comments.Add(comment);

            if (RedactedComment.Firstcommentid == null)
            {
                var RecipeComments = _context.Comments.Where(e => e.RecId == RedactedComment.RecId);
                var lastComment = await RecipeComments.OrderByDescending(c => c.Id).FirstOrDefaultAsync();
                if (lastComment != null)
                {
                    comment.Firstcommentid = lastComment.Id + 1;
                }
                else
                {
                    comment.Firstcommentid = comment.Id;
                }
            }
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Comments/5
        [HttpDelete("api/DeleteComment/{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CommentExists(int id)
        {
            return _context.Comments.Any(e => e.Id == id);
        }
    }
}
