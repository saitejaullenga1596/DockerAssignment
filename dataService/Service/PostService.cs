using dataService.Interface;
using dataService.Model;
using Microsoft.EntityFrameworkCore;

namespace dataService.Service
{
    public class PostService : IPostService
    {
        private readonly string _baseUri;
        private readonly DataContext _context;

        public PostService(DataContext context)
        {
            _context = context;
        }

        public async Task<Post?> GetPostById(int id) => await _context.Posts.Where(x => x.Id == id).FirstOrDefaultAsync().ConfigureAwait(false);

        public async Task<bool> SavePost(Post model)
        {
            try
            {
                _context.Posts.Add(model);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UpdatePost(Post model)
        {
            try
            {
                _context.Posts.Update(model);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
