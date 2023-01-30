using RssManager.Models;

namespace RssManager.Interfaces
{
    public interface IPostRepository
    {
        Task<List<Post>> GetAllUnreadPostsAsync(DateTime date);
        Task<DateTime?> GetLastPostDateTimeAsync();
        Task SetPostsAsReadAsync();
        Task AddRangeAsync(List<Post> posts);
        Task DeleteAllPostsAsync();
    }
}
