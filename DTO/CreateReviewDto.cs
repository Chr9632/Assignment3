namespace assignment_3_movievault.DTO;

using System.ComponentModel.DataAnnotations;

public class CreateReviewDto
{
    [Required]
    [StringLength(100)]
    public string Headline { get; set; } = string.Empty;

    [Required]
    public string Review_text { get; set; } = string.Empty;

    [Range(1, 5)]
    public int Rating { get; set; }

    [Range(1, int.MaxValue)]
    public int Movie_id { get; set; }

    public int? Reviewer_id { get; set; }
}
