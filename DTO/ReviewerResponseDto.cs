namespace assignment_3_movievault.DTO;

public class ReviewerResponseDto
{
    public int Reviewer_id { get; set; }
    public string Name { get; set; } = string.Empty;
    public MediaOutletResponseDto Media_outlet { get; set; } = null!;
}
