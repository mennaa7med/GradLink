using GradLink.Application.Common.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GradLink.Infrastructure.Services;

public class QueuedAnalysisService : BackgroundService
{
    private readonly IBackgroundQueue _queue;
    private readonly ILogger<QueuedAnalysisService> _logger;

    public QueuedAnalysisService(
        IBackgroundQueue queue,
        ILogger<QueuedAnalysisService> logger)
    {
        _queue = queue;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Queued Analysis Service is starting.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var workItem = await _queue.DequeueAsync(stoppingToken);

                await workItem(stoppingToken);
            }
            catch (OperationCanceledException)
            {
                // Expected when cancellation is requested
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred executing background work item.");
            }
        }

        _logger.LogInformation("Queued Analysis Service is stopping.");
    }
}

