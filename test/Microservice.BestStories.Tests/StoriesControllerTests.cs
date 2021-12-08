using FluentAssertions;
using Microservice.BestStories.Controllers;
using Microservice.BestStories.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Net;
using System.Threading;

namespace Microservice.BestStories.Tests
{
    [TestFixture]
    public class Tests
    {
        private Mock<ILogger<StoriesController>> mockLogger;
        private Mock<IMemoryCacheService> mockMemoryCache;
        private Mock<IHackerNewsService> mockHackerNewsService;
        private StoriesController underTest;

        [SetUp]
        public void SetUp()
        {
            mockLogger = new Mock<ILogger<StoriesController>>();
            mockHackerNewsService = new Mock<IHackerNewsService>();
            mockMemoryCache = new Mock<IMemoryCacheService>();
            underTest = new StoriesController(mockLogger.Object, mockMemoryCache.Object, mockHackerNewsService.Object);
        }

        [Test]
        public void When_Call_GetAsync_Should_Return_Ok()
        {
            //Arrange
            var cancellationTokenSource = new CancellationTokenSource(1000);
            // Act
            var httpResult = underTest.GetAsync(cancellationTokenSource.Token);
            var result = httpResult.Result as OkObjectResult;

            // Assert
            mockHackerNewsService.Verify(s => s.GetTwentyFirstBestStoriesIdsAsync(cancellationTokenSource.Token), Times.Once());

            result.Should().NotBeNull();
            result.StatusCode.Value.Should().Be((int)HttpStatusCode.OK);

        }
    }
}