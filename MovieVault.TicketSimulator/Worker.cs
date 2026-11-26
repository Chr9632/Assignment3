
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Net.Http.Headers;

namespace MovieVault.TicketSimulator;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private DateTime _tokenExpiresAt = DateTime.MinValue;

    public Worker(
        ILogger<Worker> logger,
        HttpClient httpClient,
        IConfiguration configuration)
    {
        _logger = logger;
        _httpClient = httpClient;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        int interval = Math.Max(1,
            _configuration.GetValue<int>(
                "Simulator:IntervalSeconds", 10));

        _logger.LogInformation("Ticket simulator started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await AuthenticateAsync(stoppingToken);
                await SimulateTicketSaleAsync(stoppingToken);
            }
            catch (OperationCanceledException)
                when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(
                    ex, "Simulation failed. Retrying next cycle.");
            }

            await Task.Delay(
                TimeSpan.FromSeconds(interval),
                stoppingToken);
        }
    }

private async Task AuthenticateAsync(
    CancellationToken cancellationToken)
    {
        if (!_configuration.GetValue<bool>(
            "Simulator:Authentication:Enabled"))
        {
            return;
        }

        // Reuse the token until it needs to be refreshed
        if (_httpClient.DefaultRequestHeaders.Authorization != null
            && DateTime.UtcNow < _tokenExpiresAt)
        {
            return;
        }

        var loginRequest = new
        {
            Name = _configuration[
                "Simulator:Authentication:Username"],
            Email = _configuration[
                "Simulator:Authentication:Username"],
            Password = _configuration[
                "Simulator:Authentication:Password"]
        };

        _httpClient.DefaultRequestHeaders.Authorization = null;

        var loginStartedAt = DateTime.UtcNow;

        using var response = await _httpClient.PostAsJsonAsync(
            _configuration["Simulator:Authentication:LoginPath"],
            loginRequest,
            cancellationToken);

        response.EnsureSuccessStatusCode();

        var token = (await response.Content
            .ReadAsStringAsync(cancellationToken))
            .Trim().Trim('"');

        if (string.IsNullOrWhiteSpace(token))
        {
            throw new InvalidOperationException(
                "Login did not return a token.");
        }

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        _tokenExpiresAt = loginStartedAt.AddSeconds(270);

        _logger.LogInformation("Authentication successful.");
    }
    private async Task SimulateTicketSaleAsync(
        CancellationToken cancellationToken)
    {
        // Retrieve movies
        var movies = await _httpClient
            .GetFromJsonAsync<List<MovieDto>>(
                _configuration["Simulator:MoviesPath"],
                cancellationToken);

        if (movies == null || movies.Count == 0)
        {
            _logger.LogWarning("No movies found.");
            return;
        }

        // Select a random movie
        var movie = movies[Random.Shared.Next(movies.Count)];

        // Retrieve screenings for the selected movie
        string screeningsPath =
            _configuration["Simulator:ScreeningsPath"]!
                .Replace("{movieId}", movie.Movie_id.ToString());

        var screenings = await _httpClient
            .GetFromJsonAsync<List<ScreeningDto>>(
                screeningsPath, cancellationToken);

        if (screenings == null || screenings.Count == 0)
        {
            _logger.LogInformation(
                "No screenings found for {Movie}.", movie.Title);

            return;
        }

        // Select a random screening
        var screening =
            screenings[Random.Shared.Next(screenings.Count)];

        // Generate a simulated ticket sale
        var ticketSale = new TicketSaleDto
        {
            Quantity = Random.Shared.Next(1, 5),
            Sale_date_time = DateTime.UtcNow.AddMinutes(-1),
            Screening_id = screening.Screening_id
        };

        // Submit ticket sale through the API
        using var response = await _httpClient.PostAsJsonAsync(
            _configuration["Simulator:TicketSalesPath"],
            ticketSale,
            cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation(
                "Sold {Quantity} tickets for {Movie}. " +
                "Screening ID: {ScreeningId}",
                ticketSale.Quantity,
                movie.Title,
                screening.Screening_id);
        }
        else
        {
            _logger.LogWarning(
                "Ticket sale rejected. Status: {StatusCode}",
                (int)response.StatusCode);
        }
    }
}

public class MovieDto
{
    [JsonPropertyName("movie_id")]
    public int Movie_id { get; set; }

    public string Title { get; set; } = string.Empty;
}

public class ScreeningDto
{
    [JsonPropertyName("screening_id")]
    public int Screening_id { get; set; }
}

public class TicketSaleDto
{
    public int Quantity { get; set; }

    public DateTime Sale_date_time { get; set; }

    public int Screening_id { get; set; }
}
