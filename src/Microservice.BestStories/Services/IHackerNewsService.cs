using Microservice.BestStories.Services.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microservice.BestStories.Services
{
    public interface IHackerNewsService
    {
        Task<IEnumerable<int>> GetTwentyFirstBestStoriesIdsAsync(CancellationToken cancellationToken);
        Task<StoryDetail> GetStoryDetailsByIdAsync(int id, CancellationToken cancellationToken);
    }
}
