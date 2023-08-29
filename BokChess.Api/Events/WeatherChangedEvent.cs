using BokChess.Api.Enums;

namespace BokChess.Api.Events;

public record WeatherChangedEvent(WeatherType OldWeather, WeatherType NewWeather);
