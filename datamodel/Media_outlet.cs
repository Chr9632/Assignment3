public class Media_outlet
{
    public int Media_outlet_id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Outlet_type { get; set; } = string.Empty;
    public string Website { get; set; } = string.Empty;
    public ICollection<Reviewer> Reviewer { get; set; } = null!;
}
