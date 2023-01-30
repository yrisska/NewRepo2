using Microsoft.EntityFrameworkCore;
using RssManager.DataAccess;
using RssManager.Interfaces;
using RssManager.Models;

namespace RssManager.Services.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly RssManagerContext _context;

        public PostRepository(RssManagerContext context)
        {
            _context = context;
        }

        public async Task<List<Post>> GetAllUnreadPostsAsync(DateTime date)
        {
            return await _context.Posts
                .Where(x => x.PubDate.Date >= date.Date && x.IsRead == false)
                .ToListAsync();
        }
        public async Task<DateTime?> GetLastPostDateTimeAsync()
        {
            var empty = !await _context.Posts.AnyAsync();
            return  empty?
                null : await _context.Posts.MaxAsync(x => x.PubDate);
        }

        public async Task AddRangeAsync(List<Post> posts)
        {
            await _context.Posts.AddRangeAsync(posts);
            await _context.SaveChangesAsync();
        }

        public async Task SetPostsAsReadAsync()
        {
            await _context.Posts
                .Where(x => x.IsRead == false)
                .ForEachAsync(x => x.IsRead = true);

            await _context.SaveChangesAsync();
        }

        //Method for testing purposes only
        public async Task DeleteAllPostsAsync()
        {
            await _context.Database.ExecuteSqlRawAsync("DELETE FROM Posts;");
            await _context.SaveChangesAsync();
        }
    }
}
