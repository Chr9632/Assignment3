/*
    sale_id INT NOT NULL PRIMARY KEY, 
    quantity INT NOT NULL CHECK (quantity > 0), 
    sale_date_time DATETIME NOT NULL CHECK(sale_date_time < GETDATE()),
    screening_id INT NOT NULL,
    FOREIGN KEY (screening_id) REFERENCES Screening(screening_id)
*/

public class Ticket_sale
{
    public int Sale_id {get; set; }
    public int Quantity {get; set; }
    public DateTime Sale_date_time {get; set; }
    public int Screening_id {get; set; }
    public Screening Screening { get; set; } = null!;
}