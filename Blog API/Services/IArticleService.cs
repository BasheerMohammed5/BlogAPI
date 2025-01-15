using Blog_API.ViewModels;
using Blog_API.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogAPI.Services
{
    public interface IArticleService
    {
        Task<List<ArticleViewModel>> GetAllArticlesAsync();
        Task<ArticleViewModel> GetArticleByIdAsync(int id);
        Task<int> CreateArticleAsync(ArticleViewModel articleViewModel);
        Task UpdateArticleAsync(ArticleViewModel articleViewModel);
        Task DeleteArticleAsync(int id);
    }
}