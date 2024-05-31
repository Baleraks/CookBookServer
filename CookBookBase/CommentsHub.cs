using CookBookBase.Models;
using CookBookBase;
using Microsoft.AspNetCore.SignalR;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;


public class CommentsHub : Hub
{
    private readonly CookBookDbContext _context;

    public CommentsHub(CookBookDbContext context)
    {
        _context = context;
    }

    public override async Task OnConnectedAsync()
    {
        Console.WriteLine($"Client connected: {Context.ConnectionId}");
        await base.OnConnectedAsync();
    }

    public async Task GetComments()
    {
        var comments = _context.Comments.ToListAsync();
        await Clients.Caller.SendAsync("ReceiveComments", comments);    
    }

    public async Task<IEnumerable<RedactedComment>> GetComment(int id)
    {
        var comments = await _context.Comments
            .Where(e => e.RecId == id)
            .Include(c => c.Use) // Убедитесь, что в контексте включены связанные сущности
            .ToListAsync();

        if (comments == null || !comments.Any())
        {
            return new List<RedactedComment>(); // Возвращаем пустой список, если комментарии не найдены
        }

        List<RedactedComment> result = new List<RedactedComment>();

        foreach (var comment in comments)
        {
            var redactedComment = new RedactedComment
            {
                Commenttext = comment.Commenttext,
                RecId = comment.RecId,
                UseId = comment.UseId,
                Firstcommentid = comment.Firstcommentid,
                UserNick = comment.Use.Nick,
                Id = comment.Id
            };
            result.Add(redactedComment);
        }

        return result;
    }

    public async Task EditComment(int id, Comment comment)
    {
        if (id != comment.Id)
        {
            throw new ArgumentException("Comment ID does not match the provided ID");
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
                throw new KeyNotFoundException("Comment not found");
            }
            else
            {
                throw;
            }
        }
    }

    public async Task AddComment(RedactedComment redactedComment)
    {
            var comment = new Comment
            {
                Commenttext = redactedComment.Commenttext,
                Firstcommentid = redactedComment.Firstcommentid,
                RecId = redactedComment.RecId,
                UseId = redactedComment.UseId
            };

            if (redactedComment.Firstcommentid == null)
            {
                var recipeComments = _context.Comments.Where(e => e.RecId == redactedComment.RecId);
                var lastComment = await recipeComments.OrderByDescending(c => c.Id).FirstOrDefaultAsync();
                if (lastComment != null)
                {
                    comment.Firstcommentid = lastComment.Id + 1;
                }
                else
                {
                    comment.Firstcommentid = comment.Id;
                }
            }

            _context.Comments.Add(comment);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Вы можете логировать исключение или выбрасывать его дальше
                throw;
            }
    }

    public async Task DeleteComment(int id)
    {
        var comment = await _context.Comments.FindAsync(id);
        if (comment == null)
        {
            throw new KeyNotFoundException("Comment not found");
        }

        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();
    }

    private bool CommentExists(int id)
    {
        return _context.Comments.Any(e => e.Id == id);
    }
}

