using Blog_API.ViewModels;
using BlogAPI.Services;
using Blog_API.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogAPI.Controllers
{
    [ApiController]
    [Route("api/articles/{articleId}/comments")] // مسار API يعتمد على معرف المقالة
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommentViewModel>>> GetComments(int articleId)
        {
            var comments = await _commentService.GetCommentsByArticleIdAsync(articleId);
            return Ok(comments);
        }

        [HttpPost]
        public async Task<ActionResult<int>> PostComment(int articleId, CommentViewModel commentViewModel)
        {
            var commentId = await _commentService.CreateCommentAsync(commentViewModel, articleId);
            return CreatedAtAction(nameof(GetComment), new { articleId, id = commentId }, commentId);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<CommentViewModel>> GetComment(int articleId, int id)
        {
            var comments = await _commentService.GetCommentsByArticleIdAsync(articleId);
            var comment = comments.Find(c => c.Id == id);
            if (comment == null)
                return NotFound();
            return Ok(comment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutComment(int id, CommentViewModel commentViewModel)
        {
            if (id != commentViewModel.Id)
            {
                return BadRequest();
            }

            await _commentService.UpdateCommentAsync(commentViewModel);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            await _commentService.DeleteCommentAsync(id);
            return NoContent();
        }
    }
}