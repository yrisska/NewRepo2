using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssManager.Models
{
    //Feed (Rss channel) entity
    public class Feed : BaseEntity
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
        public bool IsSubscribed { get; set; }
    }
}
