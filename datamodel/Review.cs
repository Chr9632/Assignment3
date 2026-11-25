public class Review
{
    public int Review_id { get; set; }
    public string Headline { get; set; } = string.Empty;
    public string Review_text { get; set; } = string.Empty;
    public int Rating { get; set; }
    public DateTime Published_at { get; set; }
    public int Movie_id { get; set; }
    public int Reviewer_id { get; set; }

    public Movie Movie { get; set; } = null!;
    public Reviewer Reviewer { get; set; } = null!;
}
