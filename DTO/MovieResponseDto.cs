namespace assignment_3_movievault.DTO;

public class MovieResponseDto
{
    public int Movie_id { get; set; }

    public string Title { get; set; } = string.Empty;

    public DateTime Release_date { get; set; }

    public int Runtime { get; set; }

    public int Age_rating { get; set; }

    public string Status { get; set; } = string.Empty;

    public int Director_id { get; set; }

    public int Company_id { get; set; }
}