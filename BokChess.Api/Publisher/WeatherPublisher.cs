using BokChess.Api.Enums;
using BokChess.Api.Events;
using BokChess.Api.Shared;
using Hangfire;
using MassTransit;

namespace BokChess.Api.Publisher;

public class WeatherPublisher : BackgroundService
{
    private readonly ILogger<WeatherPublisher> _logger;
    private readonly IBus _bus;
    private readonly IBackgroundJobClient _backgroundJobClient;

    public WeatherPublisher(ILogger<WeatherPublisher> logger, IBus bus, IBackgroundJobClient backgroundJobClient)
    {
        _logger = logger;
        _bus = bus;
        _backgroundJobClient = backgroundJobClient;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        //while (!stoppingToken.IsCancellationRequested)
        //{
        await Task.Yield();

        //var pressedKey = Console.ReadKey(true);
        //if (pressedKey.Key != ConsoleKey.Escape)
        //{
        //    var values = Enum.GetValues(typeof(WeatherType));
        //    var random = new Random();
        //    var newWeather = (WeatherType)values.GetValue(random.Next(values.Length))!;
        //    _logger.LogInformation("Change weather from {OldWeather} to {NewWeather}", GlobalVariables.CurrentWeather, newWeather);
        //    await _bus.Publish(new WeatherChangedEvent(GlobalVariables.CurrentWeather, newWeather), stoppingToken);

        //}

        //_backgroundJobClient.Schedule(() => PublishWeatherEvent(), TimeSpan.FromSeconds(10));

        await Task.Delay(100, stoppingToken);
        //}
    }

    public async Task PublishWeatherEvent()
    {
        var values = Enum.GetValues(typeof(WeatherType));
        var random = new Random();
        var newWeather = (WeatherType)values.GetValue(random.Next(values.Length))!;
        _logger.LogInformation("Change weather from {OldWeather} to {NewWeather}", GlobalVariables.CurrentWeather, newWeather);

        await _bus.Publish(new WeatherChangedEvent(GlobalVariables.CurrentWeather, newWeather));

    }
}
