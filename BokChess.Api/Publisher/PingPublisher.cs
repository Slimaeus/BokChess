using BokChess.Api.Models;
using MassTransit;

namespace BokChess.Api.Publisher;

public class PingPublisher : BackgroundService
{
    private readonly ILogger<PingPublisher> _logger;
    private readonly IBus _bus;

    public PingPublisher(ILogger<PingPublisher> logger, IBus bus)
    {
        _logger = logger;
        _bus = bus;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Yield();

            var pressedKey = Console.ReadKey(true);
            if (pressedKey.Key != ConsoleKey.Escape)
            {
                _logger.LogInformation("Pressed {Button}", pressedKey.Key.ToString());
                await _bus.Publish(new Ping(pressedKey.Key.ToString()), stoppingToken);

            }

            await Task.Delay(200, stoppingToken);
        }
    }
}
