using System.ComponentModel.DataAnnotations;

namespace assignment_3_movievault.DTO;

public class UpdateTicketSaleDto
{
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }

    [Required]
    public DateTime Sale_date_time { get; set; }

    [Range(1, int.MaxValue)]
    public int Screening_id { get; set; }
}
