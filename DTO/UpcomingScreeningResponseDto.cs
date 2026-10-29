namespace assignment_3_movievault.DTO;

public class UpcomingScreeningResponseDto
{
    public int Screening_id { get; set; }

    public string Movie_title { get; set; } = string.Empty;

    public string Cinema_name { get; set; } = string.Empty;

    public string Cinema_city { get; set; } = string.Empty;

    public int Screen_number { get; set; }

    public string Screen_name { get; set; } = string.Empty;

    public DateTime Date { get; set; }

    public DateTime Start_time { get; set; }

    public int Ticket_price { get; set; }
}