/*
    movie_id INT NOT NULL, 
    location_id INT NOT NULL,
    shooting_days INT NOT NULL CHECK (shooting_days >= 0), 
    PRIMARY KEY (movie_id, location_id),
    FOREIGN KEY (movie_id) REFERENCES Movie(movie_id) ON UPDATE CASCADE ON DELETE CASCADE,
    FOREIGN KEY (location_id) REFERENCES Filming_location(location_id) ON UPDATE CASCADE ON DELETE CASCADE
*/

public class Movie_location
{
    public int Movie_id {get; set; }
    public int Location_id {get; set; }
    public int Shooting_days {get; set; }

    public Movie Movie { get; set; } = null!;
    public Filming_location Filming_location { get; set; } = null!;
}