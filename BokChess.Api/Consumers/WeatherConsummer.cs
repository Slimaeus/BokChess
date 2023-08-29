using BokChess.Api.Events;
using BokChess.Api.Shared;
using MassTransit;

namespace BokChess.Api.Consumers;

public class WeatherConsummer : IConsumer<WeatherChangedEvent>
{
    private readonly ILogger<WeatherConsummer> _logger;

    public WeatherConsummer(ILogger<WeatherConsummer> logger)
    {
        _logger = logger;
    }
    public Task Consume(ConsumeContext<WeatherChangedEvent> context)
    {
        //var oldWeather = context.Message.OldWeather;
        var newWeather = context.Message.NewWeather;
        GlobalVariables.CurrentWeather = newWeather;
        _logger.LogInformation("Now weather is {Weather}", newWeather);
        return Task.CompletedTask;
    }
}
