# BokChess

## Structure of appsettings.json/appsettings.Development.json
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "RabbitMQSettings": {
    "Host": "localhost or ---.---.---.---",
    "VirtualHost": "I use cloudamqp so it the same as username",
    "UserName": "username",
    "Password": "password",
    "Port": 5672
  }
}

```
