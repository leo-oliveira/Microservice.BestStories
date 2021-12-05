using Newtonsoft.Json;

namespace Microservice.BestStories.HackerNews.Models
{
    public class StoryDetail
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        
        [JsonProperty("url")]
        public string Uri { get; set; }
        
        [JsonProperty("by")]
        public string PostedBy { get; set; }
        
        [JsonProperty("time")]
        public int Time { get; set; }

        [JsonProperty("score")]
        public double Score { get; set; }

        [JsonProperty("descendants")]
        public int CommentCount { get; set; }
    }
}
