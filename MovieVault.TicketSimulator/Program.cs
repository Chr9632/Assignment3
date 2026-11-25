using MovieVault.TicketSimulator;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<HttpClient>(_ =>
{
    return new HttpClient
    {
        BaseAddress = new Uri(
            builder.Configuration["Simulator:BaseUrl"]!),
        Timeout = TimeSpan.FromSeconds(15)
    };
});

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();