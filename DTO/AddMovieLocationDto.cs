using System.ComponentModel.DataAnnotations;

namespace assignment_3_movievault.DTO;

public class AddMovieLocationDto
{
    [Range(1, int.MaxValue)]
    public int Location_id { get; set; }

    [Range(0, int.MaxValue)]
    public int Shooting_days { get; set; }
}
