
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MovieWebsite.Data;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MovieWebsite.Services
{
    public class EpisodePublishingService : BackgroundService
    {
        private readonly ILogger<EpisodePublishingService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public EpisodePublishingService(ILogger<EpisodePublishingService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Episode Publishing Service is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await PublishScheduledEpisodes(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while publishing episodes.");
                }

                // Chờ 5 phút trước khi kiểm tra lại
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }

            _logger.LogInformation("Episode Publishing Service is stopping.");
        }

        private async Task PublishScheduledEpisodes(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Episode Publishing Service is checking for episodes to publish.");

            // Tạo một scope mới để lấy DbContext, vì BackgroundService là Singleton
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var episodesToPublish = await dbContext.Episodes
                    .Where(e => !e.IsPublished && e.ReleaseDate <= DateTime.UtcNow)
                    .ToListAsync(stoppingToken);

                if (episodesToPublish.Any())
                {
                    _logger.LogInformation("Found {Count} episodes to publish.", episodesToPublish.Count);
                    foreach (var episode in episodesToPublish)
                    {
                        episode.IsPublished = true;
                    }

                    await dbContext.SaveChangesAsync(stoppingToken);
                    _logger.LogInformation("Successfully published {Count} episodes.", episodesToPublish.Count);
                }
                else
                {
                    _logger.LogInformation("No new episodes to publish at this time.");
                }
            }
        }
    }
}