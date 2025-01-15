using Blog_API.ViewModels;
using Blog_API.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogAPI.Services
{
    public interface ICommentService
    {
        Task<List<CommentViewModel>> GetCommentsByArticleIdAsync(int articleId);
        Task<int> CreateCommentAsync(CommentViewModel commentViewModel, int articleId);
        Task UpdateCommentAsync(CommentViewModel commentViewModel);
        Task DeleteCommentAsync(int id);
    }
}