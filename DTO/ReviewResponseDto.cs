namespace assignment_3_movievault.DTO;

public class ReviewResponseDto
{
    public int Review_id { get; set; }
    public string Headline { get; set; } = string.Empty;
    public string Review_text { get; set; } = string.Empty;
    public int Rating { get; set; }
    public DateTime Published_at { get; set; }
    public int Movie_id { get; set; }
    public ReviewerResponseDto Reviewer { get; set; } = null!;
}
