using Microservice.BestStories.Services;
using Microservice.BestStories.Services.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Microservice.BestStories.Controllers
{
    [ApiController]
    [Route("/best20")]
    [Produces("application/json")]
    public class StoriesController : ControllerBase
    {
        private readonly ILogger<StoriesController> _logger;
        private readonly IMemoryCacheService _memoryCacheService;
        private readonly IHackerNewsService _hackerNewsService;

        public StoriesController(
            ILogger<StoriesController> logger,
            IMemoryCacheService memoryCacheService,
            IHackerNewsService hackerNewsService)
        {
            _logger = logger;
            _memoryCacheService = memoryCacheService;
            _hackerNewsService = hackerNewsService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<StoryDetail>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        // using CancellationToken to prevent canceled calls from proceeding
        public async Task<ActionResult> GetAsync(CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                _logger.LogInformation($"Getting 20 best stories");

                var cachedStories = _memoryCacheService.GetCachedStories();
                if (cachedStories != null) 
                    return Ok(cachedStories);

                var storyTasks = new List<Task<StoryDetail>>();
                var bestTwentyStoriesIds = await _hackerNewsService.GetTwentyFirstBestStoriesIdsAsync(cancellationToken);
                foreach (var id in bestTwentyStoriesIds)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    storyTasks.Add(_hackerNewsService.GetStoryDetailsByIdAsync(id, cancellationToken));
                }
                await Task.WhenAll(storyTasks);

                var results = new List<StoryDetail>();
                foreach (var task in storyTasks)
                {
                    results.Add(task.Result);
                }
                var twentyBestStories = results.OrderByDescending(x => x.Score).ToList();
                _memoryCacheService.SetCachedStories(twentyBestStories);
                return Ok(twentyBestStories);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error trying to get best stories: {ex}");
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Your request could not be processed. Try again. Error: {ex.Message}");
            }
        }
    }
}
