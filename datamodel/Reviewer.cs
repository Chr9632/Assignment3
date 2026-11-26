public class Reviewer
{
    public int Reviewer_id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Media_outlet_id { get; set; }
    public string? Identity_user_id { get; set; }

    public Media_outlet Media_outlet { get; set; } = null!;
    public ICollection<Review> Review { get; set; } = null!;
    public Identity_user? Identity_user { get; set; }
}
