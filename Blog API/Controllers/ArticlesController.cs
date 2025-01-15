using Blog_API.ViewModels;
using BlogAPI.Services; // إضافة مساحة الاسم للخدمات
using Blog_API.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // تحديد مسار API
    public class ArticlesController : ControllerBase
    {
        private readonly IArticleService _articleService;

        public ArticlesController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArticleViewModel>>> GetArticles()
        {
            var articles = await _articleService.GetAllArticlesAsync();
            return Ok(articles);
        }

        [HttpGet("{id}")] // مسار لجلب مقالة بمعرفها
        public async Task<ActionResult<ArticleViewModel>> GetArticle(int id)
        {
            var article = await _articleService.GetArticleByIdAsync(id);

            if (article == null)
            {
                return NotFound();
            }

            return Ok(article);
        }

        [HttpPost]
        public async Task<ActionResult<int>> PostArticle(ArticleViewModel articleViewModel)
        {
            var articleId = await _articleService.CreateArticleAsync(articleViewModel);
            return CreatedAtAction(nameof(GetArticle), new { id = articleId }, articleId); // إرجاع 201 Created مع رابط للمقالة الجديدة
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutArticle(int id, ArticleViewModel articleViewModel)
        {
            if (id != articleViewModel.Id)
            {
                return BadRequest();
            }

            await _articleService.UpdateArticleAsync(articleViewModel);
            return NoContent(); // إرجاع 204 No Content
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            await _articleService.DeleteArticleAsync(id);
            return NoContent();
        }
    }
}