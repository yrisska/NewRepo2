using RssManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssManager.Interfaces
{
    public interface IFeedReposiory
    {
        Task<Feed> AddFeedAsync(Feed feed);
        Task<List<Feed>> GetAllSubscribedFeedsAsync();
        Task UnsubscribeFromFeedAsync(int id);
    }
}
