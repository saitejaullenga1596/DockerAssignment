using Microsoft.AspNetCore.Mvc;
using webservice.Model;

namespace webservice.Interface
{
    public interface IPostService
    {
        Task<Post?> GetPost(int id);
        Task<bool> CreatePost(Post model);
    }
}
