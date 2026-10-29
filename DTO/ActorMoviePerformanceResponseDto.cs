namespace assignment_3_movievault.DTO;

public class ActorMoviePerformanceResponseDto
{
    public int Actor_id { get; set; }

    public string Name { get; set; } = string.Empty;

    public List<ActorMoviePerformanceMovieDto> Movies { get; set; } = new();

    public int Total_tickets_sold { get; set; }
}