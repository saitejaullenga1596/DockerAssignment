using System.Net;
using Microsoft.AspNetCore.Mvc;
using webservice.Interface;
using webservice.Model;

namespace webservice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetPost(int id)
        {
            // check for id
            if (id <= 0)
            {
                return BadRequest("Id should be greater than 0.");
            }

            try
            {
                var post = await _postService.GetPost(id);
                if(post != null) { return Ok(post); }
                return StatusCode((int)HttpStatusCode.NotFound, $"Requested post not found for id: {id}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("createPost")]
        public async Task<ActionResult<string>> CreatePost([FromBody]Post model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest($"Please check the input and try again");
            }

            try
            {
                bool isPostCreated = await _postService.CreatePost(model);
                if (isPostCreated)
                {
                    return StatusCode((int)HttpStatusCode.Created, $"Post is created with id:{model.Id}");
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError,$"Internal Server Error");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
