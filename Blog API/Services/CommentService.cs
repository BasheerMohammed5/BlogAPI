using Blog_API.Models;
using Blog_API.ViewModels;
using Blog_API.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog_API.Data;
using Blog_API.Models;
using Blog_API.ViewModels;

namespace Blog_API.Services
{
    public interface ICommentService
    {
        Task<List<CommentViewModel>> GetCommentsByArticleIdAsync(int articleId);
        Task<int> CreateCommentAsync(CommentViewModel commentViewModel, int articleId);
        Task UpdateCommentAsync(CommentViewModel commentViewModel);
        Task DeleteCommentAsync(int id);
    }

    public class CommentService : ICommentService
    {
        private readonly ApplicationDbContext _context;

        public CommentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CommentViewModel>> GetCommentsByArticleIdAsync(int articleId)
        {
            return await _context.Comments
                .Where(c => c.ArticleId == articleId)
                .Select(c => new CommentViewModel
                {
                    Id = c.Id,
                    Text = c.Text,
                    CreatedAt = c.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<int> CreateCommentAsync(CommentViewModel commentViewModel, int articleId)
        {
            var comment = new Comment
            {
                Text = commentViewModel.Text,
                CreatedAt = DateTime.Now,
                ArticleId = articleId
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return comment.Id;
        }

        public async Task UpdateCommentAsync(CommentViewModel commentViewModel)
        {
            var comment = await _context.Comments.FindAsync(commentViewModel.Id);
            if (comment != null)
            {
                comment.Text = commentViewModel.Text;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteCommentAsync(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
            }
        }
    }
}