namespace assignment_3_movievault.DTO;


using System.ComponentModel.DataAnnotations;

public class UpdateMovieDto
{
    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    public DateTime Release_date { get; set; }

    [Range(1, int.MaxValue)]
    public int Runtime { get; set; }

    [Range(0, 18)]
    public int Age_rating { get; set; }

    [Required]
    public string Status { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int Director_id { get; set; }

    [Range(1, int.MaxValue)]
    public int Company_id { get; set; }
}

