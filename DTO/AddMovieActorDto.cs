
using System.ComponentModel.DataAnnotations;

namespace assignment_3_movievault.DTO;

public class AddMovieActorDto
{
    [Range(1, int.MaxValue)]
    public int Actor_id { get; set; }

    [Required]
    [RegularExpression("lead|supporting|cameo")]
    public string Role { get; set; } = string.Empty;

    [Required]
    public string Character_name { get; set; } = string.Empty;
}
