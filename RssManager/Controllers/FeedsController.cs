using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RssManager.Interfaces;
using RssManager.Models;
using System.ServiceModel.Syndication;
using System.Xml;

namespace RssManager.Controllers
{
    [Route("api/feeds")]
    [ApiController]
    [Authorize]
    public class FeedsController : ControllerBase
    {
        private readonly IFeedReposiory _feedReposiory;

        public FeedsController(IFeedReposiory feedReposiory)
        {
            _feedReposiory = feedReposiory;
        }
        [HttpGet("active")]
        public async Task<List<Feed>> GetAllSubscribedFeeds()
        {
            return await _feedReposiory.GetAllSubscribedFeedsAsync();
        }
        [HttpPost]
        public async Task<Feed> AddFeedAsync(string url)
        {
            var client = new HttpClient();
            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode || !response.Content.Headers.ContentType.MediaType.EndsWith("xml"))
            {
                throw new BadHttpRequestException("No rss feed with such url!");
            }

            using var reader = XmlReader.Create(url);
            var loadedFeed = SyndicationFeed.Load(reader);

            var feed = new Feed
            {
                Title = loadedFeed.Title.Text,
                Description = loadedFeed.Description.Text,
                IsSubscribed = true,
                Link = url
            };

            return await _feedReposiory.AddFeedAsync(feed);
        }
        [HttpPatch("{id}/unsubscribe")]
        public async Task UnsubscribeFromFeed(int id)
        {
            await _feedReposiory.UnsubscribeFromFeedAsync(id);
        }
    }
}
