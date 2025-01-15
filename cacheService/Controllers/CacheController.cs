using System.Text.Json;
using cacheService.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace cacheService.Controllers
{
    [ApiController]
    [Route("api/cache")]
    public class CacheController : ControllerBase
    {
        private readonly ILogger<CacheController> _logger;
        private readonly IMemoryCache _memoryCache;

        public CacheController(ILogger<CacheController> logger,IMemoryCache memoryCache)
        {
            _logger = logger;
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }

        [HttpPost("save/{id}")]
        public IActionResult SaveDataAsync([FromRoute] int id, [FromBody] PostViewModel model)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid Id. Id must be greater than zero.");
                }

                _memoryCache.Set(id, model, TimeSpan.FromMinutes(10));
                return Ok($"Value stored successfully for ID {id}.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("get/{id}")]
        public async Task<ActionResult<PostViewModel>> GetDataAsync(int id)
        {
            if (id <= 0) return BadRequest("Invalid Id");
            try
            {
                if (_memoryCache.TryGetValue(id, out PostViewModel value))
                {
                    return Ok(value);
                }

                return NotFound($"No value found for id:{id}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured: {ex.Message}");
            }
        }

    }
}
