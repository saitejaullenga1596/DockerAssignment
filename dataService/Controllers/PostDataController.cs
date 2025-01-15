using dataService.Interface;
using dataService.Model;
using Microsoft.AspNetCore.Mvc;

namespace dataService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostDataController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostDataController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpPost("{id}")]
        public async Task<ActionResult> PostDataAsync([FromRoute] int id, [FromBody] Post model)
        {
            try
            {
                if (id != model.Id)
                {
                    return BadRequest($"id : {id} in route doesn't matches with model.Id: {model.Id}");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var post = await _postService.GetPostById(model.Id);
                if (post != null)
                {
                    post.Id = model.Id;
                    post.Name = model.Name;
                    post.Description = model.Description;
                    post.Author = model.Author;
                    post.AuthorId = model.AuthorId;
                    post.Price = model.Price;

                    bool isPostUpdated = await _postService.UpdatePost(model);
                    if (isPostUpdated)
                    {
                        return Ok("Post Updated Successfully");
                    }
                    return BadRequest("Something went wrong while updating post.");
                }

                bool isPostCreated = await _postService.SavePost(model);
                if (isPostCreated)
                {
                    return Ok("Post Saved Successfully");
                }

                return BadRequest("Something went wrong while creating post.");
            }
            catch (Exception ex)
            {
                return BadRequest("Something went wrong in data service with failing exception:" + ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetDataAsync(int id)
        {
            try
            {
                var post = await _postService.GetPostById(id);
                if (post != null)
                {
                    return Ok(post);
                }

                return BadRequest("Not Found");
            }
            catch (Exception ex)
            {
                return BadRequest("Unable to get post with id: " + id + "causing exception:" + ex.Message);
            }
        }
    }
}
