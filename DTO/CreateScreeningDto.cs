using System.ComponentModel.DataAnnotations;

namespace assignment_3_movievault.DTO;

public class CreateScreeningDto
{
    [Required]
    public DateTime Date { get; set; }

    [Required]
    public DateTime Start_time { get; set; }

    [Range(0, int.MaxValue)]
    public int Ticket_price { get; set; }

    [Range(1, int.MaxValue)]
    public int Movie_id { get; set; }

    [Range(1, int.MaxValue)]
    public int Screen_number { get; set; }

    [Range(1, int.MaxValue)]
    public int Cinema_id { get; set; }
}
