namespace assignment_3_movievault.DTO;

public class MovieDetailsResponseDto
{
    public int Movie_id { get; set; }

    public string Title { get; set; } = string.Empty;

    public DateTime Release_date { get; set; }

    public int Runtime { get; set; }

    public int Age_rating { get; set; }

    public string Status { get; set; } = string.Empty;

    public string Director { get; set; } = string.Empty;

    public string Production_company { get; set; } = string.Empty;

    public int? Production_budget { get; set; }

    public List<MovieDetailsActorDto> Actors { get; set; } = new();

    public List<MovieDetailsLocationDto> Filming_locations { get; set; } = new();
}