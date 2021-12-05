using Microservice.BestStories.HackerNews.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microservice.BestStories.HackerNews
{
    public interface IHackerNewsService
    {
        Task<IEnumerable<int>> GetFirstBestStoriesIds(int top = 20);
        Task<StoryDetail> GetStoryDetailsById(int id);
    }
}
