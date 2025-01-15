using dataService.Model;

namespace dataService.Interface
{
    public interface IPostService
    {
        Task<Post?> GetPostById(int id);
        Task<bool> SavePost(Post model);
        Task<bool> UpdatePost(Post model);
    }
}
