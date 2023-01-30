using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RssManager.Interfaces;
using RssManager.Models;

namespace RssManager.Controllers
{
    [Route("api/posts")]
    [ApiController]
    [Authorize]
    public class PostsController : ControllerBase
    {
        private readonly IPostRepository _postRepository;

        public PostsController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }
        [HttpGet("unread")]
        public async Task<List<Post>> GetAllUnreadPosts([FromQuery] DateTime? dateTime)
        {
            //For testing purposes parameter is nullable and can be replaced by today date
            return await _postRepository.GetAllUnreadPostsAsync(dateTime ?? DateTime.Today);
        }
        [HttpPatch("set-read")]
        public async Task SetPostsAsRead()
        {
            await _postRepository.SetPostsAsReadAsync();
        }
        //Method for testing purposes only
        //We can use here "[Authorize("access:admin-data")]" in order to secure our application
        [HttpDelete("all")]
        public async Task DeleteAllPosts()
        {
            await _postRepository.DeleteAllPostsAsync();
        }
    }
}
