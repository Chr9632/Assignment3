namespace assignment_3_movievault.DTO;

public class CinemaRevenueResponseDto
{
    public int Cinema_id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public int Total_tickets_sold { get; set; }

    public long Total_revenue { get; set; }
}