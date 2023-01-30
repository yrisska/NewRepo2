using Microsoft.EntityFrameworkCore;
using RssManager.DataAccess;
using RssManager.Interfaces;
using RssManager.Models;
using System.ServiceModel.Syndication;
using System.Xml;

namespace RssManager.Services.Repositories
{
    public class FeedRepository : IFeedReposiory
    {
        private readonly RssManagerContext _context;

        public FeedRepository(RssManagerContext context)
        {
            _context = context;
        }

        public async Task<Feed> AddFeedAsync(Feed feed)
        {
            await _context.Feeds.AddAsync(feed);
            await _context.SaveChangesAsync();
            return feed;
        }

        public async Task<List<Feed>> GetAllSubscribedFeedsAsync()
        {
            return await _context.Feeds
                .Where(x => x.IsSubscribed == true)
                .ToListAsync();
        }

        public async Task UnsubscribeFromFeedAsync(int id)
        {
            var feed = await _context.Feeds.FindAsync(id);

            if(feed == null)
            {
                throw new KeyNotFoundException();
            }

            feed.IsSubscribed = false;
            await _context.SaveChangesAsync();
        }
    }
}
