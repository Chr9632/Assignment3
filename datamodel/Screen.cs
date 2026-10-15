/*
    screen_number INT NOT NULL CHECK(screen_number > 0),
    cinema_id INT NOT NULL, 
    name VARCHAR(100) NOT NULL,
    screen_type VARCHAR(100) NOT NULL, 
    seating_capacity INT NOT NULL CHECK(seating_capacity > 0),
    PRIMARY KEY (screen_number, cinema_id),
    FOREIGN KEY (cinema_id) REFERENCES Cinema(cinema_id) ON UPDATE CASCADE ON DELETE CASCADE
*/

public class Screen
{
    public int Screen_number {get; set; }
    public int Cinema_id {get; set; }
    public string Name {get; set; } = string.Empty;
    public string Screen_type {get; set; } = string.Empty;
    public int Seating_capacity {get; set; }
    public Cinema Cinema { get; set; } = null!;
    public ICollection<Screening> Screening { get; set; } = null!;
    
}