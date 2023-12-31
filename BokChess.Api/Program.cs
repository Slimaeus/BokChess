using BokChess.Api.Publisher;
using BokChess.Api.Settings;
using Hangfire;
using Hangfire.MemoryStorage;
using MassTransit;
using Microsoft.Extensions.Options;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHostedService<WeatherPublisher>();

builder.Services.Configure<RabbitMQSettings>(builder.Configuration.GetSection(nameof(RabbitMQSettings)));

builder.Services.AddMassTransit(config =>
{
    config.SetKebabCaseEndpointNameFormatter();
    config.SetInMemorySagaRepositoryProvider();

    var assembly = Assembly.GetExecutingAssembly();

    config.AddConsumers(assembly);
    config.AddSagaStateMachines(assembly);
    config.AddSagas(assembly);
    config.AddActivities(assembly);

    //config.UsingInMemory((context, config) =>
    //{
    //    config.ConfigureEndpoints(context);
    //});


    config.UsingRabbitMq((context, rabbitMQConfig) =>
    {
        var rabbitMQSettings = context.GetRequiredService<IOptions<RabbitMQSettings>>().Value;

        rabbitMQConfig.Host(rabbitMQSettings.Host, rabbitMQSettings.VirtualHost, hostConfigurator =>
        {
            hostConfigurator.Username(rabbitMQSettings.UserName);
            hostConfigurator.Password(rabbitMQSettings.Password);
        });

        rabbitMQConfig.ConfigureEndpoints(context);
    });
});

builder.Services.AddHangfire(config => config
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseDefaultTypeSerializer()
    .UseMemoryStorage());

builder.Services.AddHangfireServer(options =>
{
    options.SchedulePollingInterval = TimeSpan.FromSeconds(1);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("hello", () => Results.Ok("Hello"));

app.UseHangfireDashboard();

RecurringJob.AddOrUpdate<WeatherPublisher>("weather", publisher => publisher.PublishWeatherEvent(), Cron.Minutely);

app.Run();
