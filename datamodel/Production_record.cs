/*
    movie_id INT NOT NULL, 
    production_budget INT NOT NULL CHECK(production_budget >= 0), 
    shooting_start_date DATETIME NOT NULL, 
    shooting_end_date DATETIME NOT NULL,
    PRIMARY KEY (movie_id),
    FOREIGN KEY (movie_id) REFERENCES Movie(movie_id) ON UPDATE CASCADE ON DELETE CASCADE,
    CONSTRAINT shooting_dates CHECK(shooting_start_date <= shooting_end_date)
*/

public class Production_record
{
    public int Movie_id {get; set; }
    public int Production_budget {get; set; }
    public DateTime Shooting_start_date {get; set; }
    public DateTime Shooting_end_date {get; set; }
    public Movie Movie { get; set; } = null!;
}