/*
    screening_id INT NOT NULL PRIMARY KEY,
    date DATETIME NOT NULL, 
    start_time DATETIME NOT NULL, 
    ticket_price INT NOT NULL CHECK (ticket_price >= 0), 
    movie_id INT NOT NULL,
    screen_number INT NOT NULL,
    cinema_id INT NOT NULL,
    FOREIGN KEY (movie_id) REFERENCES Movie(movie_id) ON UPDATE CASCADE ON DELETE CASCADE,
    FOREIGN KEY (screen_number, cinema_id) REFERENCES Screen(screen_number, cinema_id) ON UPDATE CASCADE ON DELETE CASCADE
*/

public class Screening
{
    public int Screening_id {get; set; }
    public DateTime Date {get; set; }
    public DateTime Start_time {get; set; } 
    public int Ticket_price {get; set; }
    public int Movie_id {get; set; }
    public int Screen_number {get; set; } 
    public int Cinema_id {get; set; }
    public Movie Movie { get; set; } = null!;
    public Screen Screen { get; set; } = null!;
    public ICollection<Ticket_sale> Ticket_sale { get; set; } = null!;
}