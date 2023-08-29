namespace BokChess.Api.Settings;

public class RabbitMQSettings
{
    public string Host { get; set; } = "localhost";
    public string VirtualHost { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public int Port { get; set; } = 5672;
}
