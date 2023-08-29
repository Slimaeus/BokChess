using BokChess.Api.Models;
using MassTransit;

namespace BokChess.Api.Consumers;

public class PingConsumer : IConsumer<Ping>
{
    private readonly ILogger<PingConsumer> _logger;

    public PingConsumer(ILogger<PingConsumer> logger) => _logger = logger;
    public Task Consume(ConsumeContext<Ping> context)
    {
        var button = context.Message.Button;
        _logger.LogInformation("Button {Button} pressed", button);
        return Task.CompletedTask;
    }
}
