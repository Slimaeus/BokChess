using BokChess.Api.Enums;
using BokChess.Api.Events;
using BokChess.Api.Shared;
using MassTransit;

namespace BokChess.Api.Publisher;

public class WeatherPublisher : BackgroundService
{
    private readonly ILogger<WeatherPublisher> _logger;
    private readonly IBus _bus;

    public WeatherPublisher(ILogger<WeatherPublisher> logger, IBus bus)
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
                var values = Enum.GetValues(typeof(WeatherType));
                var random = new Random();
                var newWeather = (WeatherType)values.GetValue(random.Next(values.Length))!;
                _logger.LogInformation("Change weather from {OldWeather} to {NewWeather}", GlobalVariables.CurrentWeather, newWeather);
                await _bus.Publish(new WeatherChangedEvent(GlobalVariables.CurrentWeather, newWeather), stoppingToken);

            }

            await Task.Delay(100, stoppingToken);
        }
    }
}
