using Microservice.BestStories.Services.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Microservice.BestStories.Services
{
    public class HackerNewsService : IHackerNewsService
    {
        private readonly IHttpClientFactory _clientFactory;
        private HttpClient hackerNewsClient;

        private HttpClient HackerNewsClient
        {
            get
            {
                if(hackerNewsClient == null) hackerNewsClient = _clientFactory.CreateClient("HackerNews");
                return hackerNewsClient;
            }
        }

        public HackerNewsService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<IEnumerable<int>> GetTwentyFirstBestStoriesIdsAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            using var request = new HttpRequestMessage(HttpMethod.Get, "beststories.json");

            var response = await HackerNewsClient.SendAsync(request, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var bestStories = DeserializeJson<IEnumerable<int>>(responseStream);
                return bestStories.Take(20);
            }

            throw new Exception($"Error trying to get the first 20 stories IDs: {response.ReasonPhrase}");
        }

        public async Task<StoryDetail> GetStoryDetailsByIdAsync(int id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            using var request = new HttpRequestMessage(HttpMethod.Get, $"item/{id}.json");

            var response = await HackerNewsClient.SendAsync(request, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();

                return DeserializeJson<StoryDetail>(responseStream);
            }

            throw new Exception($"Error trying to get story detail. Id: {id}. Error: {response.ReasonPhrase}");
        }

        private T DeserializeJson<T>(Stream stream)
        {
            using var reader = new StreamReader(stream);
            using var jsonReader = new JsonTextReader(reader);
            var serialize = new JsonSerializer();
            return serialize.Deserialize<T>(jsonReader);
        }
    }
}
