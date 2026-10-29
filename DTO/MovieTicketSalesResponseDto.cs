namespace assignment_3_movievault.DTO;

public class MovieTicketSalesResponseDto
{
    public int Movie_id { get; set; }

    public string Title { get; set; } = string.Empty;

    public int Total_tickets_sold { get; set; }

    public long Total_revenue { get; set; }
}