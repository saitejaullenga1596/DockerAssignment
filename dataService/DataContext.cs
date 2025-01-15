using dataService.Model;
using Microsoft.EntityFrameworkCore;

namespace dataService
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        {
            
        }

        public DbSet<Post> Posts { get; set; }
    }
}
