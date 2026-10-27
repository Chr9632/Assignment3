
using System.ComponentModel.DataAnnotations;

public class CreateMovieDto
{
    [Required]
    public string Title { get; set; } = string.Empty;

    public DateTime Release_date { get; set; }

    [Range(1, int.MaxValue)]
    public int Runtime { get; set; }

    [Range(0, int.MaxValue)]
    public int Age_rating { get; set; }

    [Required]
    public string Status { get; set; } = string.Empty;

    public int Director_id { get; set; }

    public int Company_id { get; set; }
}
