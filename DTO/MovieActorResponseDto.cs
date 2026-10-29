namespace assignment_3_movievault.DTO;

public class MovieActorResponseDto
{
    public int Actor_id { get; set; }

    public int Movie_id { get; set; }

    public string Role { get; set; } = string.Empty;

    public string Character_name { get; set; } = string.Empty;
}