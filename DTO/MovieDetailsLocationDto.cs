namespace assignment_3_movievault.DTO;

public class MovieDetailsLocationDto
{
    public int Location_id { get; set; }

    public string Location_name { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public string Country { get; set; } = string.Empty;

    public string Location_type { get; set; } = string.Empty;

    public int Shooting_days { get; set; }
}