namespace assignment_3_movievault.DTO;

public class ScreeningResponseDto
{
    public int Screening_id { get; set; }

    public DateTime Date { get; set; }

    public DateTime Start_time { get; set; }

    public int Ticket_price { get; set; }

    public int Movie_id { get; set; }

    public int Screen_number { get; set; }

    public int Cinema_id { get; set; }
}