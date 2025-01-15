using Blog_API.Models;
using Blog_API.ViewModels;
using Blog_API.Data; // إضافة مساحة الاسم لسياق قاعدة البيانات
using Microsoft.EntityFrameworkCore; // لإستخدام ToListAsync
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog_API.Data;
using Blog_API.Models;
using Blog_API.ViewModels;

namespace Blog_API.Services
{
    public interface IArticleService // تعريف واجهة للخدمة (ممارسة جيدة)
    {
        Task<List<ArticleViewModel>> GetAllArticlesAsync();
        Task<ArticleViewModel> GetArticleByIdAsync(int id);
        Task<int> CreateArticleAsync(ArticleViewModel articleViewModel);
        Task UpdateArticleAsync(ArticleViewModel articleViewModel);
        Task DeleteArticleAsync(int id);
    }

    public class ArticleService : IArticleService
    {
        private readonly ApplicationDbContext _context;

        public ArticleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ArticleViewModel>> GetAllArticlesAsync()
        {
            return await _context.Articles
                .Include(a => a.Comments) // جلب التعليقات مع المقالات
                .Select(a => new ArticleViewModel
                {
                    Id = a.Id,
                    Title = a.Title,
                    Content = a.Content,
                    CreatedAt = a.CreatedAt,
                    Comments = a.Comments.Select(c => new CommentViewModel
                    {
                        Id = c.Id,
                        Text = c.Text,
                        CreatedAt = c.CreatedAt
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<ArticleViewModel> GetArticleByIdAsync(int id)
        {
            var article = await _context.Articles
                .Include(a => a.Comments) // جلب التعليقات مع المقالة
                .FirstOrDefaultAsync(a => a.Id == id);

            if (article == null)
            {
                return null;
            }

            return new ArticleViewModel
            {
                Id = article.Id,
                Title = article.Title,
                Content = article.Content,
                CreatedAt = article.CreatedAt,
                Comments = article.Comments.Select(c => new CommentViewModel
                {
                    Id = c.Id,
                    Text = c.Text,
                    CreatedAt = c.CreatedAt
                }).ToList()
            };
        }

        public async Task<int> CreateArticleAsync(ArticleViewModel articleViewModel)
        {
            var article = new Article
            {
                Title = articleViewModel.Title,
                Content = articleViewModel.Content,
                CreatedAt = DateTime.Now
            };

            _context.Articles.Add(article);
            await _context.SaveChangesAsync();
            return article.Id; // إرجاع معرف المقالة التي تم إنشاؤها
        }

        public async Task UpdateArticleAsync(ArticleViewModel articleViewModel)
        {
            var article = await _context.Articles.FindAsync(articleViewModel.Id);

            if (article != null)
            {
                article.Title = articleViewModel.Title;
                article.Content = articleViewModel.Content;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteArticleAsync(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article != null)
            {
                _context.Articles.Remove(article);
                await _context.SaveChangesAsync();
            }
        }
    }
}