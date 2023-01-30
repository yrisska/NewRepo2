namespace RssManager.Models
{
    //Post (News) entity
    public class Post : BaseEntity
    {
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Link { get; set; }
        public DateTime PubDate { get; set; }
        public bool IsRead { get; set; }
        public int FeedId { get; set; }
        //Project is to small to implement DTO pattern so this attribute is workaround
        [System.Text.Json.Serialization.JsonIgnore]
        public Feed Feed { get; set; }
    }
}
