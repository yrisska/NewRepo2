using RssManager.Interfaces;
using RssManager.Models;
using System.ServiceModel.Syndication;
using System.Xml;

namespace RssManager.Services
{
    //Main background service to check and update posts, update frequency is set in the configuration
    public class WorkerService : BackgroundService
    {
        private readonly TimeSpan _delay;
        private readonly ILogger<WorkerService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        public WorkerService(IConfiguration configuration, ILogger<WorkerService> logger, IServiceScopeFactory scopeFactory)
        {
            if(!TimeSpan.TryParse(configuration["UpdateDelay"], out TimeSpan delay))
            {
                throw new Exception("Cannot parse update delay value");
            }

            _delay = delay;
            _logger = logger;
            _scopeFactory = scopeFactory;
        }
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(_delay, cancellationToken);
                await CheckForUpdatesAsync();
                _logger.LogDebug("Updated");
            }
        }
        //Method just checks for subscribed feeds and load all posts from some date (today if empty or MAX in posts table)
        private async Task CheckForUpdatesAsync()
        {
            using var scope = _scopeFactory.CreateScope();

            var _feedReposiory = scope.ServiceProvider.GetRequiredService<IFeedReposiory>();
            var _postRepository = scope.ServiceProvider.GetRequiredService<IPostRepository>();

            var minimumPubDate = await _postRepository.GetLastPostDateTimeAsync() ?? DateTime.Today;

            var newPosts = new List<Post>();

            var subscribedFeeds = await _feedReposiory.GetAllSubscribedFeedsAsync();
            if (subscribedFeeds.Count == 0)
                return;

            foreach (var subscribedFeed in subscribedFeeds)
            {
                using var reader = XmlReader.Create(subscribedFeed.Link);
                var loadedFeed = SyndicationFeed.Load(reader);

                loadedFeed.Items
                    .Where(x => x.PublishDate > minimumPubDate)
                    .ToList()
                    .ForEach(x => newPosts.Add(new Post
                    {
                        Title = x.Title.Text,
                        Summary = x.Summary.Text,
                        PubDate = x.PublishDate.DateTime,
                        Feed = subscribedFeed,
                        IsRead = false,
                        Link = x.Links.First().Uri.ToString()
                    }));
            }

            if (newPosts.Count == 0)
                return;

            await _postRepository.AddRangeAsync(newPosts);
        }
    }
}
